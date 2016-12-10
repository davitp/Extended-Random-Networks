using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace ModelChecking
{
    /// <author>Hovhannes Antonyan</author>
    /// <summary>
    /// Provides an API to check whether given graph is hierarhical or not
    /// (Whether some hierarchical tree exists or not which is isomorphous to
    /// the given graph)
    /// </summary>
    public class HierarchicExactChecker
    {
        //private static readonly ILog logger = log4net.LogManager.GetLogger(typeof(HierarchicExactChecker));

        private Container _container; // container which holds the graph to check for being hierarchic.

        /// <summary>
        /// Default and the only constructor
        /// </summary>
        public HierarchicExactChecker()
        {
        }

        /// <summary>
        /// Checks whether the graph specified by the file 
        /// is hierarchical or not
        /// </summary>
        /// <param name="fileName">Path to the file which contains the matrix of a graph to be checked for</param>
        /// <returns>True if the graph is hierarchical, otherwise false</returns>
        public bool IsHierarchic(string fileName)
        {
            List<List<bool>> matrix = Container.get_data(fileName);
            return IsHierarchic(matrix);
        }

        /// <summary>
        /// Checks whether the graph specified by the given matrix
        /// is hierarchical or not
        /// </summary>
        /// <param name="matrix">Matrix which specifies the given graph</param>
        /// <returns>True if the graph is hierarchical, otherwise false</returns>
        public bool IsHierarchic(List<List<bool>> matrix)
        {
            _container = new Container(matrix);
            ICollection<int> degrees = getAllDegrees(_container.Size).Keys;
            foreach (int prime in degrees)
            {
                if (isHierarchic(prime))
                {
                    BranchIndex = prime;
                    Level = Convert.ToInt32(Math.Log(_container.Size, prime));
                    return true;
                }
            }
            return false;
        }

        /////

        public int BranchIndex;
        public int Level;

        /////

        /// <summary>
        /// Checks whether the graph specified by the given matrix
        /// is hierarchical or not
        /// </summary>
        /// <param name="matrix">Matrix which specifies the given graph</param>
        /// <param name="tree">Tree object to hold the corresponding hierarchial tree
        /// if the graph is hierarchical</param>
        /// <returns>True if the graph is hierarchical, otherwise false</returns>
        public bool IsHierarchic(List<List<bool>> matrix, ref Tree tree)
        {
            _container = new Container(matrix);
            ICollection<int> degrees = getAllDegrees(_container.Size).Keys;
            foreach (int prime in degrees)
            {
                if (isHierarchic(prime, ref tree))
                {
                    return true;
                }
            }
            return false;
        }

        // Gets all possible numbers(and their degrees) if some of their 
        // degrees is equal to n.
        private static IDictionary<int, int> getAllDegrees(int n)
        {
            Dictionary<int, int> collection = new Dictionary<int, int>();
            int degree = 0;
            for (int p = 2; (degree = p * p) <= n; ++p)
            {
                if (isPrime(p))
                {
                    int k = 2;
                    while (degree < n)
                    {
                        degree *= p;
                        ++k;
                    }
                    if (degree == n)
                    {
                        collection.Add(p, k);
                        break; // as the p should be prime number then there will be no other number
                        // some degree of which will be equal to n
                    }
                }
            }
            return collection;
        }

        private static bool isPrime(int number)
        {
            if (number == 1) return false;
            if (number == 2) return true;
            for (int i = 2; i < number; ++i)
            {
                if (number % i == 0)
                {
                    return false;
                }
            }
            return true;
        }

        private bool isHierarchic(int prime)
        {
            Tree tree = generateTree(prime, _container.Size);
            return tree != null;
        }

        private bool isHierarchic(int prime, ref Tree tree)
        {
            tree = generateTree(prime, _container.Size);
            return tree != null;
        }

        // Constructs the hierarchical tree of the given graph if it is hierarchcal,
        // otherwise returns false
        private Tree generateTree(int p, int n)
        {
            try
            {
                ThreadManager manager = new ThreadManager(_container, p, n);
                Thread managerThread = new Thread(manager.threadFunction);
                managerThread.Start();
                managerThread.Join();
                return manager.Result;
            }
            catch (System.Exception e)
            {
                //logger.Error("Failed to generate hierarchic tree. The reason was: " + e.Message);
                throw e;
            }
        }

        /// <summary>
        /// Class responsible for parallelization of the calculation
        /// via launching multiple threads.
        /// </summary>
        private class ThreadManager : IThreadEvent
        {
            private ManualResetEvent _stopWorkItems;
            private Container _container;
            private Tree _tree;
            private int _working_threads;
            private int _p;
            private int _n;

            public ThreadManager(Container container, int p, int n)
            {
                _stopWorkItems = new ManualResetEvent(false);
                _container = container;
                _p = p;
                _n = n;
            }

            public void threadFunction()
            {
                _tree = null;
                _stopWorkItems.Reset();
                _working_threads = 0;
                int count = _n - _p + 1;
                for (int i = 1; i <= count && _stopWorkItems.WaitOne(0) == false; ++i)
                {
                    WorkItem workItem = new WorkItem(this, _stopWorkItems, _p, _n, i, _container);
                    ++_working_threads;
                    ThreadPool.QueueUserWorkItem(workItem.ThreadPoolCallback);
                }
                lock (this)
                {
                    while (_working_threads > 0)
                    {
                        Monitor.Wait(this);
                    }
                }
            }

            public void threadFinished(Tree tree)
            {
                lock (this)
                {
                    --_working_threads;
                    if (_tree == null && tree != null)
                    {
                        _tree = tree;
                        _stopWorkItems.Set();
                    }
                    Monitor.Pulse(this);
                }
            }

            public Tree Result
            {
                get
                {
                    return _tree;
                }
            }
        }
    }

    interface IThreadEvent
    {
        void threadFinished(Tree tree);
    }

    class WorkItem
    {
        //private static readonly ILog logger = log4net.LogManager.GetLogger(typeof(WorkItem));

        private readonly IThreadEvent _handler;
        private readonly ManualResetEvent _finishEvent;
        private readonly int _p;
        private int _n;
        private readonly int _id;
        private readonly Container _container;

        public WorkItem(IThreadEvent handler, ManualResetEvent finishEvent, int p, int n, int id, Container container)
        {
            _handler = handler;
            _finishEvent = finishEvent;
            _p = p;
            _n = n;
            _id = id;
            _container = container;
        }

        // Callback which is being launched by the thread pool
        public void ThreadPoolCallback(object threadContext)
        {
            if (_finishEvent.WaitOne(0) == false)
            {
                Tree tree = generateTree();
                _handler.threadFinished(tree);
            }
            else
            {
                _handler.threadFinished(null);
            }
        }

        // Constructs the hierarchical tree of the given graph if it is hierarchcal,
        // otherwise returns false
        private Tree generateTree()
        {
            Tree tree = new Tree(_container);
            return generateTree(tree);
        }

        // Constructs the next levels of the tree, and attaches into the given 'tree' object
        private Tree generateTree(Tree tree)
        {
            List<Group> combination = new List<Group>();
            try
            {
                Group group = getFirstGroup(tree, combination);
                if (group == null)
                {
                    return null;
                }
                combination.Add(group);
                if (_p < _n)
                {
                    do
                    {
                        combination = getNextCombination(tree, combination);
                    } while (_finishEvent.WaitOne(0) == false && combination != null && riseUp(tree, combination) == false);
                    if (_finishEvent.WaitOne(0) == true || combination == null)
                    {
                        return null;
                    }
                }
                else
                {
                    riseUp(tree, combination);
                }
            }
            catch (System.Exception e)
            {
                //logger.Error("WorkItem " + _id + " Failed to generate hierarchic tree. The reason was: " + e.Message);
                //logger.Info("WorkItem " + _id + ":", e);
                throw e;
            }
            return tree;
        }

        // Gets the first possible valid group (i.e. vertices of which are satistfying the statement to 
        // form a group) if there is such group, otherwise returns null
        private Group getFirstGroup(Tree tree, List<Group> combination)
        {
            Group group = new Group();
            Debug.Assert(_p > 0);
            group.Add(0);
            int start = 2;
            if (tree.Levels.Count == 0)
            {
                group.Add(_id);
                start = _id + 1;
            }
            else
            {
                group.Add(1);
            }
            for (int i = start; group.Count < _p; ++i)
            {
                group.Add(i);
            }
            if (checkConnections(tree, group) == false)
            {
                group = getNextValidGroup(tree, combination, group);
            }
            return group;
        }

        // Gets the next possible combination of groups, where each group is valid
        private List<Group> getNextCombination(Tree tree, List<Group> originalCombination)
        {
            Debug.Assert(originalCombination.Count != 0);
            List<Group> combination = new List<Group>(originalCombination);
            if (combination.Count == _n / _p)
            {
                Group group = null;
                while (group == null && 0 < combination.Count)
                {
                    if (_finishEvent.WaitOne(0) == true || combination.Count == 1)
                    {
                        //terminating work item
                        return null;
                    }
                    group = combination[combination.Count - 1];
                    combination.RemoveAt(combination.Count - 1);
                    group = getNextValidGroup(tree, combination, group);
                }
                if (group != null)
                {
                    combination.Add(group);
                }
            }
            while (0 < combination.Count && combination.Count < _n / _p)
            {
                Group group = null;
                group = getNextValidGroup(tree, combination);
                if (group != null)
                {
                    combination.Add(group);
                }
                else
                {
                    while (group == null && 0 < combination.Count)
                    {
                        if (_finishEvent.WaitOne(0) == true || combination.Count == 1)
                        {
                            //terminating work item
                            return null;
                        }
                        group = combination[combination.Count - 1];
                        combination.RemoveAt(combination.Count - 1);
                        group = getNextValidGroup(tree, combination, group);
                    }
                    if (group != null)
                    {
                        combination.Add(group);
                    }
                }
            }
            if (combination.Count == _n / _p)
            {
                return combination;
            }
            Debug.Assert(combination.Count == 0);
            return null;
        }

        // Gets the next possible valid group which does not contain vertices of the
        // given combination's groups vertices.
        private Group getNextValidGroup(Tree tree, List<Group> combination)
        {
            SortedSet<int> set = new SortedSet<int>();
            foreach (Group group in combination)
            {
                foreach (int vertex in group)
                {
                    set.Add(vertex);
                }
            }
            Group nextGroup = getNextGroup(combination, set);
            if (_finishEvent.WaitOne(0) == false && nextGroup != null && checkConnections(tree, nextGroup) == false)
            {
                do
                {
                    nextGroup = getNextGroup(combination, nextGroup, set);
                } while (_finishEvent.WaitOne(0) == false && nextGroup != null && checkConnections(tree, nextGroup) == false);
            }
            if (_finishEvent.WaitOne(0) == true)
            {
                return null;
            }
            return nextGroup;
        }

        // Gets the next possible valid group which succeeds the given 'group' object and 
        // does not contain vertices of the given combination's groups vertices.
        private Group getNextValidGroup(Tree tree, List<Group> combination, Group group)
        {
            SortedSet<int> set = new SortedSet<int>();
            foreach (Group g in combination)
            {
                foreach (int vertex in g)
                {
                    set.Add(vertex);
                }
            }
            Group nextGroup = group;
            do
            {
                nextGroup = getNextGroup(combination, nextGroup, set);
            } while (_finishEvent.WaitOne(0) == false && nextGroup != null && checkConnections(tree, nextGroup) == false);
            if (_finishEvent.WaitOne(0) == true)
            {
                return null;
            }
            return nextGroup;
        }

        // Gets the next possible group of vertices which is probably not valid, it has to be checked
        // and which does not contain vertices of the given combination's groups vertices.
        private Group getNextGroup(List<Group> combination, SortedSet<int> set)
        {
            Debug.Assert(combination.Count != 0);
            Debug.Assert(combination[combination.Count - 1].Count == _p);
            int prevPivot = combination[combination.Count - 1][0];
            Group next = new Group();
            int prevVertex = -1;
            foreach (int vertex in set)
            {
                if (vertex - prevVertex > 0)
                {
                    int v = prevVertex;
                    while (next.Count < _p && vertex - v > 1)
                    {
                        if (++v >= prevPivot)
                        {
                            next.Add(v);
                        }
                        else
                        {
                            return null;
                        }
                    }
                    if (next.Count == _p)
                    {
                        return next;
                    }
                }
                prevVertex = vertex;
            }
            if (set.Count > 0)
            {
                prevVertex = set.Max;
            }
            while (next.Count < _p && prevVertex < _n - 1)
            {
                next.Add(++prevVertex);
            }
            Debug.Assert(next.Count == _p);
            return next;
        }

        // Gets the next possible group of vertices which succeeds the given old group,
        // probably is not valid, it has to be checked and which does not contain 
        // vertices of the given combination's groups vertices.
        private Group getNextGroup(List<Group> combination, Group oldGroup, SortedSet<int> set)
        {
            Debug.Assert(oldGroup.Count == _p);
            int vertex = -1;
            while (vertex == -1 && 1 < oldGroup.Count)
            {
                vertex = oldGroup[oldGroup.Count - 1];
                oldGroup.RemoveAt(oldGroup.Count - 1);
                vertex = getNextValidVertex(set, oldGroup, vertex);
            }
            if (vertex != -1)
            {
                oldGroup.Add(vertex);
            }
            while (1 < oldGroup.Count && oldGroup.Count < _p)
            {
                vertex = getNextValidVertex(set, oldGroup);
                if (vertex != -1)
                {
                    oldGroup.Add(vertex);
                }
                else
                {
                    while (vertex == -1 && 1 < oldGroup.Count)
                    {
                        vertex = oldGroup[oldGroup.Count - 1];
                        oldGroup.RemoveAt(oldGroup.Count - 1);
                        vertex = getNextValidVertex(set, oldGroup, vertex);
                    }
                    if (vertex != -1)
                    {
                        oldGroup.Add(vertex);
                    }
                }
            }
            if (oldGroup.Count == _p)
            {
                return oldGroup;
            }
            Debug.Assert(oldGroup.Count == 1);
            return null;
        }

        private int getNextValidVertex(SortedSet<int> set, Group group)
        {
            Debug.Assert(group.Count != 0);
            int vertex = group[group.Count - 1] + 1;
            while (set.Contains(vertex) && vertex < _n)
            {
                ++vertex;
            }
            if (vertex < _n - _p + group.Count + 1)
            {
                return vertex;
            }
            return -1;
        }

        private int getNextValidVertex(SortedSet<int> set, Group group, int oldVertex)
        {
            Debug.Assert(group.Count != 0);
            int vertex = oldVertex + 1;
            while (set.Contains(vertex) && vertex < _n)
            {
                ++vertex;
            }
            if (vertex < _n - _p + group.Count + 1)
            {
                return vertex;
            }
            return -1;
        }

        // Checks whether the vertices of the group simulateously have no connections with vertices
        // out of the group, or are connected to the same vertices out of the group
        private bool checkConnections(Tree tree, Group group)
        {
            Debug.Assert(group.Count == _p);
            for (int i = 0; i < _p; ++i)
            {
                int vertex = group[i];
                Container container = tree.LastContainer;
                Debug.Assert(container.Neighbourship.ContainsKey(vertex));
                foreach (int neighbour in container.Neighbourship[vertex])
                {
                    if (!group.Contains(neighbour))
                    {
                        for (int j = 0; j < _p; ++j)
                        {
                            if (j != i && !container.areConnected(group[j], neighbour))
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        // Adds the current combination of groups to the tree and rises up to the next level
        // Returns true if it was able to rise to the highest level, which means the graph
        // is hierarchical, or otherwise returns false
        private bool riseUp(Tree tree, List<Group> combination)
        {
            tree.Levels.Add(combination);
            if (_n > _p)
            {
                _n /= _p;
                generateNextLevelContainer(tree, combination);
                if (generateTree(tree) == null)
                {
                    tree.Levels.RemoveAt(tree.Levels.Count - 1);
                    tree.removeLastContainer();
                    _n *= _p;
                    return false;
                }
                _n *= _p;
            }
            return true;
        }

        private void generateNextLevelContainer(Tree tree, List<Group> combination)
        {
            List<List<bool>> matrix = new List<List<bool>>();
            for (int i = 0; i < combination.Count; ++i)
            {
                List<bool> connection = new List<bool>();
                for (int j = 0; j < combination.Count; ++j)
                {
                    if (j > i)
                    {
                        bool connected = checkGroupsConnection(tree, combination, i, j);
                        connection.Add(connected);
                    }
                    else if (i == j)
                    {
                        connection.Add(false);
                    }
                    else
                    {
                        connection.Add(matrix[j][i]);
                    }
                }
                matrix.Add(connection);
            }
            tree.AddContainer(new Container(matrix));
        }

        private bool checkGroupsConnection(Tree tree, List<Group> combination, int i, int j)
        {
            Group g1 = combination[i];
            Group g2 = combination[j];
            return tree.LastContainer.areConnected(g1[0], g2[0]);
        }
    }

    // Inner class which holds the graph for check for being hierarchical
    public class Container
    {
        private int _size; // number of vertices
        private SortedDictionary<int, List<int>> _neighbourship; // list of neighbours for each vertex  
        private List<List<bool>> _matrix;

        public Container(List<List<bool>> matrix)
        {
            validateMatrix(matrix);
            _matrix = matrix;
            _size = matrix.Count;
            _neighbourship = new SortedDictionary<int, List<int>>();
            List<bool> neighbourshipOfIVertex = new List<bool>();
            for (int i = 0; i < matrix.Count; i++)
            {
                neighbourshipOfIVertex = matrix[i];
                setDataToDictionary(i, neighbourshipOfIVertex);
            }
        }

        private void validateMatrix(List<List<bool>> matrix)
        {
            for (int i = 0; i < matrix.Count; ++i)
            {
                if (matrix[i].Count != matrix.Count)
                {
                    throw new System.Exception("Given matrix is not well formed");
                }
                for (int j = 0; j < matrix.Count; ++j)
                {
                    if (i != j && matrix[i][j] != matrix[j][i])
                    {
                        throw new System.Exception("Given matrix is not well formed");
                    }
                }
            }
        }

        public int Size
        {
            get { return _size; }
        }

        public SortedDictionary<int, List<int>> Neighbourship
        {
            get { return _neighbourship; }
        }

        public bool areConnected(int vertex1, int vertex2)
        {
            return _matrix[vertex1][vertex2];
        }

        public static List<List<bool>> get_data(string filename)
        {
            List<List<bool>> matrix = new List<List<bool>>();
            using (StreamReader streamreader = new StreamReader(filename))
            {
                string contents;
                while ((contents = streamreader.ReadLine()) != null)
                {
                    string[] split = System.Text.RegularExpressions.Regex.Split(contents,
                            "\\s+", System.Text.RegularExpressions.RegexOptions.None);
                    List<bool> tmp = new List<bool>();
                    for (int i = 0; i < split.Length - 1; ++i)
                    {
                        string s = split[i];
                        if (s.Equals("0"))
                        {
                            tmp.Add(false);
                        }
                        else
                        {
                            tmp.Add(true);
                        }
                    }
                    if (!split[split.Length - 1].Equals(""))
                    {
                        string s = split[split.Length - 1];
                        if (s.Equals("0"))
                        {
                            tmp.Add(false);
                        }
                        else
                        {
                            tmp.Add(true);
                        }
                    }
                    matrix.Add(tmp);
                }
            }
            return matrix;
        }

        private void setDataToDictionary(int index, List<bool> neighbourshipOfIVertex)
        {
            _neighbourship[index] = new List<int>();
            for (int j = 0; j < _size; j++)
            {
                if ((bool)neighbourshipOfIVertex[j] == true && index != j)
                {
                    _neighbourship[index].Add(j);
                }
            }
        }
    }

    public class Tree
    {
        private readonly List<List<Group>> _levels; // contains Groups of each level starting from lowest to highest levels.
        private readonly List<Container> _containers;

        public Tree(Container container)
        {
            _levels = new List<List<Group>>();
            _containers = new List<Container>();
            _containers.Add(container);
        }

        public void AddContainer(Container container)
        {
            _containers.Add(container);
        }

        public void removeLastContainer()
        {
            if (_containers.Count > 0)
            {
                _containers.RemoveAt(_containers.Count - 1);
            }
        }

        public List<List<Group>> Levels
        {
            get
            {
                return _levels;
            }
        }

        public Container LastContainer
        {
            get
            {
                if (_containers.Count > 0)
                {
                    return _containers[_containers.Count - 1];
                }
                return null;
            }
        }
    }

    public class Group : List<int>
    {
        public Group()
            : base()
        {
        }

        public Group(IEnumerable<int> collection)
            : base(collection)
        {
        }
    }
}