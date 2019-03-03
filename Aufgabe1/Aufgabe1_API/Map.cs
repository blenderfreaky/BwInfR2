﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aufgabe1_API
{
    public class Map
    {
        public Polygon[] polygons;
        public Vector[] busPath;
        public Vector startingPosition;
        private List<PolygonVertex> allDots;
        public double busSpeed, characterSpeed, busApproachConstant;

        public Map(string[] lines)
        {
            int polygonCount = int.Parse(lines[0]);
            polygons = new Polygon[polygonCount];

            for (int i = 0; i < polygonCount; i++)
            {
                int[] polygon = lines[i + 1].Split().Select(x => int.Parse(x)).ToArray();
                var vertices = new Vector[polygon[0]];

                for (int j = 0; j < polygon[0]; j++) vertices[j] = new Vector(polygon[j * 2 + 1], polygon[j * 2 + 2]);

                polygons[i] = new Polygon(vertices);
            }

            int[] start = lines[polygonCount + 1].Split().Select(x => int.Parse(x)).ToArray();
            startingPosition = new Vector(start[0], start[1]);

            busPath = new Vector[] { new Vector(0, 0), new Vector(0, 10000) };

            busSpeed = 30;
            characterSpeed = 15;

            busApproachConstant = characterSpeed / Math.Sqrt(busSpeed * busSpeed - characterSpeed * characterSpeed);
            allDots = new List<PolygonVertex>();
            foreach (Polygon polygon in polygons)
            {
                for (int i = 0; i < polygon.Length; i++) allDots.Add(polygon[i]);
            }
        }

        public Map(Polygon[] polygons, Vector[] busPath, Vector startingPosition, double busSpeed, double characterSpeed)
        {
            this.polygons = polygons;
            this.busPath = busPath;
            this.startingPosition = startingPosition;
            this.busSpeed = busSpeed;
            this.characterSpeed = characterSpeed;
            busApproachConstant = characterSpeed / Math.Sqrt(busSpeed * busSpeed - characterSpeed * characterSpeed);
            allDots = new List<PolygonVertex>();
            foreach (Polygon polygon in polygons)
            {
                for (int i = 0; i < polygon.Length; i++) allDots.Add(polygon[i]);
            }
        }

        public Dictionary<Vector, double> GenerateVisibilityGraph(Vector origin)
        {
            List<Vector> visibilityGraph = new List<Vector>();
            List<PolygonVertex> allDots = this.allDots.Where(x => x.vector != origin).ToList();
            Dictionary<PolygonVertex, double> angles = allDots.ToDictionary(x => x, x => x.vector.Angle(origin));

            List<PolygonVertex> intersections = new List<PolygonVertex>();

            foreach (PolygonVertex polygonVertex in allDots)
            {
                Vector a = polygonVertex.vector - origin;
                Vector b = polygonVertex.polygon[polygonVertex.index + 1].vector - origin;

                if ((a.x > 0 || b.x > 0) && a.y * b.y <= 0 &&
                    (a.y == b.y ? 
                    a.y == 0
                  : (a.x >= a.y * (b.x - a.x) / (b.y - a.y)))) intersections.Add(polygonVertex);
            }
            //return intersections.Select(x => (x.vector + x.polygon[x.index+1].vector) / 2).Distinct().ToDictionary(x => x, x => x.Distance(origin));
            foreach (PolygonVertex polygonVertex in allDots.OrderBy(x => angles[x]))
            {
                PolygonVertex left = polygonVertex.polygon[polygonVertex.index - 1];
                PolygonVertex right = polygonVertex.polygon[polygonVertex.index + 1];

                double currentAngle = angles[polygonVertex];

                double dist = origin.Distance(polygonVertex.vector);
                if (intersections.All(x => x == polygonVertex || x.polygon[x.index + 1] == polygonVertex || DistanceToLineAtAngle(origin, currentAngle, x) >= dist))
                    visibilityGraph.Add(polygonVertex.vector);

                if (angles[left] < currentAngle) intersections.RemoveAll(x => x == left);
                else if (!intersections.Contains(left)) intersections.Add(left);

                if (angles[right] < currentAngle) intersections.RemoveAll(x => x == polygonVertex);
                else if (!intersections.Contains(polygonVertex)) intersections.Add(polygonVertex);
            }

            if (PossiblePath(origin, startingPosition)) visibilityGraph.Add(startingPosition);

            foreach (Vector endpoint in GetEndpoints(origin)) if (PossiblePath(origin, endpoint)) visibilityGraph.Add(endpoint);

            return visibilityGraph.Distinct().ToDictionary(x => x, x => x.Distance(origin));
        }

        public double DistanceToLineAtAngle(Vector origin, double angle, PolygonVertex segment)
        {
            Vector a = segment.vector - origin;
            Vector b = segment.polygon[segment.index + 1].vector - origin;

            return ((a.y - b.y) * b.x - (a.x - b.x) * b.y) / (Math.Cos(angle) * (a.y - b.y) - Math.Sin(angle) * (a.x - b.x));
        }

        public Vector PointToLineAtAngle(Vector origin, double angle, PolygonVertex segment)
        {
            return new Vector(Math.Cos(angle), Math.Sin(angle)) * DistanceToLineAtAngle(origin, angle, segment);
        }

        private static void AddSorted<T>(List<T> list, T elem, Comparer<T> comp)
        {
            int i = -1;
            while (++i < list.Count && comp.Compare(list[i], elem) == -1) ;
            list.Insert(i, elem);
        }

        public bool PossiblePath(Vector a, Vector b)
        {
            foreach (var poly in polygons)
                for (int i = 0; i < poly.Length; i++)
                    if (Vector.IntersectingLines(a, b, poly[i].vector, poly[i + 1].vector))
                        return false;
            return true;
        }

        public IEnumerable<Vector> GetEndpoints(Vector dot)
        {
            for (int i = 0; i < busPath.Length - 1; i++)
            {
                Vector pathToBus = dot - busPath[i];
                Vector busPathNormal = busPath[i + 1] - busPath[i];

                double dotProduct = Vector.Dot(pathToBus, busPathNormal);
                double distance = dotProduct / busPathNormal.MagnitudeSquared();
                distance += (busPath[i] + busPathNormal * distance).Magnitude() * busApproachConstant / busPathNormal.Magnitude();

                yield return busPath[i] + busPathNormal * (distance < 0 ? 0 : distance > 1 ? 1 : distance);
            }
        }
    }
}
