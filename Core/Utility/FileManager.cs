using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

using Core.Enumerations;
using Core.Exceptions;
using Core.Model;
using Core.Settings;

namespace Core.Utility
{
    /// <summary>
    /// Specialized functions for file system operations.
    /// </summary>
    public static class FileManager
    {
        /// <summary>
        /// Reads matrix and branches (if exists) from specified file.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <returns>Matrix and branches (if exists).</returns>
        /// <throws>CoreException, MatrixFormatException.</throws>
        public static NetworkInfoToRead Read(String fileName, int size)
        {
            if ((File.GetAttributes(fileName) & FileAttributes.Directory) == FileAttributes.Directory)
            {
                throw new CoreException("File should be specified.");
            }
            else
            {
                Debug.Assert(Path.GetExtension(fileName) == ".txt");
                NetworkInfoToRead r;
                if (IsMatrixFile(fileName))
                {
                    r = new MatrixInfoToRead();
                    (r as MatrixInfoToRead).Matrix = ReadMatrix(fileName);
                }
                else
                {
                    r = new NeighbourshipInfoToRead();
                    NeighbourshipInfoToRead nr = r as NeighbourshipInfoToRead;
                    nr.Neighbours = ReadNeighbourship(fileName);
                    nr.Size = size == 0 ? nr.Neighbours.Max() + 1 : size;                    
                }

                r.fileName = fileName;
                r.Branches = ReadBranches(fileName.Substring(0, fileName.Length - 4) + ".branches");
                r.ActiveStates = ReadActiveStates(fileName.Substring(0, fileName.Length - 4) + ".actives", size);

                return r;
            }
        }

