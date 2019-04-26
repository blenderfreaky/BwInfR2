using Aufgabe2_API;
using MaterialDesign2.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Path = System.IO.Path;
using Vector = Aufgabe2_API.Vector;

namespace Aufgabe2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MaterialWindow
    {
        private double off = 100000;
        private double scale = 1;

        public MainWindow()
        {
            InitializeComponent();

            Off.Margin = new Thickness(-off);
            Back.Margin = new Thickness(off, off + 50, off, off);

            Draw(File.ReadAllLines(examplesPath + "dreiecke1.txt"));
        }

        public void Draw(string[] lines)
        {
            int triangleCount = int.Parse(lines[0]);
            TriangleArchetype[] archetypes = new TriangleArchetype[triangleCount];

            for (int i = 0; i < triangleCount; i++)
            {
                double[] triangle = lines[i + 1].Split().Select(x => double.Parse(x)).ToArray();
                Debug.Assert(triangle[0] == 3);
                var vertices = new Vector[3];

                for (int j = 0; j < triangle[0]; j++) vertices[j] = new Vector(triangle[j * 2 + 1], triangle[j * 2 + 2]);

                archetypes[i] = new TriangleArchetype(new Triangle(vertices[0], vertices[1], vertices[2]));
            }

            Draw(TriangleArranger.ArrangeTriangles(archetypes.ToList(), out var debug), debug);
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

                        X1 = t.a.x * scale,
                        Y1 = -t.a.y * scale,

                        X2 = t.b.x * scale,
                        Y2 = -t.b.y * scale,
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

                        X1 = t.b.x * scale,
                        Y1 = -t.b.y * scale,

                        X2 = t.c.x * scale,
                        Y2 = -t.c.y * scale,
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

                        X1 = t.c.x * scale,
                        Y1 = -t.c.y * scale,

                        X2 = t.a.x * scale,
                        Y2 = -t.a.y * scale,
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

        private static readonly string examplesPath =
            Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Directory.GetCurrentDirectory()), "../../../Beispiele/")).Let(x =>
            Directory.Exists(x) ? x : Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Directory.GetCurrentDirectory()), "Aufgabe2/Beispiele/")).Let(y =>
            Directory.Exists(y) ? y : "C:"));
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
                Draw(File.ReadAllLines(diag.FileName));
            }
        }

        private void Triangles_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Debugging_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
