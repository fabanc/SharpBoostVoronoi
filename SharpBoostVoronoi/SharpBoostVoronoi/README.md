# SharpBoostVoronoi

##Purpose
This project creates the dll you want to use when generating voronoi cells from line and point.


##How to use
A complete example on how to use the library is in the projects SampleApp and SampleAppWPF within that same Visual Studio Solution. I have pasted the code sample below.

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

The coordinates of the input segments and points are being converted as integer behind the scene. This is a requirement from the boost library. However, you can use the property ScaleFactor. The scale factor will be used to multiply 
the input coordinates and avoid a loss of accuracy. The coordinates of the output points will be divided by the scale factor. The example below shows how to use the scale factor with number having 2 decimals, and avoid any loss in precision:

```
            List<Segment> input = new List<Segment>();
            input.Add(new Segment(0, 0, 0, 10.55));
            input.Add(new Segment(0, 10.55, 10.55, 10.55));
            input.Add(new Segment(10.55, 10.55, 10.55, 0));
            input.Add(new Segment(10.55, 0, 0, 0));

            //Instanciate the voronoi wrapper
            BoostVoronoi bv = new BoostVoronoi();
			bv.ScaleFactor = 100;
			
            //Add the segments
            foreach (var s in input)
                bv.AddSegment(s.Start.X, s.Start.Y, s.End.X, s.End.Y);	

            //Build the C# Voronoi
            bv.Construct();

			//Same code that in the example above to retrieve the output
			.....
			
```

##Output data structure
Cells, Edges, and Vertices are exposing information exposed through the boost API. They is documentation generated along with this dll. For me information about what those properties mean, I invite you to read the boost documentation here: http://www.boost.org/doc/libs/1_54_0/libs/polygon/doc/voronoi_diagram.htm

#Cells
Each cell contains a reference to the segment it is made of. Those segment indexes are stored in the property EdgesIndex as a list of integers. Those indexes are indexes referring to segment in BoostVoronoi.Edges. Each cell also contains an important property call IsOpen. Not all the polygons returned by boost are closed. Some use an infinite point when a cell has no limit. A cells that has the 
property IsOpen equals to true will contains segment that refer to a vertex id of -1. Trying to access a vertex in BoostVoronoi.Vertices is not possible.

#Edges
Each edge contain a reference to its vertex. The properties Start and End store the index of the first and last vertex of this segment. Each segment has only two vertices / nodes. Use this index to access the corresponding vertex in BoostVoronoi.Vertices.
Always check first that the value of Start or End is always different from -1.

