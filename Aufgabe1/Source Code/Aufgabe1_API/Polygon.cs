using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Aufgabe1_API
{
    public class Polygon : IEnumerable<Vertex>
    {
        public Vertex[] vertices;

        public int Length => vertices.Length;
        public Vertex this[int index] => vertices[MathHelper.PositiveModulo(index, 0, Length)];

        public Polygon(Vector[] vertices)
        {
            this.vertices = new Vertex[vertices.Length];
            for (int i = 0; i < vertices.Length; i++) this.vertices[i] = new Vertex(vertices[i], this, i);
            FixDirection();
            for (int i = 0; i < vertices.Length; i++) this.vertices[i] = this.vertices[i].Init();
        }

        protected bool FixDirection()
        {
            Vertex max = vertices.MaxValue((a, b) => a.vector.y > b.vector.y && a.vector.x <= b.vector.x ? 1 : -1);

            bool flipRequired = Vector.Orientation(max.Previous.vector, max.vector, max.Next.vector) == Vector.VectorOrder.Clockwise;

            if (flipRequired) Flip();
            return flipRequired;
        }

        public void Flip() => vertices = vertices.Reverse().Select(x => new Vertex(x.vector, this, vertices.Length - x.index - 1)).ToArray();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<Vertex>)vertices).GetEnumerator();
        public IEnumerator<Vertex> GetEnumerator() => ((IEnumerable<Vertex>)vertices).GetEnumerator();
    }
}