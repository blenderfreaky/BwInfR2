using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aufgabe2_API
{
    public static class TriangleArranger
    {
        public static List<Triangle> ArrangeTriangles(List<TriangleArchetype> triangleArchetypes, out List<(Vector, Vector)> debug)
            => ArrangeTriangles(new List<Triangle>(), triangleArchetypes, out debug);
        private static List<Triangle> ArrangeTriangles(List<Triangle> prev, List<TriangleArchetype> triangleArchetypes, out List<(Vector, Vector)> debug)
        {
            var debugOut = new List<(Vector, Vector)>();

            TriangleArchetype toAdd = triangleArchetypes.First();

            Triangle last = prev.Last();
           // Triangle added = new Triangle(toAdd, new Vector(0, 0));

            //prev.Add

            debug = debugOut;

            return prev;
        }
    }
}
