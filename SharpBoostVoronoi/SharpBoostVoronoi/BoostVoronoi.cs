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
        /// Calls the voronoi API in order to build the voronoi cells.
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

        #region Code to discretize curves
        //The code below is a simple port to C# of the C++ code in the links below
        //http://www.boost.org/doc/libs/1_54_0/libs/polygon/example/voronoi_visualizer.cpp
        //http://www.boost.org/doc/libs/1_54_0/libs/polygon/example/voronoi_visual_utils.hpp


        public List<Vertex> SampleCurvedEdge(Edge edge)
        {
            //Max distance to be refined
            double max_dist = 2;
            Point pointSite = null;
            Segment segmentSite = null;
        
            pointSite = Cells[edge.Cell].ContainsPoint ? RetrievePoint(Cells[edge.Cell]) : RetrievePoint(Cells[Edges[edge.Twin].Cell]);
            segmentSite = Cells[edge.Cell].ContainsPoint ? RetrieveSegment(Cells[Edges[edge.Twin].Cell]) : RetrieveSegment(Cells[edge.Cell]);
        
            List<Vertex> discretization = new List<Vertex>(){
                Vertices[edge.Start],
                Vertices[edge.End]
            };
        
            return Discretize(pointSite, segmentSite, max_dist, discretization);
        }


        public Point RetrievePoint(Cell cell)
        {
            if(cell.SourceCategory == CellSourceCatory.SinglePoint)
            {
                return InputPoints[cell.Site];
            }
            else if (cell.SourceCategory == CellSourceCatory.SegmentStartPoint)
            {
                return InputSegments[cell.Site].Start;
            }
            else
            {
                return InputSegments[cell.Site].End;
            }
        }

        public Segment RetrieveSegment(Cell cell)
        {
            int segmentListIndex = cell.Site - InputPoints.Count;
            return InputSegments[segmentListIndex];
        }

        /// <summary>
        /// Discretized a curved segment from the voronoi results. Curved segments generally appears when a segment is at the border
        /// of a cell generated around an input point and a cell generated around an input segment.
        /// </summary>
        /// <param name="point">The input point associated on one side of the curved edge</param>
        /// <param name="segment">The input segment associated on one side of the curved edge</param>
        /// <param name="max_dist">maximum discretization distance.</param>
        /// <param name="discretization">The output segment to discretize</param>
        /// <returns></returns>
        private List<Vertex> Discretize(Point point, Segment segment, double max_dist, List<Vertex> discretization)
        {
            //Since this list if supposed to represent a voronoi edge, it has to have 2 vertices
            if (discretization.Count != 2)
                throw new InvalidNumberOfVertexException();

            double low_segment_x = segment.Start.X < segment.End.X ? segment.Start.X : segment.End.X;
            double low_segment_y = segment.Start.Y < segment.End.Y ? segment.Start.Y : segment.End.Y;

            double max_segment_x = segment.Start.X < segment.End.X ? segment.End.X : segment.Start.X;
            double max_segment_y = segment.Start.Y < segment.End.Y ? segment.End.Y : segment.Start.Y;

            // Apply the linear transformation to move start point of the segment to
            // the point with coordinates (0, 0) and the direction of the segment to
            // coincide the positive direction of the x-axis.
            double segm_vec_x = max_segment_x - low_segment_x;
            double segm_vec_y = max_segment_y - low_segment_y;
            double sqr_segment_length = segm_vec_x * segm_vec_x + segm_vec_y * segm_vec_y;

            // Compute x-coordinates of the endpoints of the edge
            // in the transformed space.
            double projection_start = sqr_segment_length * GetPointProjection(discretization.First(), segment);
            double projection_end = sqr_segment_length * GetPointProjection(discretization.Last(), segment);

            // Compute parabola parameters in the transformed space.
            // Parabola has next representation:
            // f(x) = ((x-rot_x)^2 + rot_y^2) / (2.0*rot_y).
            double point_vec_x = point.X - low_segment_x;
            double point_vec_y = point.Y - low_segment_y;
            double rot_x = segm_vec_x * point_vec_x + segm_vec_y * point_vec_y;
            double rot_y = segm_vec_x * point_vec_y - segm_vec_y * point_vec_x;

            // Save the last point.
            Vertex last_point = discretization.Last();
            List<Vertex> discretizedPoint = new List<Vertex>() { discretization.First() };


            // Use stack to avoid recursion.
            Stack<double> point_stack = new Stack<double>();
            point_stack.Push(projection_end);

            double cur_x = projection_start;
            double cur_y = parabola_y(cur_x, rot_x, rot_y);

            // Adjust max_dist parameter in the transformed space.
            double max_dist_transformed = max_dist * max_dist * sqr_segment_length;
            while (point_stack.Count != 0)
            {
                double new_x = point_stack.Peek();
                double new_y = parabola_y(new_x, rot_x, rot_y);

                // Compute coordinates of the point of the parabola that is
                // furthest from the current line segment.
                double mid_x = (new_y - cur_y) / (new_x - cur_x) * rot_y + rot_x;
                double mid_y = parabola_y(mid_x, rot_x, rot_y);

                double dist = (new_y - cur_y) * (mid_x - cur_x) -
                    (new_x - cur_x) * (mid_y - cur_y);
                dist = dist * dist / ((new_y - cur_y) * (new_y - cur_y) +
                    (new_x - cur_x) * (new_x - cur_x));

                if (dist <= max_dist_transformed)
                {
                    // Distance between parabola and line segment is less than max_dist.
                    point_stack.Pop();
                    double inter_x = (segm_vec_x * new_x - segm_vec_y * new_y) /
                        sqr_segment_length + low_segment_x;
                    double inter_y = (segm_vec_x * new_y + segm_vec_y * new_x) /
                        sqr_segment_length + low_segment_y;

                    discretizedPoint.Add(new Vertex(inter_x, inter_y));
                    cur_x = new_x;
                    cur_y = new_y;
                }
                else
                {
                    point_stack.Push(mid_x);
                }
            }
            discretizedPoint[discretizedPoint.Count - 1] = last_point;
            return discretizedPoint;
        }

        private double parabola_y(double x, double a, double b)
        {
            return ((x - a) * (x - a) + b * b) / (b + b);
        }

        private double GetPointProjection(Vertex point, Segment segment)
        {
            double low_segment_x = segment.Start.X < segment.End.X ? segment.Start.X : segment.End.X;
            double low_segment_y = segment.Start.Y < segment.End.Y ? segment.Start.Y : segment.End.Y;
            double max_segment_x = segment.Start.X < segment.End.X ? segment.End.X : segment.Start.X;
            double max_segment_y = segment.Start.Y < segment.End.Y ? segment.End.Y : segment.Start.Y;

            double segment_vec_x = max_segment_x - low_segment_x;
            double segment_vec_y = max_segment_y - low_segment_y;
            double point_vec_x = point.X - low_segment_x;
            double point_vec_y = point.Y - low_segment_y;

            double sqr_segment_length = segment_vec_x * segment_vec_x + segment_vec_y * segment_vec_y;
            double vec_dot = segment_vec_x * point_vec_x + segment_vec_y * point_vec_y;
            return vec_dot / sqr_segment_length;
        }
        #endregion
    }
}
