using SharpBoostVoronoi.Exceptions;
using SharpBoostVoronoi.Input;
using SharpBoostVoronoi.Maths;
using SharpBoostVoronoi.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoostVoronoi.Parabolas
{
    public class ParabolaComputation
    {
        /// <summary>
        /// Return the angle between 2 points as radians
        /// </summary>
        /// <param name="start">The first point</param>
        /// <param name="end">The second point</param>
        /// <returns>The angle in radians</returns>
        static double GetLineAngleAsRadiant(Vertex start, Vertex end)
        {
            return Math.Atan2(end.Y - start.Y, end.X - start.X);
        }


        /// <summary>
        /// Find the position of y on the parabolar given focus and directix y. The equation to find a point on the parabola
        /// given those two value is: (x−a)2+b2−c2=2(b−c)y so y=((x−a)2+b2−c2)/2(b−c)
        /// </summary>
        /// <param name="x">The x value for which a y value is wanted</param>
        /// <param name="focus">The focus of the parabola</param>
        /// <param name="directrix_y">The y value of the directx line</param>
        /// <returns>The y value associated with x</returns>
        static double ParabolaY(double x, Vertex focus, double directrix_y)
        {
            return (Math.Pow(x - focus.X, 2) + Math.Pow(focus.Y, 2) - Math.Pow(directrix_y, 2)) / (2 * (focus.Y - directrix_y));
        }


        /// <summary>
        /// Interpolate parabola points between two points. The equation is computed by turning the input segment
        /// into the directix of the parabola, and the input point into the focus of the parabola. The directix used
        /// to solve the parabola is horizontal (parallel to x-axis).
        /// </summary>
        /// <param name="focus">The input point that will be used as a focus point.</param>
        /// <param name="dir">The input segment that will be used a the directix</param>
        /// <param name="par_start">The point on the parabola used as a starting point.</param>
        /// <param name="par_end">The point on the parabola used as a ending point.</param>
        /// <param name="max_distance">The maximum distance between 2 points on the parabola</param>
        /// <param name="tolerance">The maximum distance between 2 points on the parabola</param>
        /// <returns></returns>
        public static List<Vertex> Densify(Vertex focus, Vertex dir_start, Vertex dir_end, Vertex par_start, Vertex par_end, double max_distance, double tolerance)
        {

            if (max_distance <= 0)
                throw new ArgumentOutOfRangeException(String.Format("The maximum distance must be greater than 0. Value passed: {0}", max_distance));

            if (tolerance < 0)
                throw new ArgumentOutOfRangeException(String.Format("The tolerance must be greater than or equal to 0. Value passed: {0}", tolerance));

            #region Rotate Input Points

            //Compute the information required to perform rotation
            double shift_X = Math.Min(dir_start.X, dir_end.X);
            double shift_Y = Math.Min(dir_start.Y, dir_end.Y);
            double angle = GetLineAngleAsRadiant(dir_start, dir_end);

            Vertex focus_rotated = Rotation.Rotate(
                focus, 
                angle, 
                shift_X, 
                shift_Y
            );

            Vertex dir_startPoint_rotated = Rotation.Rotate(
                dir_start,
                angle,
                shift_X,
                shift_Y
            );

            Vertex dir_endPoint_rotated = Rotation.Rotate(
                dir_end,
                angle,
                shift_X,
                shift_Y);

            Vertex par_startPoint_rotated = Rotation.Rotate(
                par_start, 
                angle, 
                shift_X, 
                shift_Y
            );
            
            Vertex par_endPoint_rotated = Rotation.Rotate(
                par_end, 
                angle, 
                shift_X, 
                shift_Y
            );

            #endregion

            #region Validate the equation on first and last points given by Boost
            //Set parabola parameters
            double directrix = dir_endPoint_rotated.Y;
            double snapTolerance = 5;


            List<Vertex> densified_rotated = new List<Vertex>();
            Stack<Vertex> next = new Stack<Vertex>();

            ParabolaProblemInformation nonRotatedInformation = new ParabolaProblemInformation(
                    focus,
                    dir_start,
                    dir_end,
                    par_start,
                    par_end
            );


            double distanceFocusToDirectix = 0;
            Distance.GetClosestPointOnLine(focus, dir_start, dir_end, out distanceFocusToDirectix);
            if (distanceFocusToDirectix == 0)
                throw new FocusOnDirectixException(nonRotatedInformation);

            ParabolaProblemInformation rotatedInformation = new ParabolaProblemInformation(
                focus_rotated,
                dir_startPoint_rotated,
                dir_endPoint_rotated,
                par_startPoint_rotated,
                par_endPoint_rotated
            );

            List<Tuple<Vertex, Vertex>> points = new List <Tuple<Vertex, Vertex>>();
            points.Add(
                Tuple.Create<Vertex, Vertex>(
                    par_startPoint_rotated, 
                    new Vertex(par_startPoint_rotated.X, ParabolaY(par_startPoint_rotated.X, focus_rotated, directrix))
                 )
            );

            points.Add(
                Tuple.Create<Vertex, Vertex>(
                    par_endPoint_rotated,
                    new Vertex(par_endPoint_rotated.X, ParabolaY(par_endPoint_rotated.X, focus_rotated, directrix))
                 )
            );

            foreach (var point in points)
            {
                double delta = point.Item1.Y > point.Item2.Y ?
                        point.Item1.Y - point.Item2.Y : point.Item2.Y - point.Item1.Y;

                if (delta > snapTolerance)
                {
                    GenerateParabolaIssueInformation(rotatedInformation, nonRotatedInformation, point.Item1, point.Item2, 0.001);
                    throw new Exception(
                        String.Format(
                            "The computed Y on the parabola for the starting / ending point is different from the rotated point returned by Boost. Difference: {0}",
                            delta)
                         );
                }
            }
            #endregion

            #region Compute Intermediate Points (Rotated)
            Vertex previous = points[0].Item2;
            densified_rotated.Add(previous);
            next.Push(points[1].Item2);

            while (next.Count > 0)
            {
                Vertex current = next.Peek();
                double mid_cord_x = (previous.X + current.X) / 2;
                Vertex mid_curve = new Vertex(mid_cord_x, ParabolaY(mid_cord_x, focus_rotated, directrix));
                double distance = Distance.ComputeDistanceBetweenPoints(current, previous);
                if (distance > max_distance)
                {
                    next.Push(mid_curve);
                }
                else
                {
                    next.Pop();
                    densified_rotated.Add(current);
                    previous = current;
                }
            }
            #endregion

            #region Unrotate and validate
            List<Vertex> densified = densified_rotated.Select(w => Rotation.Unrotate(w, angle, shift_X, shift_Y)).ToList();
            
            //reset the first and last points so they match exactly.
            if (Math.Abs(densified[0].X - par_start.X) > snapTolerance ||
                Math.Abs(densified[0].Y - par_start.Y) > snapTolerance)
                throw new Exception(String.Format("Segmented curve start point is not correct. Tolerance exeeded in X ({0}) or Y ({1})",
                    Math.Abs(densified[0].X - par_start.X), Math.Abs(densified[0].Y - par_start.Y)));
            densified[0] = par_start;

            if (Math.Abs(densified[densified.Count - 1].X - par_end.X) > snapTolerance ||
                Math.Abs(densified[densified.Count - 1].Y - par_end.Y) > snapTolerance)
                throw new Exception(String.Format("Segmented curve end point is not correct. Tolerance exeeded in X ({0}) or Y ({1})",
                    Math.Abs(densified[densified.Count - 1].X - par_end.X), Math.Abs(densified[densified.Count - 1].Y - par_end.Y)));
            densified[densified.Count - 1] = par_end;
            #endregion
            
            return densified;
        }


        /// <summary>
        /// Generate an exception when the point computed by the parabola equation is different from the point computed by boost.
        /// </summary>
        /// <param name="rotatedInformation">The information used to solve parabola. This is the is the information before the rotation.</param>
        /// <param name="nonRotatedInformation">The information used to solve parabola. This is the is the information after the rotation.</param>
        /// <param name="boostPoint">The point on the parabola returned by Boost.</param>
        /// <param name="parabolaPoint">The point on the parabola computed.</param>
        /// <param name="tolerance">The tolerance used to decide if an exception need to be raise.</param>
        private static void GenerateParabolaIssueInformation(ParabolaProblemInformation rotatedInformation, ParabolaProblemInformation nonRotatedInformation, Vertex boostPoint, Vertex parabolaPoint, double tolerance)
        {
            if (tolerance < 0)
                throw new ArgumentOutOfRangeException(String.Format("Tolenrance must be greater than 0"));

            double minX = Math.Min(Math.Min(Math.Min(rotatedInformation.DirectixSegmentStart.X, rotatedInformation.DirectixSegmentStart.X), boostPoint.X), parabolaPoint.X);
            double maxX = Math.Max(Math.Max(Math.Max(rotatedInformation.DirectixSegmentStart.X, rotatedInformation.DirectixSegmentStart.X), boostPoint.X), parabolaPoint.X);

            //Compute the distance between the input parabola point
            double distanceBoostPointToFocus = Distance.ComputeDistanceBetweenPoints(boostPoint, rotatedInformation.FocusPoint);
            double distanceBoostPointToDirectix = 0;
            Distance.GetClosestPointOnLine(
                    new Vertex(minX, rotatedInformation.DirectixSegmentEnd.Y),
                    new Vertex(maxX, rotatedInformation.DirectixSegmentEnd.Y),
                    boostPoint, 
                    out distanceBoostPointToDirectix
            );


            double distanceComputedPointToFocus = Distance.ComputeDistanceBetweenPoints(parabolaPoint, rotatedInformation.FocusPoint);
            double distanceComputedPointToDirectix = 0;
            Distance.GetClosestPointOnLine(
                    new Vertex(minX, rotatedInformation.DirectixSegmentEnd.Y),
                    new Vertex(maxX, rotatedInformation.DirectixSegmentEnd.Y),
                    parabolaPoint,
                    out distanceComputedPointToDirectix
            );

            double distanceDiff = distanceComputedPointToFocus > distanceComputedPointToDirectix ?
                distanceComputedPointToFocus - distanceComputedPointToDirectix : distanceComputedPointToDirectix - distanceComputedPointToFocus;

            if (distanceDiff < tolerance || Double.IsNaN(distanceDiff) || Double.IsInfinity(distanceDiff))
                throw new UnsolvableVertexException(nonRotatedInformation, rotatedInformation, boostPoint, parabolaPoint,
                    distanceBoostPointToFocus, distanceComputedPointToFocus, distanceBoostPointToDirectix, distanceComputedPointToDirectix);

        }
    }
}
