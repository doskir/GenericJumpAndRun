using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GenericJumpAndRun
{
    class Enemy : MovingObject
    {
        public Direction movementDirection;
        public Point SpawnLocation;
        public Enemy(Vector2 position, Vector2 velocity, Texture2D sprite) : base(position, velocity, sprite, ObjectType.Enemy)
        {
            SpawnLocation = new Point((int) position.X, (int) position.Y);
        }
        public override void Update(Level currentLevel)
        {
            if (Alive)
            {
                Move(movementDirection);
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
                    if (movementDirection == Direction.Left)
                        movementDirection = Direction.Right;
                    else
                        movementDirection = Direction.Left;
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
        public override void DetectEnemyHit(MovingObject mob)
        {
            //dont kill friends
            if (mob.Type == this.Type)
                return;
            //collision with enemy detected
            if(mob.Position.Y < Position.Y - Height /2)
            {
                //enemy is above, cant kill
                return;
            }
            mob.Die();
        }

    }
}
