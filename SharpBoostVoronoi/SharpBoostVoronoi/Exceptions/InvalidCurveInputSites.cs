using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoostVoronoi.Exceptions
{
    class InvalidCurveInputSites : Exception
    {
        public InvalidCurveInputSites()
        {

        }

        public InvalidCurveInputSites(string message)
            : base(message)
        {
        }

        public InvalidCurveInputSites(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
