using System;
using System.Collections.Generic;
using System.Linq;

namespace Aufgabe1_API
{
    public class Map
    {
        public Polygon[] polygons;
        public Vector[] busPath;
        public Vertex startingPosition;
        public List<Vertex> allPolygonVertices;
        public double busSpeed, characterSpeed, busApproachConstant;

        public Map(string[] lines)
        {
            int polygonCount = int.Parse(lines[0]);
            polygons = new Polygon[polygonCount];

            for (int i = 0; i < polygonCount; i++)
            {
                int[] polygon = lines[i + 1].Split().Select(x => int.Parse(x)).ToArray();
                var vertices = new Vector[polygon[0]];

                for (int j = 0; j < polygon[0]; j++) vertices[j] = new Vector(polygon[j * 2 + 1], polygon[j * 2 + 2]);

                polygons[i] = new Polygon(vertices);
            }

            int[] start = lines[polygonCount + 1].Split().Select(x => int.Parse(x)).ToArray();
            startingPosition = new Vertex(new Vector(start[0], start[1]));

            busPath = new Vector[] { new Vector(0, 0), new Vector(0, 10000) };

            busSpeed = 30;
            characterSpeed = 15;

            allPolygonVertices = polygons.SelectMany(x => x).ToList();

            busApproachConstant = characterSpeed / Math.Sqrt(busSpeed * busSpeed - characterSpeed * characterSpeed);
        }

        public Map(Polygon[] polygons, Vector[] busPath, Vector startingPosition, double busSpeed, double characterSpeed)
        {
            this.polygons = polygons;
            this.busPath = busPath;
            this.startingPosition = new Vertex(startingPosition);
            this.busSpeed = busSpeed;
            this.characterSpeed = characterSpeed;

            allPolygonVertices = polygons.SelectMany(x => x).ToList();

            busApproachConstant = characterSpeed / Math.Sqrt(busSpeed * busSpeed - characterSpeed * characterSpeed);
        }

