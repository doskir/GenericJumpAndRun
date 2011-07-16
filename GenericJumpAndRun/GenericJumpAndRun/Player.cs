using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GenericJumpAndRun
{
    class Player : GameObject
    {
        public Player(Vector2 position, Vector2 velocity, Texture2D sprite) : base(position, velocity, sprite)
        {
            
        }

        private Vector2 _lastPosition;
        private Vector2 _lastVelocity;
        public bool CanJump = true;
        public void Jump()
        {
            if(CanJump)
            {
                Velocity.Y -= 10f;
                CanJump = false;
            }
        }
        public void Update(Level currentLevel)
        {
            _lastPosition = Position;
            _lastVelocity = Velocity;
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
                UndoLastMove();
                Velocity = Vector2.Zero;
            }
            Fall(currentLevel);
        }
        public enum Direction
        {
            Left,Right
        }
        public void Move(Direction movementDirection)
        {
            if (movementDirection == Direction.Left)
                Velocity.X -= 0.5f;
            else if (movementDirection == Direction.Right)
                Velocity.X += 0.5f;
        }

        public void UndoLastMove()
        {
            Position = _lastPosition;
            Velocity = _lastVelocity;
        }
        public void Fall(Level level)
        {            //gravity
            Vector2 originalPosition = Position;
            Velocity.Y += 0.2f;
            Vector2 verticalVelocity = new Vector2(0, Velocity.Y);
            Position += verticalVelocity;
            if(IntersectsWithAny(level.GameObjects))
            {
                Position = originalPosition;
                ReachedBottom();
            }
        }
        public void ReachedBottom()
        {
            Velocity.Y = 0;
            CanJump = true;
        }
    }
}
