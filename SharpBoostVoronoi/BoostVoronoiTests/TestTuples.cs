using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using boost;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BoostVoronoiTests
{
    [TestClass]
    public class TestTuples
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void TestTupleVertices()
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

        [TestMethod]
        public void TestTupleEdges()
        {
            VoronoiWrapper vw = new VoronoiWrapper();
            vw.AddSegment(0, 0, 0, 10);
            vw.AddSegment(0, 10, 10, 10);
            vw.AddSegment(10, 10, 10, 0);
            vw.AddSegment(10, 0, 0, 0);
            vw.AddSegment(0, 0, 5, 5);
            vw.AddSegment(5, 5, 10, 10);
            vw.ConstructVoronoi();

            List<Tuple<int, int, int, int, Tuple<bool, bool, bool, int, int>>> edges = vw.GetEdges();
            int count_finite_edge = 0;
            foreach (var e in edges)
            {
                if (e.Item5.Item3)
                {
                    count_finite_edge++;
                }
            }

            Assert.AreEqual(16, count_finite_edge);
        }

        [TestMethod]
        public void TestTupleEdgeIndexes()
        {
            VoronoiWrapper vw = new VoronoiWrapper();
            vw.AddSegment(0, 0, 0, 10);
            vw.AddSegment(0, 10, 10, 10);
            vw.AddSegment(10, 10, 10, 0);
            vw.AddSegment(10, 0, 0, 0);
            vw.AddSegment(0, 0, 5, 5);
            vw.AddSegment(5, 5, 10, 10);
            vw.ConstructVoronoi();

            List<Tuple<int, int, int, int, Tuple<bool, bool, bool, int, int>>> edges = vw.GetEdges();

            List<int> edgeIndexes = new List<int>();

            foreach (var e in edges)
            {
                edgeIndexes.Add(e.Item1);
            }

            edgeIndexes.Sort();
            int minIndex = edgeIndexes.Min();
            int maxIndex = edgeIndexes.Max();

            Assert.AreEqual(0, minIndex);
            Assert.AreEqual(edges.Count - 1, maxIndex);
        }



        [TestMethod]
        public void TestTupleVertexIndexes()
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
            List<Tuple<int, int, int, int, Tuple<bool, bool, bool, int, int>>> edges = vw.GetEdges();

            List<int> vertexIndexes = new List<int>();

            foreach (var e in edges)
            {
                if(!vertexIndexes.Exists(v=> v==e.Item2))
                    vertexIndexes.Add(e.Item2);

                if (!vertexIndexes.Exists(v => v == e.Item3))
                    vertexIndexes.Add(e.Item3);
            }

            vertexIndexes.Remove(-1);
            vertexIndexes.Sort();
            int minIndex = vertexIndexes.Min();
            int maxIndex = vertexIndexes.Max();

            Assert.AreEqual(0, minIndex);
            Assert.AreEqual(vertices.Count - 1, maxIndex);
        }


        [TestMethod]
        public void TestTupleCells()
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
            List<Tuple<int, int, int, int, Tuple<bool, bool, bool, int, int>>> edges = vw.GetEdges();
            List<Tuple<int, int, bool, bool, List<int>, bool, short>> cells = vw.GetCells();


            for (int i = 0; i < vertices.Count; i++)
			{
                TestContext.WriteLine(String.Format("Vertex {0}. X: {1}, Y: {2}", i, vertices[i].Item1, vertices[i].Item2));
			}
                
            foreach (var c in cells)
            {
                foreach (var s in c.Item5)
                {
                        TestContext.WriteLine(String.Format("Cell: {0}, Segment: {1}, Start: {2}, End: {3}", c.Item1, s, edges[s].Item2, edges[s].Item3));
                }
            }
            Assert.AreEqual(11, cells.Count);
        }

        [TestMethod]
        public void TestVertexSequences()
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
            List<Tuple<int, int, int, int, Tuple<bool, bool, bool, int, int>>> edges = vw.GetEdges();
            List<Tuple<int, int, bool, bool, List<int>, bool, short>> cells = vw.GetCells();


            foreach (var c in cells)
            {
                for (int i = 1; i < c.Item5.Count; i++)
                {
                    Assert.AreEqual(edges[c.Item5[i - 1]].Item3, edges[c.Item5[i]].Item2);
                }
            }
        }


        [TestMethod]
        public void TestCellEndNodes()
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
            List<Tuple<int, int, int, int, Tuple<bool, bool, bool, int, int>>> edges = vw.GetEdges();
            List<Tuple<int, int, bool, bool, List<int>, bool, short>> cells = vw.GetCells();


            foreach (var c in cells)
            {
                //Get first edge and last edge
                Tuple<int, int, int, int, Tuple<bool, bool, bool, int, int>> firstEdge = edges[c.Item5[0]];
                Tuple<int, int, int, int, Tuple<bool, bool, bool, int, int>> lastEdge = edges[c.Item5[c.Item5.Count -1]];

                Assert.AreEqual(firstEdge.Item2, lastEdge.Item3);
            }
        }


    }
}