        public double CalculateDistance(Vertex vertex, Vector origin, double angle)
        {
            Vector a = vertex.vector - origin;
            Vector b = vertex.Next.vector - origin;
            return ((a.y - b.y) * b.x - (a.x - b.x) * b.y) / (Math.Cos(angle) * (a.y - b.y) - Math.Sin(angle) * (a.x - b.x));
        }
        int n = 0;
        public List<Vertex> GenerateVisibilityPolygon(Vector origin, bool reduced, out List<Vertex> endpoints, out List<(Vector, Vector)> debug) => GenerateVisibilityPolygon(new Vertex(origin), reduced, out endpoints, out debug);
        public List<Vertex> GenerateVisibilityPolygon(Vertex originVert, bool reduced, out List<Vertex> endpoints, out List<(Vector, Vector)> debug)
        {
            List<(Vector, Vector)> debugOut = new List<(Vector, Vector)>();

            Vector origin = originVert.vector;

            List<Vertex> visibilityGraph = new List<Vertex>();
            List<Vertex> allPolygonVertices = this.allPolygonVertices.Where(x => x != originVert).ToList();

            double angle = 0;
            double GetAngle() => angle;

            // Edges are kept as the vertex with the lower index of the two defining vertices
            IComparer<Vertex> comparer = Comparer<Vertex>.Create((a, b) 
                => CalculateDistance(a, origin, GetAngle()).Let(x => x == 0 ? double.PositiveInfinity : x).CompareTo(CalculateDistance(b, origin, GetAngle().Let(x => x == 0 ? double.PositiveInfinity : x))));
            //SortedSet<Vertex> intersections = new SortedSet<Vertex>(comparer);
            List<Vertex> intersections = new List<Vertex>();

            foreach (Vertex polygonVertex in allPolygonVertices)
            {
                Vector a = polygonVertex.vector - origin;
                Vector b = polygonVertex.Next.vector - origin;

                if (a.y * b.y < 0 && CalculateDistance(polygonVertex, origin, 0) >= 0)
                {
                    intersections.AddSorted(polygonVertex, comparer);
                }
            }

            endpoints = GetEndpoints(origin).Select(x => new Vertex(x)).ToList();
            Dictionary<Vertex, double> angles = 
                allPolygonVertices
                .Concat(endpoints)
                .ToDictionary(x => x, x => x.vector.Angle(origin));

            bool IsVisible(Vertex target)
            {
                if (!(originVert.polygon is null))
                {
                    if (originVert.IsNeighbor(target)) return true;
                    if (reduced && originVert.BehindNeighbors(2 * origin - target.vector)) return false;
                    if (originVert.BehindNeighbors(target.vector)) return false;
                }
                if (!(target.polygon is null))
                {
                    if (target.BehindNeighbors(origin)) return false;
                    if (reduced && target.BehindNeighbors(2 * target.vector - origin)) return false;
                    if (target.polygon.Intersects(origin, target.vector)) return false;
                }

                if (intersections.Count != 0) {
                    //return !intersections.Any(x => Vector.IntersectingLines(origin, target.vector, x.vector, x.Next.vector));
                    //.Let(x => x == 0 ? double.PositiveInfinity : x)
                    //var x = intersections.MinValue(x => CalculateDistance(x, origin, GetAngle())).value;
                    //return !Vector.IntersectingLines(origin, target.vector, x.vector, x.Next.vector);
                    var x = intersections.First();
                    return !Vector.IntersectingLines(origin, target.vector, x.vector, x.Next.vector);
                }

                return true;
            }

            double edge = Math.PI * (n++/2)*0.1d;
            Vector dir = new Vector(Math.Cos(edge), Math.Sin(edge));
            //Vector dirCrossed = new Vector(dir.y, -dir.x);
            debugOut.Add((origin, origin + dir * 9999));
            List<Vertex> delta = new List<Vertex>();
            foreach ((Vertex vert, double currentAngle) in angles.OrderBy(x => x.Value))
            {
                double prevAngle = angle;
                angle = currentAngle;
                if (!(vert.polygon is null))
                {
                    Vertex previous = vert.Previous;
                    if (previous.vector == origin || Vector.Orientation(previous.vector, vert.vector, origin) != Vector.VectorOrder.Clockwise) intersections.Remove(previous);
                    //if (previous == originVert || MathHelper.GetAngleSide(angles[previous], currentAngle) <= 0) intersections.RemoveSorted(previous, comparer);
                    else delta.Add(previous);

                    Vertex next = vert.Next;
                    if (vert.Next.vector == origin || Vector.Orientation(vert.Next.vector, vert.vector, origin) != Vector.VectorOrder.Clockwise) intersections.Remove(vert);
                    //if (next == originVert || MathHelper.GetAngleSide(angles[next], currentAngle) <= 0) intersections.RemoveSorted(vert, comparer);
                    else delta.Add(vert);
                }

                if (IsVisible(vert)) visibilityGraph.Add(vert);

                if (angle != currentAngle)
                {
                    //if (angle < edge && edge <= currentAngle && intersections.Count != 0) debugOut.Add((intersections.First().vector, intersections.First().Next.vector));
                    if (prevAngle < edge && edge <= currentAngle) debugOut.AddRange(intersections.Select(x => (x.vector, x.Next.vector)));
                    //if (angle < edge && edge <= currentAngle) debugOut.AddRange(intersections.Select(x => CalculateDistance(x, origin, edge).Let(y => (origin + dir * y - dirCrossed * 8, origin + dir * y + dirCrossed * 8))));

                    delta.ForEach(x => intersections.AddSorted(x, comparer));
                    delta.Clear();
                }
            }

            var polygon = visibilityGraph.Distinct().ToList();
            debug = debugOut;
            return polygon;
        }

