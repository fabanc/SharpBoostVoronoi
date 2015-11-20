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
    }
}
