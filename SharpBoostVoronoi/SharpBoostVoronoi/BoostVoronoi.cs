using boost;
using SharpBoostVoronoi.CurveSampling;
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

            ScaleFactor = scaleFactor;
        }



        /// <summary>
        /// Calls the voronoi API in order to build the voronoi cells.
        /// </summary>
        public void Construct()
        {
            //Pass the input
            foreach (var p in InputPoints)
                VoronoiWrapper.AddPoint(p.X, p.Y);

            foreach (var s in InputSegments)
                VoronoiWrapper.AddSegment(
                        s.Start.X, 
                        s.Start.Y,
                        s.End.X, 
                        s.End.Y );

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
            InputPoints.Add(new Point(Convert.ToInt32(x * ScaleFactor), Convert.ToInt32(y * ScaleFactor)));
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
            InputSegments.Add(new Segment(
                 Convert.ToInt32(x1 * ScaleFactor),
                 Convert.ToInt32(y1 * ScaleFactor),
                 Convert.ToInt32(x2 * ScaleFactor), 
                 Convert.ToInt32(y2 * ScaleFactor)
            ));
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
            int pointCell = -1;
            int lineCell = -1;

            //Max distance to be refined
            if (max_distance <= 0)
                throw new Exception("Max distance must be greater than 0");

            Point pointSite = null;
            Segment segmentSite = null;

            Cell m_cell = Cells[edge.Cell];
            Cell m_reverse_cell = Cells[Edges[edge.Twin].Cell];

            if (m_cell.ContainsSegment == true && m_reverse_cell.ContainsSegment == true)
                                return new List<Vertex>(){Vertices[edge.Start],Vertices[edge.End]};



            if(Cells[edge.Cell].ContainsPoint)
            {
                pointCell = edge.Cell;
                lineCell = Edges[edge.Twin].Cell;
            }
            else
            {
                lineCell = edge.Cell;
                pointCell = Edges[edge.Twin].Cell;
            }
        
            pointSite = RetrievePoint(Cells[pointCell]);
            segmentSite = RetrieveSegment(Cells[lineCell]);

            if (pointSite.HasSameCoordinates(segmentSite.Start))
                throw new InvalidCurveInputSites(String.Format(
                    "The point site of one cell is located on the starting point of the segment site of the other cell. Point Site: {0}, Segment Site: {1}, Point Cell Type: {2}",
                    pointSite.ToString(), segmentSite.ToString(), Cells[pointCell].SourceCategory));

            if (pointSite.HasSameCoordinates(segmentSite.End))
                throw new InvalidCurveInputSites(String.Format("The point site of one cell is located on the ending point of the segment site of the other cell. Point Site: {0}, Segment Site: {1}, Point Cell Type: {2}",
                    pointSite.ToString(), segmentSite.ToString(), Cells[pointCell].SourceCategory));
        
            List<Vertex> discretization = new List<Vertex>(){
                Vertices[edge.Start],
                Vertices[edge.End]
            };

            if (edge.IsLinear)
                return discretization;

            return DiscretizeByRotation.Densify(pointSite, segmentSite, discretization[0], discretization[1], max_distance);
        }



        private Point RetrievePoint(Cell cell)
        {
            Point pointNoScaled = null;
            if(cell.SourceCategory == CellSourceCatory.SinglePoint)
                pointNoScaled =  InputPoints[cell.Site];
            else if (cell.SourceCategory == CellSourceCatory.SegmentStartPoint)
            {
                Segment segment = InputSegments[RetriveInputSegmentIndex(cell)];
                pointNoScaled = InputSegments[RetriveInputSegmentIndex(cell)].Start;
            }
            else
                pointNoScaled = InputSegments[RetriveInputSegmentIndex(cell)].End;
            return new Point(pointNoScaled.X / ScaleFactor, pointNoScaled.Y / ScaleFactor);
        }

        private Segment RetrieveSegment(Cell cell)
        {
            Segment segmentNotScaled = InputSegments[RetriveInputSegmentIndex(cell)];
            return new Segment(new Point(segmentNotScaled.Start.X / ScaleFactor, segmentNotScaled.Start.Y / ScaleFactor), 
                new Point(segmentNotScaled.End.X / ScaleFactor, segmentNotScaled.End.Y / ScaleFactor));
        }

        private int RetriveInputSegmentIndex(Cell cell)
        {
            if (cell.SourceCategory == CellSourceCatory.SinglePoint)
                throw new Exception("Attempting to retrive an input segment on a cell that was built around a point");
            return cell.Site - InputPoints.Count;
        }

        #endregion
    }
}
