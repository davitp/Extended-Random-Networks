/**
 * @File CyclesCounterDoubleThreaded.cs
 * 
 * @Author Hovhannes Antonyan
 */ 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace NetworkModel.Engine.Cycles
{
    /**
     * Provides a method to calculate cycles count with a specified length
     * in a graph. Uses a Double thread to perform calculation. Use 
     * CyclesParallelCounter class to increase perfomance if the platform
     * supports thread pools.
     */
    public class CyclesCounterDoubleThreaded
    {
        public CyclesCounterDoubleThreaded(NonHierarchicContainer container)
        {
            _container = container;
            _verticesCount = (int)_container.Size;
            _marked = new bool[_verticesCount];
            for (int i = 0; i < _verticesCount; ++i)
            {
                unmark(i);
            }
            _stack = new Stack<int>();
            _path = new Stack<int>();
        }

        /**
         * Returns cycles count in the graph which length is equal 
         * to the specified 'cycleLength' argument
         * @precondition cycleLength >= 3
         */
        public long getCyclesCount(int cycleLength)
        {
            _rolledBackNeighbours = new HashSet<int>[cycleLength];
            for (int i = 0; i < cycleLength; ++i)
            {
                _rolledBackNeighbours[i] = new HashSet<int>();
            }
            long cyclesCount = 0;
            /*TextWriter tw = new StreamWriter(@"C:\ComplexNetwork\Progress"
                + cycleLength.ToString() + ".txt");
            tw.WriteLine("Start time = " + DateTime.Now);*/
            for (int pivot = 0; pivot < _verticesCount; ++pivot)
            {
                cyclesCount += getPivotsCycles(pivot, cycleLength);
                Debug.Assert(cyclesCount % 2 == 0);
                /*if (pivot % 10 == 0)
                {
                    tw.WriteLine(pivot);
                    tw.Flush();
                }*/
            }
            /*tw.WriteLine("End time = " + DateTime.Now);
            tw.Close();*/
            return cyclesCount / 2; // each cycle is being counted twice
        }


        // reference to the BAContainer object which contains the graph
        private NonHierarchicContainer _container;

        // The graph's vertices count
        private int _verticesCount;

        /**
         * An array of booleans where each element respresents whether the
         * corresponding is marked during the processing of the pivot
         * 
         * An array is used for constant time access to the ith element.
         */
        private bool[] _marked;

        private bool isMarked(int vertex)
        {
            return _marked[vertex];
        }

        private void setMarked(int vertex)
        {
            _marked[vertex] = true;
        }

        private void unmark(int vertex)
        {
            _marked[vertex] = false;
        }

        /**
         * All vertices which should be considered are being pushed to this stack
         */
        private Stack<int> _stack;

        /**
         * Contains the current path in the DepthFirstSearch.
         */
        private Stack<int> _path;

        private HashSet<int>[] _rolledBackNeighbours;

        private void addToRolledBackNeighboursPeek(int vertex)
        {
            _rolledBackNeighbours[_path.Count - 1].Add(vertex);
        }

        private void clearRolledBackNeighboursPeek()
        {
            _rolledBackNeighbours[_path.Count - 1].Clear();
        }

        private int getRolledBackNeighboursPeeksCount()
        {
            return _rolledBackNeighbours[_path.Count - 1].Count;
        }

        private bool isRolledBack(int vertex)
        {
            return _rolledBackNeighbours[_path.Count - 2].Contains(vertex);
        }

        private bool areConnected(int vertex1, int vertex2)
        {
            List<int> vertex1Neighbours = _container.Neighbourship[vertex1];
            bool connected1 = vertex1Neighbours.Contains(vertex2);
            List<int> vertex2Neighbours = _container.Neighbourship[vertex2];
            bool connected2 = vertex2Neighbours.Contains(vertex1);
            return connected1 && connected2;
        }

        private bool isConnectedWithPeak(int vertex)
        {
            if (_path.Count == 0)
            {
                return true;
            }
            return areConnected(vertex, _path.Peek());
        }

        
        private long getPivotsCycles(int pivot, int cycleLength)
        {
            if (cycleLength == 2)
            {
                return _container.Neighbourship[pivot].Count;
            }
            long cyclesCount = 0;
            _stack.Push(pivot);
            while (_stack.Count != 0)
            {
                int curr = _stack.Pop();
                Debug.Assert(isConnectedWithPeak(curr));
                _path.Push(curr);
                setMarked(curr);
                bool newVertexPushed = considerNeighbours(curr,
                        pivot, cycleLength, ref cyclesCount);
                if (newVertexPushed == false)
                {
                    rollBack();
                }
            }
            Debug.Assert(_path.Count == 0);
            return cyclesCount;
        }

        private bool considerNeighbours(int curr, int pivot, int cycleLength,
               ref long cyclesCount)
        {
            bool newVertexPushed = false;
            List<int> neighbours = _container.Neighbourship[curr];
            foreach (int neighbour in neighbours)
            {
                if (neighbour >= pivot)
                {
                    if (isMarked(neighbour) == false)
                    {
                        if (_path.Count < cycleLength)
                        {
                            _stack.Push(neighbour);
                            newVertexPushed = true;
                        }
                    }
                    else
                    {
                        addToRolledBackNeighboursPeek(neighbour);
                        if (neighbour == pivot && _path.Count == cycleLength)
                        {
                            ++cyclesCount;
                        }
                    }
                }
                else
                {
                    addToRolledBackNeighboursPeek(neighbour);
                }
            }
            return newVertexPushed;
        }

        private void rollBack()
        {
            if (_path.Count != 0)
            {
                clearRolledBackNeighboursPeek();
            }
            int vertex = _path.Pop();
            unmark(vertex);
            while (_path.Count != 0)
            {
                addToRolledBackNeighboursPeek(vertex);
                vertex = _path.Peek();
                if (hasAnUnmarkedNeighbour(vertex))
                {
                    break;
                }
                unmark(vertex);
                clearRolledBackNeighboursPeek();
                _path.Pop();
            }
        }

        private bool hasAnUnmarkedNeighbour(int vertex)
        {
            int neighboursCount = _container.Neighbourship[vertex].Count;
            return getRolledBackNeighboursPeeksCount() < neighboursCount;
        }
    }
}
