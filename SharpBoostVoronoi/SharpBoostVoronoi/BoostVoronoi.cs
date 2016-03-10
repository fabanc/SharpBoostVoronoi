using boost;
using SharpBoostVoronoi.Exceptions;
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
        /// The reference to the CLR wrapper class
        /// </summary>
        private VoronoiWrapper VoronoiWrapper { get; set; }

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
        /// A scale factor. It will be used as a multiplier for input coordinates. Output coordinates will be divided by the scale factor automatically.
        /// </summary>
        private int ScaleFactor { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public BoostVoronoi()
        {
            InputPoints = new List<Point>();
            InputSegments = new List<Segment>();
            VoronoiWrapper = new VoronoiWrapper();
            ScaleFactor = 1;
        }

        /// <summary>
        /// Constructor that allows to define a scale factor.
        /// </summary>
        /// <param name="scaleFactor"> A scale factor greater than zero. It will be used as a multiplier for input coordinates. Output coordinates will be divided by the scale factor automatically.</param>
        public BoostVoronoi(int scaleFactor)
        {
            InputPoints = new List<Point>();
            InputSegments = new List<Segment>();
            VoronoiWrapper = new VoronoiWrapper();

            if (scaleFactor <= 0)
                throw new InvalidScaleFactorException();

        }



        /// <summary>
        /// Calls the voronoi API in order to build the voronoi cells. TO-BE-DONE!
        /// </summary>
        public void Construct()
        {
            //Pass the input
            foreach (var p in InputPoints)
                VoronoiWrapper.AddPoint(p.X, p.Y);

            foreach (var s in InputSegments)
                VoronoiWrapper.AddSegment(
                    s.Start.X, s.Start.Y,
                    s.End.X, s.End.Y);

            //Construct
            VoronoiWrapper.ConstructVoronoi();

            //Store the output
            Vertices = new List<Vertex>();
            foreach (var t in VoronoiWrapper.GetVertices())
                Vertices.Add(new Vertex(t,ScaleFactor));

            Edges = new List<Edge>();
            foreach (var t in VoronoiWrapper.GetEdges())
                Edges.Add(new Edge(t));

            Cells = new List<Cell>();
            foreach (var t in VoronoiWrapper.GetCells())
                Cells.Add(new Cell(t));
        }

        /// <summary>
        /// Add a point to the list of input points
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void AddPoint(int x, int y)
        {
            InputPoints.Add(new Point(x  * ScaleFactor, y * ScaleFactor));
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
            InputSegments.Add(new Segment(x1 * ScaleFactor,y1 * ScaleFactor,x2 * ScaleFactor,y2 * ScaleFactor));
        }


    }
}
