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

            /*angles = new[]
            {
                Math.Acos((lengths[0]*lengths[0]+lengths[2]*lengths[2]-lengths[1]*lengths[1]) / (2*lengths[0]*lengths[2])),
                Math.Acos((lengths[1]*lengths[1]+lengths[1]*lengths[1]-lengths[2]*lengths[2]) / (2*lengths[0]*lengths[1])),
                Math.Acos((lengths[1]*lengths[1]+lengths[2]*lengths[2]-lengths[0]*lengths[0]) / (2*lengths[1]*lengths[2])),
            };*/
            angles = new[]
            {
                MathHelper.SmallerAngleSide(triangle.a.Angle(triangle.b) - triangle.a.Angle(triangle.c)),
                MathHelper.SmallerAngleSide(triangle.b.Angle(triangle.a) - triangle.a.Angle(triangle.c)),
                MathHelper.SmallerAngleSide(triangle.c.Angle(triangle.a) - triangle.a.Angle(triangle.b)),
            };
        }

        public TriangleArchetype Turn(int amount) => this.Let(@this => new TriangleArchetype // Lambdas can't use this => pass it as an argument
        {
            angles = Enumerable.Range(0, 3).Select(z => @this.angles[(z + amount) % 3]).ToArray(),
            lengths = Enumerable.Range(0, 3).Select(z => @this.lengths[(z + amount) % 3]).ToArray()
        });
        public TriangleArchetype Mirror() => this.Let(@this => new TriangleArchetype
        {
            angles = Enumerable.Range(0, 3).Select(z => @this.angles[(3 - z) % 3]).ToArray(),
            lengths = Enumerable.Range(0, 3).Select(z => @this.lengths[(3 - z) % 3]).ToArray()
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

        /*public Triangle(TriangleArchetype archetype, Vector positionOffset, double angleOffset)
        {
            a = positionOffset;
            b = a + new Vector(archetype.angles[0] + angleOffset) * archetype.lengths[0];
            c = b + new Vector(Math.PI + archetype.angles[0] + archetype.angles[1] + angleOffset) * archetype.lengths[1];
            Debug.Assert(a.Approx(c + new Vector(archetype.angles[0] + archetype.angles[1] + archetype.angles[2] + angleOffset) * archetype.lengths[2], 1E-10));
        }*/

        public Triangle(TriangleArchetype archetype, Vector positionOffset, double angleOffset)
        {
            a = positionOffset;
            b = a + new Vector(archetype.angles[0] + angleOffset) * archetype.lengths[0];
            c = a + new Vector(angleOffset) * archetype.lengths[2];
            //Debug.Assert(a.Approx(c + new Vector(Math.PI + angleOffset - archetype.angles[1]) * archetype.lengths[1], 1E-10));
        }
    }
}
