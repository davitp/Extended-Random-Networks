using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using Core.Enumerations;
using Core.Exceptions;
using Core.Model;
using NetworkModel.HierarchicEngine;
using ModelChecking;

namespace RegularHierarchicModel
{
    /// <summary>
    /// Implementation of regularly branching block-hierarchic network's container.
    /// </summary>
    class RegularHierarchicNetworkContainer : AbstractHierarchicContainer
    {
        private const int ARRAY_MAX_SIZE = 2000000000;

        private UInt32 size = 0;
        private UInt32 branchingIndex = 0;
        private UInt32 level = 0;        
        private BitArray[][] hierarchicTree;

        public RegularHierarchicNetworkContainer(ContainerMode containerMode) : base(containerMode)
        {
            hierarchicTree = new BitArray[0][];
        }

        public override UInt32 Size 
        {
            get { return (UInt32)Math.Pow(branchingIndex, level); }
            set
            {
                throw new NotImplementedException();
            }
        }

        public UInt32 BranchingIndex
        {
            get { return branchingIndex; }
            set { branchingIndex = value; }
        }

        public UInt32 Level
        {
            get { return level; }
            set { level = value; }
        }

        public BitArray[][] HierarchicTree
        {
            get { return hierarchicTree; }
            set { hierarchicTree = value; }
        }

