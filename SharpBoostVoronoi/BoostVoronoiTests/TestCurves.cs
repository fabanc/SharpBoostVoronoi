using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpBoostVoronoi.Parabolas;
using SharpBoostVoronoi.Maths;
using SharpBoostVoronoi.Input;
using SharpBoostVoronoi.Output;
using System.Collections.Generic;

namespace BoostVoronoiTests
{
    [TestClass]
    public class TestCurves
    {
        [TestMethod]
        public void TestRotationSampling()
        {
            Vertex focus = new Vertex(-8497612.8036, 5686669.417599998);
            Vertex dir_start = new Vertex(-8497346.9281, 5686392.4868);
            Vertex dir_end = new Vertex(-8497375.7487, 5686444.038099997);
            Vertex par_start = new Vertex(-8497912.816628259, 5686117.636838556);
            Vertex par_end = new Vertex(-8497875.094202437, 5686156.284323769);
            double max_dist = 5.4005643438;
            List<Vertex> vertices = ParabolaComputation.Densify(focus, dir_start, dir_end, par_start, par_end, max_dist);
            string test = "done";
        }
    }
}