        public Dictionary<Vertex, List<Vertex>> GenerateVisibilityGraph(bool reduced, out List<Vertex> endpoints, out List<(Vector, Vector)> debug)
        {
            var debugOut = new List<(Vector, Vector)>();
            var endpointsOut = new List<Vertex>();
            var graph =
                allPolygonVertices
                .Concat(new[] { startingPosition })
                .ToDictionary(x => x, x =>
                {
                    var polygon = GenerateVisibilityPolygon(x, reduced, out var newEndpoints, out var newDebug);
                    debugOut.AddRange(newDebug);
                    endpointsOut.AddRange(newEndpoints);
                    return polygon;
                });

            debug = debugOut;
            endpoints = endpointsOut;
            return graph;
        }

        public Dictionary<Vertex, Vertex> GenerateDijkstraHeuristic(bool reduced, out Dictionary<Vertex, Dictionary<Vertex, double>> visitedNodes, out List<Vertex> endpoints, out List<(Vector, Vector)> debug)
        {
            List<Vertex> allVertices = allPolygonVertices.Concat(new[] { startingPosition }).ToList();

            var debugOut = new List<(Vector, Vector)>();
            var endpointsOut = new List<Vertex>();

            Dictionary<Vertex, Func<Dictionary<Vertex, double>>> graph = 
                allVertices.ToDictionary(x => x, x =>
                (Func<Dictionary<Vertex, double>>)(() => 
                    {
                        var polygon = GenerateVisibilityPolygon(x, reduced, out var newEndpoints, out var newDebug);
                        debugOut.AddRange(newDebug);
                        endpointsOut.AddRange(newEndpoints);
                        return polygon.ToDictionary(y => y, y => y.vector.Distance(x.vector));
                    }
                ));

            var dijkstra = Dijkstra.GenerateDijkstraHeuristicLazy(startingPosition, graph, endpointsOut, out visitedNodes);
            debug = debugOut;
            endpoints = endpointsOut;
            return dijkstra;
        }

        public List<Vertex> GetOptimalPath(out List<(Vector, Vector)> debug)
        {
            var heuristic = GenerateDijkstraHeuristic(true, out var visitedNodes, out var endpoints, out debug);

            Dictionary<Vertex, double> times = endpoints.Where(x => heuristic.ContainsKey(x)).ToDictionary(x => x, 
                x => Dijkstra.GetPathLength(startingPosition, x, heuristic, visitedNodes) * characterSpeed - GetBusDuration(x.vector) * busSpeed);

            return Dijkstra.GetPath(startingPosition, times.MinValue(x => x.Value).value.Key, heuristic);
        }

        public bool PossiblePath(Vector a, Vector b) => polygons.All(x => !x.Intersects(a, b));

        public IEnumerable<Vector> GetEndpoints(Vector dot)
        {
            for (int i = 0; i < busPath.Length - 1; i++)
            {
                Vector pathToBus = dot - busPath[i];
                Vector busPathNormal = busPath[i + 1] - busPath[i];

                double dotProduct = Vector.Dot(pathToBus, busPathNormal);
                double distance = dotProduct / busPathNormal.MagnitudeSquared();
                distance += (dot - (busPath[i] + busPathNormal * distance)).Magnitude() * busApproachConstant / busPathNormal.Magnitude();

                yield return busPath[i] + busPathNormal * (distance < 0 ? 0 : distance > 1 ? 1 : distance);
            }
        }

        public double GetBusDuration(Vector vec)
        {
            double distance = 0;
            for (int i = 0; i < busPath.Length - 1; i++)
            {
                (Vector start, Vector end) = (busPath[i], busPath[i + 1]);

                double segmentLength = start.DistanceSquared(end);
                distance += segmentLength;

                if (Vector.Orientation(start, end, vec) != Vector.VectorOrder.Colinear) continue;

                return distance - end.Distance(vec);
            }
            return double.PositiveInfinity;
        }
    }
}
