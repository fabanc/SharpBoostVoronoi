using SharpBoostVoronoi;
using SharpBoostVoronoi.Input;
using SharpBoostVoronoi.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApp
{
    class Program
    {
        static void Main(string[] args)
        {

            //Define a set of segments
            List<Segment> input = new List<Segment>();
            input.Add(new Segment(0, 0, 0, 10));
            input.Add(new Segment(0, 10, 10, 10));
            input.Add(new Segment(10, 10, 10, 0));
            input.Add(new Segment(10, 0, 0, 0));
            input.Add(new Segment(0, 0, 5, 5));
            input.Add(new Segment(5, 5, 10, 10));

            //Build the C# Voronoi
            BoostVoronoi bv = new BoostVoronoi();
            foreach (var s in input)
                bv.AddSegment(s.Start.X, s.Start.Y, s.End.X, s.End.Y);
            bv.Construct();

            //Get the voronoi output
            List<Cell> cells = bv.Cells;
            List<Edge> edges = bv.Edges;
            List<Vertex> vertices = bv.Vertices;

            foreach (var cell in cells)
            {
                Console.Out.WriteLine(String.Format("Cell Identifier{0}", cell.Index));

                foreach (var edgeIndex in cell.EdgesIndex)
                {
                    Edge edge = edges[edgeIndex];
                    Console.Out.WriteLine(
                        String.Format("  Edge Index: {0}. Start vertex index: {1}, End vertex index: {2}", 
                        edgeIndex,
                        edge.Start,
                        edge.End));

                    //If the vertex index equals -1, it means the edge is infinite. It is impossible to print the coordinates.
                    if (edge.Start != -1 && edge.End != -1)
                    {
                        Vertex start = vertices[edge.Start];
                        Vertex end = vertices[edge.End];

                        Console.Out.WriteLine(
                            String.Format("     From:{0}, To: {1}",
                            start.ToString(),
                            end.ToString()));
                    }
                }
            }

            Console.In.ReadLine();
        }
    }
}
