using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoostVoronoi.Exceptions
{
    public class InvalidScaleFactorException : Exception
    {
        public InvalidScaleFactorException()
        {

        }

        public InvalidScaleFactorException(string message)
            : base(message)
        {
        }

        public InvalidScaleFactorException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
