using System;
using System.Collections.Generic;
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
    }

    public class Triangle
    {
        public Vector a, b, c;

    }
}