        public override void SetMatrix(ArrayList matrix)
        {
            List<List<bool>> matrixInList = new List<List<bool>>();
            ArrayList arr;
            for (int i = 0; i < matrix.Count; ++i)
            {
                arr = (ArrayList)matrix[i];
                matrixInList.Add(new List<bool>());
                for(int j = 0; j < arr.Count; ++j)
                    matrixInList[i].Add((bool)arr[j]);
            }

            HierarchicExactChecker checker = new HierarchicExactChecker();
            if (!checker.IsHierarchic(matrixInList))
            {
                throw new MatrixFormatException();
            }
            else
            {
                size = (UInt32)matrix.Count;
                branchingIndex = (UInt32)checker.BranchIndex;
                level = (UInt32)checker.Level;
                hierarchicTree = new BitArray[level][];

                // Initializing and filling data for each level, beginning from root.
                uint nodeDataLength = branchingIndex * (branchingIndex - 1) / 2;
                int[] nIndexes = new int[nodeDataLength];
                int[] mIndexes = new int[nodeDataLength];
                for (int gamma = (int)level; gamma > 0; --gamma)
                {
                    // get current level data length and bitArrays count
                    long levelDataLength = Convert.ToInt64(Math.Pow(branchingIndex, gamma - 1) * nodeDataLength);
                    int arrayCountForLevel = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(levelDataLength) / ARRAY_MAX_SIZE));

                    hierarchicTree[gamma - 1] = new BitArray[arrayCountForLevel];
                    int j;
                    for (j = 0; j < arrayCountForLevel - 1; j++)
                    {
                        hierarchicTree[gamma - 1][j] = new BitArray(ARRAY_MAX_SIZE);
                    }
                    hierarchicTree[gamma - 1][j] = new BitArray(Convert.ToInt32(levelDataLength - (arrayCountForLevel - 1) * ARRAY_MAX_SIZE));

                    // fills data for current level nodes
                    // loop over all elements of given level and fill him values
                    uint lim1 = branchingIndex - 1, lim2 = 0, nt = 0;
                    nIndexes[0] = 0;
                    for (uint nIndex = 1; nIndex < branchingIndex; ++nIndex)
                    {
                        while (nt < lim1)
                        {
                            if (nIndex == 1)
                                nIndexes[nt] = nIndexes[lim2];
                            else
                                nIndexes[nt] = nIndexes[lim2] +
                                    Convert.ToInt32(Math.Pow(branchingIndex, level - gamma));
                            ++nt;
                        }
                        lim2 = lim1 - 1;
                        lim1 = lim1 + branchingIndex - 1 - nIndex;
                    }

                    mIndexes[0] = Convert.ToInt32(Math.Pow(branchingIndex, level - gamma));
                    int mt = 0;
                    int fIndex = 1;
                    while (mt < nodeDataLength)
                    {
                        for (int i = fIndex; i <= branchingIndex - 1; ++i)
                        {
                            mIndexes[mt] = i * mIndexes[0];
                            ++mt;
                        }
                        ++fIndex;
                    }

                    for (int f = 0; f < hierarchicTree[gamma - 1].Length; f++)
                    {
                        for (int g = 0; g < hierarchicTree[gamma - 1][f].Length; g++)
                        {
                            int currentIndex = g % (int)nodeDataLength;
                            int add = Convert.ToInt32((g / nodeDataLength)) *
                                Convert.ToInt32(Math.Pow(branchingIndex, level - gamma + 1));
                            hierarchicTree[gamma - 1][f][g] =
                                matrixInList[nIndexes[currentIndex] + add][mIndexes[currentIndex] + add];
                        }
                    }
                }
            }
        }

        public override bool[,] GetMatrix()
        {
            bool[,] matrix = new bool[Size, Size];

            for (int i = 0; i < Size; ++i)
            {
                for (int j = 0; j < Size; ++j)
                    matrix[i, j] = (this[i, j] == 1) ? true : false;
            }

            return matrix;
        }

        public override List<KeyValuePair<int, int>> GetNeighbourship()
        {
            List<KeyValuePair<int, int>> n = new List<KeyValuePair<int, int>>();   
            for (int i = 0; i < Size; ++i)
            {
                for (int j = i + 1; j < Size; ++j)
                    if(this[i, j] == 1)
                        n.Add(new KeyValuePair<int, int>(i, j));
            }
            return n;
        }

        public override UInt32[][] GetBranches()
        {
            UInt32[][] branches = new UInt32[Level][];
            for (int i = 0; i < Level; ++i)
            { 
                int levelVertexCount = Convert.ToInt32(Math.Pow(branchingIndex, i));
                branches[i] = new UInt32[levelVertexCount];
                for (int j = 0; j < levelVertexCount; ++j)
                {
                    branches[i][j] = branchingIndex;
                }
            }

            return branches;
        }

        /// <summary>
        /// Retrieves mark of specified node of block-hierarchical tree.
        /// </summary>
        /// <param name="currentLevel">Index of level of specified node.</param>
        /// <note>currentLevel must be in [0, level - 1] range.</note>
        /// <param name="currentNodeNumber">Index of specified node in currentLevel.</param>
        /// <note>currentNodeNumber must be in [0, pow(branchingIndex, currentLevel) - 1] range.</note>
        /// <returns>Sequence of {0, 1} values. Length is branchingIndex*(branchingIndex-1)/2 </returns>
        public BitArray TreeNode(int currentLevel, long currentNodeNumber)
        {
            if (currentLevel < 0 || currentLevel >= level)
                throw new SystemException("Wrong parameter - currentLevel.");
            if (currentNodeNumber < 0 || currentNodeNumber >= Math.Pow(branchingIndex, currentLevel))
                throw new SystemException("Wrong parameter - currentNodeNumber.");

            int resultSize = (int)(branchingIndex * (branchingIndex - 1) / 2);
            BitArray result = new BitArray(resultSize);

            long i = currentNodeNumber * resultSize;
            int ind = Convert.ToInt32(Math.Floor(Convert.ToDouble(i / ARRAY_MAX_SIZE)));
            int rangeSt = Convert.ToInt32(i - ind * ARRAY_MAX_SIZE);
            int rangeEnd = rangeSt + resultSize;
            int secArray = 0;

            if (rangeEnd > ARRAY_MAX_SIZE)
            {
                secArray = rangeEnd - ARRAY_MAX_SIZE;
                rangeEnd = ARRAY_MAX_SIZE - 1;
            }

            int counter = 0;
            for (int j = rangeSt; j < rangeEnd; j++)
            {
                result[counter] = hierarchicTree[currentLevel][ind][j];
                counter++;
            }

            for (int j = 0; j < secArray; j++)
            {
                result[counter] = hierarchicTree[currentLevel][ind + 1][j];
                counter++;
            }

            return result;
        }

        /// <summary>
        /// Retrieves the adjacency matrix for specified node of block-hierarchical tree.
        /// </summary>
        /// <param name="currentLevel">Index of level of specified node.</param>
        /// <note>currentLevel must be in [0, level - 1] range.</note>
        /// <param name="currentNodeNumber">Index of specified node in currentLevel.</param>
        /// <note>currentNodeNumber must be in [0, pow(branchingIndex, currentLevel) - 1] range.</note>
        /// <returns>Matrix of {0, 1} values. Size is branchingIndex X branchingIndex. </returns>
        public int[,] NodeMatrix(int currentLevel, long currentNodeNumber)
        {
            if (currentLevel < 0 || currentLevel >= level)
                throw new SystemException("Wrong parameter - currentLevel.");
            if (currentNodeNumber < 0 || currentNodeNumber >= Math.Pow(branchingIndex, currentLevel))
                throw new SystemException("Wrong parameter - currentNodeNumber.");

            int[,] result = new int[branchingIndex, branchingIndex];
            long i = currentNodeNumber * branchingIndex * (branchingIndex - 1) / 2;
            int ind = Convert.ToInt32(Math.Floor(Convert.ToDouble(i / ARRAY_MAX_SIZE)));
            int rangeSt = Convert.ToInt32(i - ind * ARRAY_MAX_SIZE);
            int rangeEnd = rangeSt + (int)(branchingIndex * (branchingIndex - 1) / 2);
            int secArray = 0;

            if (rangeEnd > ARRAY_MAX_SIZE)
            {
                secArray = rangeEnd - ARRAY_MAX_SIZE;
                rangeEnd = ARRAY_MAX_SIZE - 1;
            }
            int counterX = 1;
            int counterY = 0;
            for (int j = rangeSt; j < rangeEnd; j++)
            {
                result[counterX, counterY] = (hierarchicTree[currentLevel][ind][j] ? 1 : 0);
                result[counterY, counterX] = (hierarchicTree[currentLevel][ind][j] ? 1 : 0);
                ++counterX;
                if (counterX == branchingIndex)
                {
                    counterX = counterY + 2;
                    ++counterY;
                }
            }

            for (int j = 0; j < secArray; j++)
            {
                result[counterX, counterY] = (hierarchicTree[currentLevel][ind + 1][j] ? 1 : 0);
                result[counterY, counterX] = (hierarchicTree[currentLevel][ind + 1][j] ? 1 : 0);
                ++counterX;
                if (counterX == branchingIndex)
                {
                    counterX = 0;
                    ++counterY;
                }
            }

            return result;
        }

        /// <summary>
        /// Retrieves adjacency lists for specified node's mark (bit sequence).
        /// </summary>
        /// <param name="nodeInformation">The mark of node (bit sequence).</param>
        /// <returns>Adjacency lists.</returns>
        public Dictionary<int, ArrayList> NodeAdjacencyLists(BitArray nodeInformation)
        {
            Dictionary<int, ArrayList> matrixList = new Dictionary<int, ArrayList>();
            for (int i = 0; i < branchingIndex; i++)
                matrixList.Add(i, new ArrayList());

            for (int i = 0; i < branchingIndex - 1; i++)
            {
                int s = i + 1;
                for (int j = (int)(i * (branchingIndex - (i - 1) - 1) + i * (i - 1) / 2); 
                    j < (i + 1) * (branchingIndex - i - 1) + i * (i + 1) / 2; 
                    j++)
                {
                    if (nodeInformation[j] == true)
                    {
                        matrixList[i].Add(s);
                        matrixList[s].Add(i);
                    }
                    s++;
                }
            }

            return matrixList;
        }

        /// <summary>
        /// Calculates number of connected nodes (clusters) with specified node.
        /// </summary>
        /// <param name="currentLevel">Index of level of specified node.</param>
        /// <note>currentLevel must be in [0, level - 1] range.</note>
        /// <param name="currentNodeNumber">Index of specified node in currentLevel.</param>
        /// <note>currentNodeNumber must be in [0, pow(branchingIndex, currentLevel) - 1] range.</note>
        /// <param name="childNodeNumber">Index of child node.</param>
        /// <note>childNodeNumber must be in [0, pow(branchingIndex, currentLevel + 1) - 1] range.</note>
        /// <returns>Number of connected nodes with specified child node.</returns>
        public int NodeChildAdjacentsCount(int currentLevel, 
            long currentNodeNumber, 
            int childNodeNumber)
        {
            if (currentLevel < 0 || currentLevel >= level)
                throw new SystemException("Wrong parameter - currentLevel.");
            if (currentNodeNumber < 0 || currentNodeNumber >= Math.Pow(branchingIndex, currentLevel))
                throw new SystemException("Wrong parameter - currentNodeNumber.");
            if (childNodeNumber < 0 || childNodeNumber >= Math.Pow(branchingIndex, currentLevel + 1))
                throw new SystemException("Wrong parameter - childNodeNumber.");

            BitArray tempNode = TreeNode(currentLevel, currentNodeNumber);
            int tempCount = 0, j = 0;

            //adds values until current child part 
            for (int i = 1; i <= childNodeNumber; i++)
            {
                tempCount += (tempNode[j + childNodeNumber - i] ? 1 : 0);
                j += (int)branchingIndex - i;
            }

            //adds child part values
            int curChildEnd = j + (int)branchingIndex - childNodeNumber - 1;
            while (j < curChildEnd)
            {
                tempCount += (tempNode[j] ? 1 : 0);
                j++;
            }

            return tempCount;
        }

        /// <summary>
        /// Retrieves indices of connected nodes (clusters) with specified node.
        /// </summary>
        /// <param name="currentLevel">Index of level of specified node.</param>
        /// <note>currentLevel must be in [0, level - 1] range.</note>
        /// <param name="currentNodeNumber">Index of specified node in currentLevel.</param>
        /// <note>currentNodeNumber must be in [0, pow(branchingIndex, currentLevel) - 1] range.</note>
        /// <param name="childNodeNumber">Index of child node.</param>
        /// <note>childNodeNumber must be in [0, pow(branchingIndex, currentLevel + 1) - 1] range.</note>
        /// <returns>List of indices.</returns>
        public List<int> NodeChildAdjacentsArray(int currentLevel,
            long currentNodeNumber,
            int childNodeNumber)
        {
            if (currentLevel < 0 || currentLevel >= level)
                throw new SystemException("Wrong parameter - currentLevel.");
            if (currentNodeNumber < 0 || currentNodeNumber >= Math.Pow(branchingIndex, currentLevel))
                throw new SystemException("Wrong parameter - currentNodeNumber.");
            if (childNodeNumber < 0 || childNodeNumber >= Math.Pow(branchingIndex, currentLevel + 1))
                throw new SystemException("Wrong parameter - childNodeNumber.");

            BitArray tempNode = TreeNode(currentLevel, currentNodeNumber);
            List<int> tempIndexes = new List<int>();
            int j = 0;
            //adds values until current child part 
            for (int i = 1; i <= childNodeNumber; i++)
            {
                if (tempNode[j + childNodeNumber - i])
                {
                    tempIndexes.Add(i - 1);
                }
                j += (int)branchingIndex - i;
            }

            //adds child part values
            int curChildSt = j;
            int curChildEnd = j + (int)branchingIndex - childNodeNumber - 1;
            while (j < curChildEnd)
            {
                if (tempNode[j])
                {
                    tempIndexes.Add(j - curChildSt + childNodeNumber + 1);
                }
                j++;
            }

            return tempIndexes;
        }

        /// <summary>
        /// Defines if specified vertices are connected in specified cluster.
        /// </summary>
        /// <param name="currentLevel">Index of level of specified node.</param>
        /// <note>currentLevel must be in [0, level - 1] range.</note>
        /// <param name="currentNodeNumber">Index of specified node in currentLevel.</param>
        /// <note>currentNodeNumber must be in [0, pow(branchingIndex, currentLevel) - 1] range.</note>
        /// <param name="v1">Index of first vertex.</param>
        /// <param name="v2">Index of second vertex.</param>
        /// <note>Indices of vertices must be in [0, branchingIndex - 1] range.</note>
        /// <returns>1, if specified vertices are connected. 0 otherwise.</returns>
        public int AreAdjacent(int currentLevel,
            long currentNodeNumber, 
            int v1, 
            int v2)
        {
            if (currentLevel < 0 || currentLevel >= level)
                throw new SystemException("Wrong parameter - currentLevel.");
            if (currentNodeNumber < 0 || currentNodeNumber >= Math.Pow(branchingIndex, currentLevel))
                throw new SystemException("Wrong parameter - currentNodeNumber.");
            if ((v1 < 0 || v1 > branchingIndex - 1) || (v2 < 0 || v2 > branchingIndex - 1))
                throw new SystemException("Wrong parameter - v1 or v2.");

            BitArray tempNode = TreeNode(currentLevel, currentNodeNumber);
            if (v1 > v2)
            {
                int temp = v2;
                v2 = v1;
                v1 = temp;
            }

            var i = 0;
            var ind = 0;
            while (i < v1)
            {
                ind += (int)branchingIndex - i - 1;
                i++;
            }

            if (tempNode[ind + v2 - v1 - 1])
            {
                return 1;
            }

            return 0;
        }
        
        /// <summary>
        /// Gets the degree of specified node in specified level.
        /// </summary>
        /// <param name="currentVertexNumber">Index of specified node.</param>
        /// <note>currentVertexNumber must be in [0, pow(branchingIndex, level)] range.</note>
        /// <param name="currentLevel">Index of level of specified node.</param>
        /// <note>currentLevel must be in [0, level] range.</note>
        /// <returns>The degree of node.</returns>
        public double VertexDegree(int currentVertexNumber, int currentLevel)
        {
            // TODO check what is wrong
            /*if (currentLevel < 0 || currentLevel > level)
                throw new SystemException("Wrong parameter - currentLevel.");
            if (currentVertexNumber < 0 || currentVertexNumber >= Math.Pow(branchingIndex, level))
                throw new SystemException("Wrong parameter - currentNodeNumber.");*/

            double result = 0;
            int vertexIndex = 0, nodeNumber = 0;
            for (int i = (int)level - 1; i >= currentLevel; --i)
            {
                vertexIndex = TreeIndex(currentVertexNumber, i + 1) % (int)branchingIndex;
                nodeNumber = TreeIndex(currentVertexNumber, i);
                result += Links(vertexIndex, i, nodeNumber) * Math.Pow(branchingIndex, level - i - 1);
            }

            return result;
        }

        /// <summary>
        /// Recoursively calculates number of edges in specified cluster.
        /// </summary>
        /// <param name="currentLevel">Index of level of specified node.</param>
        /// <note>currentLevel must be in [0, level] range.</note>
        /// <param name="currentNodeNumber">Index of specified node in currentLevel.</param>
        /// <note>currentNodeNumber must be in [0, pow(branchingIndex, currentLevel) - 1] range.</note>
        /// <returns>Number of edges.</returns>
        public double CalculateNumberOfEdges(int currentLevel, long currentNodeNumber)
        {
            if (currentLevel < 0 || currentLevel > level)
                throw new SystemException("Wrong parameter - currentLevel.");
            if (currentNodeNumber < 0 || currentNodeNumber >= Math.Pow(branchingIndex, currentLevel))
                throw new SystemException("Wrong parameter - currentNodeNumber.");

            if (currentLevel == level)
            {
                return 0;
            }
            else
            {
                double result = 0;
                double res = 0;
                BitArray node = TreeNode(currentLevel, currentNodeNumber);

                for (int i = 0; i < (branchingIndex * (branchingIndex - 1) / 2); i++)
                {
                    res += (node[i]) ? 1 : 0;
                }
                double t = Math.Pow(branchingIndex, level - currentLevel - 1);
                result = res * t * t;

                for (long i = currentNodeNumber * branchingIndex; i < branchingIndex * (currentNodeNumber + 1); ++i)
                {
                    result += CalculateNumberOfEdges(currentLevel + 1, i);
                }

                return result;
            }
        }

        // TODO test this method
        /// <summary>
        /// Recoursively calculates number of 2-length paths in specified cluster.
        /// </summary>
        /// <param name="currentLevel">Index of level of specified node.</param>
        /// <note>currentLevel must be in [0, level] range.</note>
        /// <param name="currentNodeNumber">Index of specified node in currentLevel.</param>
        /// <note>currentNodeNumber must be in [0, pow(branchingIndex, currentLevel) - 1] range.</note>
        /// <returns>Number of edges.</returns>
        public double CalculateNumberOf2LengthPaths(int currentLevel, int currentNodeNumber)
        {
            if (currentLevel < 0 || currentLevel > level)
                throw new SystemException("Wrong parameter - currentLevel.");
            if (currentNodeNumber < 0 || currentNodeNumber >= Math.Pow(branchingIndex, currentLevel))
                throw new SystemException("Wrong parameter - currentNodeNumber.");

            double result = 0;
            if (currentLevel == level)
                return result;
            else
            {
                BitArray node = TreeNode(currentLevel, currentNodeNumber);
                double powPK = Math.Pow(branchingIndex, level - currentLevel - 1);

                int ci = (int)(currentNodeNumber * branchingIndex);
                for (int i = ci; i < (currentNodeNumber + 1) * branchingIndex; ++i)
                {
                    result += CalculateNumberOf2LengthPaths(currentLevel + 1, i);

                    for (int j = i + 1; j < (currentNodeNumber + 1) * branchingIndex; ++j)
                    {
                        if (AreConnectedTwoBlocks(node, i - ci, j - ci))
                        {
                            result += (CalculateNumberOfEdges(currentLevel + 1, i) + 
                                CalculateNumberOfEdges(currentLevel + 1, j)) * powPK * powPK * powPK;

                            for (int k = j + 1; k < (currentNodeNumber + 1) * branchingIndex; ++k)
                            {
                                if (AreConnectedTwoBlocks(node, j - ci, k - ci))
                                {
                                    result += powPK * powPK * powPK;
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Calculates number of edges in whole network.
        /// </summary>
        /// <returns>Number of edges.</returns>
        public double CalculateNumberOfEdges()
        {
            int countOne = 0;
            double count = 0;
            for (int i = 0; i < hierarchicTree.Length; i++)
            {
                for (int j = 0; j < hierarchicTree[i].Length; j++)
                    for (int h = 0; h < hierarchicTree[i][j].Length; h++)
                    {
                        countOne += (hierarchicTree[i][j][h]) ? 1 : 0;
                    }
                double t = Math.Pow(branchingIndex, level - i - 1);
                count += countOne * t * t;
                countOne = 0;
            }

            return count;
        }

        /// <summary>
        /// Defines if specified subtrees are connected in specified nodes's mark.
        /// </summary>
        /// <param name="nodeInformation">The mark of node (bit sequence).</param>
        /// <param name="n1">Index of first subtree.</param>
        /// <param name="n2">Index of second subtree.</param>
        /// <note>Indices of subtrees must be in [0, branchingIndex - 1] range.</note>
        /// <returns>true, if specifed subtrees are connected. false otherwise.</returns>
        public bool AreConnectedTwoBlocks(BitArray nodeInformation, int n1, int n2)
        {
            if((n1 < 0 || n1 > branchingIndex - 1) || (n2 < 0 || n2 > branchingIndex - 1))
                throw new SystemException("Wrong parameter - n1 or n2.");

            if (n1 == n2)
            {
                return false;
            }

            if (n1 > n2)
            {
                int k = n2;
                n2 = n1;
                n1 = k;
            }

            // Get the index of two numberes adjacent value
            int index = 0;
            for (int k = 1; k <= n1; k++)
            {
                index += (int)branchingIndex - k;
            }
            index += n2 - n1 - 1;
            return nodeInformation[index] ? true : false;
        }

        /// <summary>
        /// Calculates the number of connections between subtrees for specified node.
        /// </summary>
        /// <param name="currentLevel">Index of level of specified node.</param>
        /// <note>currentLevel must be in [0, level] range.</note>
        /// <param name="currentNodeNumber">Index of specified node in currentLevel.</param>
        /// <note>currentNodeNumber must be in [0, pow(branchingIndex, currentLevel) - 1] range.</note>
        /// <returns>Number of connections.</returns>
        public int Links(int currentLevel, int currentNodeNumber)
        {
            if (currentLevel < 0 || currentLevel > level)
                throw new SystemException("Wrong parameter - currentLevel.");
            if (currentNodeNumber < 0 || currentNodeNumber >= Math.Pow(branchingIndex, currentLevel))
                throw new SystemException("Wrong parameter - currentNodeNumber.");

            int result = 0;
            BitArray node = TreeNode(currentLevel, currentNodeNumber);
            int bi = (int)(branchingIndex * currentNodeNumber);
            for (int i = bi; i < branchingIndex * (currentNodeNumber + 1) - 1; ++i)
            {
                for (int j = i + 1; j < branchingIndex * (currentNodeNumber + 1); ++j)
                {
                    if (AreConnectedTwoBlocks(node, i - bi, j - bi))
                        ++result;
                }
            }

            return result;
        }

        /// <summary>
        /// Calculates the number of connections between specified subtree with other subtrees 
        /// for specified node.
        /// </summary>
        /// <param name="i">Index of specified subtree.</param>
        /// <note>Index must be in [0, branchingIndex - 1] range.</note>
        /// <param name="currentLevel">Index of level of specified node.</param>
        /// <note>currentLevel must be in [0, level] range.</note>
        /// <param name="currentNodeNumber">Index of specified node in currentLevel.</param>
        /// <note>currentNodeNumber must be in [0, pow(branchingIndex, currentLevel) - 1] range.</note>
        /// <returns>Number of connections.</returns>
        public int Links(int i, int currentLevel, int currentNodeNumber)
        {
            if(i < 0 || i > branchingIndex - 1)
                throw new SystemException("Wrong parameter - i.");
            if (currentLevel < 0 || currentLevel > level)
                throw new SystemException("Wrong parameter - currentLevel.");
            if (currentNodeNumber < 0 || currentNodeNumber >= Math.Pow(branchingIndex, currentLevel))
                throw new SystemException("Wrong parameter - currentNodeNumber.");

            int result = 0;
            BitArray node = TreeNode(currentLevel, currentNodeNumber);
            int bi = (int)(branchingIndex * currentNodeNumber);
            for (int j = bi; j < branchingIndex * (currentNodeNumber + 1); ++j)
            {
                if (AreConnectedTwoBlocks(node, i, j - bi))
                    ++result;
            }

            return result;
        }

        /// <summary>
        /// Calculates number of subtrees, which are connected with specified subtree.
        /// </summary>
        /// <param name="nodeInformation">The mark of node (bit sequence).</param>
        /// <param name="subtreeIndex">Index of subtree.</param>
        /// <note>Index of subtree must be in [0, branchingIndex- 1] range.</note>
        /// <returns>Number of subtrees.</returns>
        public int CountConnectedBlocks(BitArray nodeInformation, int subtreeIndex)
        {
            if (subtreeIndex < 0 || subtreeIndex > branchingIndex - 1)
                throw new SystemException("Wrong parameter - subtreeIndex.");

            subtreeIndex++;
            int s = 1, sum = 0;
            int findex = 0;
            while (s < subtreeIndex)
            {
                sum += Convert.ToInt32(nodeInformation[findex + subtreeIndex - s - 1]);
                findex += (int)branchingIndex - s;
                s++;
            }

            int endindex = findex + ((int)branchingIndex - s);
            s = findex;

            while (s < endindex)
            {
                sum += Convert.ToInt32(nodeInformation[s]);
                s++;
            }

            return sum;
        }

        /// <summary>
        /// Recoursively retrieves the index of subtree on specified level, 
        /// which contains the specified vertex.
        /// </summary>
        /// <param name="v">Index of the vertex (as leaf).</param>
        /// <note>Index of vertex must be in [0, pow(branchingIndex, level) - 1] range.</note>
        /// <param name="currentLevel">Index of level.</param>
        /// <returns>Index of subtree.</returns>
        public int TreeIndex(int v, int currentLevel)
        {
            if (v < 0 || v > (int)Math.Pow(branchingIndex, level) - 1)
                throw new SystemException("Wrong parameter - v.");

            if (currentLevel == level)
                return v;
            else
                return TreeIndex(v, currentLevel + 1) / (int)branchingIndex;
        }

        /// <summary>
        /// Calculates the length of shortest path between specified vertices.
        /// </summary>
        /// <param name="v1">Index of the first vertex.</param>
        /// <param name="v2">Index of the second vertex.</param>
        /// <note>Indices of vertices must be in [0, pow(branchingIndex, level) - 1] range.</note>
        /// <returns>-1, if there is no path between specified vertices.</returns>
        public int CalculateMinimalPathLength(int v1, int v2)
        {
            int highBound = (int)Math.Pow(branchingIndex, level) - 1;
            if ((v1 < 0 || v1 > highBound) || (v2 < 0 || v2 > highBound))
                throw new SystemException("Wrong parameter - v1 or v2.");

            if (v1 == v2)
                return 0;
            if (v1 > v2)
            {
                return CalculateMinimalPathLength(v2, v1);
            }

            int vertex1 = v1, vertex2 = v2;
            int currentLevel = (int)level - 1;
            // проверка на принадлежение к одному поддереву (для данных вершин)
            // поднимаемся по уровням до того уровна, где они будут принадлежать одному поддереву
            int numberOfGroup1 = Convert.ToInt32(v1 / branchingIndex);
            int numberOfGroup2 = Convert.ToInt32(v2 / branchingIndex);
            while (numberOfGroup1 != numberOfGroup2)
            {
                v1 = numberOfGroup1;
                v2 = numberOfGroup2;
                numberOfGroup1 = Convert.ToInt32(numberOfGroup1 / branchingIndex);
                numberOfGroup2 = Convert.ToInt32(numberOfGroup2 / branchingIndex);
                --currentLevel;
            }

            BitArray currentNode = TreeNode(currentLevel, numberOfGroup1);
            int bi = (int)branchingIndex;
            if (currentNode[AdjacentIndex(v1 % bi, v2 % bi)] == true)
                return 1;

            int tempCurrentLevel = currentLevel, vertexIndex, vI;
            while (0 != tempCurrentLevel)
            {
                vertexIndex = TreeIndex(vertex1, tempCurrentLevel - 1);
                vI = TreeIndex(vertex1, tempCurrentLevel) % bi;
                if (Links(vI, tempCurrentLevel - 1, vertexIndex) >= 1)
                    return 2;

                --tempCurrentLevel;
            }

            int[,] nodeMatrix = NodeMatrix(currentLevel, numberOfGroup1);
            int[,] distances = Engine.MinPath(nodeMatrix);
            if (distances[v1 % branchingIndex, v2 % branchingIndex] != int.MaxValue)
                return distances[v1 % branchingIndex, v2 % branchingIndex];

            return -1;
        }
        
        /// <summary>
        /// Defines if specified vertices are connected (are neighbours).
        /// </summary>
        /// <param name="v1">Index of first vertex.</param>
        /// <param name="v2">Index of second vertex.</param>
        /// <note>Indices of vertices must be in [0, pow(branchingIndex, level) - 1] range.</note>
        /// <returns>1, if specified vertices are connected. 0 otherwise.</returns>
        public int this[int v1, int v2]
        {
            get
            {
                int highBound = (int)Math.Pow(branchingIndex, level) - 1;
                if((v1 < 0 || v1 > highBound) || (v2 < 0 || v2 > highBound))
                    throw new SystemException("Wrong parameter - v1 or v2.");

                if (v1 == v2)
                {
                    return 0;
                }

                if (v1 > v2)
                {
                    return this[v2, v1];
                }

                int currentLevel = (int)level - 1;
                // проверка на принадлежение к одному поддереву (для данных вершин)
                // поднимаемся по уровням до того уровна, где они будут принадлежать одному поддереву
                int numberOfGroup1 = Convert.ToInt32(v1 / branchingIndex);
                int numberOfGroup2 = Convert.ToInt32(v2 / branchingIndex);
                while (numberOfGroup1 != numberOfGroup2)
                {
                    v1 = numberOfGroup1;
                    v2 = numberOfGroup2;
                    numberOfGroup1 = Convert.ToInt32(numberOfGroup1 / branchingIndex);
                    numberOfGroup2 = Convert.ToInt32(numberOfGroup2 / branchingIndex);
                    --currentLevel;
                }

                BitArray currentNode = TreeNode(currentLevel, numberOfGroup1);
                int bi = (int)branchingIndex;
                int index = AdjacentIndex(v1 % bi, v2 % bi);
                return Convert.ToInt32(currentNode[index]);
            }
        }

        /// <summary>
        /// Retrieves information about connectness (neighbourship) of specified vertices.
        /// </summary>
        /// <param name="v1">Index of the first vertex.</param>
        /// <param name="v2">Index of the second vertex.</param>
        /// <note>Indices of vertices must be in [0, branchingIndex - 1] range.</note>
        /// <returns>Index of a bit, which defines connectness of specified vertices.</returns>
        public int AdjacentIndex(int v1, int v2)
        {
            if ((v1 < 0 || v1 > branchingIndex - 1) || (v2 < 0 || v2 > branchingIndex - 1))
                throw new SystemException("Wrong parameter - v1 or v2.");

            if (v1 == v2)
            {
                return 0;
            }

            int result = 0;
            for (int i = 1; i <= v1; i++)
            {
                result += ((int)branchingIndex - i);
            }
            --result;
            result += v2 - v1;

            return result;
        }
    }
}
