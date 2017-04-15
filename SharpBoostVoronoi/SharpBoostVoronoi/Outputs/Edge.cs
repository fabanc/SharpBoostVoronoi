using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoostVoronoi.Output
{
    public class Edge
    {
        /// <summary>
        /// The index of the start vertex of this segment.
        /// </summary>
        public long Start { get; set; }

        /// <summary>
        /// The index of the end vertex of this segment.
        /// </summary>
        public long End { get; set; }

        /// <summary>
        /// The index of the input geometry around which the cell is built.
        /// </summary>
        public long SiteIndex{ get; set; }

        /// <summary>
        /// True is the edge is a primary edge, False otherwise.
        /// </summary>
        public bool IsPrimary { get; set; }

        /// <summary>
        /// True is a segment is a line, false if the segment is an arc.
        /// </summary>
        public bool IsLinear { get; set; }

        /// <summary>
        /// True if the edge is delimited by two known vertices, False otherwise.
        /// </summary>
        public bool IsFinite{ get; set; }

        /// <summary>
        /// The index of the cell associated with this segment
        /// </summary>
        public long Cell { get; set; }

        /// <summary>
        /// The index of the twin cell associated with this segment
        /// </summary>
        public long Twin { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="t">A tuple returned by the CLR wrapper.</param>


        public Edge(Tuple<long, long, long, bool, bool, bool, Tuple<long, long>> t)
        {
            Start = t.Item2;
            End = t.Item3;
            IsPrimary = t.Item4;
            IsLinear = t.Item5;
            IsFinite = t.Item6;
            SiteIndex = -1;

            Twin = t.Item7.Item1;
            Cell = t.Item7.Item2;
        }

       

    }
}
