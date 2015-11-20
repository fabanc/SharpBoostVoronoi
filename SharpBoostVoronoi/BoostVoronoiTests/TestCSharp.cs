using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpBoostVoronoi.Input;
using boost;
using System.Collections.Generic;
using SharpBoostVoronoi;
using SharpBoostVoronoi.Output;

namespace BoostVoronoiTests
{
    [TestClass]
    public class TestCSharp
    {
        [TestMethod]
        public void TestVerticesCount()
        {
            List<Segment> input = new List<Segment>();
            input.Add(new Segment(0, 0, 0, 10));
            input.Add(new Segment(0, 10, 10, 10));
            input.Add(new Segment(10, 10, 10, 0));
            input.Add(new Segment(10, 0, 0, 0));
            input.Add(new Segment(0, 0, 5, 5));
            input.Add(new Segment(5, 5, 10, 10));

            //Build the CLR voronoi
            VoronoiWrapper vw = new VoronoiWrapper();
            foreach (var s in input)
                vw.AddSegment(s.Start.X, s.Start.Y, s.End.X, s.End.Y);   
            
            vw.ConstructVoronoi();
            List<Tuple<double, double>> clrVertices = vw.GetVertices();
            
            //Build the C# Voronoi
            BoostVoronoi bv = new BoostVoronoi();
            foreach (var s in input)
                bv.AddSegment(s.Start.X, s.Start.Y, s.End.X, s.End.Y);
            bv.Construct();
            List<Vertex> sharpVertices = bv.Vertices;

            //Test that the outputs have the same length
            Assert.AreEqual(clrVertices.Count, sharpVertices.Count);
        }

        [TestMethod]
        public void TestEdgesCount()
        {
            List<Segment> input = new List<Segment>();
            input.Add(new Segment(0, 0, 0, 10));
            input.Add(new Segment(0, 10, 10, 10));
            input.Add(new Segment(10, 10, 10, 0));
            input.Add(new Segment(10, 0, 0, 0));
            input.Add(new Segment(0, 0, 5, 5));
            input.Add(new Segment(5, 5, 10, 10));

            //Build the CLR voronoi
            VoronoiWrapper vw = new VoronoiWrapper();
            foreach (var s in input)
                vw.AddSegment(s.Start.X, s.Start.Y, s.End.X, s.End.Y);

            vw.ConstructVoronoi();
            List<Tuple<int,int,int,int,bool,bool,bool>> clrEdges = vw.GetEdges();

            //Build the C# Voronoi
            BoostVoronoi bv = new BoostVoronoi();
            foreach (var s in input)
                bv.AddSegment(s.Start.X, s.Start.Y, s.End.X, s.End.Y);
            bv.Construct();
            List<Edge> sharpEdges = bv.Edges;

            //Test that the outputs have the same length
            Assert.AreEqual(clrEdges.Count, sharpEdges.Count);
        }

        [TestMethod]
        public void TestCellsCount()
        {
            List<Segment> input = new List<Segment>();
            input.Add(new Segment(0, 0, 0, 10));
            input.Add(new Segment(0, 10, 10, 10));
            input.Add(new Segment(10, 10, 10, 0));
            input.Add(new Segment(10, 0, 0, 0));
            input.Add(new Segment(0, 0, 5, 5));
            input.Add(new Segment(5, 5, 10, 10));

            //Build the CLR voronoi
            VoronoiWrapper vw = new VoronoiWrapper();
            foreach (var s in input)
                vw.AddSegment(s.Start.X, s.Start.Y, s.End.X, s.End.Y);

            vw.ConstructVoronoi();
            List<Tuple<int, int, bool, bool, List<int>>> clrCells = vw.GetCells();

            //Build the C# Voronoi
            BoostVoronoi bv = new BoostVoronoi();
            foreach (var s in input)
                bv.AddSegment(s.Start.X, s.Start.Y, s.End.X, s.End.Y);
            bv.Construct();
            List<Cell> sharpCells = bv.Cells;

            //Test that the outputs have the same length
            Assert.AreEqual(clrCells.Count, sharpCells.Count);
        }
    }
}
