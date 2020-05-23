namespace Progeto.Geometry
{
    public class Vector
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

        public Vector(double x, double y)
        {
            _x = x;
            _y = y;
        }

        public Vector(Point p1, Point p2)
        {
            _x = p1.x - p2.x;
            _y = p1.y - p2.y;
        }

        public double Magnitude
        {
            get
            {
                return System.Math.Sqrt(_x * _x + _y * _y);
            }
        }

        public Vector Perpendicular
        {
            get
            {
                return new Vector(_y, -_x);
            }
        }

        public void Normalize()
        {
            double norm = this.Magnitude;

            _x /= norm;
            _y /= norm;
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            return new Vector(v1._x + v2._x, v1._y + v2._y);
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            return new Vector(v1._x - v2._x, v1._y - v2._y);
        }

        public static double DotProduct(Vector v1, Vector v2)
        {
            return v1._x * v2._x + v1._y * v2._y;
        }

        public static double CrossProduct(Vector v1, Vector v2)
        {
            return v1.x * v2.y - v1.y * v2.x;
        }

        public static Vector operator +(Vector v, double value)
        {
            return new Vector(v._x + value, v._y + value);
        }

        public static Vector operator -(Vector v, double value)
        {
            return new Vector(v._x - value, v._y - value);
        }

        public static Vector operator *(Vector v, double value)
        {
            return new Vector(v._x * value, v._y * value);
        }
    }
}