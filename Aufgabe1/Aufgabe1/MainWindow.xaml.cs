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
            map.GenerateNavmap().ToList();

            InitializeComponent();
        }

        public WriteableBitmap writeableBitmap;
        private void ViewPort_Loaded(object sender, RoutedEventArgs e)
        {
            DrawMap(map);
        }

        public void DrawMap(Map map)
        {
            foreach (var polygon in map.polygons) ViewPort.Children.Add(
                new Polygon
                {
                    Stroke = Brushes.Black,
                    Fill = Brushes.Transparent,
                    StrokeThickness = 2,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    
                    Points = new PointCollection(polygon.Select(x => new Point(x.x, x.y))),
                });

            ViewPort.Children.Add(
                new Polygon
                {
                    Stroke = Brushes.Blue,
                    Fill = Brushes.Transparent,
                    StrokeThickness = 2,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,

                    Points = new PointCollection(map.busPath.Select(x => new Point(x.x, x.y))),
                });


            foreach (var origin in map.navmap.nodes) foreach (var target in origin.Value) ViewPort.Children.Add(
                new Line
                {
                    Stroke = Brushes.Red,
                    Fill = Brushes.Transparent,
                    StrokeThickness = 0.2,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,

                    X1 = origin.Key.x,
                    Y1 = origin.Key.y,

                    X2 = target.Key.x,
                    Y2 = target.Key.y,
                });
            //int width = map.;
            //int height = 500;
            //PixelFormat pixelFormat = PixelFormats.Rgba64;
            //ViewPort.Source = writeableBitmap = (WriteableBitmap)BitmapSource.Create(width, height, 96, 96, PixelFormats.Rgba64, null, new int[width * height * pixelFormat.BitsPerPixel], 4);
        }
    }
}
