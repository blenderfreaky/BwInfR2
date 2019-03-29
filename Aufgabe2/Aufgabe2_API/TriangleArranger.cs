using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aufgabe2_API
{
    public static class TriangleArranger
    {
        public static List<Triangle> ArrangeTriangles(List<TriangleArchetype> triangleArchetypes)
        {
            List<TriangleArchetype> byMinAngles = triangleArchetypes.Select(x => new TriangleArchetype { angles = x.angles, lengths = x.lengths})
        }
    }
}
