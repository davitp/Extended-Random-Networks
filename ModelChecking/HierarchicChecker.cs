using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ModelChecking
{
    public enum PossibleAnswers
    {
        GraphicalNotBlockHierarchic,
        GraphicalBlockHierarchic,
        NotGraphical,
        MaybeNotBlockHierarchic
    }


    public class HierarchicChecker
    {
        private List<int> d;
        private int p;
        private int t;
        private string path = "";

        public HierarchicChecker(List<int> degrees)
        {
            d = degrees;
        }

        public HierarchicChecker(string filePath)
        {
            path = filePath;
        }

        public PossibleAnswers IsHierarchic()
        {
            if (path != "")
                d = FromMatrixToDegrees();
            CalcParams();
            List<List<int>> p = Polynom();
            PossibleAnswers answer = Process(p);
            if (answer == PossibleAnswers.GraphicalBlockHierarchic)
                MatrixToFile(BlockHierarchicMatrix(p));
            return answer;
        }

        public List<int> FromMatrixToDegrees() // matricic stanum e hajordakanutyun@
        {
            int sum = 0;
            List<string> lines = new List<string>();

            List<int> deg = new List<int>();

            using (TextReader r = new StreamReader(path))
            {

                string line;
                while ((line = r.ReadLine()) != null)
                {

                    lines.Add(line);
                }
            }


            int l = 2 * lines.Count;
            char[] lines1 = new char[l];
            int[] lines2 = new int[l];
            try
            {

                for (int i = 0; i < lines.Count; i++)
                {


                    lines1 = lines[i].ToCharArray();
                    if (lines1.Last() != ' ')
                        if (lines1.Length != l - 1)
                            throw new IndexOutOfRangeException("Type a squere matrix");
                    for (int j = 0; j < l - 1; j++)
                    {

                        if (lines1[j] != ' ')
                        {
                            if (lines1[j] == '0' || lines1[j] == '1')
                            {
                                lines2[j] = (int)(lines1[j] - '0');

                                sum += lines2[j];
                            }
                            else
                            {
                                lines1[j] = '1';
                                lines2[j] = (int)(lines1[j] - '0');

                                sum += lines2[j];
                            }
                        }

                    }

                    deg.Add(sum);
                    sum = 0;


                }


            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Type a squere matrix");
                throw new IndexOutOfRangeException();
            }


            return deg;
        }
        
        // Utilities //

        private void CalcParams()
        {
            int k;

            for (p = 2; ; p++)
                for (t = 1; ; t++)
                {
                    k = (int)Math.Pow(p, t);

                    if (k == d.Count)
                        return;

                    if (k > d.Count)
                        break;
                }

        }

        private List<List<int>> Polynom()
        {
            List<List<int>> k = new List<List<int>>();
            List<int> copy_d = new List<int>();
            copy_d = d;
            for (int i = 0; i < d.Count; i++)
            {
                k.Add(new List<int>(d.Count));
                int degree = copy_d[i];
                for (int j = 0; j < t; j++)
                {
                    k[i].Insert(j, degree % p);
                    degree /= p;
                }


            }
            k.Sort(Comparing);

            return k;

        }

        private void MatrixToFile(List<List<int>> Matrix)
        {
            using (TextWriter tw = new StreamWriter("C:\\ComplexNetwork\\output.txt"))
            {
                for (int j = 0; j < Matrix.Count; j++)
                {
                    for (int i = 0; i < Matrix.Count; i++)
                    {
                        tw.Write(Matrix[i][j] + " ");
                    }
                    tw.WriteLine();
                }
            }
        }


        private static int Comparing(List<int> k1, List<int> k2)
        {
            if (k1.Count != k2.Count)
                throw new SystemException();

            for (int i = k1.Count - 1; i >= 0; i--)
                if (k1[i] != k2[i])
                    return k1[i] - k2[i];

            return 0;
        }

        private static List<int> HavelHakimi(List<int> d)
        {
            List<int> result = new List<int>();
            for (int i = 0; i < d[0]; i++)
                result.Add(d[i + 1] - 1);
            for (int i = d[0] + 1; i < d.Count; i++)
                result.Add(d[i]);
            return result;

        }

        private static List<List<int>> HavelHakimiMatrix(List<int> d)
        {
            d.Sort();
            d.Reverse();
            List<List<int>> matrix = new List<List<int>>();
            for (int i = 0; i < d.Count; i++)
            {
                matrix.Add(new List<int>(d.Count));
                for (int j = 0; j < d.Count; j++)
                    matrix[i].Add(0);
            }
            for (int v = 0; v < d.Count; v++)
            {
                for (int j = v; j < v + 1; j++)
                    for (int i = v; i < d[v]; i++)


                        matrix[j][i + 1] = 1;

                for (int i = d[v] + 1; i < d.Count; i++)
                    for (int j = d[v] + 1; j < d.Count; j++)

                        matrix[i][j] = 0;

            }
            for (int i = 0; i < d.Count; i++)
                for (int j = 0; j < d.Count; j++)
                    matrix[j][i] = matrix[i][j];
            return matrix;

        }

        private static bool HavelHakimiCheck(List<int> d)
        {
            List<int> f = new List<int>();
            List<int> copy_of_d = new List<int>();
            copy_of_d = d;
            for (int i = 0; i < d.Count; i++)
                if (d[i] >= d.Count || d[i] < 0)
                    return false;



            for (int i = 0; i < copy_of_d.Count; i++)
            {
                d.Sort();
                d.Reverse();
                f = HavelHakimi(d);
                d = f;


            }
            for (int i = 0; i < d.Count; i++)
                if (d[i] != 0)
                    return false;
            return true;
        }

        private static bool IsGraphicSeq(List<int> d) // Erdos-Gallay-i teoremn a stugum
        {
            int S;

            S = d.Sum();
            if (S % 2 != 0)
                return false;
            else
            {
                d.Sort();
                d.Reverse();
                List<int> sort_d = new List<int>(d);
                for (int k = 1; k < d.Count; k++)
                {
                    int temp_sum = 0;
                    int sort_S = 0;
                    for (int i = k; i < d.Count; i++)
                    {
                        temp_sum = temp_sum + (int)Math.Min(sort_d[i], k);

                    }
                    for (int j = 1; j <= k; j++)
                        sort_S += sort_d[j];
                    if (sort_S > k * (k - 1) + temp_sum)
                        return false;
                }
                return true;
            }
        }

        
        public List<List<int>> BlockHierarchicMatrix(List<List<int>> k)
        {
            List<List<int>> Matrix = new List<List<int>>();


            for (int i = 0; i < d.Count; i++)
            {
                Matrix.Add(new List<int>(d.Count));
                for (int j = 0; j < d.Count; j++)
                    Matrix[i].Add(0);
            }

            List<List<int>> matrix = new List<List<int>>();
            List<List<int>> helpmatrix = new List<List<int>>();


            List<int> subk = new List<int>();

            List<int> subk1 = new List<int>();

            for (int l = 1; l <= t; l++) // makardakner@
            {
                int localLev = (int)Math.Pow(p, t - l);
                int levelElemCount = (int)Math.Pow(p, l);
                int prevLevelElemCount = (int)Math.Pow(p, l - 1);


                for (int r = 0; r < prevLevelElemCount; r++)
                {
                    helpmatrix.Add(new List<int>(prevLevelElemCount));
                    for (int q = 0; q < prevLevelElemCount; q++)
                        helpmatrix[r].Add(0);
                }



                for (int h = 1; h <= localLev; h++) // qani hat p-yak ka amen makardakum
                {



                    for (int j = (h - 1) * levelElemCount; j < h * levelElemCount; j += prevLevelElemCount)
                        subk.Add(k[j][l - 1]);
                    matrix.Add(new List<int>(subk.Count));

                    matrix = HavelHakimiMatrix(subk);

                    if (l == 1)
                        for (int i = (h - 1) * levelElemCount; i < h * levelElemCount; )
                            for (int v = 0; v < subk.Count; v++)
                            {
                                for (int j = (h - 1) * levelElemCount; j < h * levelElemCount; )

                                    for (int w = 0; w < subk.Count; w++)
                                    {
                                        Matrix[i][j] = matrix[v][w];

                                        j++;
                                    }
                                i++;
                            }
                    else

                        for (int v = 0; v < subk.Count - 1; v++)
                        {
                            for (int w = 0; w < subk.Count - 1; w++)
                            {
                                for (int x = 0; x < prevLevelElemCount; x++)
                                    for (int y = 0; y < prevLevelElemCount; y++)
                                        helpmatrix[x][y] = matrix[v][w + 1];

                                for (int i = (h - 1) * levelElemCount + v * prevLevelElemCount; i < h * levelElemCount - (p - v - 1) * prevLevelElemCount; )
                                    for (int c = 0; c < prevLevelElemCount; c++)
                                    {
                                        for (int j = (h - 1) * levelElemCount + (w + 1) * prevLevelElemCount + v * prevLevelElemCount; j < h * levelElemCount - (p - 1 - (w + 1)) * prevLevelElemCount; )
                                            for (int s = 0; s < prevLevelElemCount; s++)
                                            {
                                                Matrix[i][j] = helpmatrix[c][s];
                                                j++;
                                            }
                                        i++;
                                    }
                            }


                        }





                    subk.Clear();

                }
                helpmatrix.Clear();
            }


            for (int i = 0; i < d.Count; i++)
                for (int j = 0; j < d.Count; j++)
                    Matrix[j][i] = Matrix[i][j];

            return Matrix;
        }


        public PossibleAnswers Process(List<List<int>> k)
        {
            if (!HavelHakimiCheck(d))
                return PossibleAnswers.NotGraphical;

            List<int> subk = new List<int>();

            List<int> subk1 = new List<int>();

            for (int l = 1; l <= t; l++) // makardakner@
            {
                int localLev = (int)Math.Pow(p, t - l);
                int levelElemCount = (int)Math.Pow(p, l);
                int prevLevelElemCount = (int)Math.Pow(p, l - 1);

                if (p == d.Count)
                    return PossibleAnswers.GraphicalNotBlockHierarchic;
                for (int h = 1; h <= localLev; h++) // qani hat p-yak ka amen makardakum
                {
                    for (int i = l; i < t; i++)
                    {

                        for (int j = (h - 1) * levelElemCount; j + prevLevelElemCount < h * levelElemCount; j += prevLevelElemCount)
                        {
                            if (k[j][i] != k[j + prevLevelElemCount][i])
                            {

                                return PossibleAnswers.GraphicalNotBlockHierarchic;
                            }

                        }

                        subk1.Add(k[(h - 1) * levelElemCount][i]);
                    }


                    for (int j = (h - 1) * levelElemCount; j < h * levelElemCount; j += prevLevelElemCount)
                        subk.Add(k[j][l - 1]);

                    if (HavelHakimiCheck(subk))
                    {
                        subk1.Clear();
                        subk.Clear();
                    }
                    else
                    {


                        bool chk = true;
                        if (h > 1)
                        {


                            for (int v = (h - 1) * levelElemCount - levelElemCount; v < (h - 1) * levelElemCount; v++)
                                for (int i = l; i < t; i++)
                                    if (subk1[i - 1] != k[v][i])
                                        chk = false;

                        }
                        if (h < localLev)
                        {

                            for (int v = (h - 1) * levelElemCount + levelElemCount; v < h * levelElemCount + levelElemCount; v++)
                                for (int i = l; i < t; i++)
                                    if (subk1[i - 1] != k[v][i])
                                        chk = false;

                        }
                        if (chk == false)
                            return PossibleAnswers.GraphicalNotBlockHierarchic;
                        if (localLev == 1 && chk == true)
                            return PossibleAnswers.GraphicalNotBlockHierarchic;
                        return PossibleAnswers.MaybeNotBlockHierarchic;
                    }
                }
            }

            return PossibleAnswers.GraphicalBlockHierarchic;
        }
    }

}
