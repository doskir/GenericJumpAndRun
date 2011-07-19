using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GenericJumpAndRun
{
    class Player : MovingObject
    {
        public Player(Vector2 position, Vector2 velocity, Texture2D sprite) : base(position, velocity, sprite,ObjectType.Player)
        {
            
        }

        public int Score;
        public bool CanJump = true;
        public void Jump()
        {
            if(Alive && CanJump)
            {
                Velocity.Y = -10f;
                CanJump = false;
            }
        }
        public override void ReachedBottom()
        {
            base.ReachedBottom();
            CanJump = true;
        }
        public override void DetectEnemyHit(MovingObject mob)
        {
            //collision with enemy detected
            if (Position.Y < mob.Position.Y - mob.Height / 10)
            {
                //hit enemy from above
                mob.Die();
                Score += 100;
                //bounce off of enemy
                CanJump = true;
                Jump();
            }
            else
            {
                Die();
            }
        }
        public void Move(Direction movementDirection)
        {
            base.Move(movementDirection, 0.5f);
        }
        public bool HasFinished(Level currentLevel)
        {
            if (IntersectsWith(currentLevel.FinishZone))
                return true;
            return false;
        }
    }
}
