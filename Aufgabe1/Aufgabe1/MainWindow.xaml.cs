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

namespace Aufgabe1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public WriteableBitmap writeableBitmap;
        private void ViewPort_Loaded(object sender, RoutedEventArgs e)
        {
            int width = 500;
            int height = 500;
            PixelFormat pixelFormat = PixelFormats.Rgba64;
            ViewPort.Source = writeableBitmap = (WriteableBitmap)BitmapSource.Create(width, height, 96, 96, PixelFormats.Rgba64, null, new int[width*height*pixelFormat.BitsPerPixel], 4);
        }
    }
}
