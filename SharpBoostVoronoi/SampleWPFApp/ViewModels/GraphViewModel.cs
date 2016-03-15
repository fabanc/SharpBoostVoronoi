using SampleWPFApp.Models;
using SharpBoostVoronoi.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleWPFApp.ViewModels
{
    class GraphViewModel
    {
        public List <GraphData> Graphs { get; set; }

        public GraphViewModel()
        {
            Graphs = new List<GraphData>();
            Graphs.Add(BuildGraph1());
            Graphs.Add(BuildGraph2());
        }
        private GraphData BuildGraph1()
        {
            Point p0 = new Point(200, 250);
            Point p1 = new Point(400, 250);

            List<Point> InputPoints = new List<Point>();
            List<Segment> InputSegments = new List<Segment> {
                new Segment(new Point(0,0), new Point(0,500)),
                new Segment(new Point(0,0), new Point(500,0)),
                new Segment(new Point(500,0), new Point(500,500)),
                new Segment(new Point(0,500), new Point(500,500)),
                new Segment(new Point(50,50), new Point(50,450)),
                new Segment(new Point(50,50), new Point(450,50)),
                new Segment(new Point(450,50), new Point(450,450)),
                new Segment(new Point(50,450), new Point(450,450)),
                new Segment(new Point(50,50), p0),
                new Segment(new Point(50,450), p0),
                new Segment(p0,p1),
                new Segment(p1,new Point(45,45))
            };
            return new GraphData("Graph1", InputPoints, InputSegments);
        }

        private GraphData BuildGraph2()
        {
            Point p0 = new Point(20, 25);
            Point p1 = new Point(40, 25);

            List<Point> InputPoints = new List<Point>();
            List<Segment> InputSegments = new List<Segment> {
                new Segment(new Point(0,0), new Point(0,500)),
                new Segment(new Point(0,0), new Point(500,500)),
                new Segment(new Point(500,0), new Point(500,500)),
                new Segment(new Point(0,500), new Point(500,500)),
                new Segment(new Point(50,50), new Point(500,450)),
                new Segment(new Point(50,50), new Point(450,500)),
                new Segment(new Point(450,50), new Point(450,450)),
                new Segment(new Point(50,450), new Point(450,450))
            };
            return new GraphData("Graph2", InputPoints, InputSegments);
        }

    }
}
