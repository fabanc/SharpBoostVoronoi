using SharpBoostVoronoi;
using SharpBoostVoronoi.Input;
using SharpBoostVoronoi.Output;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleWPFApp.Models
{
    class GraphData
    {
        public List<Point> InputPoints{ get; set; }
        public List<Segment> InputSegments { get; set; }
        public List<Vertex> OutputVertices { get; set; }
        public List<Edge> OutputEdges { get; set; }
        public List<Cell> OutputCells { get; set; }

        public GraphData(List<Point> inputPoints, List<Segment> segments)
        {
            if (inputPoints == null || InputSegments == null)
                throw new ArgumentNullException();

            InputPoints = new List<Point>();
            InputSegments = new List<Segment>();
            BoostVoronoi v = new BoostVoronoi();

            //Populate the input
            foreach (var point in inputPoints)
            {
                InputPoints.Add(point);
                v.AddPoint(point.X, point.Y);
            }

            foreach (var segment in segments)
            {
                InputSegments.Add(segment);
                v.AddSegment(segment.Start.X, segment.Start.Y, segment.End.X, segment.End.Y);
            }

            //Construct
            v.Construct();

            //Populate the output
            OutputVertices = v.Vertices;
            OutputEdges = v.Edges;
            OutputCells = v.Cells;
        }
    }
}
