using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoostVoronoi.Output
{
    public class Vertex
    {
        public double X { get; set; }
        public double Y { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="t">A tuple where the first value represents the X-axis and the second value the Y-axis</param>
        public Vertex(Tuple<double, double> t)
        {
            X = t.Item1;
            Y = t.Item2;
        }

        public Vertex(Tuple<long, double, double> t)
        {
            X = t.Item2;
            Y = t.Item3;
        }

        public  Vertex(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="t">A tuple where the first value represents the X-axis and the second value the Y-axis</param>
        /// <param name="scaleFactor">A number that will be used to divide the coordinates</param>
        public Vertex(Tuple<double, double> t, int scaleFactor)
        {
            X = t.Item1 / scaleFactor;
            Y = t.Item2 / scaleFactor;
        }

        /// <summary>
        /// Returns a concatenation of the coordinates, separated by a comma
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("{0}, {1}", X, Y);
        }
    }
}
