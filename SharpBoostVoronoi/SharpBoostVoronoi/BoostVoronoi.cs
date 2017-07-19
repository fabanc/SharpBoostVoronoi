using boost;
using SharpBoostVoronoi.Parabolas;
using SharpBoostVoronoi.Exceptions;
using SharpBoostVoronoi.Input;
using SharpBoostVoronoi.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace SharpBoostVoronoi
{
    public class BoostVoronoi : IDisposable
    {

        public bool disposed = false;

        private int _scaleFactor = 0;


        /// <summary>
        /// The reference to the CLR wrapper class
        /// </summary>
        private VoronoiWrapper VoronoiWrapper { get; set; }

        /// <summary>
        /// The input points used to construct the voronoi diagram
        /// </summary>
        public Dictionary<long, Point> InputPoints { get; private set; }

        /// <summary>
        /// The input segments used to construct the voronoi diagram
        /// </summary>
        public Dictionary<long, Segment> InputSegments { get; private set; }

        /// <summary>
        /// A scale factor. It will be used as a multiplier for input coordinates. Output coordinates will be divided by the scale factor automatically.
        /// </summary>
        public int ScaleFactor { get { return _scaleFactor; } private set { _scaleFactor = value; Tolerance = Convert.ToDouble(1) / _scaleFactor; } }

        /// <summary>
        /// A property used to define tolerance to parabola interpolation.
        /// </summary>
        public double Tolerance { get; set; } 


        public long CountVertices {get; private set;}
        public long CountEdges {get; private set;}
        public long CountCells {get; private set;}

        /// <summary>
        /// Default constructor
        /// </summary>
        public BoostVoronoi ()
        {
            InputPoints = new Dictionary<long, Point>();
            InputSegments = new Dictionary<long, Segment>();
            VoronoiWrapper = new VoronoiWrapper();
            ScaleFactor = 1;
            CountVertices = -1;
            CountEdges = -1;
            CountCells = -1;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); 
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            //if (disposing)
            //{
            //    //Free managed object here
            //
            //}

            // Free any unmanaged objects here.
            VoronoiWrapper.Dispose();

            disposed = true;
        }

        /// <summary>
        /// Constructor that allows to define a scale factor.
        /// </summary>
        /// <param name="scaleFactor"> A scale factor greater than zero. It will be used as a multiplier for input coordinates. Output coordinates will be divided by the scale factor automatically.</param>
        public BoostVoronoi(int scaleFactor)
        {
            InputPoints = new Dictionary<long, Point>();
            InputSegments = new Dictionary<long, Segment>();
            VoronoiWrapper = new VoronoiWrapper();

            if (scaleFactor <= 0)
                throw new InvalidScaleFactorException();

            ScaleFactor = scaleFactor;
        }


        /// <summary>
        /// Calls the voronoi API in order to build the voronoi cells.
        /// </summary>
        public void Construct()
        {
            //Construct
            VoronoiWrapper.Construct();

            //Build Maps
            VoronoiWrapper.CreateVertexMap();
            VoronoiWrapper.CreateEdgeMap();
            VoronoiWrapper.CreateCellMap();

            //long maxEdgeSize = VoronoiWrapper.GetEdgeMapMaxSize();
            //long maxEdgeIndexSize = VoronoiWrapper.GetEdgeIndexMapMaxSize();

            this.CountVertices = VoronoiWrapper.CountVertices();
            this.CountEdges = VoronoiWrapper.CountEdges();
            this.CountCells = VoronoiWrapper.CountCells();
        }

        /// <summary>
        /// Clears the list of the inserted geometries.
        /// </summary>
        public void Clear()
        {
            VoronoiWrapper.Clear();
        }

        public Vertex GetVertex(long index)
        {
            if (index < -0 || index > this.CountVertices -1)
                throw new IndexOutOfRangeException();

            return new Vertex(VoronoiWrapper.GetVertex(index));
        }

        public Edge GetEdge(long index)
        {
            if (index < -0 || index > this.CountEdges -1)
                throw new IndexOutOfRangeException();
            return new Edge(VoronoiWrapper.GetEdge(index));
        }

        public Cell GetCell(long index)
        {
            if (index < -0 || index > this.CountCells -1)
                throw new IndexOutOfRangeException();
            return new Cell(VoronoiWrapper.GetCell(index));
        }

        



        /// <summary>
        /// Add a point to the list of input points
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        //public void AddPoint(int x, int y)
        //{
        //    InputPoints.Add(new Point(x * ScaleFactor, y * ScaleFactor));
        //}

        /// <summary>
        /// Add a point to the list of input points. The input points will be applied a scale factor
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void AddPoint(double x, double y)
        {
            Point p = new Point(Convert.ToInt32(x * ScaleFactor), Convert.ToInt32(y * ScaleFactor));
            InputPoints[InputPoints.Count] = p;
            VoronoiWrapper.AddPoint(p.X, p.Y);
        }

        /// <summary>
        /// Add a segment to the list of input segments
        /// </summary>
        /// <param name="x1">X coordinate of the start point</param>
        /// <param name="y1">Y coordinate of the start point</param>
        /// <param name="x2">X coordinate of the end point</param>
        /// <param name="y2">Y coordinate of the end point</param>
        //public void AddSegment(int x1, int y1, int x2, int y2)
        //{
        //    InputSegments.Add(new Segment(x1 * ScaleFactor,y1 * ScaleFactor,x2 * ScaleFactor,y2 * ScaleFactor));
        //}


        /// <summary>
        /// Add a segment to the list of input segments
        /// </summary>
        /// <param name="x1">X coordinate of the start point</param>
        /// <param name="y1">Y coordinate of the start point</param>
        /// <param name="x2">X coordinate of the end point</param>
        /// <param name="y2">Y coordinate of the end point</param>
        public void AddSegment(double x1, double y1, double x2, double y2)
        {
            Segment s = new Segment(
                 Convert.ToInt32(x1 * ScaleFactor),
                 Convert.ToInt32(y1 * ScaleFactor),
                 Convert.ToInt32(x2 * ScaleFactor),
                 Convert.ToInt32(y2 * ScaleFactor)
            );

            InputSegments[InputSegments.Count] = s;
            VoronoiWrapper.AddSegment(
                s.Start.X,
                s.Start.Y,
                s.End.X,
                s.End.Y
            );
        }

        #region Code to discretize curves
        //The code below is a simple port to C# of the C++ code in the links below
        //http://www.boost.org/doc/libs/1_54_0/libs/polygon/example/voronoi_visualizer.cpp
        //http://www.boost.org/doc/libs/1_54_0/libs/polygon/example/voronoi_visual_utils.hpp

        /// <summary>
        /// Generate a polyline representing a curved edge.
        /// </summary>
        /// <param name="edge">The curvy edge.</param>
        /// <param name="max_distance">The maximum distance between two vertex on the output polyline.</param>
        /// <returns></returns>
        public List<Vertex> SampleCurvedEdge(Edge edge, double max_distance)
        {
            long pointCell = -1;
            long lineCell = -1;

            //Max distance to be refined
            if (max_distance <= 0)
                throw new Exception("Max distance must be greater than 0");

            Point pointSite = null;
            Segment segmentSite = null;

            Edge twin = this.GetEdge(edge.Twin);
            Cell m_cell = this.GetCell(edge.Cell);
            Cell m_reverse_cell = this.GetCell(twin.Cell);

            if (m_cell.ContainsSegment == true && m_reverse_cell.ContainsSegment == true)
                                return new List<Vertex>(){this.GetVertex(edge.Start),this.GetVertex(edge.End)};

            if (m_cell.ContainsPoint)
            {
                pointCell = edge.Cell;
                lineCell = twin.Cell;
            }
            else
            {
                lineCell = edge.Cell;
                pointCell = twin.Cell;
            }
        
            pointSite = RetrieveInputPoint(this.GetCell(pointCell));
            segmentSite = RetrieveInputSegment(this.GetCell(lineCell));
        
            List<Vertex> discretization = new List<Vertex>(){
                this.GetVertex(edge.Start),
                this.GetVertex(edge.End)
            };

            if (edge.IsLinear)
                return discretization;


            return ParabolaComputation.Densify(
                new Vertex(Convert.ToDouble(pointSite.X) / Convert.ToDouble(ScaleFactor), Convert.ToDouble(pointSite.Y) / Convert.ToDouble(ScaleFactor)), 
                new Vertex(Convert.ToDouble(segmentSite.Start.X) / Convert.ToDouble(ScaleFactor), Convert.ToDouble(segmentSite.Start.Y) / Convert.ToDouble(ScaleFactor)),
                new Vertex(Convert.ToDouble(segmentSite.End.X) / Convert.ToDouble(ScaleFactor), Convert.ToDouble(segmentSite.End.Y) / Convert.ToDouble(ScaleFactor)), 
                discretization[0], 
                discretization[1], 
                max_distance,
                Tolerance
            );
        }


        /// <summary>
        ///  Retrieve the input point site asssociated with a cell. The point returned is the one
        ///  sent to boost. If a scale factor was used, then the output coordinates should be divided by the
        ///  scale factor. An exception will be returned if this method is called on a cell that does
        ///  not contain a point site.
        /// </summary>
        /// <param name="cell">The cell that contains the point site.</param>
        /// <returns>The input point site of the cell.</returns>
        public Point RetrieveInputPoint(Cell cell)
        {
            Point pointNoScaled = null;
            if (cell.SourceCategory == CellSourceCatory.SinglePoint)
                pointNoScaled = InputPoints[cell.Site];
            else if (cell.SourceCategory == CellSourceCatory.SegmentStartPoint)
                pointNoScaled = InputSegments[RetriveInputSegmentIndex(cell)].Start;
            else if (cell.SourceCategory == CellSourceCatory.SegmentEndPoint)
                pointNoScaled = InputSegments[RetriveInputSegmentIndex(cell)].End;
            else
                throw new Exception("This cells does not have a point as input site");

            return new Point(pointNoScaled.X, pointNoScaled.Y);
        }


        /// <summary>
        ///  Retrieve the input segment site asssociated with a cell. The segment returned is the one
        ///  sent to boost. If a scale factor was used, then the output coordinates should be divided by the
        ///  scale factor. An exception will be returned if this method is called on a cell that does
        ///  not contain a segment site.
        /// </summary>
        /// <param name="cell">The cell that contains the segment site.</param>
        /// <returns>The input segment site of the cell.</returns>
        public Segment RetrieveInputSegment(Cell cell)
        {
            Segment segmentNotScaled = InputSegments[RetriveInputSegmentIndex(cell)];
            return new Segment(new Point(segmentNotScaled.Start.X, segmentNotScaled.Start.Y), 
                new Point(segmentNotScaled.End.X, segmentNotScaled.End.Y));
        }

        private long RetriveInputSegmentIndex(Cell cell)
        {
            if (cell.SourceCategory == CellSourceCatory.SinglePoint)
                throw new Exception("Attempting to retrive an input segment on a cell that was built around a point");
            return cell.Site - InputPoints.Count;
        }

        #endregion
    }
}
