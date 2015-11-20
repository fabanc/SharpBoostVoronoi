using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoostVoronoi.Input
{
    public class Point
    {
        public int X{ get; set; }
        public int Y { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Point()
        {
 
        }

        /// <summary>
        /// Construct a point by passing the x & y coordinates.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

    }
}
