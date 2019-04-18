using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

using Vector = Aufgabe1_API.Vector;

namespace Aufgabe1_GUI
{
    public static class Helper
    {
        public static void DrawLines(this Canvas canvas, IEnumerable<(Vector start, Vector end)> lines, Brush stroke, double thickness)
        {
            foreach ((Vector a, Vector b) in lines)
            {
                canvas.Children.Add(
                    new Line
                    {
                        Stroke = stroke,
                        Fill = Brushes.Transparent,
                        StrokeThickness = thickness,
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
