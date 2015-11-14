using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using boost;
using System.Collections.Generic;

namespace BoostVoronoiTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            VoronoiWrapper vw = new VoronoiWrapper();
            vw.AddSegment(0, 0, 0, 10);
            vw.AddSegment(0, 10, 10, 10);
            vw.AddSegment(10, 10, 10, 0);
            vw.AddSegment(10, 0, 0, 0);
            vw.AddSegment(0, 0, 5, 5);
            vw.AddSegment(5, 5, 10, 10);
            vw.ConstructVoronoi();

            List<Tuple<int, int, int, bool, bool, bool>> edges = vw.GetEdges();
            int count_finite_edge = 0;
            foreach (var e in edges)
            {
                if (e.Item6)
                {
                    count_finite_edge++;
                }
            }
           
            Assert.AreEqual(16, count_finite_edge);
        }

        [TestMethod]
        public void TestMethod2()
        {
            VoronoiWrapper vw = new VoronoiWrapper();
            vw.AddSegment(0, 0, 0, 10);
            vw.AddSegment(0, 10, 10, 10);
            vw.AddSegment(10, 10, 10, 0);
            vw.AddSegment(10, 0, 0, 0);
            vw.AddSegment(0, 0, 5, 5);
            vw.AddSegment(5, 5, 10, 10);
            vw.ConstructVoronoi();

            List<Tuple<double, double>> vertices = vw.GetVertices();
            Assert.AreEqual(6, vertices.Count);
        }

    }
}
