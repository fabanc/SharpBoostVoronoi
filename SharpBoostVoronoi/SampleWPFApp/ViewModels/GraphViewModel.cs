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
        }
        private GraphData BuildGraph1()
        {
            Point p0 = new Point(20, 25);
            Point p1 = new Point(40, 25);

            List<Point> InputPoints = new List<Point>();
            List<Segment> InputSegments = new List<Segment> {
                new Segment(new Point(0,0), new Point(0,50)),
                new Segment(new Point(0,0), new Point(50,50)),
                new Segment(new Point(50,0), new Point(50,50)),
                new Segment(new Point(0,50), new Point(50,50)),
                new Segment(new Point(5,5), new Point(5,45)),
                new Segment(new Point(5,5), new Point(45,5)),
                new Segment(new Point(45,5), new Point(45,45)),
                new Segment(new Point(5,45), new Point(45,45)),
                new Segment(new Point(5,5), p0),
                new Segment(new Point(5,45), p0),
                new Segment(p0,p1),
                new Segment(p1,new Point(45,45))
            };
            return new GraphData(InputPoints, InputSegments);
        }

    }
}
