using System;
using System.Linq;

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
        public Vector this[int index] => MathHelper.PositiveModulo(index, 0, 3).Let(x =>
                x == 0 ? a
            : x == 1 ? b
            : x == 2 ? c
            : throw new InvalidOperationException());

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
            c = a + new Vector(angleOffset) * archetype.lengths[2];
        }

        public bool Intersects(Triangle other)
        {
            var edges = new[] { (a, b), (b, c), (c, a) };
            var otherEdges = new[] { (other.a, other.b), (other.b, other.c), (other.c, other.a) };
            return edges.Any(x => otherEdges.Any(y => Vector.IntersectingLines(x.Item1, x.Item2, y.Item1, y.Item2)));
        }

        public bool Surrounds(Vector other, double epsilon) =>
            (Vector.OrientationApprox(a, b, other, epsilon), Vector.OrientationApprox(b, c, other, epsilon), Vector.OrientationApprox(c, a, other, epsilon))
            .Let(x => x.Item1 == x.Item2 && x.Item2 == x.Item3 && x.Item1 != Vector.VectorOrder.Collinear);
    }
}
