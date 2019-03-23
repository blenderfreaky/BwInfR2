using System;
using System.Collections.Generic;
using System.Linq;

namespace Aufgabe1_API
{
    public class Polygon
    {
        public Vertex[] vertices;

        public Polygon(Vector[] vertices)
        {
            this.vertices = new Vertex[vertices.Length];
            for (int i = 0; i < vertices.Length; i++) this.vertices[i] = new Vertex(vertices[i], this, i);
        }

        public int Length => vertices.Length;
        public Vertex this[int index] => index < 0 ? this[index + Length] : vertices[index % Length];
        public int this[Vector vec]
        {
            get
            {
                for (int i = 0; i < Length; i++) if (vertices[i].vector == vec) return i;
                return -1;
            }
        }

        public bool Intersects(Vector a, Vector b)
        {
            for (int i = 0; i < Length; i++) if (Vector.IntersectingLines(a, b, this[i].vector, this[i + 1].vector)) return true;
            return false;
        }

        public void FixDirection()
        {
            Vertex max = vertices[0];

            foreach (Vertex polygonVertex in vertices)
            {
                if (max.vector.y > polygonVertex.vector.y
                 && max.vector.x <= polygonVertex.vector.x)
                {
                    max = polygonVertex;
                }
            }

            Vector left = max.Previous.vector;
            Vector center = max.vector;
            Vector right = max.Next.vector;

            double direction = 
                (center.x * right.y + left.x * center.y + left.y * right.x)
              - (left.y * center.x + center.y * right.x + left.x * right.y);

            if (direction < 0) Flip();
        }

        public void Flip() => vertices = vertices.Reverse().Select(x => new Vertex(x.vector, this, vertices.Length - x.index - 1)).ToArray();
    }

    public struct Vertex
    {
        public readonly Vector vector;
        public readonly Polygon polygon;
        public readonly int index;

        public Vertex(Vector vector, Polygon polygon, int index)
        {
            this.vector = vector;
            this.polygon = polygon;
            this.index = index;
        }

        public Vertex Previous => polygon[index - 1];
        public Vertex Next => polygon[index + 1];

        // Assumes polygon to be properly oriented, i.e. FixDirection() has been executed
        //public bool IsConvex => Vector.AngleTo(Next.vector - vector, vector - Previous.vector) < Math.PI;
        public bool IsConvex => Vector.Orientation(Previous.vector, vector, Next.vector) == Vector.VectorOrder.Clockwise;

        public Vector Normal => IsConvex
                              ?  ((Previous.vector - vector).Normalize() + (Next.vector - vector).Normalize()).Normalize()
                              : -((Previous.vector - vector).Normalize() + (Next.vector - vector).Normalize()).Normalize();

        public bool IsNeighbor(Vertex other) => Previous == other || Next == other;

        // Assumes proper normals
        public bool BehindNeighbors(Vector other)
        {
            bool enclosed = Vector.Orientation(vector, Previous.vector, other) == Vector.VectorOrder.Counterclockwise
                          ^ Vector.Orientation(vector, Next.vector,     other) == Vector.VectorOrder.Counterclockwise;
            double dot = Vector.Dot(other - vector, Normal);

            return IsConvex
                 ? !enclosed || dot < 0
                 :  enclosed && dot < 0;
        }

        public static bool operator ==(Vertex a, Vertex b) => a.polygon == b.polygon && a.index == b.index;
        public static bool operator !=(Vertex a, Vertex b) => a.polygon != b.polygon || a.index != b.index;
        public override bool Equals(object obj) => obj is Vertex segment && polygon == segment.polygon && index == segment.index;
        public override int GetHashCode()
        {
            var hashCode = 1956617662;
            hashCode = hashCode * -1521134295 + EqualityComparer<Vector>.Default.GetHashCode(vector);
            hashCode = hashCode * -1521134295 + EqualityComparer<Polygon>.Default.GetHashCode(polygon);
            hashCode = hashCode * -1521134295 + index.GetHashCode();
            return hashCode;
        }
    }
}