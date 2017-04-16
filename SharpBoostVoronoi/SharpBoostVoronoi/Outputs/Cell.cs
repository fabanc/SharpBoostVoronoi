using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoostVoronoi.Output
{
    public enum CellSourceCatory:short { Unknown = -1, SinglePoint = 0, SegmentStartPoint = 1, SegmentEndPoint = 2, InitialSegment = 3, ReverseSegment = 4, GeometryShift = 5, BitMask = 6};

    public class Cell
    {
        /// <summary>
        /// The voronoi cell identifier
        /// </summary>
        public long Index { get; set; }

        /// <summary>
        /// The index of the source feature
        /// </summary>
        public long Site { get; set; }

        /// <summary>
        /// True if the cell is made from a point
        /// </summary>
        public bool ContainsPoint { get; set; }

        /// <summary>
        /// True if the cell is made from a segment
        /// </summary>
        public bool ContainsSegment { get; set; }

        /// <summary>
        /// Indexes of the segment that makes the cell
        /// </summary>
        public List<long> EdgesIndex { get; set; }

        /// <summary>
        /// Indexes of the vertices that makes the cell
        /// </summary>
        public List<long> VerticesIndex { get; set; }

        /// <summary>
        /// True if the cell has a segment that is infinite
        /// </summary>
        public bool IsOpen { get; set; }

        /// <summary>
        /// Returns true if the cell doesn't have an incident edge. Can happen if a few input segments share a common endpoint.
        /// </summary>
        public bool IsDegnerate { get; set; }

        /// <summary>
        /// The type of element used to create the edge.
        /// </summary>
        public CellSourceCatory SourceCategory { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="t">A tuple returned by the CLR wrapper.</param>
        public Cell(Tuple <int, int, bool, bool, List<long>, bool, short> t)
        {
            Index = t.Item1;
            Site = t.Item2;
            ContainsPoint = t.Item3;
            ContainsSegment = t.Item4;
            EdgesIndex = t.Item5;
            IsOpen = t.Item6;
            SourceCategory = (CellSourceCatory)t.Item7;

        }


        public Cell(Tuple<long, long, short, Tuple<bool, bool, bool, bool>, List<long>, List<long>> t)
        {
            Index = t.Item1;
            Site = t.Item2;
            SourceCategory = (CellSourceCatory)t.Item3;
            ContainsPoint = t.Item4.Item1;
            ContainsSegment = t.Item4.Item2;
            IsOpen = t.Item4.Item3;
            IsDegnerate = t.Item4.Item4;
            EdgesIndex = t.Item5;
            VerticesIndex = t.Item6;
        }


        /// <summary>
        /// Derive the list of vertices in the cell from the list of segments
        /// </summary>
        //public List<int> GetVertices(ref List<Edge> edges)
        //{
        //    //Assume the segments are returned chained, which they should be.
        //    List<int> vertices = new List<int>();
        //    for (int i = 0; i < EdgesIndex.Count; i++)
        //    {
        //        Edge edge = edges[EdgesIndex[i]];
        //        vertices.Add(edge.Start);
        //        if (vertices.Count == EdgesIndex.Count)
        //            vertices.Add(edge.End);
        //    }
        //    return vertices;
        //}


        public string FormattedCellInputInformation()
        {
            return String.Format("Site: {0}, Source Category: {1}", Site, SourceCategory);
        }

    }
}
