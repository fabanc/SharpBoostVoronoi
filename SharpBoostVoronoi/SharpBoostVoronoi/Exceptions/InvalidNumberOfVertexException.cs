using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoostVoronoi.Exceptions
{
    class InvalidNumberOfVertexException : Exception
    {
        public InvalidNumberOfVertexException()
        {

        }

        public InvalidNumberOfVertexException(string message)
            : base(message)
        {
        }

        public InvalidNumberOfVertexException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
