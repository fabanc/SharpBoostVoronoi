# SharpBoostVoronoi

## Purpose
This project creates the dll you want to use when generating voronoi cells from line and point.


## How to use
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
            using(BoostVoronoi bv = new BoostVoronoi())
			{
				//Add a point
				bv.AddPoint(5, 5);

				//Add the segments
				foreach (var s in input)
					bv.AddSegment(s.Start.X, s.Start.Y, s.End.X, s.End.Y);

				//Build the C# Voronoi
				bv.Construct();

				//Get the voronoi output
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
			//Coordinates will be multiplied by 100 before being converted as integer and sent to Boost.
			//Coordinates returned by boost will be divided by 100 before being returned to you.
			bv.ScaleFactor = 100;
			
            //Add the segments
            foreach (var s in input)
                bv.AddSegment(s.Start.X, s.Start.Y, s.End.X, s.End.Y);	

            //Build the C# Voronoi
            bv.Construct();

			//Same code that in the example above to retrieve the output
			.....
			
```

## Output data structure
Cells, Edges, and Vertices are exposing information exposed through the boost API. There is documentation generated along with this dll. For more information about what those properties mean, I invite you to read the boost documentation here: http://www.boost.org/doc/libs/1_54_0/libs/polygon/doc/voronoi_diagram.htm

### Cells
Each cell contains a reference to the segment it is made of. Those segment indexes are stored in the property EdgesIndex as a list of integers. Those indexes are indexes referring to segment in BoostVoronoi.Edges. Each cell also contains an important property call IsOpen. Not all the polygons returned by boost are closed. Some use an infinite point when a cell has no limit. A cells that has the 
property IsOpen equals to true will contains segment that refer to a vertex id of -1. Trying to access a vertex in BoostVoronoi.Vertices is not possible.

### Edges
Each edge contain a reference to its vertex. The properties Start and End store the index of the first and last vertex of this segment. Each segment has only two vertices / nodes. Use this index to access the corresponding vertex in BoostVoronoi.Vertices.
Always check first that the value of Start or End is always different from -1.

### Vertices
They contains the coordinates of all the points used to build segments and cells.



## Discretizing segments

Note that some segments are actually not a straight line. This is the case for example with cells around the end point of a input segments, and common borders between cells created from segments and points. 
The boost API does not provide an ad-hoc method to draw those curves. The SharpBoostVoronoi library does through the method SampleCurvedEdge. The example below is from the WPF sample app.


```
            for (long edgeIndex = 0; edgeIndex < gData.VoronoiSolution.CountEdges; edgeIndex++)
            {
                Edge outputSegment = gData.VoronoiSolution.GetEdge(edgeIndex);
                if (!outputSegment.IsFinite)
                    continue;

                Vertex start = gData.VoronoiSolution.GetVertex(outputSegment.Start);
                Vertex end = gData.VoronoiSolution.GetVertex(outputSegment.End);

                if (outputSegment.IsLinear)
                {
                    DrawingArea.Children.Add(new Line()
                    {
                        X1 = start.X,
                        Y1 = start.Y,
                        X2 = end.X,
                        Y2 = end.Y,
                        Stroke = OutputStroke
                    });

                    DrawPoint(start.X, start.Y, OutputPointColoBrush, outputPointWidth, outputPointRadius);
                    DrawPoint(end.X, end.Y, OutputPointColoBrush, outputPointWidth, outputPointRadius);
                }
                else
                {
                    List<Vertex> discretizedEdge = gData.VoronoiSolution.SampleCurvedEdge(outputSegment, 2);
                    for (int i = 1; i < discretizedEdge.Count; i++)
                    {
                        DrawingArea.Children.Add(new Line()
                        {
                            X1 = discretizedEdge[i - 1].X,
                            Y1 = discretizedEdge[i - 1].Y,
                            X2 = discretizedEdge[i].X,
                            Y2 = discretizedEdge[i].Y,
                            Stroke = OutputStroke
                        });

                        DrawPoint(start.X, start.Y, OutputPointColoBrush, outputPointWidth, outputPointRadius);
                        DrawPoint(end.X, end.Y, OutputPointColoBrush, outputPointWidth, outputPointRadius);
                    }
                }
            }
```
## Exceptions

Trying to interpolate curves into a set of segments can return an exception for 2 identified cases:

1. FocusOnDirectixException: The input point is actually located on the input line. In that case, there can not be any computation of a parabola since a parabola is a line of equal distance between a point (focus) and a line (directix).
2. UnsolvableVertexException: The point computed by at the start / end of the parabola does not match the point returned by Boost, even though the point computed is indeed at mid distance between the input point and the input segment. In that case, we can not rely on the parabola equation.

The current recommendation is to catch those exceptions an draw the voronoi edge as a straight line. Both of those exception will return an attribute of type ParabolaProblemInformation returning all the parameters that were used when attempting to solve the parabola equation. This can be used for logging purposes.
At that point it is not sure why Boost sometimes return cases that trigger those exceptions. This is currently under investigation.

