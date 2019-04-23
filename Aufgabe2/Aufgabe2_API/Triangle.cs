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
            angles = new double[3];
            lengths = new double[3];

            angles[0] = triangle.a.AngleTo(triangle.b);
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

        public Triangle(Vector positionOffset, double angleOffset, TriangleArchetype archetype)
        {
            a = positionOffset;
            b = a + new Vector(archetype.angles[0] + angleOffset) * archetype.lengths[0];
            c = b + new Vector(archetype.angles[1] + angleOffset) * archetype.lengths[1];
            Debug.Assert(a.Approx(c + new Vector(archetype.angles[2] + angleOffset) * archetype.lengths[2], 1E-20));
        }
    }
}
