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

        public bool Alive;
        protected MovingObject(Vector2 position, Vector2 velocity, Texture2D sprite, ObjectType objectType) : base(position, velocity, sprite, objectType)
        {
            Alive = true;
        }
        public override void Update(Level currentLevel)
        {
            if (Alive)
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
            else
            {
                if (Position.Y < -5000)
                    Velocity = Vector2.Zero;
                Position += Velocity;
            }
        }
        public void Move(Direction movementDirection)
        {
            if (Alive)
            {
                if (movementDirection == Direction.Left)
                    Velocity.X -= 0.5f;
                else if (movementDirection == Direction.Right)
                    Velocity.X += 0.5f;
            }
        }
        public void Fall(Level level)
        {
            if (Alive)
            {
                //gravity
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
        }

        public virtual void ReachedBottom()
        {
            Velocity.Y = 0;
        }
        public virtual void Die()
        {
            if(Alive)
            {
                Alive = false;
                Velocity = new Vector2(0, -10f);
            }
        }

        public override bool IntersectsWithAny(List<GameObject> gameObjects)
        {
            if (!Alive)
                return false;
            foreach (GameObject gobj in gameObjects)
            {
                if (gobj == this)
                    continue;
                if (IntersectsWith(gobj))
                {
                    if (gobj.Type == ObjectType.Enemy || gobj.Type == ObjectType.Player)
                    {
                        if (Type == ObjectType.Enemy && gobj.Type == ObjectType.Enemy)
                            return true;
                        MovingObject mob = (MovingObject) gobj;
                        if (mob.Alive)
                        {
                            DetectEnemyHit(mob);
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public virtual void DetectEnemyHit(MovingObject mob)
        {
        }
    }

}
