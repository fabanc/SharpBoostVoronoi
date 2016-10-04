using SharpBoostVoronoi.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoostVoronoi.Parabolas
{
    public class ParabolaProblemInformation
    {
        public Vertex FocusPoint { get; set; }
        public Vertex DirectixSegmentStart { get; set; }
        public Vertex DirectixSegmentEnd { get; set; }
        public Vertex ParabolaStart { get; set; }
        public Vertex ParabolaEnd { get; set; }

        public ParabolaProblemInformation(Vertex focusPoint, Vertex directixSegmentStart, Vertex directixSegmentEnd, Vertex parabolaStart, Vertex parabolaEnd)
        {
            FocusPoint = focusPoint;
            DirectixSegmentStart = directixSegmentStart;
            DirectixSegmentEnd = directixSegmentEnd;
            ParabolaStart = parabolaStart;
            ParabolaEnd = parabolaEnd;
        }

        public List<Tuple<Vertex, string>> GetAsVertexList()
        {
            return new List<Tuple<Vertex, string>>(){
                Tuple.Create<Vertex, string>(FocusPoint, "Focus"),
                Tuple.Create<Vertex, string>(DirectixSegmentStart, "DirectixStart"),
                Tuple.Create<Vertex, string>(DirectixSegmentEnd, "DirectixEnd"),
                Tuple.Create<Vertex, string>(ParabolaStart, "ParabolaStart"),
                Tuple.Create<Vertex, string>(ParabolaEnd, "ParabolaEnd")
            };
        }
    }
}
