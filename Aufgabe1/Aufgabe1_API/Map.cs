using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aufgabe1_API
{
    public class Map
    {
        public Vector[][] polygons;

        public Vector[] busPath;
        public double busSpeed;

        public Vector startingPosition;
        public double characterSpeed;
        public double securityDistance;

        public Navmap navmap;

        public void GenerateNavmap()
        {
            List<Vector> allDots = polygons.SelectMany(x => x).ToList();
            allDots.Add(startingPosition);
            foreach (Vector dot in new List<Vector>(allDots))
            {
                for (int i = 0; i < busPath.Length - 1; i++)
                {
                    Vector a = busPath[i];
                    Vector b = busPath[i+1];

                    Vector AP = dot - a;
                    Vector AB = b - a;

                    double magnitudeAB = AB.MagnitudeSquared();
                    double ABAPproduct = Vector.Dot(AP, AB);    //The DOT product of a_to_p and a_to_b     
                    double distance = ABAPproduct / magnitudeAB; //The normalized "distance" from a to your closest point  

                    if (distance < 0)     //Check if P projection is over vectorAB     
                    {
                        return a;

                    }
                    else if (distance > 1)
                    {
                        return b;
                    }
                    else
                    {
                        return a + AB * distance;
                    }
                }
            }
            Dictionary<Vector, List<Vector>> nodes = allDots.ToDictionary(x => x, x => allDots.Where(y => x != y && PossiblePath(x, y)).ToList());
            navmap = new Navmap(nodes);
            navmap.heuristic = navmap.GenerateDijkstraHeuristic(startingPosition);
        }

        public bool PossiblePath(Vector a, Vector b)
        {
            foreach (var poly in polygons) for (int i = 0; i <= poly.Length; i++) if (Vector.IntersectingLines(a, b, poly[i], poly[(i+1)%poly.Length])) return false;
            return true;
        }
    }
}
