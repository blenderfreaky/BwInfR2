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
            foreach (Vector vec in polygons.SelectMany(x => x))
            {

            }

            Dictionary<Vector, List<Vector>> nodes = allDots.ToDictionary(x => x, x => allDots.Where(y => x != y && PossiblePath(x, y)).ToList());
            navmap = new Navmap(nodes);
        }

        public bool PossiblePath(Vector a, Vector b)
        {
            foreach (var poly in polygons) for (int i = 0; i <= poly.Length; i++) if (Vector.IntersectingLines(a, b, poly[i], poly[(i+1)%poly.Length])) return false;
            return true;
        }
    }
}
