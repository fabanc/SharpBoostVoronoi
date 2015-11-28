using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpBoostVoronoi.Input;
using boost;
using SharpBoostVoronoi;
using SharpBoostVoronoi.Output;
using System.Collections.Generic;

namespace BoostVoronoiTests
{
    [TestClass]
    public class TestWrapping
    {
        [TestMethod]
        public void TestWrapperCells()
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

            for (int i = 0; i < clrCells.Count; i++)
            {
                for (int j = 0; j < clrCells[i].Item5.Count; j++)
                {
                    Assert.AreEqual(clrCells[i].Item5[j], sharpCells[i].EdgesIndex[j]);
                }
            }
        }
    }
}
