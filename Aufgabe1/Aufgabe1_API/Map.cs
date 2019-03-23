using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace Aufgabe1_API
{
    public class Map
    {
        public Polygon[] polygons;
        public Vector[] busPath;
        public Vector startingPosition;
        public List<Vertex> allDots;
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
            startingPosition = new Vector(start[0], start[1]);

            busPath = new Vector[] { new Vector(0, 0), new Vector(0, 10000) };

            busSpeed = 30;
            characterSpeed = 15;

            busApproachConstant = characterSpeed / Math.Sqrt(busSpeed * busSpeed - characterSpeed * characterSpeed);
            allDots = new List<Vertex>();
            foreach (Polygon polygon in polygons)
            {
                polygon.FixDirection();
                for (int i = 0; i < polygon.Length; i++) allDots.Add(polygon[i]);
            }
        }

        public Map(Polygon[] polygons, Vector[] busPath, Vector startingPosition, double busSpeed, double characterSpeed)
        {
            this.polygons = polygons;
            this.busPath = busPath;
            this.startingPosition = startingPosition;
            this.busSpeed = busSpeed;
            this.characterSpeed = characterSpeed;
            busApproachConstant = characterSpeed / Math.Sqrt(busSpeed * busSpeed - characterSpeed * characterSpeed);
            allDots = new List<Vertex>();
            foreach (Polygon polygon in polygons)
            {
                polygon.FixDirection();
                for (int i = 0; i < polygon.Length; i++) allDots.Add(polygon[i]);
            }
        }

        public double CalculateDistance(Vertex vertex, Vector origin, double angle)
        {
            Vector a = vertex.vector - origin;
            Vector b = vertex.Next.vector - origin;
            return ((a.y - b.y) * b.x - (a.x - b.x) * b.y) / (Math.Cos(angle) * (a.y - b.y) - Math.Sin(angle) * (a.x - b.x));
        }

        public Dictionary<Vector, double> GenerateVisibilityPolygon(Vector origin, out List<(Vector, Vector)> debug) => GenerateVisibilityPolygon(new Vertex(origin, null, 0), out debug);
        public Dictionary<Vector, double> GenerateVisibilityPolygon(Vertex originVert, out List<(Vector, Vector)> debug)
        {
            List<(Vector, Vector)> debugOut = new List<(Vector, Vector)>();

            Vector origin = originVert.vector;

            List<Vertex> visibilityGraph = new List<Vertex>();
            List<Vertex> allDots = this.allDots.Where(x => x.vector != origin).ToList();

            double angle = 0;
            double GetAngle() => angle;

            // Edges are kept as the vertex with the lower index of the two defining vertices
            SortedSet<Vertex> intersections = new SortedSet<Vertex>(Comparer<Vertex>.Create((a, b) => CalculateDistance(a, origin, GetAngle()).CompareTo(CalculateDistance(b, origin, GetAngle()))));
            //List<Vertex> intersections = new List<Vertex>();

            foreach (Vertex polygonVertex in allDots)
            {
                Vector a = polygonVertex.vector - origin;
                Vector b = polygonVertex.Next.vector - origin;

                // Intersections with x+ Axis
                /*if (a.y * b.y <= 0
                 && (a.x > 0 || b.x > 0)
                 && (a.y == b.y
                   ? a.y == 0
                   : (a.x >= a.y * (b.x - a.x) / (b.y - a.y))))*/
                if (a.y * b.y <= 0 && CalculateDistance(polygonVertex, origin, 0) >= 0)
                {
                    intersections.Add(polygonVertex);
                }
            }

            //return new Dictionary<Vector, double> { { intersections.Count == 0 ? origin : (intersections.Min.first.vector + intersections.Min.second.vector) / 2, 0 } };
            //return intersections.Select(x => (x.vector + x.Next.vector)/2).Distinct().ToDictionary(x => x, x => 0d);

            Dictionary<Vertex, double> angles = 
                allDots
                .Concat(
                    GetEndpoints(origin)
                    .Select(x => new Vertex(x, null, 0)))
                .ToDictionary(x => x, x => x.vector.Angle(origin));

            bool IsVisible(Vertex target)
            {
                // Line 1 of VISIBLE(wi)
                if (!(originVert.polygon is null))
                {
                    if (originVert.IsNeighbor(target)) return true;
                    if (originVert.BehindNeighbors(target.vector)) return false;
                }
                if (!(target.polygon is null))
                {
                    if (target.BehindNeighbors(origin)) return false;
                    if (target.polygon.Intersects(origin, target.vector)) return false;
                }

                if (intersections.Count != 0) {
                    //Vertex min = intersections.Min;
                    //if (CalculateDistance(min, origin, GetAngle()) < origin.Distance(target.vector)) return false;
                    //return intersections.All(x => angles[x] == GetAngle() || CalculateDistance(x, origin, GetAngle()) >= target.vector.Distance(origin));
                    return !intersections.Any(x => Vector.IntersectingLines(origin, target.vector, x.vector, x.Next.vector));
                }

                return true;
            }

            //double edge = Math.PI * 1.5d;
            //debugOut.Add((origin, origin + new Vector(Math.Cos(edge), Math.Sin(edge)) * 999));
            foreach ((Vertex vert, double currentAngle) in angles.OrderBy(x => x.Value))
            {
                //if (angle < edge && edge <= currentAngle) debugOut.AddRange(intersections.Select(x => (x.vector, x.Next.vector)));
                angle = currentAngle;

                if (IsVisible(vert)) visibilityGraph.Add(vert);

                if (vert.polygon is null) continue;

                Vertex previous = vert.Previous;
                //if (previous.vector == origin || Vector.Orientation(previous.vector, vert.vector, origin) != Vector.VectorOrder.Clockwise) intersections.Remove(previous);
                if (previous.vector == origin || MathHelper.GetAngleSide(angles[previous], currentAngle) <= 0) intersections.Remove(previous);
                else intersections.Add(previous);

                //if (vert.Next.vector == origin || Vector.Orientation(vert.Next.vector, vert.vector, origin) != Vector.VectorOrder.Clockwise) intersections.Remove(vert);
                if (vert.Next.vector == origin || MathHelper.GetAngleSide(angles[vert.Next], currentAngle) <= 0) intersections.Remove(vert);
                else intersections.Add(vert);
            }

            debug = debugOut;
            return visibilityGraph.Distinct().ToDictionary(x => x.vector, x => x.vector.Distance(origin));
        }

        public Dictionary<Vector, Dictionary<Vector, double>> GenerateVisibilityGraph(out List<(Vector, Vector)> debug)
        {
            var debugOut = new List<(Vector, Vector)>();
            var output =
                allDots
                .Select(x =>
                {
                    var polygon = GenerateVisibilityPolygon(x, out var add);
                    debugOut.AddRange(add);
                    return new KeyValuePair<Vector, Dictionary<Vector, double>>(x.vector, polygon);
                })
                .Concat(new[] { new KeyValuePair<Vector, Dictionary<Vector, double>>(startingPosition, GenerateVisibilityPolygon(startingPosition, out _)) })
                .ToDictionary(x => x.Key, x => x.Value);

            //foreach (Vertex vert in allDots) debugOut.Add((vert.vector, vert.vector + vert.Normal * 5));
            //foreach (Vertex vert in allDots) if (vert.IsConvex) debugOut.Add((vert.vector - vert.Normal * 5, vert.vector + vert.Normal * 5));
            /*foreach (Vertex vert in allDots)
            {
                Vector vec = vert.Normal;
                for (int i = 0; i < 4; i++) if (vert.BehindNeighbors((vec = new Vector(vec.y, -vec.x)) + vert.vector)) debugOut.Add((vert.vector, vert.vector + vec * 5));
            }*/

            debug = debugOut;
            return output;
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
    }
}
