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
        {
            List<TriangleArchetype> byMinAngles = triangleArchetypes
                .Select(x => Enumerable.Range(0, 3)
                    .MinValue(y => x.angles[y])
                    .Let(y => new TriangleArchetype {
                        angles = Enumerable.Range(0, 3)
                            .Select(z => x.angles[(z + y.value) % 3])
                            .ToArray(),
                        lengths = Enumerable.Range(0, 3)
                            .Select(z => x.lengths[(z + y.value) % 3])
                            .ToArray()
                    }))
                .OrderBy(x => x.angles[0])
                .ToList();

            debug = new List<(Vector, Vector)>();

            List<Triangle> triangles = new List<Triangle>();
        }
    }
}
