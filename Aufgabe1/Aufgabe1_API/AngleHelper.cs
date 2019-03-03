using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aufgabe1_API
{
    public static class AngleHelper
    {
        public static double ClampAngle(double angle)
        {
            while (angle < 0) angle += Math.PI * 2;
            while (angle > Math.PI * 2) angle -= Math.PI * 2;
            return angle;
        }

        public static int GetSide(double angle1, double angle2) => angle1 == angle2 ? 0 : ClampAngle(angle2  - angle1).CompareTo(Math.PI);
    }
}
