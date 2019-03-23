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
            map = new Map(File.ReadAllLines(root + "lisarennt3.txt"));
            
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

            double off = 1000;
            Off.Margin = new Thickness(-off);
            Back.Margin = new Thickness(off, off + map.allDots.Average(x => x.vector.y)*2, off, off);
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

        private void Navmap_Loaded(object sender, RoutedEventArgs e) => DrawNavmap();
        private void Navmap_Move(object sender, RoutedEventArgs e) => DrawNavmap();
        private void DrawNavmap()
        {
            Navmap.Children.Clear();
            Debugging.Children.Clear();

            List<(Vector, Vector)> debug;
            foreach (var origin in map.GenerateVisibilityGraph(out debug))
            {
                foreach (var target in origin.Value)
                {
                    Navmap.Children.Add(
                        new Line
                        {
                            Stroke = Brushes.Red,
                            Fill = Brushes.Transparent,
                            StrokeThickness = 0.8,
                            VerticalAlignment = VerticalAlignment.Center,
                            HorizontalAlignment = HorizontalAlignment.Center,

                            X1 = origin.Key.x,
                            Y1 = -origin.Key.y,

                            X2 = target.Key.x,
                            Y2 = -target.Key.y,
                        }
                    );
                }
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
        private void DrawVisibilityGraph()
        {
            Point mousePositionPoint = Mouse.GetPosition(Navmap);
            Vector mousePosition = new Vector(mousePositionPoint.X, -mousePositionPoint.Y);
            //Vector mousePosition = new Vector(148, 141);

            Navmap.Children.Clear();

            List<(Vector, Vector)> debug;
            foreach (var origin in map.GenerateVisibilityPolygon(mousePosition, out debug))
            {
                Navmap.Children.Add(
                    new Line
                    {
                        Stroke = Brushes.Red,
                        Fill = Brushes.Transparent,
                        StrokeThickness = 0.8,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,

                        X1 = origin.Key.x,
                        Y1 = -origin.Key.y,

                        X2 = mousePosition.x,
                        Y2 = -mousePosition.y,
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
    }
}
