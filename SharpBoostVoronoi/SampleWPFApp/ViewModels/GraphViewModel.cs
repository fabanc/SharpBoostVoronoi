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
            Graphs.Add(BuildGraph3());
            Graphs.Add(BuildGraph4());
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
                new Segment(p1,new Point(50,50))
            };
            return new GraphData("Graph1", InputPoints, InputSegments);
        }

        private GraphData BuildGraph2()
        {
            Point p0 = new Point(200, 250);
            Point p1 = new Point(400, 250);

            List<Point> InputPoints = new List<Point>();
            List<Segment> InputSegments = new List<Segment> {
                new Segment(new Point(0,0), new Point(0,500)),
                new Segment(new Point(0,0), new Point(500,0)),
                new Segment(new Point(0,0), new Point(500,500)),
                new Segment(new Point(500,0), new Point(500,500)),
                new Segment(new Point(0,500), new Point(500,500)),
            };
            return new GraphData("Graph2", InputPoints, InputSegments);
        }

        private GraphData BuildGraph3()
        {

            List<Point> InputPoints = new List<Point>() { new Point(250, 250) };
            List<Segment> InputSegments = new List<Segment> {
                new Segment(new Point(0,0), new Point(0,500)),
                new Segment(new Point(0,0), new Point(500,0)),
                new Segment(new Point(500,0), new Point(500,500)),
                new Segment(new Point(0,500), new Point(500,500)),
            };
            return new GraphData("Graph3", InputPoints, InputSegments);
        }

        private GraphData BuildGraph4()
        {
            List<Point> InputPoints = new List<Point>() 
            { 
                new Point(250, 250),
                new Point (0,0),
                new Point (150,200),
                new Point (0,500),
                new Point (500,0),
                new Point (500,500),
                new Point (400,400),
            };

            List<Segment> InputSegments = new List<Segment> {
                new Segment(new Point(0,0), new Point(0,500)),
                new Segment(new Point(0,0), new Point(500,0)),
                new Segment(new Point(500,0), new Point(500,500)),
                new Segment(new Point(0,500), new Point(500,500)),
            };
            return new GraphData("Graph4", InputPoints, InputSegments);

        }

    }
}
