using System;
using System.Collections.Generic;
using System.Linq;

namespace Aufgabe1_API
{
    public class Navmap
    {
        public Dictionary<Vector, Dictionary<Vector, double>> nodes;
        public Dictionary<Vector, Vector> heuristic;

        public Navmap(Dictionary<Vector, Dictionary<Vector, double>> nodes)
        {
            this.nodes = nodes;
        }

        public Navmap(Dictionary<Vector, List<Vector>> nodesList)
        {
            nodes = nodesList.ToDictionary(x => x.Key, x => x.Value.ToDictionary(y => y, y => y.Distance(x.Key)));
        }

        public Dictionary<Vector, Vector> GenerateDijkstraHeuristic(Vector start)
        {
            List<Vector> priorityList = nodes.Select(x => x.Key).ToList();
            Dictionary<Vector, double> distance = priorityList.ToDictionary(x => x, x => double.PositiveInfinity);
            Dictionary<Vector, Vector> path = priorityList.ToDictionary(x => x, x => (Vector)null);
            distance[start] = 0;

            void Step(Vector current)
            {
                foreach (var connection in nodes[current])
                {
                    if (distance[connection.Key] > connection.Value + distance[current])
                    {
                        path[connection.Key] = current;
                        distance[connection.Key] = connection.Value + distance[current];
                    }
                }

                priorityList.Remove(current);
            }

            Step(start);

            while (priorityList.Any())
            {
                Step(priorityList.First());
            }

            return path;
        }

        public List<Vector> Dijkstra(Vector start, Vector end)
        {
            List<Vector> finalPath = new List<Vector>();

            for (Vector current = end; current != start; current = heuristic[current]) finalPath.Add(current);
            finalPath.Add(start);
            finalPath.Reverse();

            return finalPath;
        }

        /*public List<Vector> AStar(Vector start, Vector end) => ShortestPath(start, end, (node, shortest, distance) => distance[node] + node.Distance(end) < distance[shortest] + shortest.Distance(end));
        public List<Vector> Dijkstra(Vector start, Vector end) => ShortestPath(start, end, (node, shortest, distance) => distance[node] < distance[shortest]);

        private List<Vector> ShortestPath(Vector start, Vector end, Func<Vector, Vector, Dictionary<Vector, double>, bool> comparer)
        {
            List<Vector> priorityList = nodes.Select(x => x.Key).ToList();
            Dictionary<Vector, double> distance = priorityList.ToDictionary(x => x, x => double.PositiveInfinity);
            Dictionary<Vector, Vector> path = priorityList.ToDictionary(x => x, x => (Vector)null);
            distance[start] = 0;

            while (true)
            {
                Vector shortest = priorityList.First();
                foreach (var node in priorityList) if (comparer(node, shortest, distance)) shortest = node;

                if (shortest == end)
                {
                    List<Vector> finalPath = new List<Vector>();
                    
                    for (Vector current = end; current != start; current = path[current]) finalPath.Add(current);
                    finalPath.Add(start);
                    finalPath.Reverse();

                    return finalPath;
                }

                foreach (var connection in nodes[shortest])
                {
                    if (distance[connection.Key] > connection.Value + distance[shortest])
                    {
                        path[connection.Key] = shortest;
                        distance[connection.Key] = connection.Value + distance[shortest];
                    }
                }
            }
        }*/
    }
}