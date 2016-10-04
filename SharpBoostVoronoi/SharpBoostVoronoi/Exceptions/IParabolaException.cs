using SharpBoostVoronoi.Parabolas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoostVoronoi.Exceptions
{
    public interface IParabolaException
    {
         ParabolaProblemInformation InputParabolaProblemInfo { get; set; }
    }
}
