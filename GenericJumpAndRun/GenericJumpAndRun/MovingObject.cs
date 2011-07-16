using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GenericJumpAndRun
{
    abstract class MovingObject : GameObject
    {
        public enum Direction
        {
            Left, Right
        }
        protected MovingObject(Vector2 position, Vector2 velocity, Texture2D sprite, ObjectType objectType) : base(position, velocity, sprite, objectType)
        {
            
        }
        public override void Update(Level currentLevel)
        {
            Vector2 _lastPosition = Position;
            if (Velocity.Y > 0)
                Position += new Vector2(Velocity.X, 0);
            else
                Position += Velocity;
            Velocity.X *= 0.9f;
            //going up
            if (Velocity.Y < 0)
                Velocity.Y *= 0.9f;
            if (IntersectsWithAny(currentLevel.GameObjects))
            {
                Position = _lastPosition;
                Velocity = Vector2.Zero;
            }
            Fall(currentLevel);
        }
        public void Move(Direction movementDirection)
        {
            if (movementDirection == Direction.Left)
                Velocity.X -= 0.5f;
            else if (movementDirection == Direction.Right)
                Velocity.X += 0.5f;
        }
        public void Fall(Level level)
        {            //gravity
            Vector2 originalPosition = Position;
            Velocity.Y += 0.2f;
            Vector2 verticalVelocity = new Vector2(0, Velocity.Y);
            Position += verticalVelocity;
            if (IntersectsWithAny(level.GameObjects))
            {
                Position = originalPosition;
                ReachedBottom();
            }
        }
        public virtual void ReachedBottom()
        {
            Velocity.Y = 0;
        }
    }
}
