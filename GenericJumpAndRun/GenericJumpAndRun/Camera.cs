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
        private BoundingRectangle _boundingRectangle;
        public bool LockToPlayingArea = true;
        public Camera(int x, int y, int width, int height)
        {
            _viewport = new Viewport(x, y, width, height);
            _boundingRectangle = new BoundingRectangle(x, y, width, height);
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
                    _boundingRectangle.X = _viewport.X;
                }
            }
            if (LockToPlayingArea && _viewport.X < 0)
            {
                _viewport.X = 0;
                _boundingRectangle.X = _viewport.X;
            }
        }

        public bool Visible(GameObject gobj)
        {
            return _boundingRectangle.IntersectsWith(gobj.BoundingRectangle);
        }
    }
}
