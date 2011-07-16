using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GenericJumpAndRun
{
    internal class Camera
    {
        private enum CameraMode
        {
            Free,
            Locked
        }

        private Viewport _viewport;
        private GameObject _focusObject;
        private CameraMode _mode;

        public Camera(int x, int y, int width, int height)
        {
            _viewport = new Viewport(x, y, width, height);
        }

        public Vector2 Position
        {
            get { return new Vector2(_viewport.X, _viewport.Y); }
        }

        public void LockToObject(GameObject gameObject)
        {
            _focusObject = gameObject;
            _mode = CameraMode.Locked;
        }

        public void Update()
        {
            if (_mode == CameraMode.Locked)
            {
                if (_viewport.Bounds.Center != _focusObject.BoundingRectangle.Center)
                {
                    _viewport.X = (int) _focusObject.Position.X - _viewport.Width/2;
                }
            }
        }

        public bool Visible(GameObject gobj)
        {
            Vector2 minPosition = new Vector2(_viewport.Bounds.Left - 50, _viewport.Bounds.Top - 50);
            Vector2 maxPosition = new Vector2(_viewport.Bounds.Right + 50, _viewport.Bounds.Bottom + 50);
            if (gobj.Position.X < minPosition.X || gobj.Position.Y < minPosition.Y || gobj.Position.X > maxPosition.X || gobj.Position.Y > maxPosition.Y)
                return false;
            return true;
        }
    }
}
