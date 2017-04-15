using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpBoostVoronoi.Input;
using boost;
using System.Collections.Generic;
using SharpBoostVoronoi;
using SharpBoostVoronoi.Output;
using SharpBoostVoronoi.Maths;

namespace BoostVoronoiTests
{
    [TestClass]
    public class TestCSharp
    {


        //[TestMethod]
        //public void TestVerticesSequence()
        //{
        //    List<Segment> input = new List<Segment>();
        //    input.Add(new Segment(0, 0, 0, 10));
        //    input.Add(new Segment(0, 10, 10, 10));
        //    input.Add(new Segment(10, 10, 10, 0));
        //    input.Add(new Segment(10, 0, 0, 0));
        //    input.Add(new Segment(0, 0, 5, 5));
        //    input.Add(new Segment(5, 5, 10, 10));

        //    //Build the CLR voronoi
        //    VoronoiWrapper vw = new VoronoiWrapper();
        //    foreach (var s in input)
        //        vw.AddSegment(s.Start.X, s.Start.Y, s.End.X, s.End.Y);

        //    vw.ConstructVoronoi();
        //    List<Tuple<long, double, double>> clrVertices = new List<Tuple<long, double, double>>();
        //    for (long i = 0; i < vw.CountVertices(); i++)
        //        clrVertices.Add(vw.GetVertex(i));

        //    //Build the C# Voronoi
        //    BoostVoronoi bv = new BoostVoronoi();
        //    foreach (var s in input)
        //        bv.AddSegment(s.Start.X, s.Start.Y, s.End.X, s.End.Y);
        //    bv.Construct();
        //    List<Vertex> sharpVertices = bv.Vertices;

        //    //Test that the outputs have the same length
        //    Assert.AreEqual(clrVertices.Count, sharpVertices.Count);

        //    for (int i = 0; i < sharpVertices.Count; i++)
        //    {
        //        Assert.AreEqual(clrVertices[i].Item2, sharpVertices[i].X);
        //        Assert.AreEqual(clrVertices[i].Item3, sharpVertices[i].Y);
        //    }
        //}


        //[TestMethod]
        //public void TestEdgesSequence()
        //{
        //    List<Segment> input = new List<Segment>();
        //    input.Add(new Segment(0, 0, 0, 10));
        //    input.Add(new Segment(0, 10, 10, 10));
        //    input.Add(new Segment(10, 10, 10, 0));
        //    input.Add(new Segment(10, 0, 0, 0));
        //    input.Add(new Segment(0, 0, 5, 5));
        //    input.Add(new Segment(5, 5, 10, 10));

        //    //Build the CLR voronoi
        //    VoronoiWrapper vw = new VoronoiWrapper();
        //    foreach (var s in input)
        //        vw.AddSegment(s.Start.X, s.Start.Y, s.End.X, s.End.Y);

        //    vw.ConstructVoronoi();
        //    List<Tuple<int, int, int, int, Tuple<bool, bool, bool, int, int>>> clrEdges = vw.GetEdges();

        //    //Build the C# Voronoi
        //    BoostVoronoi bv = new BoostVoronoi();
        //    foreach (var s in input)
        //        bv.AddSegment(s.Start.X, s.Start.Y, s.End.X, s.End.Y);
        //    bv.Construct();
        //    List<Edge> sharpEdges = bv.Edges;

        //    //Test that the outputs have the same length
        //    Assert.AreEqual(clrEdges.Count, sharpEdges.Count);

        //    for (int i = 0; i < clrEdges.Count; i++)
        //    {
        //        Assert.AreEqual(clrEdges[i].Item2, sharpEdges[i].Start);
        //        Assert.AreEqual(clrEdges[i].Item3, sharpEdges[i].End);
        //    }
        //}


        //[TestMethod]
        //public void TestCellsSequence()
        //{
        //    List<Segment> input = new List<Segment>();
        //    input.Add(new Segment(0, 0, 0, 10));
        //    input.Add(new Segment(0, 10, 10, 10));
        //    input.Add(new Segment(10, 10, 10, 0));
        //    input.Add(new Segment(10, 0, 0, 0));
        //    input.Add(new Segment(0, 0, 5, 5));
        //    input.Add(new Segment(5, 5, 10, 10));

