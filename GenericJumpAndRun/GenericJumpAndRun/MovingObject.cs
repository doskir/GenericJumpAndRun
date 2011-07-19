using System.Collections.Generic;
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
                Vector2 lastPosition = Position;
                if (Velocity.Y > 0)
                    Position += new Vector2(Velocity.X, 0);
                else
                    Position += Velocity;
                Velocity.X *= 0.90f;
                if (Velocity.Y < 0)
                    Velocity.Y *= 0.90f;

                if (IntersectsWithAny(currentLevel.GameObjects) != null)
                {
                    Position = lastPosition;
                    Velocity = new Vector2(0, Velocity.Y);
                    HitWall();
                }
                Fall(currentLevel);
                if(Position.Y > 800)
                {
                    //below visible screen
                    Die();
                }
            }
            else
            {
                if (Position.Y < -5000)
                    Velocity = Vector2.Zero;
                Position += Velocity;
            }
        }
        public virtual void HitWall()
        {

        }
        public virtual void Move(Direction movementDirection,float speed)
        {
            if (Alive)
            {
                if (movementDirection == Direction.Left)
                    Velocity.X -= speed;
                else if (movementDirection == Direction.Right)
                    Velocity.X += speed;
            }
        }
        public void Fall(Level level)
        {
            if (Alive)
            {
                //gravity
                Vector2 originalPosition = Position;
                Velocity.Y += 0.3f;
                var verticalVelocity = new Vector2(0, Velocity.Y);
                Position += verticalVelocity;
                GameObject intersects = IntersectsWithAny(level.GameObjects);
                if (intersects != null)
                {
                    Position = originalPosition;
                    //object is below this
                    if (intersects.Position.Y > Position.Y)
                        ReachedBottom();
                }
            }
        }

        public virtual void ReachedBottom()
        {
            Velocity.Y = 0;
        }
        public void Die()
        {
            if(Alive)
            {
                Alive = false;
                Velocity = new Vector2(0, -10f);
            }
        }

        public override GameObject IntersectsWithAny(List<GameObject> gameObjects)
        {
            if (!Alive)
                return null;
            foreach (GameObject gobj in gameObjects)
            {
                if (gobj == this)
                    continue;
                if (IntersectsWith(gobj))
                {
                    if (gobj.Type == ObjectType.Enemy || gobj.Type == ObjectType.Player)
                    {
                        if (Type == ObjectType.Enemy && gobj.Type == ObjectType.Enemy)
                            return gobj;
                        var mob = (MovingObject) gobj;
                        if (mob.Alive)
                        {
                            DetectEnemyHit(mob);
                        }
                    }
                    else
                    {
                        return gobj;
                    }
                }
            }
            return null;
        }
        public virtual void DetectEnemyHit(MovingObject mob)
        {
        }
    }

}
