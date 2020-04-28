using Microsoft.VisualStudio.TestTools.UnitTesting;
using Progeto.Geometry;

namespace Tests
{
    [TestClass]
    public class VectorTests
    {

        static Point p1 = new Point(3, 6);
        static Point p2 = new Point(5, 12);

        Vector vector1 = new Vector(0, 1);  //vector double
        Vector vector2 = new Vector(p1, p2); //vector points


        [TestMethod]
        public void VectorMagnitudeTest1()
        {
            Assert.AreEqual<double>(1, vector1.Magnitude);
        }

        [TestMethod]
        public void VectorMagnitudeTest2()
        {
            double result = System.Math.Sqrt(4 + 36);
            Assert.AreEqual<double>(result, vector2.Magnitude);

        }

        [TestMethod]
        public void VectorMagnitudeTest3()
        {
            Vector vectorN = new Vector(-1, 0);
            double result = 1;
            Assert.AreEqual<double>(result, vectorN.Magnitude);

        }


        [TestMethod]
        public void VectorPerpendicular()
        {
            Vector vectorP = new Vector(1, 0);
            vector1 = vector1.Perpendicular;
            Assert.AreEqual<double>(vectorP.x, vector1.x);

        }

        [TestMethod]
        public void VectorDotProduct()
        {
            Vector _vector1 = new Vector(1, 3);
            Vector _vector2 = new Vector(2, 4);
            double product = Vector.DotProduct(_vector1, _vector2);
            double result = 2 + 12;
            Assert.AreEqual<double>(result, product);

        }

        [TestMethod]
        public void VectorCrossProduct()
        {
            Vector _vector1 = new Vector(1, 3);
            Vector _vector2 = new Vector(2, 4);
            double product = Vector.CrossProduct(_vector1, _vector2);
            double result = 4 - 6;
            Assert.AreEqual<double>(result, product);
        }

    }
}

