using SharpBoostVoronoi;
using SharpBoostVoronoi.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Point> points = new List<Point>();
            List<Segment> segments = new List<Segment>();
            int limit = 10;
            segments = PopulateSegment(limit, limit);
            ConstructAndMeasure(ref points, ref segments);


            points = new List<Point>();
            segments = PopulateSegment(100, 100);
            ConstructAndMeasure(ref points, ref segments);

            points = new List<Point>();
            segments = PopulateSegment(100, 1000);
            ConstructAndMeasure(ref points, ref segments);

            points = new List<Point>();
            segments = PopulateSegment(100, 2500);
            ConstructAndMeasure(ref points, ref segments);

            points = new List<Point>();
            segments = PopulateSegment(100, 3500);
            ConstructAndMeasure(ref points, ref segments);

            points = new List<Point>();
            segments = PopulateSegment(100, 4000);
            ConstructAndMeasure(ref points, ref segments);


            //points = new List<Point>();
            //segments = PopulateSegment(100, 4500);
            //ConstructAndMeasure(ref points, ref segments);

            //points = new List<Point>();
            //segments = PopulateSegment(100, 5000);
            //ConstructAndMeasure(ref points, ref segments);

            //points = new List<Point>();
            //segments = PopulateSegment(100, 5500);
            //ConstructAndMeasure(ref points, ref segments);

            //points = new List<Point>();
            //segments = PopulateSegment(100, 6000);
            //ConstructAndMeasure(ref points, ref segments);

            //points = new List<Point>();
            //segments = PopulateSegment(100, 6500);
            //ConstructAndMeasure(ref points, ref segments);

            //points = new List<Point>();
            //segments = PopulateSegment(100, 7000);
            //ConstructAndMeasure(ref points, ref segments);

            //points = new List<Point>();
            //segments = PopulateSegment(100, 7500);
            //ConstructAndMeasure(ref points, ref segments);

            //points = new List<Point>();
            //segments = PopulateSegment(100, 10000);
            //ConstructAndMeasure(ref points, ref segments);

            //points = new List<Point>();
            //segments = PopulateSegment(100, 12500);
            //ConstructAndMeasure(ref points, ref segments);


            Console.In.ReadLine();
        }


        static List<Segment> PopulateSegment(int maxX, int maxY)
        {
            List<Segment>  segments = new List<Segment>();
            for (int i = 0; i < maxX; i++)
            {
                for (int j = 0; j < maxY; j++)
                {
                    segments.Add(new Segment(new Point(i, j), new Point(i, j + 1)));
                    //segments.Add(new Segment(new Point(i, j), new Point(i + 1, j)));
                    //segments.Add(new Segment(new Point(i, j), new Point(i + 1, j + 1)));
                }
            }
            return segments;
        }

        static void ConstructAndMeasure(ref List<Point> inputPoints, ref List<Segment> inputSegments)
        {
            Console.WriteLine(String.Format("Testing with {0} points and {1} segments", inputPoints.Count, inputSegments.Count));
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            BoostVoronoi bv = new BoostVoronoi();
            foreach (var point in inputPoints)
                bv.AddPoint(point.X, point.Y);

            foreach (var segment in inputSegments)
                bv.AddSegment(segment.Start.X, segment.Start.Y, segment.End.X, segment.End.Y);



            bv.Construct();

            // Stop timing.
            stopwatch.Stop();
            Console.WriteLine(String.Format("Vertices: {0}, Edges: {1}, Cells: {2}", bv.Vertices.Count, bv.Edges.Count, bv.Cells.Count));
            Console.WriteLine("Time elapsed: {0:hh\\:mm\\:ss\\:ff}.", stopwatch.Elapsed);

        }
    }
}
