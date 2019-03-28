using Aufgabe1_API;
using MaterialDesign2.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Polygon = Aufgabe1_API.Polygon;
using Vector = Aufgabe1_API.Vector;

namespace Aufgabe1
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
            map = new Map(File.ReadAllLines(root + "lisarennt5.txt"));
            
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
            Back.Margin = new Thickness(off, off + map.allPolygonVertices.Average(x => x.vector.y)*2, off, off);
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

        private void Navmap_Loaded(object sender, RoutedEventArgs e) => Draw();
        private void Navmap_Move(object sender, RoutedEventArgs e) => Draw();

        private void Draw() => DrawVisibilityPolygonVertex();

        private void DrawVisibilityPolygon()
        {
            Point mousePositionPoint = Mouse.GetPosition(Navmap);
            Vector mousePosition = new Vector(mousePositionPoint.X, -mousePositionPoint.Y);

            DrawLines(map.GenerateVisibilityPolygon(mousePosition, true, out _, out var debug).Select(x => (mousePosition, x.vector)), debug);
        }
        private void DrawVisibilityPolygonVertex()
        {
            Point mousePositionPoint = Mouse.GetPosition(Navmap);
            Vector mousePosition = new Vector(mousePositionPoint.X, -mousePositionPoint.Y);
            Vertex vert = map.allPolygonVertices.MinValue(x => x.vector.Distance(mousePosition)).value;

            DrawLines(map.GenerateVisibilityPolygon(vert, true, out _, out var debug).Select(x => (vert.vector, x.vector)), debug);
        }
        private void DrawVisibilityGraph() => 
            DrawLines(map.GenerateVisibilityGraph(true, out _, out var debug)
            .SelectMany(x => x.Value.Select(y => y.vector.CompareTo(x.Key.vector) < 0 ? (y.vector, x.Key.vector) : (x.Key.vector, y.vector)))
            .Distinct(), debug);
        private void DrawDijkstraHeuristic() => 
            DrawLines(map.GenerateDijkstraHeuristic(true, out _, out _, out var debug).Select(x => (x.Key.vector, x.Value.vector)), debug);
        private void DrawOptimalPath()
        {
            var optimalPath = map.GetOptimalPath(out var debug);
            List<(Vector, Vector)> vertices = new List<(Vector, Vector)>();
            for (int i = 0; i < optimalPath.Count - 1; i++) vertices.Add((optimalPath[i].vector, optimalPath[i+1].vector));
            DrawLines(vertices, debug);
        }

        private void DrawLines(IEnumerable<(Vector, Vector)> vertices, IEnumerable<(Vector, Vector)> debug)
        {
            Navmap.Children.Clear();
            Debugging.Children.Clear();

            foreach ((Vector a, Vector b) in vertices)
            {
                Navmap.Children.Add(
                    new Line
                    {
                        Stroke = Brushes.Red,
                        Fill = Brushes.Transparent,
                        StrokeThickness = 1,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,

                        X1 = a.x,
                        Y1 = -a.y,

                        X2 = b.x,
                        Y2 = -b.y,
                    }
                );
            }
            foreach ((Vector a, Vector b) in debug)
            {
                Debugging.Children.Add(
                    new Line
                    {
                        Stroke = Brushes.Blue,
                        Fill = Brushes.Transparent,
                        StrokeThickness = 1,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,

                        X1 = a.x,
                        Y1 = -a.y,

                        X2 = b.x,
                        Y2 = -b.y,
                    }
                );
            }
        }

        private void OpenAdvanced_Click(object sender, RoutedEventArgs e) => AdvancedOptions.Visibility = Visibility.Visible;
        private void CloseAdvanced_Click(object sender, RoutedEventArgs e) => AdvancedOptions.Visibility = Visibility.Collapsed;
    }
}
