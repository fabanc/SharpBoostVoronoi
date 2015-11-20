using SharpBoostVoronoi.Input;
using SharpBoostVoronoi.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoostVoronoi
{
    public class BoostVoronoi
    {

        /// <summary>
        /// The input points used to construct the voronoi diagram
        /// </summary>
        public List<Point> InputPoints { get; private set; }

        /// <summary>
        /// The input segments used to construct the voronoi diagram
        /// </summary>
        public List<Segment> InputSegments { get; private set; }

        /// <summary>
        /// The output list of vertices
        /// </summary>
        public List<Vertex> Vertices { get; set; }

        /// <summary>
        /// The output list of edges
        /// </summary>
        public List<Edge> Edges { get; set; }

        /// <summary>
        /// The output list of cells
        /// </summary>
        public List<Cell> Cells { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public BoostVoronoi()
        {
            InputPoints = new List<Point>();
            InputSegments = new List<Segment>();

        }

        /// <summary>
        /// Calls the voronoi API in order to build the voronoi cells. TO-BE-DONE!
        /// </summary>
        public void Construct()
        {

        }

        /// <summary>
        /// Add a point to the list of input points
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void AddPoint(int x, int y)
        {
            InputPoints.Add(new Point(x, y));
        }

        /// <summary>
        /// Add a segment to the list of input segments
        /// </summary>
        /// <param name="x1">X coordinate of the start point</param>
        /// <param name="y1">Y coordinate of the start point</param>
        /// <param name="x2">X coordinate of the end point</param>
        /// <param name="y2">Y coordinate of the end point</param>
        public void AddSegment(int x1, int y1, int x2, int y2)
        {
            InputSegments.Add(new Segment()
            {
                Start = new Point(x1,y1), 
                End = new Point(x2,y2)
            });
        }

    }
}
