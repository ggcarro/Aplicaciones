using Microsoft.VisualStudio.TestTools.UnitTesting;
using Progeto.Geometry;

namespace Tests
{
    [TestClass]
    public class CircleTests
    {

        static Point p1 = new Point(2, 2);
        static double rad = 1;

        Circle circ = new Circle(p1, rad); //vector points


        [TestMethod]
        public void CircleRadius()
        {
            Assert.AreEqual<double>(1, circ.Radius);
        }

        [TestMethod]
        public void CircleCenter()
        {
            Assert.AreEqual<Point>(p1, circ.Center);
        }

        [TestMethod]
        public void CirclePerimeterTest1()
        {
            double result = 2 * System.Math.PI;
            Assert.AreEqual<double>(result, circ.Perimeter);
        }

        [TestMethod]
        public void CirclePerimeterTest2()  //Creamos un círculo con radio 0:
                                            //permite crearlo; perímetro = 0;
        {
            double rad = 0;
            Point p2 = new Point(1, -4);
            Circle c2 = new Circle(p2, rad);
            double result = 2 * System.Math.PI * rad;
            Assert.AreEqual<double>(result, c2.Perimeter);
        }


        [TestMethod]
        public void CirclePerimeterTest3()  //Creamos un círculo con radio negativo:
                                            //permite crearlo; perímetro negativo;
        {
            double rad = -3;
            Point p2 = new Point(1, -4);
            Circle c3 = new Circle(p2, rad);
            double result = 2 * System.Math.PI * rad;
            Assert.AreEqual<double>(result, c3.Perimeter);
        }

    }
}

