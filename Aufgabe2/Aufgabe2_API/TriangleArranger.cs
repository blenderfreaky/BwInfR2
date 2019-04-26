using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aufgabe2_API
{
    public static class TriangleArranger
    {
        public static List<Triangle> ArrangeTriangles(List<TriangleArchetype> triangleArchetypes, out List<(Vector, Vector)> debug)
        {
            var debugOut = new List<(Vector, Vector)>();
            var triangles = new List<Triangle>();

            IEnumerator<TriangleArchetype> enumerator = triangleArchetypes.GetEnumerator();
            if (!enumerator.MoveNext()) throw new Exception();
            Triangle last;
            triangles.Add(last = new Triangle(enumerator.Current, new Vector(), 0));

            while (enumerator.MoveNext())
            {
                TriangleArchetype toAdd = enumerator.Current;

                double angle = last.a.Angle(last.c);
                double angleDiff = angle - toAdd.angles[0];

                Triangle added;
                if (angleDiff > 0)
                {
                    added = new Triangle(toAdd, last.a, angleDiff);
                }
                else
                {
                    double otherAngle = last.c.Angle(last.b);
                    added = new Triangle(toAdd, last.c - new Vector(-otherAngle).Let(x => x / x.y) * last.c.y, otherAngle);
                }

                triangles.Add(last = added);
            }

            debug = debugOut;

            return triangles;
        }
    }
}
