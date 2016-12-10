using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core.Model;
using NetworkModel.HierarchicEngine;

namespace NonRegularHierarchicModel
{
    /// <summary>
    /// Implementation of non regularly branching block-hierarchic network's container.
    /// </summary>
    class NonRegularHierarchicNetworkContainer : AbstractHierarchicContainer
    {
        private const int ARRAY_MAX_SIZE = 2000000000;

        private UInt32 vertices = 0;
        private UInt32 branchIndex = 0;
        private UInt32 level = 0;
        private UInt32[][] branches;
        private BitArray[][] hierarchicTree;

        public NonRegularHierarchicNetworkContainer()
        {
            branches = new UInt32[0][];
            hierarchicTree = new BitArray[0][];            
        }

        public override UInt32 Size
        {
            get 
            {
                if (vertices == 0)
                {
                    uint vertexCount = 0;
                    for (int i = 0; i < Branches[level - 1].Length; ++i)
                    {
                        for (int j = 0; j < Branches[level - 1][i]; ++j)
                        {
                            ++vertexCount;
                        }
                    }
                    vertices = vertexCount;
                }

                return vertices;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public UInt32 Vertices
        {
            set { vertices = value; }
        }

        public UInt32 BranchIndex
        {
            get { return branchIndex; }
            set { branchIndex = value; }
        }

        public UInt32 Level
        {
            get { return level; }
            set { level = value; }
        }

        public UInt32[][] Branches
        {
            get { return branches; }
            set { branches = value; }
        }

        public BitArray[][] HierarchicTree
        {
            set { hierarchicTree = value; }
        }

        public void SetBranches(ArrayList branches)
        {
            // Fill this.branches from branch
            this.branches = new UInt32[branches.Count][];
            ArrayList arr;
            for (int i = 0; i < branches.Count; ++i)
            {
                arr = (ArrayList)branches[i];
                this.branches[i] = new UInt32[arr.Count];
                for (int j = 0; j < arr.Count; ++j)
                    this.branches[i][j] = (UInt32)arr[j];
            }

            this.level = (UInt32)branches.Count;
        }

        public override void SetMatrix(ArrayList matrix)
        {
            // Move the matrix to List
            ArrayList arr;
            List<List<bool>> matrixInList = new List<List<bool>>();
            for (int i = 0; i < matrix.Count; ++i)
            {
                arr = (ArrayList)matrix[i];
                matrixInList.Add(new List<bool>());
                for (int j = 0; j < arr.Count; ++j)
                    matrixInList[i].Add((bool)arr[j]);
            }

            // Create empty TreeMatrix

            this.hierarchicTree = new BitArray[Level][];
            for (int i = 0; i < level; ++i)
            {
                long dataLength = 0;
                for (int j = 0; j < branches[i].Length; ++j)
                {
                    uint nodeDataLength = branches[i][j];
                    dataLength += nodeDataLength * (nodeDataLength - 1) / 2;
                }
                int arrCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(dataLength) / ARRAY_MAX_SIZE));

                hierarchicTree[i] = new BitArray[arrCount];
                if (arrCount > 0)
                {
                    int t;
                    for (t = 0; t < arrCount - 1; ++t)
                    {
                        hierarchicTree[i][t] = new BitArray(ARRAY_MAX_SIZE);
                    }
                    hierarchicTree[i][t] = new BitArray(Convert.ToInt32(dataLength - (arrCount - 1) * ARRAY_MAX_SIZE));
                }
            }

            // Fill the TreeMatrix from matrixInList

            for (int i = 0; i < matrixInList.Count; ++i)
            {
                for (int j = i + 1; j < matrixInList[i].Count; ++j)
                {
                    int currentLevel = (int)level;
                    int numberOfGroup1 = 0, numberOfGroup2 = 0;
                    int v1 = i, v2 = j; 
                    do
                    {
                        --currentLevel;
                        numberOfGroup1 = TreeIndexStep(v1, currentLevel);
                        numberOfGroup2 = TreeIndexStep(v2, currentLevel);

                        if (numberOfGroup1 != numberOfGroup2)
                        {
                            v1 = numberOfGroup1;
                            v2 = numberOfGroup2;
                        }

                    } while (numberOfGroup1 != numberOfGroup2);

                    int branchSize = (int)branches[currentLevel][numberOfGroup1];
                    int index = AdjacentIndex(branchSize, v1 % branchSize, v2 % branchSize);
                    int resultSize = branchSize * (branchSize - 1) / 2;

                    long count = 0;
                    for (int t = 0; t < numberOfGroup1; ++t)
                    {
                        count += branches[currentLevel][t] * (branches[currentLevel][t] - 1) / 2;
                    }

                    int ind = Convert.ToInt32(Math.Floor(Convert.ToDouble(count / ARRAY_MAX_SIZE)));
                    int rangeSt = Convert.ToInt32(count - ind * ARRAY_MAX_SIZE);
                    int rangeEnd = rangeSt + resultSize;
                    int secArray = 0;

                    if (rangeEnd > ARRAY_MAX_SIZE)
                    {
                        secArray = rangeEnd - ARRAY_MAX_SIZE;
                        rangeEnd = ARRAY_MAX_SIZE - 1;
                    }

                    int counter1 = 0;
                    bool result = false;
                    for (int t = rangeSt; t < rangeEnd; t++)
                    {
                        if (counter1 == index)
                        {
                            hierarchicTree[currentLevel][ind][t] = matrixInList[i][j];
                            result = true;
                        }
                        counter1++;
                    }

                    if (result == false)
                    {
                        for (int t = 0; t < secArray; t++)
                        {
                            hierarchicTree[currentLevel][ind + 1][t] = matrixInList[i][j];
                            counter1++;
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
                matrix[i, i] = false;
                for (int j = 0; j < i; ++j)
                {
                    matrix[i, j] = matrix[j, i] = ((this[j, i] == 1) ? true : false);
                }
            }

            return matrix;
        }

        public override List<KeyValuePair<int, int>> GetNeighbourship()
        {
            List<KeyValuePair<int, int>> n = new List<KeyValuePair<int, int>>();
            for (int i = 0; i < Size; ++i)
            {
                for (int j = i + 1; j < Size; ++j)
                    if (this[i, j] == 1)
                        n.Add(new KeyValuePair<int, int>(i, j));
            }
            return n;
        }

        public override UInt32[][] GetBranches()
        {
            return Branches;
        }

        /// <summary>
        /// Retrieves mark of specified node of non-regular block-hierarchical tree.
        /// </summary>
        /// <param name="currentLevel">Index of level of specified node.</param>
        /// <note>currentLevel must be in [0, level - 1] range.</note>
        /// <param name="currentNodeNumber">Index of specified node in currentLevel.</param>
        /// <returns>Sequence of {0, 1} values. Length is branchSize*(branchSize-1)/2</returns>
        public BitArray TreeNode(int currentLevel, long currentNodeNumber)
        {
            if (currentLevel < 0 || currentLevel > level - 1)
                throw new SystemException("Wrong parameter (number of level).");

            int branchesCount = (int)branches[currentLevel][currentNodeNumber];
            int resultSize = branchesCount * (branchesCount - 1) / 2;
            BitArray result = new BitArray(resultSize);

            long count = 0;
            for (int i = 0; i < currentNodeNumber; ++i)
            {
                count += branches[currentLevel][i] * (branches[currentLevel][i] - 1) / 2;
            }

            int ind = Convert.ToInt32(Math.Floor(Convert.ToDouble(count / ARRAY_MAX_SIZE)));
            int rangeSt = Convert.ToInt32(count - ind * ARRAY_MAX_SIZE);
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
        /// Retrieves the adjacency matrix for specified node of non-regular block-hierarchical tree.
        /// </summary>
        /// <param name="currentLevel">Index of level of specified node.</param>
        /// <note>currentLevel must be in [0, level - 1] range.</note>
        /// <param name="currentNodeNumber">Index of specified node in currentLevel.</param>
        /// <returns>Matrix of {0, 1} values. Size is branchSize X branchSize.</returns>
        public int[,] NodeMatrix(int currentLevel, long currentNodeNumber)
        {
            if (currentLevel < 0 || currentLevel >= level)
                throw new SystemException("Wrong parameter (number of level).");

            int branchSize = (int)branches[currentLevel][currentNodeNumber];
            int[,] result = new int[branchSize, branchSize];
            long indexCounter = 0;
            for (int j = 0; j < currentNodeNumber; ++j)
            {
                indexCounter += (branches[currentLevel][j] * (branches[currentLevel][j] - 1) / 2);
            }
            int ind = Convert.ToInt32(Math.Floor(Convert.ToDouble(indexCounter / ARRAY_MAX_SIZE)));
            int rangeSt = Convert.ToInt32(indexCounter - ind * ARRAY_MAX_SIZE);
            int rangeEnd = rangeSt + branchSize * (branchSize - 1) / 2;
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
                counterX++;
                if (counterX == branchSize)
                {
                    counterX = counterY + 2;
                    counterY++;
                }
            }

            for (int j = 0; j < secArray; j++)
            {
                result[counterX, counterY] = (hierarchicTree[currentLevel][ind + 1][j] ? 1 : 0);
                result[counterY, counterX] = (hierarchicTree[currentLevel][ind + 1][j] ? 1 : 0);
                counterX++;
                if (counterX == branchSize)
                {
                    counterX = 0;
                    counterY++;
                }
            }

            return result;
        }

        /// <summary>
        /// Retrieves adjacency lists for specified node's mark (bit sequence).
        /// </summary>
        /// <param name="nodeInformation">The mark of node (bit sequence).</param>
        /// <param name="branchSize">The size of the branch.</param>
        /// <returns>Adjacency lists.</returns>
        public Dictionary<int, ArrayList> NodeAdjacencyLists(BitArray nodeInformation, 
            int branchSize)
        {
            Dictionary<int, ArrayList> matrixList = new Dictionary<int, ArrayList>();
            for (int i = 0; i < branchSize; ++i)
            {
                matrixList.Add(i, new ArrayList());
            }

            for (int i = 0; i < branchSize - 1; ++i)
            {
                int s = i + 1;
                for (int j = i * (branchSize - (i - 1) - 1) + i * (i - 1) / 2;
                    j < (i + 1) * (branchSize - i - 1) + i * (i + 1) / 2;
                    ++j)
                {
                    if (nodeInformation[j] == true)
                    {
                        matrixList[i].Add(s);
                        matrixList[s].Add(i);
                    }
                    ++s;
                }
            }

            return matrixList;
        }

        /// <summary>
        /// Calculates number of edges for the given cluster.
        /// </summary>
        /// <param name="currentLevel">Index of level of specified node.</param>
        /// <note>currentLevel must be in [0, level - 1] range.</note>
        /// <param name="currentNodeNumber">Index of specified node in currentLevel.</param>
        /// <returns>Number of edges.</returns>
        public int CalculateNumberOfEdges(int currentLevel, int currentNodeNumber)
        {
            if (currentLevel < 0 || currentLevel > level)
                throw new SystemException("Wrong parameter (number of level).");

            if (currentLevel == level)
            {
                return 0;
            }

            int count = 0;
            int branchSize = (int)branches[currentLevel][currentNodeNumber];
            BitArray node = TreeNode(currentLevel, currentNodeNumber);
            int StartPoint = FindBranches(currentLevel, currentNodeNumber);
            for (int i = 0; i < branchSize; ++i)
            {
                count += CalculateNumberOfEdges(currentLevel + 1, i + StartPoint);
                for (int j = i + 1; j < branchSize; ++j)
                {
                    if (AreConnectedTwoBlocks(node, branchSize, i, j))
                    {
                        count += CountLeaves(currentLevel + 1, i + StartPoint) *
                            CountLeaves(currentLevel + 1, j + StartPoint);
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// Defines if specified subtrees are connected in specified nodes's mark.
        /// </summary>
        /// <param name="nodeInformation">The mark of node (bit sequence).</param>
        /// <param name="branchSize">The size of the branch.</param>
        /// <param name="n1">Index of first subtree.</param>
        /// <param name="n2">Index of first subtree.</param>
        /// <returns>true, if specifed subtrees are connected. false otherwise.</returns>
        public bool AreConnectedTwoBlocks(BitArray nodeInformation, int branchSize, int n1, int n2)
        {
            if (n1 == n2)
                return false;

            if (n1 > n2)
            {
                int k = n2;
                n2 = n1;
                n1 = k;
            }

            // Get the index of two numbers adjacent value
            int index = 0;
            for (int k = 1; k <= n1; k++)
            {
                index += branchSize - k;
            }
            index += n2 - n1 - 1;
            return nodeInformation[index] ? true : false;
        }

        /// <summary>
        /// Calculates the number of connections between specified subtree with other subtrees 
        /// for specified node.
        /// </summary>
        /// <param name="child">Index of specified subtree.</param>
        /// <param name="parent">Index of specified node in parentLevel.</param>
        /// <param name="parentLevel">Index of level of specified node.</param>
        /// <note>currentLevel must be in [0, level] range.</note>
        /// <returns>Number of connections.</returns>
        public int Links(int child, int parent, int parentLevel)
        {
            int result = 0;
            int branchSize = (int)branches[parentLevel][parent];
            if (branchSize == 1)
            {
                return 0;
            }
            BitArray node = TreeNode(parentLevel, parent);
            int branchStartPnt = FindBranches(parentLevel, parent);
            int childIndex = (child - branchStartPnt) % branchSize;
            for (int i = 0; i < branchSize; ++i)
            {
                if (AreConnectedTwoBlocks(node, branchSize, childIndex, i))
                {
                    ++result;
                }
            }

            return result;
        }

        /// <summary>
        /// Calculates number of subtrees, which are connected with specified subtree.
        /// </summary>
        /// <param name="nodeInformation">The mark of node (bit sequence).</param>
        /// <param name="branchSize">The size of the branch.</param>
        /// <param name="subtreeIndex">Index of subtree.</param>
        /// <note>Index of subtree must be in [0, branchSize - 1] range.</note>
        /// <returns>Number of subtrees.</returns>
        public int CountConnectedBlocks(BitArray nodeInformation, int branchSize, int subtreeIndex)
        {
            subtreeIndex++;
            int s = 1, sum = 0;
            int findex = 0;
            while (s < subtreeIndex)
            {
                sum += Convert.ToInt32(nodeInformation[findex + subtreeIndex - s - 1]);
                findex += branchSize - s;
                s++;
            }

            int endindex = findex + (branchSize - s);
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
        /// <param name="currentLevel">Index of level.</param>
        /// <returns>Index of subtree.</returns>
        public int TreeIndex(int v, int currentLevel)
        {
            if (currentLevel == level)
            {
                return v;
            }
            else
            {
                int cLevel = (int)level;
                do
                {
                    --cLevel;
                    v = TreeIndexStep(v, cLevel);
                }
                while (cLevel != currentLevel);
                return v;
            }
        }

        /// <summary>
        /// Calculates the length of shortest path between specified vertices.
        /// </summary>
        /// <param name="v1">Index of the first vertex.</param>
        /// <param name="v2">Index of the second vertex.</param>
        /// <note>Indices of vertices must be in [0, Size] range, where Size is the 
        /// number of vertices in the network.</note>
        /// <returns>-1, if there is no path between specified vertices.</returns>
        public int CalculateMinimalPathLength(int v1, int v2)
        {
            int vertex1 = v1, vertex2 = v2;

            if (v1 == v2)
                return 0;
            if (v1 > v2)
            {
                return CalculateMinimalPathLength(v2, v1);
            }

            // проверка на принадлежение к одному поддереву (для данных вершин)
            // поднимаемся по уровням до того уровна, где они будут принадлежать одному поддереву
            int numberOfGroup1 = 0, numberOfGroup2 = 0;
            int currentLevel = (int)level;
            do
            {
                --currentLevel;
                numberOfGroup1 = TreeIndexStep(v1, currentLevel);
                numberOfGroup2 = TreeIndexStep(v2, currentLevel);

                if (numberOfGroup1 != numberOfGroup2)
                {
                    v1 = numberOfGroup1;
                    v2 = numberOfGroup2;
                }

            } while (numberOfGroup1 != numberOfGroup2);

            int branchSize = (int)branches[currentLevel][numberOfGroup1];
            BitArray currentNode = TreeNode(currentLevel, numberOfGroup1);
            if (currentNode[AdjacentIndex(branchSize, v1 % branchSize, v2 % branchSize)] == true)
                return 1;

            int tempCurrentLevel = currentLevel, vertexIndex, vI;
            while (0 < tempCurrentLevel)
            {
                vertexIndex = TreeIndex(vertex1, tempCurrentLevel - 1);
                vI = TreeIndex(vertex1, tempCurrentLevel);
                if (Links(vI, vertexIndex, tempCurrentLevel - 1) > 0)
                    return 2;

                --tempCurrentLevel;
            }

            int[,] nodeMatrix = NodeMatrix(currentLevel, numberOfGroup1);
            int[,] distances = Engine.MinPath(nodeMatrix);
            if (distances[v1 % branchSize, v2 % branchSize] != int.MaxValue)
                return distances[v1 % branchSize, v2 % branchSize];

            return -1;
        }

        /// <summary>
        /// Defines if specified vertices are connected (are neighbours).
        /// <param name="v1">Index of the first vertex.</param>
        /// <param name="v2">Index of the second vertex.</param>
        /// <note>Indices of vertices must be in [0, Size] range, where Size is the 
        /// number of vertices in the network.</note>
        /// <returns>1, if specified vertices are connected. 0 otherwise.</returns>
        private int this[int v1, int v2]
        {
            get
            {
                // проверка на принадлежение к одному поддереву (для данных вершин)
                // поднимаемся по уровням до того уровна, где они будут принадлежать одному поддереву
                int numberOfGroup1 = 0, numberOfGroup2 = 0;
                int currentLevel = (int)level;
                do
                {
                    --currentLevel;
                    numberOfGroup1 = TreeIndexStep(v1, currentLevel);
                    numberOfGroup2 = TreeIndexStep(v2, currentLevel);
                 
                    if (numberOfGroup1 != numberOfGroup2)
                    {
                        v1 = numberOfGroup1;
                        v2 = numberOfGroup2;
                    }

                } while (numberOfGroup1 != numberOfGroup2);

                int branchSize = (int)branches[currentLevel][numberOfGroup1];
                BitArray currentNode = TreeNode(currentLevel, numberOfGroup1);
                int index = AdjacentIndex(branchSize, v1, v2);
                return Convert.ToInt32(currentNode[index]);
            }
        }        

        /// <summary>
        /// Retrieves information about connectness (neighbourship) of specified vertices.
        /// </summary>
        /// <param name="branchSize">The size of the branch.</param>
        /// <param name="v1">Index of the first vertex.</param>
        /// <param name="v2">Index of the second vertex.</param>
        /// <note>Indices of vertices must be in [0, Size] range, where Size is the 
        /// number of vertices in the network.</note>
        /// <returns>Index of a bit, which defines connectness of specified vertices.</returns>
        private int AdjacentIndex(int branchSize, int v1, int v2)
        {
            if (branchSize == 2)
            {
                return 0;
            }

            if (v1 == v2)
            {
                return 0;
            }

            int result = 0;
            for (int i = 1; i <= v1; i++)
            {
                result += (branchSize - i);
            }
            --result;
            result += v2 - v1;

            return result;
        }

        /// <summary>
        /// Retrieves the index of tree on specified level, 
        /// which contains the specified subtree that is lower with one level.
        /// </summary>
        /// <param name="v">The index of subtree.</param>
        /// <param name="currentLevel">The level of the tree, which contains the subtree.</param>
        /// <returns>The index of tree, which is on currentLevel.</returns>
        public int TreeIndexStep(int v, int currentLevel)
        {
            if (currentLevel > level - 1)
                throw new SystemException("Wrong parameter (number of level).");

            int counter = 0;
            bool found = false;
            for (int i = 0; i < branches[currentLevel].Length; ++i)
            {
                for (int j = 0; j < branches[currentLevel][i]; ++j)
                {
                    if (counter == v)
                    {
                        v = i;
                        found = true;
                        break;
                    }
                    ++counter;
                }
                if (found == true)
                {
                    break;
                }
            }
            return v;
        }

        /// <summary>
        /// Calculate the number of vertices in the given cluster.
        /// </summary>
        /// <param name="currentLevel">Index of level of specified node.</param>
        /// <note>currentLevel must be in [0, level - 1] range.</note>
        /// <param name="currentNodeNumber">Index of specified node in currentLevel.</param>
        /// <returns>Number of vertices.</returns>
        public int CountLeaves(int currentLevel, int currentNodeNumber)
        {
            if (currentLevel < 0 || currentLevel > level)
                throw new SystemException("Wrong parameter (number of level).");

            if (currentLevel == level)
            {
                return 1;
            }

            else
            {
                int branchStartPnt = FindBranches(currentLevel, currentNodeNumber);
                int countLeaves = 0;
                int branchSize = (int)branches[currentLevel][currentNodeNumber];
                for (int i = 0; i < branchSize; ++i)
                {
                    countLeaves += CountLeaves(currentLevel + 1, branchStartPnt + i);
                }
                return countLeaves;
            }
        }

        /// <summary>
        /// Finds the first index of a subtree that is lower with one level and belongs to the given tree.
        /// </summary>
        /// <param name="currentLevel">Index of level of specified node.</param>
        /// <note>currentLevel must be in [0, level - 1] range.</note>
        /// <param name="currentNodeNumber">Index of specified node in currentLevel.</param>
        /// <returns>The index of the subtree.</returns>
        public int FindBranches(int currentLevel, int currentNodeNumber)
        {
            if (currentLevel < 0 || currentLevel > level - 1)
                throw new SystemException("Wrong parameter (number of level).");

            int counter = 0;
            for (int i = 0; i < currentNodeNumber; ++i)
            {
                counter += (int)branches[currentLevel][i];
            }
            return counter;
        }
    }
}
