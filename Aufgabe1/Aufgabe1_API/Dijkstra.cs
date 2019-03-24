using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aufgabe1_API
{
    public static class Dijkstra
    {
        public static Dictionary<Vertex, Vertex> GenerateDijkstraHeuristicLazy(Vertex start, Dictionary<Vertex, Func<Dictionary<Vertex, double>>> nodes, List<Vertex> reachingRequired, out Dictionary<Vertex, Dictionary<Vertex, double>> visitedNodes)
        {
            List<Vertex> priorityList = nodes.Keys.ToList();
            reachingRequired = reachingRequired.Where(x => priorityList.Contains(x)).ToList();

            Dictionary<Vertex, double> distance = new Dictionary<Vertex, double>();
            Dictionary<Vertex, Vertex> path = new Dictionary<Vertex, Vertex>();
            Dictionary<Vertex, Dictionary<Vertex, double>> visitedNodesOut = new Dictionary<Vertex, Dictionary<Vertex, double>>();
            distance[start] = 0;
            path[start] = start;

            void Step(Vertex current)
            {
                foreach (var connection in (visitedNodesOut[current] = nodes[current]()))
                {
                    double newDistance = connection.Value + distance[current];
                    if (!distance.ContainsKey(connection.Key)) distance[connection.Key] = double.PositiveInfinity;
                    if (distance[connection.Key] > newDistance)
                    {
                        path[connection.Key] = current;
                        distance[connection.Key] = newDistance;
                    }
                }

                priorityList.Remove(current);
                reachingRequired.Remove(current);
            }
            IComparer<Vertex> comparer = Comparer<Vertex>.Create((a, b) => distance[a].CompareTo(distance[b]));
            while (priorityList.Any()) Step(priorityList.Min(x => distance.ContainsKey(x) ? distance[x] : double.PositiveInfinity).value);

            visitedNodes = visitedNodesOut;
            return path.ToDictionary(x => x.Key, x => x.Value);
        }

        public static List<Vertex> GetPath(Vertex start, Vertex end, Dictionary<Vertex, Vertex> heuristic)
        {
            List<Vertex> path = new List<Vertex>();
            for (Vertex current = end; current != start; current = heuristic[current]) path.Add(current);
            path.Add(start);
            return path;
        }

        public static double GetPathLength(Vertex start, Vertex end, Dictionary<Vertex, Vertex> heuristic, Dictionary<Vertex, Dictionary<Vertex, double>> visitedNodes)
        { 
            double length = 0;
            for (Vertex current = end; current != start; current = heuristic[current]) length += visitedNodes[current][heuristic[current]];
            return length;
        }
    }
}
