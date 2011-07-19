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
        public Direction MovementDirection;
        public Point SpawnLocation;
        public Enemy(Vector2 position, Vector2 velocity, Texture2D sprite) : base(position, velocity, sprite, ObjectType.Enemy)
        {
            SpawnLocation = new Point((int) position.X, (int) position.Y);
        }
        public override void Update(Level currentLevel)
        {
            if (Alive)
            {
                Move(MovementDirection, 0.1f);
            }
            base.Update(currentLevel);
        }
        public override void HitWall()
        {
            if (MovementDirection == Direction.Left)
                MovementDirection = Direction.Right;
            else
                MovementDirection = Direction.Left;
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
