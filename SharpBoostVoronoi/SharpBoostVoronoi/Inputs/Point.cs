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

        /// <summary>
        /// Compare coordinates with another point.
        /// </summary>
        /// <param name="p">The other point</param>
        /// <returns>True if the points have the same X and Y coordinates.</returns>
        public bool HasSameCoordinates(Point p)
        {
            if(p == null)
                throw new ArgumentNullException();
            if(p.X == X && p.Y == Y)
                return true;
            return false;
        }

        public override string ToString()
        {
            return String.Format("{0}, {1}", X, Y);
        }

    }
}
