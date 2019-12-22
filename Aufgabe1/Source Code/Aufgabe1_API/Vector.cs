using System;

namespace Aufgabe1_API
{
    public class Vector : IComparable<Vector>
    {
        public double X, Y;

        public Vector()
        {
        }

        public Vector(double angle) : this(Math.Cos(angle), Math.Sin(angle))
        {
        }

        public Vector(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public Vector Left => new Vector(-Y, X);
        public Vector Right => new Vector(Y, -X);
        public Vector Back => new Vector(-X, -Y);

        public double Angle() => MathHelper.ModuloAngle(Math.Atan2(Y, X));

        public double Angle(Vector origin) => MathHelper.ModuloAngle(Math.Atan2(Y - origin.Y, X - origin.X));

        public static double Angle(Vector vec, Vector origin) => MathHelper.ModuloAngle(Math.Atan2(vec.Y - origin.Y, vec.X - origin.X));

        public double DistanceSquared(Vector other) => Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2);

        public double Distance(Vector other) => Math.Sqrt(DistanceSquared(other));

        public static double DistanceSquared(Vector vec, Vector other) => Math.Pow(vec.X - other.X, 2) + Math.Pow(vec.Y - other.Y, 2);

        public static double Distance(Vector vec, Vector other) => Math.Sqrt(vec.DistanceSquared(other));

        public double MagnitudeSquared() => Math.Pow(X, 2) + Math.Pow(Y, 2);

        public double Magnitude() => Math.Sqrt(MagnitudeSquared());

        public static double MagnitudeSquared(Vector vec) => Math.Pow(vec.X, 2) + Math.Pow(vec.Y, 2);

        public static double Magnitude(Vector vec) => Math.Sqrt(vec.MagnitudeSquared());

        public double Dot(Vector other) => (X * other.X) + (Y * other.Y);

        public static double Dot(Vector vec, Vector other) => (vec.X * other.X) + (vec.Y * other.Y);

        public double WedgeProduct(Vector other) => (X * other.Y) - (Y * other.X);

        public static double WedgeProduct(Vector vec, Vector other) => (vec.X * other.Y) - (vec.Y * other.X);

        public Vector Normalize() => X == 0 && Y == 0 ? this : this / Magnitude();

        public static Vector Normalize(Vector vec) => vec.X == 0 && vec.Y == 0 ? vec : vec / vec.Magnitude();

        public static Vector operator +(Vector a, Vector b) => new Vector(a.X + b.X, a.Y + b.Y);

        public static Vector operator -(Vector vec) => new Vector(-vec.X, -vec.Y);

        public static Vector operator -(Vector a, Vector b) => new Vector(a.X - b.X, a.Y - b.Y);

        public static Vector operator *(Vector a, Vector b) => new Vector(a.X * b.X, a.Y * b.Y);

        public static Vector operator *(Vector a, double b) => new Vector(a.X * b, a.Y * b);

        public static Vector operator *(double b, Vector a) => new Vector(a.X * b, a.Y * b);

        public static Vector operator /(Vector a, Vector b) => new Vector(a.X / b.X, a.Y / b.Y);

        public static Vector operator /(Vector a, double b) => new Vector(a.X / b, a.Y / b);

        // Algorithm from https://bryceboe.com/2006/10/23/line-segment-intersection-algorithm/
        public enum VectorOrder : int
        {
            Collinear = -1,
            Clockwise = 0,
            Counterclockwise = 1,
        }

        /// <summary>
        /// Calculates the orientation of the triangle defined by a, b and c
        /// </summary>
        /// <returns>The Orientation of the triangle</returns>
        public static VectorOrder Orientation(Vector a, Vector b, Vector c)
        {
            double orientation = ((c.Y - a.Y) * (b.X - a.X)) - ((b.Y - a.Y) * (c.X - a.X));

            if (orientation < 0) return VectorOrder.Clockwise;
            if (orientation == 0) return VectorOrder.Collinear;
            if (orientation > 0) return VectorOrder.Counterclockwise;

            throw new NotFiniteNumberException();
        }

        /// <summary>
        /// Like Orientation, but provides a margin for collinearity
        /// </summary>
        /// <param name="epsilon">The margin for collinearity</param>
        public static VectorOrder OrientationApprox(Vector a, Vector b, Vector c, double epsilon)
        {
            double orientation = ((c.Y - a.Y) * (b.X - a.X)) - ((b.Y - a.Y) * (c.X - a.X));

            if (orientation < -epsilon) return VectorOrder.Clockwise;
            if (orientation > epsilon) return VectorOrder.Counterclockwise;

            if (double.IsNaN(orientation)) throw new NotFiniteNumberException();
            return VectorOrder.Collinear;
        }

        public static bool IntersectingLines(Vector startA, Vector endA, Vector startB, Vector endB)
        {
            VectorOrder sAsBeB = Orientation(startA, startB, endB);
            VectorOrder eAsBeB = Orientation(endA, startB, endB);
            VectorOrder sAeAsB = Orientation(startA, endA, startB);
            VectorOrder sAeAeB = Orientation(startA, endA, endB);

            return (sAsBeB != eAsBeB && sAeAsB != sAeAeB)
                    && !(sAeAeB == VectorOrder.Collinear || eAsBeB == VectorOrder.Collinear || sAeAsB == VectorOrder.Collinear || sAeAeB == VectorOrder.Collinear);
        }

        public override bool Equals(object obj) => obj is Vector vec && vec.X == X && vec.Y == Y;

        public override int GetHashCode()
        {
            var hashCode = 1502939027;
            hashCode = (hashCode * -1521134295) + X.GetHashCode();
            hashCode = (hashCode * -1521134295) + Y.GetHashCode();
            return hashCode;
        }

        public override string ToString() => $"x: {X} y: {Y}";

        public int CompareTo(Vector other) => X == other.X ? Y.CompareTo(other.Y) : X.CompareTo(other.X);
    }
}