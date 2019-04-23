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

        public bool isConcave;
        public Vector normal;

        public Vertex(Vector vector)
        {
            this.vector = vector;
            polygon = null;
            index = 0;
            isConcave = false;
            normal = null;
        }

        public Vertex(Vector vector, Polygon polygon, int index)
        {
            this.vector = vector;
            this.polygon = polygon;
            this.index = index;
            isConcave = false;
            normal = null;
        }

        public Vertex Init()
        {
            isConcave = Vector.Orientation(Previous.vector, vector, Next.vector) == Vector.VectorOrder.Clockwise;
            normal = isConcave
                    ? ((Previous.vector - vector).Normalize() + (Next.vector - vector).Normalize())
                    : -((Previous.vector - vector).Normalize() + (Next.vector - vector).Normalize());
            return this;
        }

        public Vertex Previous => polygon[index - 1];
        public Vertex Next => polygon[index + 1];

        public bool IsNeighbor(Vertex other) => Previous == other || Next == other;

        // Assumes proper normals
        public bool BehindNeighbors(Vector other)
        {
            bool enclosed = Vector.Orientation(vector, Previous.vector, other) == Vector.VectorOrder.Counterclockwise
                          ^ Vector.Orientation(vector, Next.vector, other) == Vector.VectorOrder.Counterclockwise;
            double dot = Vector.Dot(other - vector, normal);

            return isConcave
                    ? !enclosed || dot < 0
                    : enclosed && dot < 0;
        }

        public bool BetweenNeighbors(Vector other) =>
            !isConcave && (Vector.Orientation(vector, Previous.vector, other) != Vector.Orientation(Next.vector, vector, other));

        public double Dot(Vertex other) => (Next.vector - vector).Normalize().Dot((other.Next.vector - other.vector).Normalize());
        public double Dot(Vector start, Vector end) => (Next.vector - vector).Normalize().Dot((end - start).Normalize());

        public static bool operator ==(Vertex a, Vertex b) =>
              a.polygon == null || b.polygon == null
            ? a.vector == b.vector
            : a.polygon == b.polygon && a.index == b.index;
        public static bool operator !=(Vertex a, Vertex b) => !(a == b);
        public override bool Equals(object obj) => obj is Vertex segment && this == segment;
        public override int GetHashCode()
        {
            var hashCode = -2056935238;
            hashCode = hashCode * -1521134295 + EqualityComparer<Vector>.Default.GetHashCode(vector);
            hashCode = hashCode * -1521134295 + EqualityComparer<Polygon>.Default.GetHashCode(polygon);
            hashCode = hashCode * -1521134295 + index.GetHashCode();
            return hashCode;
        }
        public override string ToString() => $"{vector.ToString()} in {polygon?.ToString()} at {index}";
    }
}
