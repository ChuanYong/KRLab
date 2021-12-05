using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KRLab.Core.DataStructures.Graphs;
using LocalLists = KRLab.Core.DataStructures.Lists;

namespace KRLab.Core.Algorithms.Graphs
{
    public class GraphPath<T> where T : IComparable<T>
    {
        public Func<T, IEnumerable<T>> ShortestPathFunction(IGraph<T> graph, T start)
        {
            var previous = new Dictionary<T, T>();

            var queue = new Queue<T>();
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                var vertex = queue.Dequeue();
                foreach (var neighbor in graph.Neighbours(vertex))
                {
                    if (previous.ContainsKey(neighbor))
                        continue;

                    previous[neighbor] = vertex;
                    queue.Enqueue(neighbor);
                }
            }

            Func<T, IEnumerable<T>> shortestPath = v => {
                var path = new List<T> { };

                var current = v; 
                while (!current.Equals(start))
                {
                    path.Add(current);
                    current = previous[current]; 
                };

                path.Add(start);
                path.Reverse();

                return path;
            };

            return shortestPath;
        }

    }
}
