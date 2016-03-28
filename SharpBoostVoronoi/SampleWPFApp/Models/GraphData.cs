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
        public string Name { get; set; }
        //public List<Point> InputPoints{ get; set; }
        //public List<Segment> InputSegments { get; set; }
        //public List<Vertex> OutputVertices { get; set; }
        //public List<Edge> OutputEdges { get; set; }
        //public List<Cell> OutputCells { get; set; }
        public BoostVoronoi VoronoiSolution { get; set; }

        public GraphData(string name, List<Point> inputPoints, List<Segment> segments)
        {
            if (String.IsNullOrEmpty(name) || inputPoints == null || segments == null)
                throw new ArgumentNullException();

            Name = name;
            //InputPoints = new List<Point>();
            //InputSegments = new List<Segment>();
            VoronoiSolution = new BoostVoronoi();

            //Populate the input
            foreach (var point in inputPoints)
            {
                //InputPoints.Add(point);
                VoronoiSolution.AddPoint(point.X, point.Y);
            }

            foreach (var segment in segments)
            {
                //InputSegments.Add(segment);
                VoronoiSolution.AddSegment(segment.Start.X, segment.Start.Y, segment.End.X, segment.End.Y);
            }

            //Construct
            VoronoiSolution.Construct();

            //Populate the output
            //OutputVertices = VoronoiSolution.Vertices;
            //OutputEdges = VoronoiSolution.Edges;
            //OutputCells = VoronoiSolution.Cells;
        }
    }
}
