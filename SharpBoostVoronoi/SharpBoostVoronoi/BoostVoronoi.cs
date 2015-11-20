using SharpBoostVoronoi.Input;
using SharpBoostVoronoi.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoostVoronoi
{
    public class BoostVoronoi
    {

        /// <summary>
        /// The input points used to construct the voronoi diagram
        /// </summary>
        public Point InputPoints { get; private set; }

        /// <summary>
        /// The input segments used to construct the voronoi diagram
        /// </summary>
        public Point InputSegments { get; private set; }

        /// <summary>
        /// The output list of vertices
        /// </summary>
        public List<Vertex> Vertices { get; set; }

        /// <summary>
        /// The output list of edges
        /// </summary>
        public List<Edge> Edges { get; set; }

        /// <summary>
        /// The output list of cells
        /// </summary>
        public List<Cell> Cells { get; set; }

    }
}