        private static bool IsMatrixFile(String fileName)
        {
            using (StreamReader streamreader = new StreamReader(fileName, System.Text.Encoding.Default))
            {
                char[] buffer = new char[10];
                string[] saparators = { " " };
                streamreader.ReadBlock(buffer, 0, 10);
                string content = new string(buffer);
                string[] split = content.Split(saparators, StringSplitOptions.RemoveEmptyEntries);
                if (split.Count() <= 2)
                    return false;
                else
                {
                    foreach (string str in split)
                    {
                        if (str == "0" || str == "1")
                            continue;
                        else
                            return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public static bool ReadSubgraphMatrix(string fileName, out int[] m)
        {
            m = new int[0];
            if (Path.GetExtension(fileName) != ".sm")
                return false;

            string content = File.ReadAllText(fileName);
            string[] saparators = { " ", "\t", "\r", "\n" };
            string[] split = content.Split(saparators, StringSplitOptions.RemoveEmptyEntries);
            m = new int[split.Count()];
            for (int i = 0; i < split.Count(); ++i)
            {
                int v;
                if (!int.TryParse(split[i], out v))
                    return false;
                m[i] = v;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="subGraphs"></param>
        /// <returns></returns>
        public static bool ReadSubgraphMatrix(string fileName, ref Dictionary<int, int[]> subGraphs)
        {
            bool isGroup = false, isInterval = false;
            int delimiterCount = 0;

            string content = File.ReadAllText(fileName);

            // Replace all whitespaces
            content = Regex.Replace(content, @"\s+", "");

            // Determine the type of components based on brackets' type: groups{} or intervals[]
            if ((delimiterCount = content.Count(c => c == '[')) == content.Count(c => c == ']') &&
                0 != delimiterCount)
                isInterval = true;
            if ((delimiterCount = content.Count(c => c == '{')) == content.Count(c => c == '}') &&
                0 != delimiterCount)
                isGroup = true;

            // Should be or groups, or intervals (not both)
            if (isInterval == isGroup)
                return false;

            //All components should be separated by ';' symbol.
            string[] components = content.Split(';');
            for (int i = 0; i < components.Count(); ++i)
            {
                MatchCollection matchedItems = Regex.Matches(components[i],
                                            isInterval ? @"(?<=\[)(.*?)(?=\])" :
                                            @"(?<=\{)(.*?)(?=\})");
                if (1 != matchedItems.Count || !Regex.IsMatch(matchedItems[0].ToString(), @"^[0-9\,]+$"))
                    return false;

                String[] elements = matchedItems[0].ToString().Split(',');
                // For intervals the number of elements should be 2.
                if (isInterval && 2 != elements.Count())
                    return false;

                // All elements should be integers.
                for (int j = 0, intVal; j < elements.Count(); ++j)
                    if (!int.TryParse(elements[j], out intVal))
                        return false;

                if (isInterval)
                {
                    int val1, val2;
                    int.TryParse(elements[0], out val1);
                    int.TryParse(elements[1], out val2);
                    if (val1 >= val2)
                        return false;
                }
            }

            if (isInterval)
            {
                for (int i = 0; i < components.Count(); ++i)
                {
                    int[] interval = components[i].Replace("[", "").Replace("]", "").Split(',').Select(int.Parse).ToArray();
                    int[] group = new int[interval[1] - interval[0] + 1];
                    for (int j = interval[0], k = 0; j < interval[1]; ++j, k++)
                        group[k] = j;
                    subGraphs.Add(subGraphs.Count, group);
                }
            }
            else
            {
                for (int i = 0; i < components.Count(); ++i)
                    subGraphs.Add(subGraphs.Count, components[i].Replace("{", "").Replace("}", "").Split(',').Select(int.Parse).ToArray());
            }

            return true;
        }

        /// <summary>
        /// Writes matrix and branches (if exists) to specified file.
        /// </summary>
        /// <param name="matrixInfo">Matrix and branches (if exists).</param>
        /// <param name="filePath">File path.</param>
        public static void Write(MatrixInfoToWrite matrixInfo, String filePath)
        {
            String directoryPath = RandNetSettings.TracingDirectory;
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            WriteMatrix(matrixInfo.Matrix, filePath);
            if (matrixInfo.Branches != null)
                WriteBranches(matrixInfo.Branches, filePath);
            if (matrixInfo.ActiveStates != null)
                WriteActiveStates(matrixInfo.ActiveStates, filePath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="neighbourshipInfo"></param>
        /// <param name="filePath"></param>
        public static void Write(NeighbourshipInfoToWrite neighbourshipInfo, String filePath)
        {
            String directoryPath = RandNetSettings.TracingDirectory;
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            WriteNeighbourship(neighbourshipInfo.Neighbourship, filePath);
            if (neighbourshipInfo.Branches != null)
                WriteBranches(neighbourshipInfo.Branches, filePath);
            if (neighbourshipInfo.ActiveStates != null)
                WriteActiveStates(neighbourshipInfo.ActiveStates, filePath);
        }

        private static ArrayList ReadMatrix(String filePath)
        {
            ArrayList matrix = new ArrayList();
            using (StreamReader streamreader = new StreamReader(filePath, System.Text.Encoding.Default))
            {
                string[] saparators = { " " };
                string contents;
                while ((contents = streamreader.ReadLine()) != null)
                {
                    string[] split = contents.Split(saparators, StringSplitOptions.None);
                    ArrayList tmp = new ArrayList();
                    for (int i = 0; i < split.Length - 1; ++i)
                    {
                        if (split[i].Equals("0"))
                            tmp.Add(false);
                        else if (split[i].Equals("1"))
                            tmp.Add(true);
                        else throw new MatrixFormatException();
                    }

                    matrix.Add(tmp);
                }
            }
            return matrix;
        }

        private static List<int> ReadNeighbourship(String filePath)
        {
            List<int> neighbours = new List<int>();
            using (StreamReader streamreader = new StreamReader(filePath, System.Text.Encoding.Default))
            {
                string[] saparators = { " ", ",", ";" };
                string contents;
                while ((contents = streamreader.ReadLine()) != null)
                {
                    string[] split = contents.Split(saparators, StringSplitOptions.None);

                    try
                    {
                        int i;// Convert.ToInt32(split[0])
                        int j;// Convert.ToInt32(split[1]);

                        if(!int.TryParse(split[0], out i) || !int.TryParse(split[1], out j)) continue;

                        neighbours.Add(i);
                        neighbours.Add(j);
                    }
                    catch (SystemException)
                    {
                        throw new MatrixFormatException();
                    }
                }
            }
            if (neighbours.Count() % 2 != 0)
                throw new MatrixFormatException();

            return neighbours;
        }

        private static ArrayList ReadBranches(String filePath)
        {
            Debug.Assert(Path.GetExtension(filePath) == ".branches");
            if (File.Exists(filePath))
            {
                ArrayList branches = new ArrayList();
                try
                {
                    using (StreamReader streamreader =
                        new StreamReader(filePath, System.Text.Encoding.Default))
                    {
                        string contents;
                        while ((contents = streamreader.ReadLine()) != null)
                        {
                            string[] split = System.Text.RegularExpressions.Regex.Split(contents,
                                "\\s+", System.Text.RegularExpressions.RegexOptions.None);
                            ArrayList tmp = new ArrayList();
                            foreach (string s in split)
                            {
                                if (s != "")
                                    tmp.Add(UInt32.Parse(s));
                            }
                            branches.Add(tmp);
                        }
                    }
                }
                catch (SystemException)
                {
                    throw new BranchesFormatException();
                }

                return branches;
            }
            else return null;
        }

        private static BitArray ReadActiveStates(String filePath, int size)
        {            
            Debug.Assert(Path.GetExtension(filePath) == ".actives");
            if (File.Exists(filePath))
            {
                if (size == 0)
                    throw new ActiveStatesFormatException();
                BitArray activeStates = new BitArray(size);
                try
                {
                    using (StreamReader streamreader =
                        new StreamReader(filePath, System.Text.Encoding.Default))
                    {
                        string contents;
                        while ((contents = streamreader.ReadLine()) != null)
                        {
                            string[] split = System.Text.RegularExpressions.Regex.Split(contents,
                                "\\s+", System.Text.RegularExpressions.RegexOptions.None);
                            if (split.Count() != 1 && split.Count() != 2)
                                throw new ActiveStatesFormatException();
                            if (split.Count() == 1)
                            {
                                int i = int.Parse(split[0]);
                                if(i >= size)
                                    throw new ActiveStatesFormatException();
                                activeStates[i] = true;
                            } else
                            {
                                int i = int.Parse(split[0]);
                                int j = int.Parse(split[1]);
                                if (i >= size || j >= size || i >= j)
                                    throw new ActiveStatesFormatException();
                                for (int l = i; l <= j; ++l)
                                    activeStates[l] = true;
                            }
                        }
                    }
                }
                catch (SystemException)
                {
                    throw new ActiveStatesFormatException();
                }

                return activeStates;
            }
            else return null;
        }

        private static void WriteMatrix(bool[,] matrix, String filePath)
        {
            using (StreamWriter file = new StreamWriter(filePath + ".txt"))
            {
                for (int i = 0; i < matrix.GetLength(0); ++i)
                {
                    for (int j = 0; j < matrix.GetLength(1); ++j)
                    {
                        if (matrix[i, j])
                        {
                            file.Write(1 + " ");
                        }
                        else
                        {
                            file.Write(0 + " ");
                        }
                    }
                    file.WriteLine("");
                }
            }
        }

        private static void WriteNeighbourship(List<KeyValuePair<int, int>> neighbourship, String filePath)
        {
            using (StreamWriter file = new StreamWriter(filePath + ".txt"))
            {
                foreach (KeyValuePair<int, int> p in neighbourship)
                    file.WriteLine(p.Key.ToString() + " " + p.Value.ToString());
            }
        }

        private static void WriteBranches(UInt32[][] branches, String filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath + ".branches"))
            {
                for (int i = 0; i < branches.Length; i++)
                {
                    for (int k = 0; k < branches[i].Length; k++)
                    {
                        writer.Write(branches[i][k]);
                        writer.Write(" ");
                    }
                    writer.WriteLine();
                }
            }
        }

        private static void WriteActiveStates(BitArray activeStates, String filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath + ".actives"))
            {
                for(int i = 0; i < activeStates.Length - 1; ++i)
                {
                    if (!activeStates[i])
                    {
                        if (i == activeStates.Length - 2 && activeStates[activeStates.Length - 1])
                        {
                            writer.Write(i + 1);
                        }
                        continue;
                    }
                    if (activeStates[i] && !activeStates[i + 1])
                    {
                        writer.WriteLine(i);
                        ++i;
                        continue;
                    }
                    int j = i + 1;
                    while (j < activeStates.Length && activeStates[j])
                        ++j;
                    writer.Write(i);
                    writer.Write(" ");
                    writer.WriteLine(j - 1);
                    i = j;            
                }
                if (!activeStates[activeStates.Length - 1])
                { }
            }
        }
    }
}