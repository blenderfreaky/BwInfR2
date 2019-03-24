using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Aufgabe1_API
{
    public class Polygon : IEnumerable<Vertex>
    {
        public Vertex[] vertices;

        public Polygon(Vector[] vertices)
        {
            this.vertices = new Vertex[vertices.Length];
            for (int i = 0; i < vertices.Length; i++) this.vertices[i] = new Vertex(vertices[i], this, i);
            FixDirection();
            for (int i = 0; i < vertices.Length; i++) this.vertices[i] = this.vertices[i].Init();
        }

        protected bool FixDirection()
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

            /*Vector previous = max.Previous.vector;
            Vector center = max.vector;
            Vector next = max.Next.vector;

            double direction =
                (center.x * next.y + previous.x * center.y + previous.y * next.x)
              - (previous.y * center.x + center.y * next.x + previous.x * next.y);*/

            bool flipRequired = Vector.Orientation(max.Previous.vector, max.vector, max.Next.vector) == Vector.VectorOrder.Clockwise;

            if (flipRequired) Flip();
            return flipRequired;
        }

        public void Flip() => vertices = vertices.Reverse().Select(x => new Vertex(x.vector, this, vertices.Length - x.index - 1)).ToArray();

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

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<Vertex>)vertices).GetEnumerator();
        public IEnumerator<Vertex> GetEnumerator() => ((IEnumerable<Vertex>)vertices).GetEnumerator();
    }
}