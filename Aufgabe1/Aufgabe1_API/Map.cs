using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public void FixPolygons()
        {
            foreach (Polygon polygon in polygons) polygon.FixDirection();
        }

        private struct SortableEdge : IComparable<SortableEdge>
        {
            public Vertex first, second;
            public double distance;

            public SortableEdge(Vertex first, double distance)
            {
                this.first = first;
                second = first.Next;
                this.distance = distance;
            }

            public SortableEdge(Vertex first, Vector origin)
            {
                this.first = first;
                second = first.Next;

                double distanceSquared = Vector.DistanceSquared(first.vector, second.vector);
                if (distanceSquared == 0) distance = origin.Distance(first.vector);
                else
                {
                    double dot = Vector.Dot(origin - first.vector, second.vector - first.vector);
                    // using 1E-15 instead of double.Epsilon, as double.Epsilon has some very weird behaviour
                    double factor = MathHelper.Clamp(dot / distanceSquared, 1E-15, 1 - 1E-15);

                    distance = (first.vector + factor * (second.vector - first.vector)).DistanceSquared(origin);
                }
            }

            public int CompareTo(SortableEdge other) => distance.CompareTo(other.distance);
            public override bool Equals(object obj) => 
                obj is SortableEdge edge ? edge.first == first
              : obj is Vertex vertex ? vertex == first
              : obj is Vector vector ? vector == first.vector
              : false;

            public override int GetHashCode()
            {
                var hashCode = 893733272;
                hashCode = hashCode * -1521134295 + EqualityComparer<Vertex>.Default.GetHashCode(first);
                hashCode = hashCode * -1521134295 + EqualityComparer<Vertex>.Default.GetHashCode(second);
                hashCode = hashCode * -1521134295 + distance.GetHashCode();
                return hashCode;
            }

            public static implicit operator Vertex(SortableEdge edge) => edge.first;
        }

        public Dictionary<Vector, double> GenerateVisibilityGraph(Vector origin) => GenerateVisibilityGraph(new Vertex(origin, null, 0));
        public Dictionary<Vector, double> GenerateVisibilityGraph(Vertex originVert)
        {
            Vector origin = originVert.vector;

            List<Vector> visibilityGraph = new List<Vector>();
            List<Vertex> allDots = this.allDots.Where(x => x.vector != origin).ToList();
            
            // Edges are kept as the vertex with the lower index of the two defining vertices
            SortedSet<SortableEdge> intersections = new SortedSet<SortableEdge>();

            foreach (Vertex polygonVertex in allDots)
            {
                Vector a = polygonVertex.vector - origin;
                Vector b = polygonVertex.Next.vector - origin;

                // Intersections with x+ Axis
                if (a.y * b.y <= 0
                 && (a.x > 0 || b.x > 0)
                 && (a.y == b.y
                   ? a.y == 0
                   : (a.x >= a.y * (b.x - a.x) / (b.y - a.y))))
                {
                    //intersections.Add(new SortableEdge(polygonVertex, (a.x * (b.y - a.y)) / (a.y * (b.x - a.x))));
                    intersections.Add(new SortableEdge(polygonVertex, origin));
                }
            }

            //return new Dictionary<Vector, double> { { intersections.Count == 0 ? origin : (intersections.Min.first.vector + intersections.Min.second.vector) / 2, 0 } };
            //return intersections.Select(x => (x.first.vector + x.second.vector)/2).Distinct().ToDictionary(x => x, x => 0d);

            Dictionary<Vertex, double> angles = 
                allDots
                .Union(
                    GetEndpoints(origin)
                    .Select(x => new Vertex(x, null, 0)))
                .ToDictionary(x => x, x => x.vector.Angle(origin));
            IEnumerable<(double, IEnumerable<Vertex>)> allDotsByAngles =
                angles
                .Select(x => x.Value)
                .Distinct()
                .OrderBy(x => x)
                .Select(x =>
                    (x,
                    angles
                    .Where(y => y.Value == x)
                    .Select(y => y.Key)));

            foreach ((double currentAngle, IEnumerable<Vertex> vertices) in allDotsByAngles)
            {
                foreach (Vertex polygonVertex in vertices)
                {
                    if (IsVisible(originVert, polygonVertex, intersections)) visibilityGraph.Add(polygonVertex.vector);
                }

                foreach (Vertex polygonVertex in vertices)
                {
                    if (polygonVertex.polygon is null) continue;

                    Vertex previous = polygonVertex.Previous;
                    //if (previous.vector == origin || MathHelper.GetAngleSide(angles[previous], currentAngle) < 0) intersections.RemoveSimilar(previous);
                    if (previous.vector == origin || intersections.ContainsSimilar(previous)) intersections.RemoveSimilar(previous);
                    else intersections.Add(new SortableEdge(previous, origin));

                    //if (polygonVertex.vector == origin || MathHelper.GetAngleSide(angles[polygonVertex], currentAngle) < 0) intersections.RemoveSimilar(polygonVertex);
                    if (polygonVertex.Next.vector == origin || intersections.ContainsSimilar(polygonVertex)) intersections.RemoveSimilar(polygonVertex);
                    else intersections.Add(new SortableEdge(polygonVertex, origin));
                }
            }

            return visibilityGraph.Distinct().ToDictionary(x => x, x => x.Distance(origin));
        }

        private bool IsVisible(Vertex origin, Vertex target, SortedSet<SortableEdge> intersections)
        {
            if (!(origin.polygon is null))
            {
                if (origin.IsNeighbor(target)) return true;
                if (origin.polygon.Intersects(origin.vector, target.vector)) return false;
            }

            if (!(target.polygon is null) && target.BehindNeighbors(origin.vector)) return false;

            SortableEdge? min = intersections.Count == 0 ? (SortableEdge?)null : intersections.Min;
            if (min.HasValue && Vector.IntersectingLines(min.Value.first.vector, min.Value.second.vector, origin.vector, target.vector)) return false;

            return true;
        }

        public Dictionary<Vector, Dictionary<Vector, double>> GenerateNavmap() => allDots
            .Select(x => x.vector)
            .Concat(new[] { startingPosition })
            .ToDictionary(x => x, x => GenerateVisibilityGraph(x));

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
