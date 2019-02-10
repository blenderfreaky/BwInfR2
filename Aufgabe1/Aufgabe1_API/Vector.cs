using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aufgabe1_API
{
    public class Vector
    {
        public double x, y;

        public Vector()
        { 
        }

        public Vector(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public double Distance(Vector other)
        {
            return Math.Sqrt(Math.Pow(x - other.x, 2) + Math.Pow(y - other.y, 2));
        }

        #region Intersection
        //Algorithm from https://bryceboe.com/2006/10/23/line-segment-intersection-algorithm/
        public static bool CounterclockwiseOrder(Vector a, Vector b, Vector c) => (c.y - a.y) * (b.x - a.x) > (b.y - a.y) * (c.x - a.x);
        public static bool IntersectingLines(Vector startA, Vector endA, Vector startB, Vector endB) => 
            CounterclockwiseOrder(startA, startB, endB) != CounterclockwiseOrder(endA, startB, endB) &&
            CounterclockwiseOrder(startA, endA, startB) != CounterclockwiseOrder(startA, endA, endB);
        #endregion
    }
}
