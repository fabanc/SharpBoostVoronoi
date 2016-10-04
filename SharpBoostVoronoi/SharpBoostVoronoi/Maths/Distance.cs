using SharpBoostVoronoi.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoostVoronoi.Maths
{
    public class Distance
    {
        /// <summary>
        /// Fetch the distance between two points
        /// </summary>
        /// <param name="p1">A point</param>
        /// <param name="p2">A point</param>
        /// <returns></returns>
        public static double ComputeDistanceBetweenPoints(Vertex p1, Vertex p2)
        {
            if (p1.X == p2.X && p1.Y == p2.Y)
            {
                return 0;
            }
            return Math.Sqrt(ComputeSquareDistanceBetweenPoints(p1, p2));
        }

        /// <summary>
        /// Fetch the square distance between 2 points
        /// </summary>
        /// <param name="p1">A point</param>
        /// <param name="p2">A point</param>
        /// <returns></returns>
        public static double ComputeSquareDistanceBetweenPoints(Vertex p1, Vertex p2)
        {
            return Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2);
        }

        /*****************************************
        Inspiration for finding the closest point : 
        http://stackoverflow.com/questions/849211/shortest-distance-between-a-point-and-a-line-segment/1501725#1501725
        ******************************************/
        /// <summary>
        /// Find the closest point on a segment from another point. The segment is expected to be a straight line between two points
        /// </summary>
        /// <param name="lineStart">The first point of the segment</param>
        /// <param name="lineEnd">The last point of the segment</param>
        /// <param name="point">The point projected on the line</param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static Vertex GetClosestPointOnLine(Vertex lineStart, Vertex lineEnd, Vertex point, out double distance)
        {
            //Test if the line has a length <> 0
            double dist2 = ComputeSquareDistanceBetweenPoints(lineStart, lineEnd);
            if (dist2 == 0)
            {
                distance = ComputeDistanceBetweenPoints(point, lineStart);
                return lineStart;
            }

            //Compute the projection of on the line
            double t = ((point.X - lineStart.X) * (lineEnd.X - lineStart.X) + (point.Y - lineStart.Y) * (lineEnd.Y - lineStart.Y)) / dist2;


            if (t < 0)//point projection falls beyond the first node of the segment
            {
                distance = ComputeDistanceBetweenPoints(point, lineStart);
                return lineStart;
            }

            else if (t > 1)//point projection falls beyond the first node of the segment
            {
                distance = ComputeDistanceBetweenPoints(point, lineEnd);
                return lineEnd;
            }

            Vertex projectionPoint = new Vertex(lineStart.X + t * (lineEnd.X - lineStart.X), lineStart.Y + t * (lineEnd.Y - lineStart.Y));
            distance = ComputeDistanceBetweenPoints(point, projectionPoint);
            return projectionPoint;
        }


        /// <summary>
        /// Find the closest point on a segment from another point. The segment is expected to be a straight line between two points
        /// </summary>
        /// <param name="lineStart">The first point of the segment</param>
        /// <param name="lineEnd">The last point of the segment</param>
        /// <param name="point">The point projected on the line</param>
        /// <returns></returns>
        public static Vertex GetClosestPointOnLine(Vertex lineStart, Vertex lineEnd, Vertex point)
        {
            double distance = 0;
            return GetClosestPointOnLine(lineStart, lineEnd, point, out distance);
        }



        /// <summary>
        /// Compute the vector between two points
        /// </summary>
        /// <param name="lineStart">The first point of the segment</param>
        /// <param name="lineEnd">The last point of the segment</param>
        /// <returns></returns>
        public static Vertex ComputeVector(Vertex lineStart, Vertex lineEnd)
        {
            return new Vertex(lineEnd.X - lineStart.X, lineEnd.Y - lineStart.Y);
        }

        /// <summary>
        /// Compute a point on the line at a specific distance.
        /// </summary>
        /// <param name="p1">The first point of the line.</param>
        /// <param name="p2">The last point of the line.</param>
        /// <param name="distanceOnLine">The distance on the line where the point will be fetched.</param>
        /// <returns></returns>
        public static Vertex GetPointAtDistance(Vertex p1, Vertex p2, double distanceOnLine)
        {
            double dx = p2.X - p1.X;
            double dy = p2.Y - p1.Y;
            double l = Math.Sqrt(Math.Pow(dx,2) + Math.Pow(dy,2));
            if(distanceOnLine > l)
                throw new Exception ("Length is greater than the length of the segment");

            return new Vertex(p1.X + dx/l * distanceOnLine, p1.Y + dy/l * distanceOnLine);
        }

    }
}
