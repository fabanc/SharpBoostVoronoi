# SharpBoostVoronoi

##This .NET solution has 3 project
1. SharpBoostVoronoi is the library that exposes the C++ wrapper through a set of C# classes. If you want to use the boost voronoi library in .NET, this is the project you need to import.
2. Sample App and SampleWPFApp are two applications that illustrate the use of SharpBoostVoronoi. SampleWPFApp allows you to visualize some test data and the results computed by the boost library.


##How to use
A complete example on how to use the library is in the project SampleApp of the solution SharpBoostVoronoi. I have pasted the code sample below.

In a nutshell, you pass a set of Segments or Points to the class BoostVoronoi. Those classes are defined in SharpBoostVoronoi.Input and their use is demonstrated in the code below. Constructing the diagram populates 3 properties of the class BoostVoronoi:

1. Vertices: the list of output vertices and their coordinates.
2. Edges: the list of edges that connect the vertices in a cell. The properties Start and End of member of the class Edge represent the indexes of the start and end vertices in Vertices.
3. Cells: the list of voronoi cells created by boost.

Those properties also contains other attributes of the boost voronoi API described in http://www.boost.org/doc/libs/1_59_0/libs/polygon/doc/voronoi_diagram.htm. Take a look at the boost documentation to understand what they mean.

```
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

            //Add the segments
            foreach (var s in input)
                bv.AddSegment(s.Start.X, s.Start.Y, s.End.X, s.End.Y);

            //Add a point
            bv.AddPoint(5, 5);

            //Build the C# Voronoi
            bv.Construct();

            //Get the voronoi output
            List<Cell> cells = bv.Cells;
            List<Edge> edges = bv.Edges;
            List<Vertex> vertices = bv.Vertices;

            foreach (var cell in cells)
            {
                Console.Out.WriteLine(String.Format("Cell Identifier {0}", cell.Index));

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
```