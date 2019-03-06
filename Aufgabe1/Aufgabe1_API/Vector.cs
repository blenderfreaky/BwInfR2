using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Aufgabe1_API.Vector;

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

        public double Angle() => AngleHelper.ClampAngle(Math.Atan2(y, x));
        public double Angle(Vector origin) => AngleHelper.ClampAngle(Math.Atan2(y-origin.y, x-origin.x));

        public double Distance(Vector other) => Math.Sqrt(Math.Pow(x - other.x, 2) + Math.Pow(y - other.y, 2));
        
        public double MagnitudeSquared() => Math.Pow(x, 2) + Math.Pow(y, 2);
        public double Magnitude() => Math.Sqrt(MagnitudeSquared());

        public double Dot(Vector other) => x * other.x + y * other.y;
        public static double Dot(Vector vec, Vector other) => vec.x * other.x + vec.y * other.y;

        public double Cross(Vector other) => x * other.y - y * other.x;
        public static double Cross(Vector vec, Vector other) => vec.x * other.y - vec.y * other.x;

        public Vector Normalize() => this/Magnitude();
        public static Vector Normalize(Vector vec) => vec / vec.Magnitude();

        public static Vector operator +(Vector a, Vector b) => new Vector(a.x + b.x, a.y + b.y);
        public static Vector operator -(Vector vec) => new Vector(-vec.x, -vec.y);
        public static Vector operator -(Vector a, Vector b) => new Vector(a.x - b.x, a.y - b.y);
        public static Vector operator *(Vector a, Vector b) => new Vector(a.x * b.x, a.y * b.y);
        public static Vector operator *(Vector a, double b) => new Vector(a.x * b, a.y * b);
        public static Vector operator /(Vector a, Vector b) => new Vector(a.x / b.x, a.y / b.y);
        public static Vector operator /(Vector a, double b) => new Vector(a.x / b, a.y / b);

        #region Intersection
        //Algorithm from https://bryceboe.com/2006/10/23/line-segment-intersection-algorithm/
        public enum VectorOrder
        {
            Colinear,
            Clockwise,
            Counterclockwise,
        }
        public static VectorOrder Orientation(Vector a, Vector b, Vector c)
        {
            double orientation = (c.y - a.y) * (b.x - a.x) - (b.y - a.y) * (c.x - a.x);

            if (orientation < 00) return VectorOrder.Clockwise;
            if (orientation == 0) return VectorOrder.Colinear;
            if (orientation > 00) return VectorOrder.Counterclockwise;

            throw new NotFiniteNumberException();
        }
        public static bool IntersectingLines(Vector startA, Vector endA, Vector startB, Vector endB)
        {
            //if (startA == startB && endA == endB || startA == endB && endA == startB) return true;

            VectorOrder sAsBeB = Orientation(startA, startB, endB);
            VectorOrder eAsBeB = Orientation(endA, startB, endB);
            VectorOrder sAeAsB = Orientation(startA, endA, startB);
            VectorOrder sAeAeB = Orientation(startA, endA, endB);

            return (sAsBeB != eAsBeB && sAeAsB != sAeAeB)
                  && !(sAeAeB == VectorOrder.Colinear || eAsBeB == VectorOrder.Colinear || sAeAsB == VectorOrder.Colinear || sAeAeB == VectorOrder.Colinear);
            //    || sAsBeB == VectorOrder.Colinear 
            //    || eAsBeB == VectorOrder.Colinear 
            //    || sAeAsB == VectorOrder.Colinear
            //    || sAeAeB == VectorOrder.Colinear;
        }
        #endregion

        public override bool Equals(object obj) => obj is Vector vec ? vec.x == x && vec.y == y : false;

        public override int GetHashCode()
        {
            var hashCode = 1502939027;
            hashCode = hashCode * -1521134295 + x.GetHashCode();
            hashCode = hashCode * -1521134295 + y.GetHashCode();
            return hashCode;
        }

        public override string ToString() => $"x: {x} y: {y}";
    }
}
