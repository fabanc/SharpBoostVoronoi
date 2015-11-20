using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoostVoronoi.Output
{
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
        public List<long> Segments { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="t">A tuple returned by the CLR wrapper.</param>
        public Cell(Tuple <long, long, bool, bool, List<long>> t)
        {
            Index = t.Item1;
            Site = t.Item2;
            ContainsPoint = t.Item3;
            ContainsSegment = t.Item4;
            Segments = t.Item5;
        }
    }
}
