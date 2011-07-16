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
        public enum ObjectType
        {
            Block,
            Player,
            Enemy
        }
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
        public int Width
        {
            get { return Sprite.Width; }
        }
        public int Height
        {
            get { return Sprite.Height; }
        }
        private Vector2 _position;
        public Vector2 Velocity;
        public Texture2D Sprite;
        public BoundingRectangle BoundingRectangle;
        public ObjectType Type;
        public GameObject(Vector2 position,Vector2 velocity,Texture2D sprite,ObjectType objectType)
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
        public bool IntersectsWithAny(List<GameObject> gameObjects)
        {
            foreach(GameObject gobj in gameObjects)
            {
                if (gobj == this)
                    continue;
                if (IntersectsWith(gobj))
                    return true;
            }
            return false;
        }
        public virtual void Update(Level currentLevel)
        {
            
        }
    }
}
