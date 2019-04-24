using Aufgabe2_API;
using MaterialDesign2.Controls;
using System;
using System.Collections.Generic;
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
using Vector = Aufgabe2_API.Vector;

namespace Aufgabe2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MaterialWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void Draw()
        {
            Draw(new[] { new Triangle(new TriangleArchetype(new Triangle(new Vector(0, 0), new Vector(1, 1), new Vector(2, 0))), new Vector(), 0) }.ToList(), new List<(Vector, Vector)>());
            List<TriangleArchetype> triangleArchetypes = new List<TriangleArchetype>();
            //Draw(TriangleArranger.ArrangeTriangles(triangleArchetypes, out var debug), debug);
        }

        public void Draw(List<Triangle> triangles, List<(Vector, Vector)> debug)
        {
            Triangles.Children.Clear();
            Debugging.Children.Clear();

            foreach (Triangle t in triangles)
            {
                Triangles.Children.Add(
                    new Line
                    {
                        Stroke = Brushes.Red,
                        Fill = Brushes.Transparent,
                        StrokeThickness = 1,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,

                        X1 = t.a.x,
                        Y1 = -t.a.y,

                        X2 = t.b.x,
                        Y2 = -t.b.y,
                    }
                );
                Triangles.Children.Add(
                    new Line
                    {
                        Stroke = Brushes.Red,
                        Fill = Brushes.Transparent,
                        StrokeThickness = 1,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,

                        X1 = t.b.x,
                        Y1 = -t.b.y,

                        X2 = t.c.x,
                        Y2 = -t.c.y,
                    }
                );
                Triangles.Children.Add(
                    new Line
                    {
                        Stroke = Brushes.Red,
                        Fill = Brushes.Transparent,
                        StrokeThickness = 1,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,

                        X1 = t.c.x,
                        Y1 = -t.c.y,

                        X2 = t.a.x,
                        Y2 = -t.a.y,
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

        private void Triangles_Loaded(object sender, RoutedEventArgs e)
        {
            Draw();
        }

        private void Debugging_Loaded(object sender, RoutedEventArgs e)
        {
            Draw();
        }
    }
}
