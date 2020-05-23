using System.Drawing;

namespace Progeto.Geometry
{
    public interface IGraphics
    {
        void Draw(Segment s, Color color, double width);
        void Draw(Circle s, Color color, double width);
        void Draw(Point s, Color color, double width);
    }
}