# SharpBoostVoronoi

##This repository has 2 projects.

1. A CLR wrapper around the boost voronoi API. It will enable users to generate voronoi cells from line and segments in .NET. The content of this project is in the directory C++ wrapper.
2. A .NET wrapper around the CLR code to make the library easy to use in C#. The content of this project is in the directory SharpBoostVoronoi. SharpBoostVoronoi references a compiled version of the CLR wrapper in its subfolder lib. 

##Links:
 
1. Boost library: http://www.boost.org/
2. Step-by-step install guide for Boost with Visual Studio. https://www.youtube.com/watch?v=6trC5zVXzG0
3. Boost documentation about the voronoi API: http://www.boost.org/doc/libs/1_59_0/libs/polygon/doc/voronoi_diagram.htm

##Project status
The CLR wrapper and the C# wrapper are closed to completion. I want to add an attribute to identify cells that are not closed by the voronoi API, and add a WPF project that will demonstrate the input and the output.

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

##External links
1. A very useful link about the wrapping Boost with CLR and C# here: https://grevit.wordpress.com/2015/07/03/boost-c-library-in-c-revit-api/	
2. A similar project to SharpBoostVoronoi in python. It does not return a cell data structure per say but works very well: https://github.com/Voxel8/pyvoronoi

