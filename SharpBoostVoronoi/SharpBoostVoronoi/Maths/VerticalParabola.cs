using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoostVoronoi.Maths
{
    class VerticalParabola
    {
        public double FocusX { get; set; }
        public double FocusY { get; set; }
        /// <summary>
        /// The horizontal directx
        /// </summary>
        public double Directix { get; set; }


        public VerticalParabola(double focus_x, double focus_y, double directix)
        {
            FocusX = focus_x;
            FocusY = focus_y;
            Directix = directix;
        }

        /// <summary>
        /// http://hotmath.com/hotmath_help/topics/finding-the-equation-of-a-parabola-given-focus-and-directrix.html
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double GetParabolaY(double x)
        {
            return (Math.Pow(x - FocusX, 2) + Math.Pow(FocusY, 2) - Math.Pow(Directix, 2)) / (2 * (FocusY - Directix));
        }


    }
}
