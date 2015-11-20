using SharpBoostVoronoi.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoostVoronoi.Input
{
    public class Segment
    {
        public Point Start{ get; set; }
        public Point End { get; set; }

        public Segment(Point start, Point end)
        {
            Start = start;
            End = end;
        }

        public Segment(int x1, int y1, int x2, int y2)
        {
            Start = new Point(x1, y1);
            End = new Point(x2, y2);
        }
    }
}
