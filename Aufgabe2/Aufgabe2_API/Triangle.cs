using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aufgabe2_API
{
    public struct TriangleArchetype
    {
        public double[] angles, lengths;

        public TriangleArchetype(Triangle triangle)
        {
            lengths = new[]
            {
                triangle.a.Distance(triangle.b),
                triangle.b.Distance(triangle.c),
                triangle.c.Distance(triangle.a),
            };

            angles = new[]
            {
                Math.Acos((lengths[1]*lengths[1]+lengths[2]*lengths[2]-lengths[0]*lengths[0]) / (2*lengths[1]*lengths[2])),
                Math.Acos((lengths[0]*lengths[0]+lengths[2]*lengths[2]-lengths[1]*lengths[1]) / (2*lengths[0]*lengths[2])),
                Math.Acos((lengths[1]*lengths[1]+lengths[1]*lengths[1]-lengths[2]*lengths[2]) / (2*lengths[0]*lengths[1])),
            };
        }

        public TriangleArchetype Turn(int amount) => this.Let(@this => new TriangleArchetype
            {
                angles  = Enumerable.Range(0, 3).Select(z => @this.angles [(z + amount) % 3]).ToArray(),
                lengths = Enumerable.Range(0, 3).Select(z => @this.lengths[(z + amount) % 3]).ToArray()
            });
    }

    public class Triangle
    {
        public Vector a, b, c;

        public Triangle(Vector a, Vector b, Vector c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }

        public Triangle(TriangleArchetype archetype, Vector positionOffset, double angleOffset)
        {
            a = positionOffset;
            b = a + new Vector(archetype.angles[0] + angleOffset) * archetype.lengths[0];
            c = b + new Vector(archetype.angles[0] - archetype.angles[1] + angleOffset) * archetype.lengths[1];
            Debug.Assert(a.Approx(c + new Vector(archetype.angles[0] + archetype.angles[1] + archetype.angles[2] + angleOffset) * archetype.lengths[2], 1E-10));
        }
    }
}
