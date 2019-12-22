using System.Collections.Generic;

namespace Aufgabe1_API
{
    /// <summary>
    /// A container class for vertices of polygons
    /// </summary>
    public struct Vertex
    {
        public readonly Vector vector;
        public readonly Polygon polygon;
        public readonly int index;

        public bool notConvex;
        public Vector normal;

        public Vertex(Vector vector)
        {
            this.vector = vector;
            polygon = null;
            index = 0;
            notConvex = false;
            normal = null;
        }

        public Vertex(Vector vector, Polygon polygon, int index)
        {
            this.vector = vector;
            this.polygon = polygon;
            this.index = index;
            notConvex = false;
            normal = null;
        }

        public Vertex Init()
        {
            notConvex = Vector.Orientation(Previous.vector, vector, Next.vector) == Vector.VectorOrder.Clockwise;
            normal = notConvex
                    ? ((Previous.vector - vector).Normalize() + (Next.vector - vector).Normalize())
                    : -((Previous.vector - vector).Normalize() + (Next.vector - vector).Normalize());
            return this;
        }

        public Vertex Previous => polygon[index - 1];
        public Vertex Next => polygon[index + 1];

        public bool IsNeighbor(Vertex other) => Previous == other || Next == other;

        // Assumes notConvex to be properly calculated
        public bool BetweenNeighbors(Vector other) =>
            !notConvex && (Vector.Orientation(vector, Previous.vector, other) != Vector.Orientation(Next.vector, vector, other));

        public static bool operator ==(Vertex a, Vertex b) =>
              a.polygon == null || b.polygon == null
            ? a.vector == b.vector
            : a.polygon == b.polygon && a.index == b.index;

        public static bool operator !=(Vertex a, Vertex b) => !(a == b);

        public override bool Equals(object obj) => obj is Vertex segment && this == segment;

        public override int GetHashCode()
        {
            var hashCode = -2056935238;
            hashCode = (hashCode * -1521134295) + EqualityComparer<Vector>.Default.GetHashCode(vector);
            hashCode = (hashCode * -1521134295) + EqualityComparer<Polygon>.Default.GetHashCode(polygon);
            hashCode = (hashCode * -1521134295) + index.GetHashCode();
            return hashCode;
        }

        public override string ToString() => $"{vector.ToString()} in {polygon?.ToString()} at {index}";
    }
}