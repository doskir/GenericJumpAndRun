using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GenericJumpAndRun
{
    class BoundingRectangle
    {
        private Rectangle _rectangle;
        public int X
        {
            get { return _rectangle.X; }
            set { _rectangle.X = value; }
        }
        public int Y
        {
            get { return _rectangle.Y; }
            set { _rectangle.Y = value; }
        }
        public BoundingRectangle(int x, int y, int width, int height)
        {
            _rectangle = new Rectangle(x, y, width, height);
        }
        public bool IntersectsWith(BoundingRectangle boundingRectangle)
        {
            return _rectangle.Intersects(boundingRectangle._rectangle);
        }
    }
}
