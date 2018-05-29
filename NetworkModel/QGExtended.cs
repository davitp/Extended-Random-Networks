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
            // initialize empty coreness map for vertex->coreness
            var corenesses = new Dictionary<int, int>();

            // initialize degeneracy with 0
            var degeneracy = 0;
            
            // initialize graph size
            var size = graph.VertexCount;

            // create a set of bucket queues
            var buckets = new HashSet<int>[size];

            // initialize all the bucket queues (per degree)
            for (var i = 0; i < buckets.Length; i++)
            {
                buckets[i] = new HashSet<int>();
            }

            // initialize minimum degree with the vertecies count
            var minDegree = size;

            // initialize new map of vertex and its degree
            var degrees = new Dictionary<int, int>();

            // for each vertex
            foreach (var vertex in graph.Vertices)
            {
                // calculate degree of the vertex
                var degree = graph.AdjacentEdges(vertex).Count();

                // add the vertex to bucket with corresponding degree
                buckets[degree].Add(vertex);

                // store degree of vertex in degrees storage
                degrees.Add(vertex, degree);

                // update minimum degree if current degree is less
                minDegree = Math.Min(minDegree, degree);
            }

            // keep minimum degree
            var minimumDegree = minDegree;
            
            // while current degree does not reach the maximum degree
            while (minDegree < size)
            {
                // get the bucket queue for current degree
                var queue = buckets[minDegree];

                // if queue isi empty
                if (queue.Count == 0)
                {
                    // add current degree
                    minDegree++;
                    // get to next degree
                    continue;
                }

                // get and store next vertex from queue
                var vertex = queue.First();

                // remove the vertex from the queue
                queue.Remove(vertex);

                // assign the coreness of vertex to be the current degree
                corenesses[vertex] = minDegree;

                // update degeneracy value if current degree increasess
                degeneracy = Math.Max(degeneracy, minDegree);

                // for every edge adjecent to vertex
                foreach (var e in graph.AdjacentEdges(vertex))
                {
                    // get the paired (neighbour) of the vertex
                    var pairVertex = e.GetOtherVertex(vertex);

                    // get degree of paired vertex
                    var pairDegree = degrees[pairVertex];

                    // if degree of the neighbour is less or equal than current processing degree
                    // or we have already processed it go to next neighbour
                    if (pairDegree <= minDegree || corenesses.ContainsKey(pairVertex)) continue;

                    // remove the paired vertex from its old queue
                    buckets[pairDegree].Remove(pairVertex);

                    // decrease degree of paired vertex
                    pairDegree--;

                    // assign new degree of the paired vertex
                    degrees[pairVertex] = pairDegree;

                    // place the paired vertex into queue with new degree
                    buckets[pairDegree].Add(pairVertex);

                    // update current degree if neighbour has less degree
                    minDegree = Math.Min(minDegree, pairDegree);
                }
            }

            if (degeneracy == 0)
            {
                return new DegeneracyResult(new SortedDictionary<int, double>(), minimumDegree, degeneracy, new Dictionary<int, CoreDescriptor>());
            }

            // edges per core
            var coreEdges = new Dictionary<int, List<Edge<int>>>();

            // for every edge in the graph
            foreach (var edge in graph.Edges)
            {
                // the core of edge
                var core = Math.Min(corenesses[edge.Source], corenesses[edge.Target]);

                if (coreEdges.ContainsKey(core))
                {
                    coreEdges[core].Add(edge);
                }
                else
                {
                    coreEdges[core] = new List<Edge<int>>{edge};
                }              
            }

            var coreVertexes = corenesses.GroupBy(e => e.Value).ToDictionary(g => g.Key, g => g.Select(kv => kv.Key).ToList());

            // core descriptors
            var coreDescriptors = new Dictionary<int, CoreDescriptor>();

            var currentVertexes = coreVertexes[degeneracy].Count;
            var currentEdges = coreEdges[degeneracy].Count;

            coreDescriptors.Add(degeneracy, new CoreDescriptor(degeneracy, currentVertexes, currentEdges));
            
            for (var k = degeneracy - 1; k >= 1; --k)
            {
                if (coreVertexes.ContainsKey(k))
                {
                    currentVertexes += coreVertexes[k].Count;
                }

                if (coreEdges.ContainsKey(k))
                {
                    currentEdges += coreEdges[k].Count;
                }

                coreDescriptors.Add(k, new CoreDescriptor(k, currentVertexes, currentEdges));

            }

            // group the vertecies by the "coreness" value 
            var groups = corenesses.GroupBy(kv => kv.Value).ToDictionary(g => g.Key, g => g.Select(k => k.Key).ToList());

            // initialize core collapse sequence
            var ccs = new SortedDictionary<int, double>();

            // for every k value from zero up to degeneracy
            for (var k = 0; k <= degeneracy; ++k)
            {
                // if there is a vertex group with coreness k
                if (groups.ContainsKey(k))
                {
                    // add the fraction of group size to the CCS
                    ccs.Add(k, groups[k].Count * 1.0 / size);
                }
                // if there is not a group with coreness k
                else
                {
                    // add zero value to CCS
                    ccs.Add(k, 0);
                }
            }

            // craete and return result
            return new DegeneracyResult(ccs, minimumDegree, degeneracy, coreDescriptors);

        }
    }
}