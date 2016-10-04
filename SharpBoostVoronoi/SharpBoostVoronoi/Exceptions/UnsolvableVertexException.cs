using SharpBoostVoronoi.Output;
using SharpBoostVoronoi.Parabolas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoostVoronoi.Exceptions
{
    public class UnsolvableVertexException:Exception, IParabolaException
    {
        public Vertex BoostVertex { get; set; }
        public Vertex ComputedVertex { get; set; }
        public double DistanceBoostVertexToFocus { get; set; }
        public double DistanceComputedVertexToFocus { get; set; }
        public double DistanceBoostVertexToDirectix { get; set; }
        public double DistanceComputedVertexToDirectix { get; set; }

        public ParabolaProblemInformation InputParabolaProblemInfo { get; set; }
        public ParabolaProblemInformation RoatatedParabolaProblemInfo { get; set; }

        public UnsolvableVertexException(ParabolaProblemInformation nonRotatedInformation, ParabolaProblemInformation rotatedInformation, Vertex boostVertex, Vertex computedVertex,
            double distanceBoostVertexToFocus, double distanceComputedVertexToFocus, double distanceBoostVertexToDirectix, double distanceComputedVertexToDirectix)
        {
            InputParabolaProblemInfo = nonRotatedInformation;
            RoatatedParabolaProblemInfo = rotatedInformation;

            BoostVertex = boostVertex;
            ComputedVertex = computedVertex;

            DistanceBoostVertexToFocus = distanceBoostVertexToFocus;
            DistanceComputedVertexToFocus = distanceComputedVertexToFocus;
            DistanceBoostVertexToDirectix = distanceBoostVertexToDirectix;
            DistanceComputedVertexToDirectix = distanceComputedVertexToDirectix;
        }

        public UnsolvableVertexException(string message)
            : base(message)
        {
        }

        public UnsolvableVertexException(string message, Exception inner)
            : base(message, inner)
        {
        }

    }
}
