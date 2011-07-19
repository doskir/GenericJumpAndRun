using System;
using System.Collections.Generic;
using System.Linq;
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
            Enemy,
            StartZone,
            FinishZone
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
            Type = objectType;
            BoundingRectangle = new BoundingRectangle((int) _position.X, (int) _position.Y, sprite.Width, sprite.Height);
        }

        public bool IntersectsWith(GameObject gameObject)
        {
            if (BoundingRectangle.IntersectsWith(gameObject.BoundingRectangle))
            {
                if (PixelsIntersect(BoundingRectangle.Rectangle, TextureToArray(Sprite),
                                    gameObject.BoundingRectangle.Rectangle, TextureToArray(gameObject.Sprite)))
                    return true;
            }
            return false;
        }
        //IntersectPixels method taken directly from the XNA 2D per pixel collision check. Doesn't need to be changed as far as I can see. 
        private bool PixelsIntersect(Rectangle rectangleA, Color[] dataA, Rectangle rectangleB, Color[] dataB)
        {
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);

            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    Color colorA = dataA[(x - rectangleA.Left) +
                                (y - rectangleA.Top) * rectangleA.Width];
                    Color colorB = dataB[(x - rectangleB.Left) +
                                (y - rectangleB.Top) * rectangleB.Width];

                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private Color[] TextureToArray(Texture2D texture)
        {
            Color[] colors1D = new Color[texture.Width * texture.Height];
            texture.GetData(colors1D);
            return colors1D;
        }
        public virtual bool IntersectsWithAny(List<GameObject> gameObjects)
        {
            return gameObjects.Where(gobj => gobj != this).Any(IntersectsWith);
        }

        public virtual void Update(Level currentLevel)
        {
            
        }
    }
}
