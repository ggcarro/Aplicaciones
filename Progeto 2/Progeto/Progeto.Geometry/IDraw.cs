using System.Drawing;

namespace Progeto.Geometry
{
    public interface IDraw
    {
        void Draw(IGraphics g, Color color, double width);
    }
}