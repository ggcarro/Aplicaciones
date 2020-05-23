using Progeto.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Progeto.Lua
{
    class DrawingPrimitive
    {
        IPrimitive _primitive;
        Color _color;
        double _width;

        public IPrimitive Primitive
        {
            get { return _primitive; }
        }
        public Color Color
        {
            get { return _color; }
        }

        public double Width
        {
            get { return _width; }
        }

        public DrawingPrimitive(IPrimitive primitive, Color color, double width)
        {
            _primitive = primitive;
            _color = color;
            _width = width;
        }

        public void Draw(IGraphics g)
        {
            _primitive.Draw(g, _color, _width);
        }
    }
}