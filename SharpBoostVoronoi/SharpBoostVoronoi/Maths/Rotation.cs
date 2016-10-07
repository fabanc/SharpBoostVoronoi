using SharpBoostVoronoi.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoostVoronoi.Maths
{
    public class Rotation
    {
        /// <summary>
        /// Rotate a point by an angle around another point (origin). 
        /// The rotation happen clockwise. 
        /// https://www.siggraph.org/education/materials/HyperGraph/modeling/mod_tran/2drota.htm
        /// The orgin point is computed as the point moved by a shift on X and Y axis.
        /// </summary>
        /// <param name="p">The point to be rotated</param>
        /// <param name="theta">The angle for the rotation (in radians).</param>
        /// <param name="shift_x">The translation along the x-axis used during the rotation.</param>
        /// <param name="shift_y">The translation along the y-axis used during the rotation.</param>
        /// <returns></returns>
        public static Vertex Rotate(Vertex p, double theta, double shift_x, double shift_y)
        {
            return Rotate(new Vertex(p.X - shift_x, p.Y - shift_y), theta);
        }

        /// <summary>
        /// Rotate a point around another point (anchor)
        /// </summary>
        /// <param name="point">The anchor point used for the rotation</param>
        /// <param name="theta">The angle for the rotation</param>
        /// <returns>The rotated point</returns>
        public static Vertex Rotate(Vertex point, double theta)
        {
            double t = -1 * theta;
            double cos = Math.Cos(t);
            double sin = Math.Sin(t);
            return new Vertex(
                (point.X * cos) - (point.Y * sin),
                (point.X * sin) + (point.Y * cos)
                );
        }

        /// <summary>
        /// Undo the rotation done for a point.
        /// </summary>
        /// <param name="p">The point to rotate.</param>
        /// <param name="theta">The angle in radians</param>
        /// <param name="shift_x">The translation along the x-axis used during the rotation.</param>
        /// <param name="shift_y">The translation along the y-axis used during the rotation.</param>
        /// <returns></returns>
        public static Vertex Unrotate(Vertex p, double theta, double shift_x, double shift_y)
        {
            double cos = Math.Cos(theta);
            double sin = Math.Sin(theta);

            return new Vertex(
                (p.X * cos) - (p.Y * sin) + shift_x,
                (p.X * sin) + (p.Y * cos) + shift_y
                );
        }
    }
}
