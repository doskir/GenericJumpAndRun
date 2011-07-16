using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GenericJumpAndRun
{
    class GameObject
    {
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                BoundingRectangle.X = (int) _position.X;
                BoundingRectangle.Y = (int) _position.Y;
            }
        }
        private Vector2 _position;
        public Vector2 Velocity;
        public readonly Texture2D Sprite;
        public BoundingRectangle BoundingRectangle;
        public GameObject(Vector2 position,Vector2 velocity,Texture2D sprite)
        {
            _position = position;
            Velocity = velocity;
            Sprite = sprite;
            BoundingRectangle = new BoundingRectangle((int) _position.X, (int) _position.Y, sprite.Width, sprite.Height);
        }

        public bool IntersectsWith(GameObject gameObject)
        {
            if(BoundingRectangle.IntersectsWith(gameObject.BoundingRectangle))
            {
                //TODO: Implement higher precision collision detection
                return true;
            }
            return false;
        }
    }
}
