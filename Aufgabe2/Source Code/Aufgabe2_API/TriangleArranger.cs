using System;
using System.Collections.Generic;
using System.Linq;

namespace Aufgabe2_API
{
    public static class TriangleArranger
    {
        public static List<Triangle> ArrangeTriangles(in List<TriangleArchetype> triangleArchetypesIn, out Dictionary<Triangle, int> order, out List<(Vector, Vector)> debug)
        {
            var debugOut = new List<(Vector, Vector)>();
            var orderOut = new Dictionary<Triangle, int>();
            var triangles = new List<Triangle>();

            Triangle last;
            triangles.Add(last = new Triangle(new Vector(-1, 0), new Vector(-1, 0), new Vector(0, 0)));

            List<TriangleArchetype> triangleArchetypes = new List<TriangleArchetype>(triangleArchetypesIn);

            int n = 0;
            while (triangleArchetypes.Any())
            {
                var (value, comparable) = triangleArchetypes
                    .SelectMany(x => new[] { (x, x), (x, x.Mirror()), (x, x.Turn(1)), (x, x.Turn(1).Mirror()), (x, x.Turn(2)), (x, x.Turn(2).Mirror()) })
                    .Select(x => (x.Item1, AddTriangle(x.Item2, new List<Triangle>(triangles))))
                    .MinValue(x => Math.Max(x.Item2.a.x, Math.Max(x.Item2.b.x, x.Item2.c.x)) - Math.Min(x.Item2.a.x, Math.Min(x.Item2.b.x, x.Item2.c.x)));

                triangles.Add(last = value.Item2);
                triangleArchetypes.Remove(value.Item1);
                orderOut[value.Item2] = triangleArchetypesIn.IndexOf(value.Item1);
            }

            debug = debugOut;
            order = orderOut;
            return triangles.Skip(1).ToList();
        }

        public static double epsilon = 1E-10;

        public static Triangle AddTriangle(TriangleArchetype toAdd, List<Triangle> triangles)
        {
            Triangle added;

            Vector upper = new Vector(toAdd.angles[0]) * toAdd.lengths[0];
            double rise = upper.x / upper.y;

            double maxX = double.NegativeInfinity;

            foreach ((Vector start, Vector end) in triangles.SelectMany(x => new[] { (x.a, x.b), (x.b, x.c), (x.c, x.a) }))
            {
                if (start.y.Approx(end.y, epsilon)) maxX = Math.Max(maxX, Math.Max(start.x, end.x));
                else
                {
                    double startX = start.x - rise * start.y;
                    double endX = end.x - rise * end.y;
                    double lambda = MathHelper.Clamp((upper.y - start.y) / (end.y - start.y), 0, 1);
                    maxX = Math.Max(maxX, Math.Max(startX * (1 - lambda) + endX * lambda, end.y > start.y ? startX : endX));
                }
            }

            added = null; // Avoid unassigned compilation errors
                          // Use small steps to approximate max angle
            double lastSuccess = 0, lastFailure = Math.PI - toAdd.angles[0];
            for (double angleCurrent = lastFailure; Math.Abs(lastSuccess - lastFailure) > 1E-5; angleCurrent = (lastSuccess + lastFailure) / 2d)
            {
                added = new Triangle(toAdd, new Vector(maxX, 0), angleCurrent);
                // Check for intersections
                if (triangles.Any(x => x.Intersects(added)
                || x.Surrounds(added.a, epsilon) || x.Surrounds(added.b, epsilon) || x.Surrounds(added.c, epsilon)
                || added.Surrounds(x.a, epsilon) || added.Surrounds(x.b, epsilon) || added.Surrounds(x.c, epsilon)))
                {
                    lastFailure = angleCurrent;
                }
                else
                {
                    lastSuccess = angleCurrent;
                }
            }
            added = new Triangle(toAdd, new Vector(maxX, 0), lastSuccess);

            return added;
        }

        public static double Length(List<Triangle> triangles) =>
            triangles.Max(x => Math.Min(x.a.y == 0 ? x.a.x : double.PositiveInfinity, Math.Max(x.b.y == 0 ? x.b.x : double.PositiveInfinity, x.c.y == 0 ? x.c.x : double.PositiveInfinity)))
          - triangles.Min(x => Math.Max(x.a.y == 0 ? x.a.x : double.NegativeInfinity, Math.Max(x.b.y == 0 ? x.b.x : double.NegativeInfinity, x.c.y == 0 ? x.c.x : double.NegativeInfinity)));

        public static double SortedLength(List<Triangle> triangles) =>
            triangles.Last().Let(x => Math.Min(x.a.y == 0 ? x.a.x : double.PositiveInfinity, Math.Max(x.b.y == 0 ? x.b.x : double.PositiveInfinity, x.c.y == 0 ? x.c.x : double.PositiveInfinity)))
          - triangles.First().Let(x => Math.Max(x.a.y == 0 ? x.a.x : double.NegativeInfinity, Math.Max(x.b.y == 0 ? x.b.x : double.NegativeInfinity, x.c.y == 0 ? x.c.x : double.NegativeInfinity)));
    }
}
