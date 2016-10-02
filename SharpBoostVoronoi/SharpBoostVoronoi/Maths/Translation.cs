using SharpBoostVoronoi.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoostVoronoi.Maths
{
    public class Translation
    {
        public double XVector { get; private set; }
        public double YVector { get; private set; }

        public Translation(double x, double y)
        {
            XVector = x;
            YVector = y;
        }

        public Vertex Translate(Vertex v)
        {
            return new Vertex(v.X + XVector, v.Y + YVector);
        }

        public Vertex UndoTranslate(Vertex v)
        {
            return new Vertex(v.X - XVector, v.Y - YVector);
        }
    }
}
