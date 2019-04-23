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

            Triangle added= ;

            prev.Add

            debug = debugOut;
        }
    }
}
