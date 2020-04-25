using System.Drawing;

namespace Progeto.Geometry
{
    public class Point : IPrimitive
    {
        private double _x;
        private double _y;

        public double x
        {
            get { return _x; }
            set { _x = value; }
        }

        public double y
        {
            get { return _y; }
            set { _y = value; }
        }

        public Point(double x, double y)
        {
            _x = x;
            _y = y;
        }

        public static double SquaredDistance(Point p1, Point p2)
        {
            double dx = p1._x - p2._x;
            double dy = p1._y - p2._y;

            return dx * dx + dy * dy;
        }

        public static double Distance(Point p1, Point p2)
        {
            return System.Math.Sqrt(SquaredDistance(p1, p2));
        }

        public double Distance(Point p)
        {
            return System.Math.Sqrt(SquaredDistance(this, p));
        }

        public static Vector operator -(Point p1, Point p2)
        {
            return new Vector(p1._x - p2._x, p1._y - p2._y);
        }

        public static Point operator +(Point p, Vector v)
        {
            return new Point(p._x + v.x, p._y + v.y);
        }

        public static Point operator -(Point p, Vector v)
        {
            return new Point(p._x - v.x, p._y - v.y);
        }

        public void Draw(IGraphics g, Color color, double width)
        {
            g.Draw(this, color, width);
        }
    }
}