﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Aufgabe1_API
{
    public class Polygon
    {
        public PolygonVertex[] vertices;

        public Polygon(Vector[] vertices)
        {
            this.vertices = new PolygonVertex[vertices.Length];
            for (int i = 0; i < vertices.Length; i++) this.vertices[i] = new PolygonVertex { vector = vertices[i], polygon = this, index = i };
            Flip();
        }

        public int Length => vertices.Length;
        public PolygonVertex this[int index] => index < 0 ? this[index + Length] : vertices[index % Length];
        public int this[Vector vec]
        {
            get
            {
                for (int i = 0; i < Length; i++) if (vertices[i].vector == vec) return i;
                return -1;
            }
        }

        public void FixDirection()
        {
            PolygonVertex max = vertices[0];

            foreach (PolygonVertex polygonVertex in vertices)
            {
                if (max.vector.y > polygonVertex.vector.y
                 && max.vector.x <= polygonVertex.vector.x)
                {
                    max = polygonVertex;
                }
            }

            Vector left = max.Left.vector;
            Vector center = max.vector;
            Vector right = max.Right.vector;

            double direction = 
                (center.x * right.y + left.x * center.y + left.y * right.x)
              - (left.y * center.x + center.y * right.x + left.x * right.y);

            if (direction < 0) Flip();
        }

        public void Flip()
        {
            vertices = vertices.Reverse().ToArray();
        }
    }

    public struct PolygonVertex
    {
        public Vector vector;
        public Polygon polygon;
        public int index;

        public PolygonVertex Left  => polygon[index - 1];
        public PolygonVertex Right => polygon[index + 1];

        public static bool operator ==(PolygonVertex a, PolygonVertex b) => a.polygon == b.polygon && a.index == b.index;
        public static bool operator !=(PolygonVertex a, PolygonVertex b) => a.polygon != b.polygon || a.index != b.index;
        public override bool Equals(object obj) => obj is PolygonVertex segment && polygon == segment.polygon && index == segment.index;
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