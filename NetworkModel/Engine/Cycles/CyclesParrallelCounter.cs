/**
 * @File CyclesParrallelCounter.cs
 * 
 * @Author Hovhannes Antonyan
 */ 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace NetworkModel.Engine.Cycles
{
    interface IThreadEventHandler
    {
        void threadFinished(long result);
    }

    /**
     * Provides a method to calculate cycles count with a specified length
     * in a graph. Uses multiple threads to increase calculation perfomance.
     * Use CyclesCounterDoubleThreaded class, if the platform does not
     * support thread pools.
     */
    public class CyclesParallelCounter : IThreadEventHandler
    {
        public CyclesParallelCounter(NonHierarchicContainer container)
        {
            _container = container;
            _verticesCount = (int)_container.Size;
            _counters = new PivotsCyclesCounter[_verticesCount];
        }

        /**
         * Returns cycles count in the graph which length is equal 
         * to the specified 'cycleLength' argument
         * @precondition cycleLength >= 3
         */ 
        public double calculateCyclesCount(int cyclesLenght)
        {
            /*TextWriter tw = new StreamWriter(@"C:\ComplexNetwork\Progress"
                + cyclesLenght.ToString() + ".txt", true);
            tw.WriteLine("Execution start time = " + DateTime.Now);
            tw.Flush();*/
            _cyclesCount = 0;
            _working_threads = _verticesCount;
            for (int i = 0; i < _verticesCount; ++i)
            {
                _counters[i] = new PivotsCyclesCounter(this, _container, i, cyclesLenght);
                ThreadPool.QueueUserWorkItem(_counters[i].ThreadPoolCallback);
            }
            lock (this)
            {
                while (_working_threads > 0)
                {
                    Monitor.Wait(this);
                }
            }
            /*tw.WriteLine("Execution end time = " + DateTime.Now);
            tw.Close();*/
            return _cyclesCount;
        }

        public void threadFinished(long cyclesCount)
        {
            lock (this)
            {
                --_working_threads;
                _cyclesCount += cyclesCount;
                Monitor.Pulse(this);
            }
        }

        private PivotsCyclesCounter[] _counters;
        private NonHierarchicContainer _container;
        private int _verticesCount;
        private int _working_threads;
        private long _cyclesCount;
    }

    /**
     * Provides a method to calculate cycles count with specified length containing 
     * specified pivot vertex.
     * @NOTE: Makes an assumption that cycles count for vertices preceeding pivot 
     * are already calculated and therefore DOES NOT count cycles which contain
     * vertices preceeding pivot(which id lesser than the pivot's id) as they will be
     * computed by the preceeding vertices threads. 
     */
    class PivotsCyclesCounter
    {
        public PivotsCyclesCounter(IThreadEventHandler handler, NonHierarchicContainer container, 
                int pivot, int cycleLength)
        {
            _handler = handler;
            _container = container;
            _pivot = pivot;
            _cycleLength = cycleLength;
        }

        // Callback which is being launched by the thread pool
        public void ThreadPoolCallback(Object threadContext)
        {
            initialise();/*
            TextWriter tw = new StreamWriter(@"C:\ComplexNetwork\Progress"
                + _cycleLength.ToString() + ".txt", true);
            tw.WriteLine("Pivot" + _pivot + " start time = " + DateTime.Now);
            tw.Flush();*/
            _cyclesCount = getPivotsCycles() / 2;
            /*tw.WriteLine("Pivot" + _pivot + " End time = " + DateTime.Now);
            tw.Close();*/
            _handler.threadFinished(_cyclesCount);
        }

        private IThreadEventHandler _handler;

        // reference to the BAContainer object which contains the graph
        private NonHierarchicContainer _container;

        // The graph's vertices count
        private int _verticesCount;

        private int _pivot;
        private int _cycleLength;

        private long _cyclesCount;

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

        private void initialise()
        {
            _cyclesCount = 0;
            _verticesCount = (int)_container.Size;
            _marked = new bool[_verticesCount];
            for (int i = 0; i < _verticesCount; ++i)
            {
                unmark(i);
            }
            _stack = new Stack<int>();
            _path = new Stack<int>();
            _rolledBackNeighbours = new HashSet<int>[_cycleLength];
            for (int i = 0; i < _cycleLength; ++i)
            {
                _rolledBackNeighbours[i] = new HashSet<int>();
            }
        }

        private long getPivotsCycles()
        {
            if (_cycleLength == 2)
            {
                return _container.Neighbourship[_pivot].Count;
            }
            long cyclesCount = 0;
            _stack.Push(_pivot);
            while (_stack.Count != 0)
            {
                int curr = _stack.Pop();
                Debug.Assert(isConnectedWithPeak(curr));
                _path.Push(curr);
                setMarked(curr);
                bool newVertexPushed = considerNeighbours(curr, ref cyclesCount);
                if (newVertexPushed == false)
                {
                    rollBack();
                }
            }
            Debug.Assert(_path.Count == 0);
            return cyclesCount;
        }

        private bool considerNeighbours(int curr, ref long cyclesCount)
        {
            bool newVertexPushed = false;
            List<int> neighbours = _container.Neighbourship[curr];
            foreach (int neighbour in neighbours)
            {
                if (neighbour >= _pivot)
                {
                    if (isMarked(neighbour) == false)
                    {
                        if (_path.Count < _cycleLength)
                        {
                            _stack.Push(neighbour);
                            newVertexPushed = true;
                        }
                    }
                    else
                    {
                        addToRolledBackNeighboursPeek(neighbour);
                        if (neighbour == _pivot && _path.Count == _cycleLength)
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
