using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Aufgabe1_API.Vector;

namespace Aufgabe1_API
{
    public class Vector : IComparable<Vector>
    {
        public double x, y;

        public Vector() { }

        public Vector(double angle) : this(Math.Cos(angle), Math.Sin(angle)) { }

        public Vector(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector Left => new Vector(-y, x);
        public Vector Right => new Vector(y, -x);
        public Vector Back => new Vector(-x, -y);

        public double Angle() => MathHelper.ModuloAngle(Math.Atan2(y, x));
        public double Angle(Vector origin) => MathHelper.ModuloAngle(Math.Atan2(y - origin.y, x - origin.x));
        public static double Angle(Vector vec, Vector origin) => MathHelper.ModuloAngle(Math.Atan2(vec.y - origin.y, vec.x - origin.x));

        public double DistanceSquared(Vector other) => Math.Pow(x - other.x, 2) + Math.Pow(y - other.y, 2);
        public double Distance(Vector other) => Math.Sqrt(DistanceSquared(other));
        public static double DistanceSquared(Vector vec, Vector other) => Math.Pow(vec.x - other.x, 2) + Math.Pow(vec.y - other.y, 2);
        public static double Distance(Vector vec, Vector other) => Math.Sqrt(vec.DistanceSquared(other));

        public double MagnitudeSquared() => Math.Pow(x, 2) + Math.Pow(y, 2);
        public double Magnitude() => Math.Sqrt(MagnitudeSquared());
        public static double MagnitudeSquared(Vector vec) => Math.Pow(vec.x, 2) + Math.Pow(vec.y, 2);
        public static double Magnitude(Vector vec) => Math.Sqrt(vec.MagnitudeSquared());

        public double Dot(Vector other) => x * other.x + y * other.y;
        public static double Dot(Vector vec, Vector other) => vec.x * other.x + vec.y * other.y;

        public double Determinant(Vector other) => x * other.y - y * other.x;
        public static double Determinant(Vector vec, Vector other) => vec.x * other.y - vec.y * other.x;

        public double AngleTo(Vector other) => MathHelper.ModuloAngle(-Math.Atan2(Determinant(other), Dot(other)));
        public static double AngleTo(Vector vec, Vector other) => MathHelper.ModuloAngle(-Math.Atan2(Determinant(vec, other), Dot(vec, other)));

        public Vector Normalize() => x == 0 && y == 0 ? this : this / Magnitude();
        public static Vector Normalize(Vector vec) => vec.x == 0 && vec.y == 0 ? vec : vec / vec.Magnitude();

        public static Vector operator +(Vector a, Vector b) => new Vector(a.x + b.x, a.y + b.y);
        public static Vector operator -(Vector vec) => new Vector(-vec.x, -vec.y);
        public static Vector operator -(Vector a, Vector b) => new Vector(a.x - b.x, a.y - b.y);
        public static Vector operator *(Vector a, Vector b) => new Vector(a.x * b.x, a.y * b.y);
        public static Vector operator *(Vector a, double b) => new Vector(a.x * b, a.y * b);
        public static Vector operator *(double b, Vector a) => new Vector(a.x * b, a.y * b);
        public static Vector operator /(Vector a, Vector b) => new Vector(a.x / b.x, a.y / b.y);
        public static Vector operator /(Vector a, double b) => new Vector(a.x / b, a.y / b);

        #region Intersection
        //Algorithm from https://bryceboe.com/2006/10/23/line-segment-intersection-algorithm/
        public enum VectorOrder : int
        {
            Colinear = -1,
            Clockwise = 0,
            Counterclockwise = 1,
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
        }
        #endregion

        public override bool Equals(object obj) => obj is Vector vec && vec.x == x && vec.y == y;

        public override int GetHashCode()
        {
            var hashCode = 1502939027;
            hashCode = hashCode * -1521134295 + x.GetHashCode();
            hashCode = hashCode * -1521134295 + y.GetHashCode();
            return hashCode;
        }

        public override string ToString() => $"x: {x} y: {y}";

        public int CompareTo(Vector other) => x == other.x ? y.CompareTo(other.y) : x.CompareTo(other.x);
    }
}
