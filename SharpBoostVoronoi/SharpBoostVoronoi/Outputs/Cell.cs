using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoostVoronoi.Output
{
    public enum CellSourceCatory { Unknown = -1, SinglePoint = 0, SegmentStartPoint = 1, SegmentEndPoint = 2, InitialSegment = 3, ReverseSegment = 4, GeometryShift = 5, BitMask = 6};

    public class Cell
    {
        /// <summary>
        /// The voronoi cell identifier
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// The index of the source feature
        /// </summary>
        public int Site { get; set; }

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
        public List<int> EdgesIndex { get; set; }

        /// <summary>
        /// True if the cell has a segment that is infinite
        /// </summary>
        public bool IsOpen { get; set; }

        /// <summary>
        /// The type of element
        /// </summary>
        public CellSourceCatory SourceCategory { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="t">A tuple returned by the CLR wrapper.</param>
        public Cell(Tuple <int, int, bool, bool, List<int>, bool, int> t)
        {
            Index = t.Item1;
            Site = t.Item2;
            ContainsPoint = t.Item3;
            ContainsSegment = t.Item4;
            EdgesIndex = t.Item5;
            IsOpen = t.Item6;
            SourceCategory = (CellSourceCatory)t.Item7;

        }
    }
}
