using Microsoft.VisualStudio.TestTools.UnitTesting;
using Progeto.Geometry;
using Progeto.Lua;


namespace Tests
{
    [TestClass]
    public class SegmentTests
    {

        static Point p1 = new Point(1, 1);
        static Point p2 = new Point(3, 3);
        static Point p3 = new Point(-1, -7);
        

        [TestMethod]
        public void SegmentInitialPTest1()
        {
            Segment s1 = new Segment(p1, p2);
            Segment s2 = new Segment(p1, p3);
            Assert.AreEqual<Point>(s1.InitialPoint, s2.InitialPoint);
        }


        [TestMethod]
        public void SegmentInitialPTest2()
        {
            Segment s1 = new Segment(p3, p2);
            Segment s2 = new Segment(p3, p3);
            Assert.AreEqual<Point>(s1.InitialPoint, s2.InitialPoint);
        }


        [TestMethod]
        public void SegmentFinalPoint()
        {
            Segment s1 = new Segment(p1, p2);
            Segment s2 = new Segment(p3, p2);
            Assert.AreEqual<Point>(s1.FinalPoint, s2.FinalPoint);
        }
        

        [TestMethod]
        public void SegmentLength1()
        {
            Segment s1 = new Segment(p1, p2);
            double result = System.Math.Sqrt(8);

            Assert.AreEqual<double>(result, s1.Length);
        }


        [TestMethod]
        public void SegmentLength2()
        {
            Segment s1 = new Segment(p1, p3);
            double result = System.Math.Sqrt(68);

            Assert.AreEqual<double>(result, s1.Length);
        }
    }
}