        //    //Build the CLR voronoi
        //    VoronoiWrapper vw = new VoronoiWrapper();
        //    foreach (var s in input)
        //        vw.AddSegment(s.Start.X, s.Start.Y, s.End.X, s.End.Y);

        //    vw.ConstructVoronoi();
        //    List<Tuple<int, int, bool, bool, List<int>, bool, short>> clrCells = vw.GetCells();

        //    //Build the C# Voronoi
        //    BoostVoronoi bv = new BoostVoronoi();
        //    foreach (var s in input)
        //        bv.AddSegment(s.Start.X, s.Start.Y, s.End.X, s.End.Y);
        //    bv.Construct();
        //    List<Cell> sharpCells = bv.Cells;

        //    //Test that the outputs have the same length
        //    Assert.AreEqual(clrCells.Count, sharpCells.Count);

        //    for (int i = 0; i < clrCells.Count; i++)
        //    {
        //        for (int j = 0; j < clrCells[i].Item5.Count; j++)
        //        {
        //            Assert.AreEqual(clrCells[i].Item5[j], sharpCells[i].EdgesIndex[j]);
        //        }
        //    }
        //}


        [TestMethod]
        public void TestSegmentTwin()
        {
            List<Point> inputPoint = new List<Point>() { new Point(5, 5) };
            List<Segment> inputSegment = new List<Segment>();
            inputSegment.Add(new Segment(0, 0, 0, 10));
            inputSegment.Add(new Segment(0, 10, 10, 10));
            inputSegment.Add(new Segment(10, 10, 10, 0));
            inputSegment.Add(new Segment(10, 0, 0, 0));
            


            //Build the CLR voronoi
            VoronoiWrapper vw = new VoronoiWrapper();

            foreach (var p in inputPoint)
                vw.AddPoint(p.X, p.Y);
                
            foreach (var s in inputSegment)
                vw.AddSegment(s.Start.X, s.Start.Y, s.End.X, s.End.Y);

            vw.Construct();
            //List<Tuple<int, int, bool, bool, List<int>, bool, short>> clrCells = vw.GetCells();

            //Build the C# Voronoi
            BoostVoronoi bv = new BoostVoronoi();
            foreach (var p in inputPoint)
                bv.AddPoint(p.X, p.Y);
            foreach (var s in inputSegment)
                bv.AddSegment(s.Start.X, s.Start.Y, s.End.X, s.End.Y);

            bv.Construct();
            

            //Test twin reciprocity
            for (long i = 0; i < bv.CountEdges; i++)
            {
                Edge edge = bv.GetEdge(i);
                Edge twin = bv.GetEdge(edge.Twin);
                Assert.AreEqual(i, twin.Twin);
            }
        }

        //FA: To rewrite with new code
        [TestMethod]
        public void TestSegmentDicretization()
        {
            List<Point> inputPoint = new List<Point>() { new Point(5, 5) };
            List<Segment> inputSegment = new List<Segment>();
            inputSegment.Add(new Segment(0, 0, 0, 10));
            inputSegment.Add(new Segment(0, 0, 10, 0));
            inputSegment.Add(new Segment(0, 10, 10, 10));
            inputSegment.Add(new Segment(10, 0, 10, 10));


            ////Build the CLR voronoi
            //VoronoiWrapper vw = new VoronoiWrapper();

            //foreach (var p in inputPoint)
            //    vw.AddPoint(p.X, p.Y);

            //foreach (var s in inputSegment)
            //    vw.AddSegment(s.Start.X, s.Start.Y, s.End.X, s.End.Y);

            //vw.Construct();
            //List<Tuple<int, int, bool, bool, List<int>, bool, short>> clrCells = vw.GetCells();

            //Build the C# Voronoi
            BoostVoronoi bv = new BoostVoronoi();
            foreach (var p in inputPoint)
                bv.AddPoint(p.X, p.Y);
            foreach (var s in inputSegment)
                bv.AddSegment(s.Start.X, s.Start.Y, s.End.X, s.End.Y);

            bv.Construct();
            //List<Vertex> vertices = bv.Vertices;
            //List<Edge> sharpEdges = bv.Edges;
            //List<Cell> sharpCells = bv.Cells;

            long testEdgeIndex = 2;

            for (long i = 0; i < bv.CountEdges; i++)
            {
                Edge edge = bv.GetEdge(i);
                Edge twin = bv.GetEdge(edge.Twin);

                Cell edgeCell = bv.GetCell(edge.Cell);
                Cell twinCell = bv.GetCell(twin.Cell);

                if (twinCell.SourceCategory == CellSourceCatory.SinglePoint
                    &&
                    edgeCell.Site == 1)
                    testEdgeIndex = i;

            }

            Edge testEdge = bv.GetEdge(testEdgeIndex);
            Vertex startVertex = bv.GetVertex(testEdge.Start);
            Vertex endVertex = bv.GetVertex(testEdge.End);
            List<Vertex> dvertices = bv.SampleCurvedEdge(testEdge, Distance.ComputeDistanceBetweenPoints(startVertex, endVertex) / 2);
            int lastDicretizedVertexIndex = dvertices.Count - 1;

            //Make sure that the end points are consistents
            Assert.AreEqual(dvertices[0].X, startVertex.X);
            Assert.AreEqual(dvertices[0].Y, startVertex.Y);

            Assert.AreEqual(dvertices[lastDicretizedVertexIndex].X, endVertex.X);
            Assert.AreEqual(dvertices[lastDicretizedVertexIndex].Y, endVertex.Y);

            Assert.AreEqual(dvertices[2].X, 2.5);
            Assert.AreEqual(dvertices[2].Y, 5);

        }


