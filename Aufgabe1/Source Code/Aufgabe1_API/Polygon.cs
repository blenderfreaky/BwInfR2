using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Aufgabe1_API
{
    public class Polygon : IEnumerable<Vertex>
    {
        public Vertex[] Vertices;

        public int Length => Vertices.Length;
        public Vertex this[int index] => Vertices[MathHelper.PositiveModulo(index, 0, Length)];

        public Polygon(Vector[] vertices)
        {
            this.Vertices = new Vertex[vertices.Length];
            for (int i = 0; i < vertices.Length; i++) this.Vertices[i] = new Vertex(vertices[i], this, i);
            FixDirection();
            for (int i = 0; i < vertices.Length; i++) this.Vertices[i] = this.Vertices[i].Init();
        }

        protected bool FixDirection()
        {
            Vertex max = Vertices.MaxValue((a, b) => a.vector.Y > b.vector.Y && a.vector.X <= b.vector.X ? 1 : -1);

            bool flipRequired = Vector.Orientation(max.Previous.vector, max.vector, max.Next.vector) == Vector.VectorOrder.Clockwise;

            if (flipRequired) Flip();
            return flipRequired;
        }

        public void Flip() => Vertices = Vertices.Reverse().Select(x => new Vertex(x.vector, this, Vertices.Length - x.index - 1)).ToArray();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<Vertex>)Vertices).GetEnumerator();

        public IEnumerator<Vertex> GetEnumerator() => ((IEnumerable<Vertex>)Vertices).GetEnumerator();
    }
}