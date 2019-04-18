using Aufgabe1_API;
using MaterialDesign2.Controls;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

using Vector = Aufgabe1_API.Vector;

namespace Aufgabe1_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MaterialWindow
    {
        public Map map;

        public MainWindow()
        {
            string root = "../../../../Examples/Aufgabe1/";
            map = new Map(File.ReadAllLines(root + "lisarennt3.txt"));

            /**
            map = new Map(new Polygon[]
                {
                    new Polygon(new Vector[] {new Vector(50, 50), new Vector(100, 50), new Vector(100,100), new Vector(50, 100), }),
                    new Polygon(new Vector[] {new Vector(70, 150), new Vector(120, 150), new Vector(120,200), new Vector(70, 200), }),
                    new Polygon(new Vector[] {new Vector(50, 250), new Vector(100, 250), new Vector(100,300), new Vector(50, 300), }),
                },
                new Vector[] { new Vector(0, 0), new Vector(0, 1000000) },
                new Vector(50, 150),
                30, 15);
            /**/
            /**
            map = new Map(new Polygon[]
                {
                    new Polygon(new Vector[] {new Vector(50, 50), new Vector(50, 100), new Vector(100, 100), new Vector(100, 50), }),
                    new Polygon(new Vector[] {new Vector(150, 50), new Vector(150, 100), new Vector(200, 100), new Vector(200, 50), }),
                },
                new Vector[] { new Vector(0, 0), new Vector(0, 1000000) },
                new Vector(50, 150),
                30, 15);
            /**/

            InitializeComponent();

            double off = 100000;
            Off.Margin = new Thickness(-off);
            Back.Margin = new Thickness(off, off + map.allPolygonVertices.Average(x => x.vector.y) * 2, off, off);
        }

        private void Polygons_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var polygon in map.polygons)
                Polygons.Children.Add(
                    new System.Windows.Shapes.Polygon
                    {
                        Stroke = Brushes.Transparent,
                        Fill = Brushes.Gray,
                        StrokeThickness = 2,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,

                        Points = new PointCollection(polygon.vertices.Select(x => new Point(x.vector.x, -x.vector.y))),
                    });
        }

        private void Buspath_Loaded(object sender, RoutedEventArgs e)
        {
            Buspath.Children.Add(
                new System.Windows.Shapes.Polygon
                {
                    Stroke = Brushes.Blue,
                    Fill = Brushes.Transparent,
                    StrokeThickness = 2,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,

                    Points = new PointCollection(map.busPath.Select(x => new Point(x.x, -x.y))),
                });
        }

        private void Draw(object sender = null, RoutedEventArgs e = null)
        {
            Vector Mix(Vertex vert, double length, double side) => vert.Next.vector - (vert.Next.vector - vert.vector).Let(x => x.Left * side + x).Normalize() * length;
            if (direction.IsToggledOn) Debugging.DrawLines(map.polygons.SelectMany(x => x.SelectMany(y => new[] { (y.vector, y.Next.vector), (y.Next.vector, Mix(y, 5, -1)), (y.Next.vector, Mix(y, 5, 1)) })), Brushes.Orange, 0.75);

            if (convexity.IsToggledOn) Debugging.DrawLines(map.allPolygonVertices.Where(x => x.isConvex).Select(x => (x.vector - x.normal * 5, x.vector + x.normal * 5)), Brushes.Green, 0.75);

            if (normals.IsToggledOn) Debugging.DrawLines(map.allPolygonVertices.Select(x => (x.vector, x.vector + x.normal * 5)), Brushes.Blue, 0.75);


            if (mouseRay.IsToggledOn) { DrawVisibilityPolygon(); }
            if (mouseRayVertex.IsToggledOn) { DrawVisibilityPolygonVertex(); }
            if (visibility.IsToggledOn) { DrawVisibilityGraph(); }
            if (heuristic.IsToggledOn) { DrawDijkstraHeuristic(); }
            if (optimal.IsToggledOn) { DrawOptimalPath(); }
        }

        private void DrawVisibilityPolygon(bool redraw = true)
        {
            Point mousePositionPoint = Mouse.GetPosition(Navmap);
            Vector mousePosition = new Vector(mousePositionPoint.X, -mousePositionPoint.Y);

            DrawLines(map
                .GenerateVisibilityPolygon(mousePosition, true, out _, out var debug)
                .Select(x => (mousePosition, x.vector)), debug, redraw);
        }
        private void DrawVisibilityPolygonVertex(bool redraw = true)
        {
            Point mousePositionPoint = Mouse.GetPosition(Navmap);
            Vector mousePosition = new Vector(mousePositionPoint.X, -mousePositionPoint.Y);
            Vertex vert = map.allPolygonVertices.MinValue(x => x.vector.Distance(mousePosition)).value;

            DrawLines(map.GenerateVisibilityPolygon(vert, true, out _, out var debug).Select(x => (vert.vector, x.vector)), debug, redraw);
        }
        private void DrawVisibilityGraph(bool redraw = true) =>
            DrawLines(map.GenerateVisibilityGraph(true, out _, out var debug)
            .SelectMany(x => x.Value.Select(y => y.vector.CompareTo(x.Key.vector) < 0 ? (y.vector, x.Key.vector) : (x.Key.vector, y.vector)))
            .Distinct(), debug, redraw);
        private void DrawDijkstraHeuristic(bool redraw = true) =>
            DrawLines(map.GenerateDijkstraHeuristic(true, out _, out _, out var debug).Select(x => (x.Key.vector, x.Value.vector)), debug, redraw);
        private void DrawOptimalPath(bool redraw = true)
        {
            var optimalPath = map.GetOptimalPath(out var debug);
            List<(Vector, Vector)> vertices = new List<(Vector, Vector)>();
            for (int i = 0; i < optimalPath.Count - 1; i++) vertices.Add((optimalPath[i].vector, optimalPath[i + 1].vector));
            DrawLines(vertices, debug, redraw);
        }

        private void DrawLines(IEnumerable<(Vector, Vector)> vertices, IEnumerable<(Vector, Vector)> debug, bool redraw = true)
        {
            if (redraw)
            {
                Navmap.Children.Clear();
                Debugging.Children.Clear();
            }

            Navmap.DrawLines(vertices, Brushes.Red, 1);
            Debugging.DrawLines(debug, Brushes.Blue, 1);
        }

        private void OpenAdvanced_Click(object sender, RoutedEventArgs e) => AdvancedOptions.Visibility = Visibility.Visible;
        private void CloseAdvanced_Click(object sender, RoutedEventArgs e) => AdvancedOptions.Visibility = Visibility.Collapsed;
    }
}