        [TestMethod]
        public void TestPrimaryEdges()
        {
            List<Point> inputPoint = new List<Point>() { new Point(5, 5) };
            List<Segment> inputSegment = new List<Segment>();
            inputSegment.Add(new Segment(0, 0, 0, 10));
            inputSegment.Add(new Segment(0, 0, 10, 0));
            inputSegment.Add(new Segment(0, 10, 10, 10));
            inputSegment.Add(new Segment(10, 0, 10, 10));


            ////Build the CLR voronoi
            //VoronoiWrapper vw = new VoronoiWrapper();

            //foreach (var p in inputPoint)
            //    vw.AddPoint(p.X, p.Y);

            //foreach (var s in inputSegment)
            //    vw.AddSegment(s.Start.X, s.Start.Y, s.End.X, s.End.Y);

            //vw.Construct();
            //List<Tuple<int, int, bool, bool, List<int>, bool, short>> clrCells = vw.GetCells();

            //Build the C# Voronoi
            BoostVoronoi bv = new BoostVoronoi();
            foreach (var p in inputPoint)
                bv.AddPoint(p.X, p.Y);
            foreach (var s in inputSegment)
                bv.AddSegment(s.Start.X, s.Start.Y, s.End.X, s.End.Y);

            bv.Construct();
            //List<Vertex> vertices = bv.Vertices;
            //List<Edge> sharpEdges = bv.Edges;

            int countPrimary = 0;
            int countSecondary = 0;
            int countFinite = 0;
            for (long i = 0; i < bv.CountEdges; i++)
            {
                Edge edge = bv.GetEdge(i);
                if (edge.IsPrimary)
                    countPrimary++;

                if (edge.IsFinite)
                    countFinite++;

                if (!edge.IsPrimary && edge.IsFinite)
                    countSecondary++;
            }

            //8 finites from the center of the square corner + 8 edges arount the center point.
            Assert.AreEqual(countFinite, 16);

            //Check the number of secondary edge. Because this input is a square with a point in the center, the expected count is 0.
            Assert.AreEqual(countSecondary, 0);

            Assert.AreEqual(countPrimary, countFinite - countSecondary);
        }

