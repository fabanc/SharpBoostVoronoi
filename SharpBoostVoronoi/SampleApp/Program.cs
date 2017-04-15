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

            //Define a set of segments and pass them to the voronoi wrapper
            List<Segment> input = new List<Segment>();
            input.Add(new Segment(0, 0, 0, 10));
            input.Add(new Segment(0, 10, 10, 10));
            input.Add(new Segment(10, 10, 10, 0));
            input.Add(new Segment(10, 0, 0, 0));

            //Instanciate the voronoi wrapper
            BoostVoronoi bv = new BoostVoronoi();

            //Add a point
            bv.AddPoint(5, 5);

            //Add the segments
            foreach (var s in input)
                bv.AddSegment(s.Start.X, s.Start.Y, s.End.X, s.End.Y);

            //Build the C# Voronoi
            bv.Construct();

            //Get the voronoi output
            //List<Cell> cells = bv.Cells;
            //List<Edge> edges = bv.Edges;
            //List<Vertex> vertices = bv.Vertices;

            for (long i = 0; i < bv.CountCells; i++ )
            {
                Cell cell = bv.GetCell(i);
                Console.Out.WriteLine(String.Format("Cell Identifier {0}. Is open = {1}", cell.Index, cell.IsOpen));
                foreach (var edgeIndex in cell.EdgesIndex)
                {
                    Edge edge = bv.GetEdge(edgeIndex);
                    Console.Out.WriteLine(
                        String.Format("  Edge Index: {0}. Start vertex index: {1}, End vertex index: {2}",
                        edgeIndex,
                        edge.Start,
                        edge.End));

                    //If the vertex index equals -1, it means the edge is infinite. It is impossible to print the coordinates.
                    if (edge.IsLinear)
                    {
                        Vertex start = bv.GetVertex(edge.Start);
                        Vertex end = bv.GetVertex(edge.End);

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
