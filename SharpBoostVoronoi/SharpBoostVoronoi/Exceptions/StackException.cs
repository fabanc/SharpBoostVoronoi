using SharpBoostVoronoi.Parabolas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoostVoronoi.Exceptions
{
    public class StackException: Exception, IParabolaException
    {
        public ParabolaProblemInformation InputParabolaProblemInfo { get; set; }

        public StackException(ParabolaProblemInformation nonRotatedInformation)
        {
            InputParabolaProblemInfo = nonRotatedInformation;
        }
    }
}
