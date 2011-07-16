using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GenericJumpAndRun
{
    class Camera
    {
        enum CameraMode
        {
            Free,Locked
        }
        private Viewport _viewport;
        private GameObject _focusObject;
        private CameraMode _mode;
        public Camera(int x,int y,int width,int height)
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
    }
}
