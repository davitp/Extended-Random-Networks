using System;
using System.Collections.Generic;
using System.Linq;
using QuickGraph;

namespace NetworkModel
{
    public static class QGExtended
    {
        /// <summary>
        /// The core decomposition algorithm
        /// </summary>
        /// <param name="graph">The given graph</param>
        /// <returns></returns>
        public static DegeneracyResult CoreDecomposition(this UndirectedGraph<int, Edge<int>> graph)
        {
            // coreness per vertex
            var corenesses = new Dictionary<int, int>();

            // the graph degeneracy (maximum k core size)
            var degeneracy = 0;
            
            // graph size
            var size = graph.VertexCount;

            // the bucket queues
            var buckets = new HashSet<int>[size];

            // initialize bucket queue for each vertex
            for (var i = 0; i < buckets.Length; i++)
            {
                buckets[i] = new HashSet<int>();
            }

            // minimum degree 
            var minDegree = size;

            // degrees of vertexes
            var degrees = new Dictionary<int, int>();

            // fill all the values of degrees and keep the minimum degree
            foreach (var vertex in graph.Vertices)
            {
                // degree of vertex
                var degree = graph.AdjacentEdges(vertex).Count();

                // add the vertex to bucket with corresponding degree
                buckets[degree].Add(vertex);

                // add degree 
                degrees.Add(vertex, degree);

                // keep if currently the minimum 
                minDegree = Math.Min(minDegree, degree);
            }
            
            // start removing from queue starting from the minimum degree so far
            while (minDegree < size)
            {
                // get minimal bucket for processing
                var queue = buckets[minDegree];

                // if queue is empty nothing to process
                // just increment minimum degree to go 
                // to the next queue
                if (queue.Count == 0)
                {
                    minDegree++;
                    continue;
                }

                // get next vertex to process
                var vertex = queue.First();

                // remove from queue
                queue.Remove(vertex);

                // assign the coreness of vertex to be minimum degree 
                corenesses[vertex] = minDegree;

                // degeneracy will be the minimum from current value and degree of vertex
                degeneracy = Math.Max(degeneracy, minDegree);

                // process edges
                foreach (var e in graph.AdjacentEdges(vertex))
                {
                    // get the neighbour of the vertex
                    var pairVertex = e.GetOtherVertex(vertex);

                    // get degree of pair vertex
                    var pairDegree = degrees[pairVertex];

                    // if the pair of degree is less/equal than current minimum or pair is already processed
                    // continue to the next edge
                    if (pairDegree <= minDegree || corenesses.ContainsKey(pairVertex)) continue;

                    // remove pair from queue 
                    buckets[pairDegree].Remove(pairVertex);

                    // decrease its degree
                    pairDegree--;

                    // and add to new degree's queue
                    degrees[pairVertex] = pairDegree;

                    // add queue
                    buckets[pairDegree].Add(pairVertex);

                    // minimum degree will be minimum from current vertex and it's edge pair
                    minDegree = Math.Min(minDegree, pairDegree);
                }
            }

            // core counts 
            var coreGroups = new SortedDictionary<int, HashSet<int>> {[0] = new HashSet<int>(Enumerable.Range(0, 6))};


            for (var k = 1; k <= degeneracy; ++k)
            {
                coreGroups[k] = new HashSet<int>();
                for (var vertex = 0; vertex < size; ++vertex)
                {
                    if (corenesses[vertex] < k) continue;
                    coreGroups[k].Add(vertex);
                }
            }

            // max collapse core
            var maxCollapse = coreGroups.Reverse().First().Key;

            var ccs = new SortedDictionary<int, double>();

            for (var k = 0; k <= maxCollapse; ++k)
            {
                HashSet<int> kGroup;
                HashSet<int> k1Group;

                if (!coreGroups.TryGetValue(k, out kGroup))
                {
                    kGroup = new HashSet<int>();
                }

                if (!coreGroups.TryGetValue(k + 1, out k1Group))
                {
                    k1Group = new HashSet<int>();
                }

                var remainder = new HashSet<int>(kGroup);
                remainder.RemoveWhere(item => k1Group.Contains(item));

                ccs.Add(k, (double)remainder.Count / size);
            
            }

            // craete and return result
            return new DegeneracyResult(ccs, degeneracy);

        }
    }
}