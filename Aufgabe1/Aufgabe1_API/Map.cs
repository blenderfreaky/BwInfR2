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
                double[] polygon = lines[i + 1].Split().Select(x => double.Parse(x)).ToArray();
                var vertices = new Vector[(int)polygon[0]];

                for (int j = 0; j < polygon[0]; j++) vertices[j] = new Vector(polygon[j * 2 + 1], polygon[j * 2 + 2]);

                polygons[i] = new Polygon(vertices);
            }

            allPolygonVertices = polygons.SelectMany(x => x).ToList();

            double[] start = lines[polygonCount + 1].Split().Select(x => double.Parse(x)).ToArray();
            startingPosition = new Vertex(new Vector(start[0], start[1]));

            if (lines.Length <= polygonCount + 2 || string.IsNullOrWhiteSpace(lines[polygonCount + 2]))
            {
                busPath = new Vector[] { new Vector(0, 0), new Vector(0, Math.Sqrt(double.MaxValue)) };

                SetSpeed(15, 30);
            }
            else
            {
                double[] path = lines[polygonCount + 2].Split().Select(x =>
                    x == "inf"
                  ? Math.Sqrt(double.MaxValue)
                  : x == "-inf"
                    ? Math.Sqrt(double.MinValue)
                    : double.Parse(x)).ToArray();
                busPath = new Vector[(int)path[0]];

                for (int j = 0; j < path[0]; j++) busPath[j] = new Vector(path[j * 2 + 1], path[j * 2 + 2]);

                double[] speeds = lines[polygonCount + 3].Split().Select(x => double.Parse(x)).ToArray();

                SetSpeed(speeds[0], speeds[1]);
            }
        }

        public void SetSpeed(double characterSpeed, double busSpeed)
        {
            this.characterSpeed = characterSpeed;
            this.busSpeed = busSpeed;
            busApproachConstant = characterSpeed / Math.Sqrt(busSpeed * busSpeed - characterSpeed * characterSpeed);
        }

        public Map(Polygon[] polygons, Vector[] busPath, Vector startingPosition, double busSpeed, double characterSpeed)
        {
            this.polygons = polygons;
            this.busPath = busPath;
            this.startingPosition = new Vertex(startingPosition);

            allPolygonVertices = polygons.SelectMany(x => x).ToList();

            SetSpeed(characterSpeed, busSpeed);
        }

        public IEnumerable<Vector> GetEndpoints(Vector dot)
        {
            for (int i = 0; i < busPath.Length - 1; i++)
            {
                Vector pathToBus = dot - busPath[i];
                Vector busPathNormal = busPath[i + 1] - busPath[i];

                double dotProduct = Vector.Dot(pathToBus, busPathNormal);
                double distance = dotProduct / busPathNormal.MagnitudeSquared();
                distance += (dot - (busPath[i] + busPathNormal * distance)).Magnitude() * busApproachConstant / busPathNormal.Magnitude();

                yield return busPath[i] + busPathNormal * MathHelper.Clamp(distance, 0, 1);
            }
        }

        public double GetBusLength(Vector vec)
        {
            double distance = 0;
            for (int i = 0; i < busPath.Length - 1; i++)
            {
                (Vector start, Vector end) = (busPath[i], busPath[i + 1]);

                double segmentLength = start.Distance(end);

                if (Vector.OrientationApprox(start, end, vec, epsilon) == Vector.VectorOrder.Collinear) return distance + start.Distance(vec);

                distance += segmentLength;
            }
            return double.NaN;
        }

        public double CalculateDistanceAtAngle(Vertex vertex, Vector origin, double angle)
        {
            Vector a = vertex.vector - origin;
            Vector b = vertex.Next.vector - origin;
            return ((a.y - b.y) * b.x - (a.x - b.x) * b.y) / (Math.Cos(angle) * (a.y - b.y) - Math.Sin(angle) * (a.x - b.x));
        }

        public double epsilon = 1E-15;

        public List<Vertex> GenerateVisibilityPolygon(Vector origin, out List<Vertex> endpoints, out List<(Vector, Vector)> debug) => GenerateVisibilityPolygon(new Vertex(origin), out endpoints, out debug);
        public List<Vertex> GenerateVisibilityPolygon(Vertex originVertex, out List<Vertex> endpoints, out List<(Vector, Vector)> debug)
        {
            List<(Vector, Vector)> debugOut = new List<(Vector, Vector)>();

            Vector origin = originVertex.vector;

            List<Vertex> visibilityGraph = new List<Vertex>();
            List<Vertex> allPolygonVertices = this.allPolygonVertices.Where(x => !x.vector.Approx(origin, epsilon)).ToList();

            endpoints = GetEndpoints(origin).Select(x => new Vertex(x)).ToList();
            Dictionary<Vertex, double> angles =
                allPolygonVertices
                .Concat(endpoints)
                .ToDictionary(x => x, x => x.vector.Angle(origin));

            // Edges are kept as the vertex with the lower index of the two defining vertices
            IComparer<Vertex> comparer = Comparer<Vertex>.Create((a, b) =>
            {
                if (ReferenceEquals(a, b) || a == b) return 0;

                // Based on https://github.com/trylock/visibility/blob/master/visibility/visibility.hpp Lines 17-89

                Vector a1 = a.vector;
                Vector a2 = a.Next.vector;
                Vector b1 = b.vector;
                Vector b2 = b.Next.vector;

                // If there are common endpoints, let them be a1 and b1
                if (a2.Approx(b1, epsilon) || a2.Approx(b2, epsilon)) (a1, a2) = (a2, a1);
                if (a1.Approx(b2, epsilon)) (b1, b2) = (b2, b1);

                if (a1.Approx(b1, epsilon)) // If there are common endpoints a1 and b1 this is true
                {
                    if (a2.Approx(b2, epsilon)) return 0; // Same Lines
                    // a and b are on opposing sides of ray from origin to shared point (current ray in sweep-line algorithm)
                    if (Vector.OrientationApprox(origin, a1, b2, epsilon) != Vector.OrientationApprox(origin, a1, a2, epsilon)) 
                    {
                        throw new Exception("Attempted Change to early");
                    }

                    // b2 is on the same side of a as origin => b is below a
                    return Vector.OrientationApprox(a1, a2, b2, epsilon) == Vector.OrientationApprox(a1, a2, origin, epsilon) ? 1 : -1; 
                }
                else
                {
                    var ba1 = Vector.OrientationApprox(b1, b2, a1, epsilon);
                    var ba2 = Vector.OrientationApprox(b1, b2, a2, epsilon);

                    // Line Segments are on a shared line but don't have common endpoints 
                    if (ba2 == Vector.VectorOrder.Collinear && ba1 == Vector.VectorOrder.Collinear)
                    { 
                        // Since the line segments are on a shared line, only one point needs to be compared
                        return origin.DistanceSquared(a1).CompareTo(origin.DistanceSquared(b1));
                    }
                    else if (ba1 == ba2 // a1 and a2 are entirely above or below b
                          || ba1 == Vector.VectorOrder.Collinear || ba2 == Vector.VectorOrder.Collinear) // or a has one point on b => a is entirely above or below b
                    { 
                        var bOrigin = Vector.OrientationApprox(b1, b2, origin, epsilon);
                        return bOrigin == ba1 // a1 is on the same side of b as origin => a is closer 
                            || bOrigin == ba2 // a2 is on the same side of b as origin => a is closer // Check both as one might be collinear
                            ? -1 : 1;
                    }
                    else // a1 and a2 are on opposing sides of b (a crosses the infinite line containing b) => b is entirely above or below a
                    {
                        return Vector.OrientationApprox(a1, a2, origin, epsilon) == Vector.OrientationApprox(a1, a2, b1, epsilon) // b1 is on the same side of a as origin => b is below a
                             ? 1 : -1;
                    }
                }
            });
            SortedSet<Vertex> intersections = new SortedSet<Vertex>(comparer);

            foreach (Vertex polygonVertex in allPolygonVertices)
            {
                if ((polygonVertex.Next.vector - origin).y * (polygonVertex.vector - origin).y < -epsilon
                 && CalculateDistanceAtAngle(polygonVertex, origin, 0) >= epsilon)
                {
                    intersections.Add(polygonVertex);
                }
            }

            List<(double min, double max)> leftTouching = new List<(double, double)>();
            List<(double min, double max)> rightTouching = new List<(double, double)>();

            List<(double min, double max)> GetLeft() => leftTouching;
            List<(double min, double max)> GetRight() => rightTouching;

            bool IsVisible(Vertex target)
            {
                if (!(target.polygon is null))
                {
                    if (!target.BetweenNeighbors(origin)) return false;
                }
                if (!(originVertex.polygon is null))
                {
                    if (!originVertex.BetweenNeighbors(target.vector)) return false;
                    if (originVertex.IsNeighbor(target)) return true; // Neighbours are not always visible in a reduced graph
                }

                if (intersections.Count != 0 &&
                    intersections.First().Let(x => Vector.IntersectingLines(origin, target.vector, x.vector, x.Next.vector))) return false;

                var furthestDistance =
                    GetLeft()
                    .SelectMany(x => GetRight()
                        .Where(y =>
                            (x.min <= y.min && y.min <= x.max)
                         || (x.min <= y.max && y.max <= x.max)
                        ) // Only take intersections
                        .Select(y => Math.Max(x.min, y.min))
                    )
                    .Let(blocked => blocked.Any() ? blocked.Min() : double.PositiveInfinity);

                if (origin.Distance(target.vector) > furthestDistance) return false;

                return true;
            }

            (Vertex vert, double currentAngle)[] sortedAngles = angles
                .Select(x => (x.Key, x.Value)).ToArray();
            Array.Sort(sortedAngles, Comparer<(Vertex vert, double currentAngle)>.Create((a, b) => a.currentAngle.CompareTo(b.currentAngle)));
            IEnumerable<(Vertex vert, double currentAngle)> sortedAnglesEnum = sortedAngles;

            // Group vertices with the same angle together
            var vertsByAngle = new List<(List<Vertex> vertices, double prevAngle, double angle, double nextAngle)>();
            {
                double angle;
                double prevAngle = 0;
                while (sortedAnglesEnum.Any())
                {
                    angle = sortedAnglesEnum.First().currentAngle;
                    List<Vertex> buffer = sortedAnglesEnum.TakeWhile(x => x.currentAngle == angle).Select(x => x.vert).ToList();
                    sortedAnglesEnum = sortedAnglesEnum.Skip(buffer.Count);
                    vertsByAngle.Add((buffer, prevAngle, angle, sortedAnglesEnum.Any() ? sortedAnglesEnum.First().currentAngle : Math.PI * 2));
                    prevAngle = angle;
                }
            }

            List<Vertex> delta = new List<Vertex>();

            void Add(double currentAngle, double nextAngle, Vertex first, Vertex second)
            {
                if (first.vector.Approx(origin, epsilon) || second.vector.Approx(origin, epsilon)) return; // Already handeled by BetweenNeighbours

                // Collinear lines aren't intersections, only their position on the ray is used
                if (Vector.OrientationApprox(origin, first.vector, second.vector, epsilon) != Vector.VectorOrder.Collinear) delta.Add(first);
                leftTouching.Add(angles[first] == angles[second]
                       ? (origin.DistanceSquared(first.vector), origin.DistanceSquared(second.vector)) // Squaring later is cheaper than Sqrt here
                         .Let(x => x.Item1 < x.Item2 ? x : (x.Item2, x.Item1))
                       : origin.DistanceSquared(first.vector).Let(x => (x, x)));
            }
            void Remove(double prevAngle, double currentAngle, Vertex first, Vertex second)
            {
                if (first.vector.Approx(origin, epsilon) || second.vector.Approx(origin, epsilon)) return; // Already handeled by BetweenNeighbours

                // Collinear lines aren't intersections, only their position on the ray is used
                if (Vector.OrientationApprox(origin, first.vector, second.vector, epsilon) != Vector.VectorOrder.Collinear) intersections.Remove(first);
                rightTouching.Add(angles[first] == angles[second]
                       ? (origin.DistanceSquared(first.vector), origin.DistanceSquared(second.vector)) // Squaring later is cheaper than Sqrt here
                         .Let(x => x.Item1 < x.Item2 ? x : (x.Item2, x.Item1))
                       : origin.DistanceSquared(first.vector).Let(x => (x, x)));
            }

            foreach ((List<Vertex> vertices, double prevAngle, double currentAngle, double nextAngle) in vertsByAngle)
            {
                foreach (Vertex vert in vertices)
                {
                    if (vert.polygon is null) continue;

                    Vertex previous = vert.Previous;
                    if (Vector.Orientation(previous.vector, vert.vector, origin) != Vector.VectorOrder.Clockwise) Remove(prevAngle, currentAngle, previous, vert);
                    else Add(currentAngle, nextAngle, previous, vert);

                    Vertex next = vert.Next;
                    if (Vector.Orientation(next.vector, vert.vector, origin) != Vector.VectorOrder.Clockwise) Remove(prevAngle, currentAngle, vert, next);
                    else Add(currentAngle, nextAngle, vert, next);
                }

                visibilityGraph.AddRange(vertices.Where(IsVisible));

                leftTouching.Clear();
                rightTouching.Clear();

                delta.ForEach(x => intersections.Add(x));
                delta.Clear();
            }

            var polygon = visibilityGraph.Distinct().ToList();
            debug = debugOut;
            return polygon;
        }

        public Dictionary<Vertex, List<Vertex>> GenerateVisibilityGraph(out List<Vertex> endpoints, out List<(Vector, Vector)> debug)
        {
            var debugOut = new List<(Vector, Vector)>();
            var endpointsOut = new List<Vertex>();
            var graph =
                allPolygonVertices
                .Concat(new[] { startingPosition })
                .ToDictionary(x => x, x =>
                {
                    var polygon = GenerateVisibilityPolygon(x, out var newEndpoints, out var newDebug);
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
                        var polygon = GenerateVisibilityPolygon(x, out var newEndpoints, out var newDebug);
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

        public List<Vertex> GetOptimalPath(out double characterLength, out double busLength, out double advantage, out List<(Vector, Vector)> debug)
        {
            var heuristic = GenerateDijkstraHeuristic(true, out var visitedNodes, out var endpoints, out debug);

            IEnumerable<(Vertex vert, double characterLength, double busLength)> times = endpoints
                .Where(x => heuristic.ContainsKey(x))
                .Select(x => 
                    (x, Dijkstra.GetPathLength(startingPosition, x, heuristic, visitedNodes), GetBusLength(x.vector)));

            Vertex min;
            ((min, characterLength, busLength), advantage) = times.MinValue(x => x.characterLength / characterSpeed - x.busLength / busSpeed);
            return Dijkstra.GetPath(startingPosition, min, heuristic);
        }
    }
}
