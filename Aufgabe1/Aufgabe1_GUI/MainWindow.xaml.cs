using Aufgabe1_API;
using MaterialDesign2.Controls;
using Microsoft.Win32;
using System;
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
        private double off = 100000;
        private Map _map;
        private Vertex startingPosition;
        public Map map
        {
            get => _map;
            set
            {
                _map = value;
                Back.Margin = new Thickness(off, off + map.allPolygonVertices.Average(x => x.vector.y) * 2, off, off);
                startingPosition = _map.startingPosition;
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            string root = "../../../../Examples/Aufgabe1/";
            map = new Map(File.ReadAllLines(root + "lisarennt1.txt"));

            Off.Margin = new Thickness(-off);
        }

        private void DrawPolygons(object sender = null, RoutedEventArgs e = null)
        {
            Polygons.Children.Clear();

            foreach (var polygon in map.polygons)
            {
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
        }

        private void DrawBuspath(object sender = null, RoutedEventArgs e = null)
        {
            Buspath.Children.Clear();

            Buspath.Children.Add(
                new System.Windows.Shapes.Polyline
                {
                    Stroke = Brushes.Blue,
                    Fill = Brushes.Transparent,
                    StrokeThickness = 2,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    
                    Points = new PointCollection(map.busPath.Select(x => new Point(MathHelper.Clamp(x.x, -99999, 99999), -MathHelper.Clamp(x.y, -99999, 99999)))), //Values bigger than 199999 don't render properly
                });
        }

        private void Draw(object sender = null, RoutedEventArgs e = null)
        {
            Navmap.Children.Clear();
            Debugging.Children.Clear();

            if (mouseRay.IsToggledOn) DrawVisibilityPolygon();
            if (mouseRayVertex.IsToggledOn) DrawVisibilityPolygonVertex();
            if (visibility.IsToggledOn) DrawVisibilityGraph();
            if (heuristic.IsToggledOn) DrawDijkstraHeuristic();
            if (optimal.IsToggledOn) DrawOptimalPathDefault(); 
            if (fromMouse.IsToggledOn) DrawOptimalPathMouse();
            if (!(optimal.IsToggledOn || fromMouse.IsToggledOn)) outputBorder.Visibility = Visibility.Collapsed;

            Vector Mix(Vertex vert, double length, double side) => vert.Next.vector - (vert.Next.vector - vert.vector).Let(x => x.Left * side + x).Normalize() * length;
            if (direction.IsToggledOn) Debugging.DrawLines(map.polygons.SelectMany(x => x.SelectMany(y => new[] { (y.vector, y.Next.vector), (y.Next.vector, Mix(y, 5, -1)), (y.Next.vector, Mix(y, 5, 1)) })), Brushes.Orange, 0.75);
            if (convexity.IsToggledOn) Debugging.DrawLines(map.allPolygonVertices.Where(x => !x.notConvex).Select(x => (x.vector - x.normal * 5, x.vector + x.normal * 5)), Brushes.Green, 0.75);
            if (normals.IsToggledOn) Debugging.DrawLines(map.allPolygonVertices.Select(x => (x.vector, x.vector + x.normal * 5)), Brushes.Blue, 0.75);
        }

        private void DrawVisibilityPolygon(bool redraw = true)
        {
            Point mousePositionPoint = Mouse.GetPosition(Navmap);
            Vector mousePosition = new Vector(mousePositionPoint.X, -mousePositionPoint.Y);

            DrawLines(map
                .GenerateVisibilityPolygon(mousePosition, out _, out var debug)
                .Select(x => (mousePosition, x.vector)), debug, redraw);
        }
        private void DrawVisibilityPolygonVertex(bool redraw = true)
        {
            Point mousePositionPoint = Mouse.GetPosition(Navmap);
            Vector mousePosition = new Vector(mousePositionPoint.X, -mousePositionPoint.Y);
            Vertex vert = map.allPolygonVertices.Concat(new[] { map.startingPosition }).MinValue(x => x.vector.Distance(mousePosition)).value;

            DrawLines(map.GenerateVisibilityPolygon(vert, out _, out var debug).Select(x => (vert.vector, x.vector)), debug, redraw);
        }
        private void DrawVisibilityGraph(bool redraw = true) =>
            DrawLines(map.GenerateVisibilityGraph(out _, out var debug)
            .SelectMany(x => x.Value.Select(y => y.vector.CompareTo(x.Key.vector) < 0 ? (y.vector, x.Key.vector) : (x.Key.vector, y.vector)))
            .Distinct(), debug, redraw);
        private void DrawDijkstraHeuristic(bool redraw = true) =>
            DrawLines(map.GenerateDijkstraHeuristic(true, out _, out _, out var debug).Select(x => (x.Key.vector, x.Value.vector)), debug, redraw);
        private void DrawOptimalPathDefault(bool redraw = true)
        {
            map.startingPosition = startingPosition;
            DrawOptimalPath(redraw);
        }
        private void DrawOptimalPathMouse(bool redraw = true)
        {
            Point mousePositionPoint = Mouse.GetPosition(Navmap);
            map.startingPosition = new Vertex(new Vector(mousePositionPoint.X, -mousePositionPoint.Y));
            DrawOptimalPath(redraw);
        }
        private void DrawOptimalPath(bool redraw = true)
        {
            try
            {
                outputBorder.Visibility = Visibility.Visible;

                var optimalPath = map.GetOptimalPath(out double characterLength, out double busLength, out double advantage, out var debug);
                List<(Vector, Vector)> vertices = new List<(Vector, Vector)>();
                for (int i = 0; i < optimalPath.Count - 1; i++) vertices.Add((optimalPath[i].vector, optimalPath[i + 1].vector));
                DrawLines(vertices, debug, redraw);

                DateTime start = DateTime.Now.Let(x => new DateTime(x.Year, x.Month, x.Day, 7, 30, 0));

                output.Text =
    @$"Startzeit: {(start - TimeSpan.FromSeconds(advantage)).ToLongTimeString()}
Ankunft: {(start + TimeSpan.FromSeconds(busLength / map.busSpeed)).ToLongTimeString()}
Fahrzeit: {TimeSpan.FromSeconds(busLength / map.busSpeed).ToString(@"hh\:mm\:ss\.ffff")}
Laufzeit: {TimeSpan.FromSeconds(characterLength / map.characterSpeed).ToString(@"hh\:mm\:ss\.ffff")}
Weg: {string.Join("\n    => ", Enumerable.Reverse(optimalPath).Select(x => $"({x.vector.x.ToString("0.####")}, {x.vector.y.ToString("0.####")})"))}";
            }
            catch (Exception) { }
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

        private static readonly string examplesPath = 
            Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Directory.GetCurrentDirectory()), "../../../Examples/Aufgabe1/"));
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog diag = new OpenFileDialog
            {
                InitialDirectory = examplesPath,
                CustomPlaces = new[] { new FileDialogCustomPlace(examplesPath) }.ToList(),
                CheckFileExists = true,
                CheckPathExists = true,
            };

            if (diag.ShowDialog() == true)
            {
                map = new Map(File.ReadAllLines(diag.FileName));
                Draw();
                DrawBuspath();
                DrawPolygons();
            }
        }

        private void ReDraw(object sender, MouseEventArgs e)
        {
            if (mouseRay.IsToggledOn) Draw();
            if (mouseRayVertex.IsToggledOn) Draw();
            if (fromMouse.IsToggledOn) Draw();
        }

        private void MouseRay_Click(object sender, RoutedEventArgs e)
        {
            mouseRayVertex.IsToggledOn = visibility.IsToggledOn = heuristic.IsToggledOn = optimal.IsToggledOn = fromMouse.IsToggledOn = false;
            Draw();
        }

        private void MouseRayVertex_Click(object sender, RoutedEventArgs e)
        {
            mouseRay.IsToggledOn = visibility.IsToggledOn = heuristic.IsToggledOn = optimal.IsToggledOn = fromMouse.IsToggledOn = false;
            Draw();
        }

        private void Visibility_Click(object sender, RoutedEventArgs e)
        {
            mouseRay.IsToggledOn = mouseRayVertex.IsToggledOn = heuristic.IsToggledOn = optimal.IsToggledOn = fromMouse.IsToggledOn = false;
            Draw();
        }

        private void Heuristic_Click(object sender, RoutedEventArgs e)
        {
            mouseRay.IsToggledOn = mouseRayVertex.IsToggledOn = visibility.IsToggledOn = optimal.IsToggledOn = fromMouse.IsToggledOn = false;
            Draw();
        }

        private void Optimal_Click(object sender, RoutedEventArgs e)
        {
            mouseRay.IsToggledOn = mouseRayVertex.IsToggledOn = visibility.IsToggledOn = heuristic.IsToggledOn = fromMouse.IsToggledOn = false;
            Draw();
        }

        private void FromMouse_Click(object sender, RoutedEventArgs e)
        {
            mouseRay.IsToggledOn = mouseRayVertex.IsToggledOn = visibility.IsToggledOn = heuristic.IsToggledOn = optimal.IsToggledOn = false;
            Draw();
        }
    }
}
