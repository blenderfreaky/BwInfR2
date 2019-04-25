using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDesign2
{
    public static class Graphs
    {
        public static readonly Func<double, double> Linear = x => x;
        public static readonly Func<double, double> Quadratic = x => x * x;
        public static readonly Func<double, double> Cubic = x => x * x * x;
        public static readonly Func<double, double> Sinusoidal = x => Math.Cos(x * Math.PI / 2);
    }
}
