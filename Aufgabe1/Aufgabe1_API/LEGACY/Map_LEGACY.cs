using SomeExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aufgabe1_API
{
    public class Map_LEGACY
    {
        public Vector[][] polygons;
        public Vector[] busPath;
        public Vector startingPosition;

        public double busSpeed;
        public double characterSpeed;

        public Navmap navmap;

        public Map_LEGACY(string[] lines)
        {
            int polygonCount = int.Parse(lines[0]);
            polygons = new Vector[polygonCount][];

            for (int i = 0; i < polygonCount; i++)
            {
                int[] polygon = lines[i + 1].Split().Select(x => int.Parse(x)).ToArray();
               polygons[i] = new Vector[polygon[0]];

                for (int j = 0; j < polygon[0]; j++) polygons[i][j] = new Vector(polygon[j*2+1], polygon[j*2+2]);
            }

            int[] start = lines[polygonCount + 1].Split().Select(x => int.Parse(x)).ToArray();
            startingPosition = new Vector(start[0], start[1]);

            busPath = new Vector[] { new Vector(0,0), new Vector(0, 10000) };
        }

        public IEnumerable<Vector> GenerateNavmap()
        {
            List<Vector> allDots = new List<Vector>();
            Dictionary<Vector, (Vector, Vector)> neighbours = new Dictionary<Vector, (Vector, Vector)>();
            foreach (var poly in polygons)
            {
                for (int i = 0; i < poly.Length; i++)
                {
                    (Vector, Vector) neighbour = (poly[(i - 1 + poly.Length) % poly.Length], poly[(i + 1) % poly.Length]);
                    neighbours.Add(poly[i], neighbour);
                    allDots.Add(poly[i]);// + Vector.Cross(neighbour.Item1 - poly[i], neighbour.Item2 - poly[i]).Normalize() * 1);// * (Vector.Orientation(neighbour.Item1, poly[i], neighbour.Item2) == Vector.VectorOrder.Counterclockwise ? -1 : 1));
                }
            }
            
            allDots.Add(startingPosition);
            
            Dictionary<Vector, List<Vector>> nodes = new Dictionary<Vector, List<Vector>>(); 

            for (int i = 0; i < allDots.Count; i++)
            {
                Vector vec = allDots[i];
                List<Vector> connections = nodes.GetValueOrAddDefault(vec, new List<Vector>());//[vec] ?? (nodes[vec] = new List<Vector>());
                var neighbour = neighbours.GetValueOrDefault(vec, (null, null));

                Vector other;
                for (int j = i + 1; j < allDots.Count; j++)
                {
                    if (connections.Contains(other = allDots[j])) continue;
                    if (neighbour.Item1 == other || neighbour.Item2 == other || PossiblePath(vec, other))
                    {
                        nodes.GetValueOrAddDefault(vec, new List<Vector>()).AddIfNew(vec);
                        connections.AddIfNew(other);
                    }
                }
                foreach (Vector endPoint in GetExtra(vec))
                {
                    if (PossiblePath(vec, endPoint))
                    {
                        (nodes[endPoint] = new List<Vector>()).AddIfNew(vec);
                        connections.AddIfNew(endPoint);
                        yield return endPoint;
                    }
                }
            }

            navmap = new Navmap(nodes);
            navmap.heuristic = navmap.GenerateDijkstraHeuristic(startingPosition);
        }

        public IEnumerable<Vector> GetExtra(Vector dot)
        {
            for (int i = 0; i < busPath.Length - 1; i++)
            {
                Vector pathToBus = dot - busPath[i];
                Vector busPathNormal = (busPath[i + 1] - busPath[i]).Normalize();

                double distance = Vector.Dot(pathToBus, busPathNormal) / busPathNormal.MagnitudeSquared();

                yield return busPath[i] + busPathNormal * distance;
            }
        }

        public void Solve()
        {

        }

        /// <summary>
        /// Checks whether a point is inside a polygon, using the even-odd rule
        /// </summary>
        public bool PointInPolygon(Vector point, Vector[] poly)
        {
            Vector rayOrigin = new Vector(poly.Min(x => x.x), poly.Min(x => x.y));

            int intersectionCounter = poly.Contains(rayOrigin) ? 1 : 0;
            for (int i = 0; i < poly.Length; i++)
                if (Vector.IntersectingLines(rayOrigin, point, poly[i], poly[(i + 1) % poly.Length]))
                    intersectionCounter++;

            return intersectionCounter % 2 == 1;
        }

        public bool PossiblePath(Vector a, Vector b)
        {
            foreach (var poly in polygons)
            {
                for (int i = 0; i < poly.Length; i++)
                    if (Vector.IntersectingLines(a, b, poly[i], poly[(i + 1) % poly.Length]))
                        return false;

                if (PointInPolygon((a + b) / 2, poly))
                    return false;
            }
            return true;
        }
    }
}
