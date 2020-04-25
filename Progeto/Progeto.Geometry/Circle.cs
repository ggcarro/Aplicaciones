using System.Drawing;

namespace Progeto.Geometry
{
    public class Circle : IPrimitive
    {
        private readonly Point _center;
        private readonly double _radius;

        public double Radius
        {
            get { return _radius; }
        }

        public Point Center
        {
            get { return _center; }
        }

        public Circle(Point center, double radius)
        {
            _center = center;
            _radius = radius;
        }

        public double Perimeter
        {
            get { return 2 * System.Math.PI * _radius; }
        }

        public void Draw(IGraphics g, Color color, double width)
        {
            g.Draw(this, color, width);
        }
    }
}