        [TestMethod]
        public void TestGetCellVertices()
        {
            List<Point> inputPoint = new List<Point>() { new Point(5, 5) };
            List<Segment> inputSegment = new List<Segment>();
            inputSegment.Add(new Segment(0, 0, 0, 10));
            inputSegment.Add(new Segment(0, 0, 10, 0));
            inputSegment.Add(new Segment(0, 10, 10, 10));
            inputSegment.Add(new Segment(10, 0, 10, 10));

            //Build the C# Voronoi
            BoostVoronoi bv = new BoostVoronoi();
            foreach (var p in inputPoint)
                bv.AddPoint(p.X, p.Y);
            foreach (var s in inputSegment)
                bv.AddSegment(s.Start.X, s.Start.Y, s.End.X, s.End.Y);

            bv.Construct();
            //List<Vertex> vertices = bv.Vertices;
            //List<Edge> sharpEdges = bv.Edges;
            //List<Cell> cells = bv.Cells;

            for (long i = 0; i < bv.CountCells; i++)
            {
                Cell cell = bv.GetCell(i);
                if (!cell.IsOpen)
                {
                    List<long> vertexIndexes = cell.VerticesIndex;
                    Assert.AreEqual(vertexIndexes.Count, 5);
                    Assert.AreEqual(vertexIndexes[0], vertexIndexes[vertexIndexes.Count - 1]);
                }
                

            }
        }


        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestFindInputPointSiteException()
        {
            List<Point> inputPoint = new List<Point>() { new Point(5, 5) };
            List<Segment> inputSegment = new List<Segment>();
            inputSegment.Add(new Segment(0, 0, 0, 10));
            inputSegment.Add(new Segment(0, 0, 10, 0));
            inputSegment.Add(new Segment(0, 10, 10, 10));
            inputSegment.Add(new Segment(10, 0, 10, 10));

            //Build the C# Voronoi
            BoostVoronoi bv = new BoostVoronoi();
            foreach (var p in inputPoint)
                bv.AddPoint(p.X, p.Y);
            foreach (var s in inputSegment)
                bv.AddSegment(s.Start.X, s.Start.Y, s.End.X, s.End.Y);

            bv.Construct();
            //List<Vertex> vertices = bv.Vertices;
            //List<Edge> sharpEdges = bv.Edges;
            //List<Cell> cells = bv.Cells;

            for (long i = 0; i < bv.CountCells; i++)
            {
                Cell cell = bv.GetCell(i);
                if (cell.SourceCategory != CellSourceCatory.SegmentStartPoint
                    && cell.SourceCategory != CellSourceCatory.SegmentEndPoint
                    && cell.SourceCategory != CellSourceCatory.SinglePoint)
                {
                    bv.RetrieveInputPoint(cell);
                }   
            }
        }


        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestFindInputSegmentSiteException()
        {
            List<Point> inputPoint = new List<Point>() { new Point(5, 5) };
            List<Segment> inputSegment = new List<Segment>();
            inputSegment.Add(new Segment(0, 0, 0, 10));
            inputSegment.Add(new Segment(0, 0, 10, 0));
            inputSegment.Add(new Segment(0, 10, 10, 10));
            inputSegment.Add(new Segment(10, 0, 10, 10));

            //Build the C# Voronoi
            BoostVoronoi bv = new BoostVoronoi();
            foreach (var p in inputPoint)
                bv.AddPoint(p.X, p.Y);
            foreach (var s in inputSegment)
                bv.AddSegment(s.Start.X, s.Start.Y, s.End.X, s.End.Y);

            bv.Construct();
            //List<Vertex> vertices = bv.Vertices;
            //List<Edge> sharpEdges = bv.Edges;
            //List<Cell> cells = bv.Cells;

            for (long i = 0; i < bv.CountCells; i++)
            {
                Cell cell = bv.GetCell(i);
                if (cell.SourceCategory != CellSourceCatory.InitialSegment
                    && cell.SourceCategory != CellSourceCatory.ReverseSegment)
                {
                    bv.RetrieveInputSegment(cell);
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void TestNegativeVertexIndexException()
        {
            List<Point> inputPoint = new List<Point>() { new Point(5, 5) };
            List<Segment> inputSegment = new List<Segment>();
            inputSegment.Add(new Segment(0, 0, 0, 10));
            inputSegment.Add(new Segment(0, 0, 10, 0));
            inputSegment.Add(new Segment(0, 10, 10, 10));
            inputSegment.Add(new Segment(10, 0, 10, 10));

            //Build the C# Voronoi
            BoostVoronoi bv = new BoostVoronoi();
            foreach (var p in inputPoint)
                bv.AddPoint(p.X, p.Y);
            foreach (var s in inputSegment)
                bv.AddSegment(s.Start.X, s.Start.Y, s.End.X, s.End.Y);

            bv.Construct();
            bv.GetVertex(-1);
        }


        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void TestNegativeEdgeIndexException()
        {
            List<Point> inputPoint = new List<Point>() { new Point(5, 5) };
            List<Segment> inputSegment = new List<Segment>();
            inputSegment.Add(new Segment(0, 0, 0, 10));
            inputSegment.Add(new Segment(0, 0, 10, 0));
            inputSegment.Add(new Segment(0, 10, 10, 10));
            inputSegment.Add(new Segment(10, 0, 10, 10));

            //Build the C# Voronoi
            BoostVoronoi bv = new BoostVoronoi();
            foreach (var p in inputPoint)
                bv.AddPoint(p.X, p.Y);
            foreach (var s in inputSegment)
                bv.AddSegment(s.Start.X, s.Start.Y, s.End.X, s.End.Y);

            bv.Construct();
            bv.GetEdge(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void TestNegativeCellIndexException()
        {
            List<Point> inputPoint = new List<Point>() { new Point(5, 5) };
            List<Segment> inputSegment = new List<Segment>();
            inputSegment.Add(new Segment(0, 0, 0, 10));
            inputSegment.Add(new Segment(0, 0, 10, 0));
            inputSegment.Add(new Segment(0, 10, 10, 10));
            inputSegment.Add(new Segment(10, 0, 10, 10));

            //Build the C# Voronoi
            BoostVoronoi bv = new BoostVoronoi();
            foreach (var p in inputPoint)
                bv.AddPoint(p.X, p.Y);
            foreach (var s in inputSegment)
                bv.AddSegment(s.Start.X, s.Start.Y, s.End.X, s.End.Y);

            bv.Construct();
            bv.GetCell(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void TestInvalidVertexIndexException()
        {
            List<Point> inputPoint = new List<Point>() { new Point(5, 5) };
            List<Segment> inputSegment = new List<Segment>();
            inputSegment.Add(new Segment(0, 0, 0, 10));
            inputSegment.Add(new Segment(0, 0, 10, 0));
            inputSegment.Add(new Segment(0, 10, 10, 10));
            inputSegment.Add(new Segment(10, 0, 10, 10));

            //Build the C# Voronoi
            BoostVoronoi bv = new BoostVoronoi();
            foreach (var p in inputPoint)
                bv.AddPoint(p.X, p.Y);
            foreach (var s in inputSegment)
                bv.AddSegment(s.Start.X, s.Start.Y, s.End.X, s.End.Y);

            bv.Construct();
            bv.GetVertex(bv.CountVertices);
        }


        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void TestInvalidEdgeIndexException()
        {
            List<Point> inputPoint = new List<Point>() { new Point(5, 5) };
            List<Segment> inputSegment = new List<Segment>();
            inputSegment.Add(new Segment(0, 0, 0, 10));
            inputSegment.Add(new Segment(0, 0, 10, 0));
            inputSegment.Add(new Segment(0, 10, 10, 10));
            inputSegment.Add(new Segment(10, 0, 10, 10));

            //Build the C# Voronoi
            BoostVoronoi bv = new BoostVoronoi();
            foreach (var p in inputPoint)
                bv.AddPoint(p.X, p.Y);
            foreach (var s in inputSegment)
                bv.AddSegment(s.Start.X, s.Start.Y, s.End.X, s.End.Y);

            bv.Construct();
            bv.GetEdge(bv.CountEdges);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void TestInvalidCellIndexException()
        {
            List<Point> inputPoint = new List<Point>() { new Point(5, 5) };
            List<Segment> inputSegment = new List<Segment>();
            inputSegment.Add(new Segment(0, 0, 0, 10));
            inputSegment.Add(new Segment(0, 0, 10, 0));
            inputSegment.Add(new Segment(0, 10, 10, 10));
            inputSegment.Add(new Segment(10, 0, 10, 10));

            //Build the C# Voronoi
            BoostVoronoi bv = new BoostVoronoi();
            foreach (var p in inputPoint)
                bv.AddPoint(p.X, p.Y);
            foreach (var s in inputSegment)
                bv.AddSegment(s.Start.X, s.Start.Y, s.End.X, s.End.Y);

            bv.Construct();
            bv.GetCell(bv.CountCells);
        }

    }
}
