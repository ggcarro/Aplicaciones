using Progeto.Geometry;
using System;
using System.Drawing;
using System.Globalization;
using System.Text;

namespace Progeto.Lua
{
    public class SvgGraphics : IGraphics
    {
        private StringBuilder content = new StringBuilder();

        public void Draw(Segment s, Color color, double width)
        {
            var lineStr = "<line x1=\"{0}\" y1=\"{1}\" x2=\"{2}\" y2=\"{3}\" style=\"{4}\"/>\n";
            content.AppendFormat(CultureInfo.InvariantCulture, lineStr, s.InitialPoint.x, s.InitialPoint.y, s.FinalPoint.x, s.FinalPoint.y, toStyle(color, width));
        }

        public void Draw(Circle c, Color color, double width)
        {
            var lineStr = "<circle cx=\"{0}\" cy=\"{1}\" r=\"{2}\" style =\"{3}\"/>\n";
            content.AppendFormat(CultureInfo.InvariantCulture, lineStr, c.Center.x, c.Center.y, c.Radius, toStyle(color, width));
        }

        public void Draw(Geometry.Point p, Color color, double width)
        {
            var ellipseStr = "<ellipse cx=\"{0}\" cy=\"{1}\" rx=\"{2}\" ry=\"{3}\" style=\"{4}\"/>\n";
            content.AppendFormat(CultureInfo.InvariantCulture, ellipseStr, p.x, p.y, width, width, toStyle(color, width));
        }

        public string Text()
        {
            var start = "<svg xmlns=\"http://www.w3.org/2000/svg\" id=\"drawing\" version=\"1.1\" height=\"100%\" width=\"100%\">\n";
            var end = "</svg>";
            return start + content + end;
        }

        private string toStyle(Color color, double width)
        {
            return String.Format(CultureInfo.InvariantCulture, "stroke-width:{0};stroke:{1};fill:none;", width, toColorString(color));
        }

        private string toColorString(Color c)
        {
            return string.Format("rgb({0},{1},{2})", c.R, c.G, c.B);
        }
    }
}