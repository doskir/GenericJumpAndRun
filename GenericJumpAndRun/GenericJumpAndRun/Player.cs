using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GenericJumpAndRun
{
    class Player : MovingObject
    {
        public Player(Vector2 position, Vector2 velocity, Texture2D sprite) : base(position, velocity, sprite,ObjectType.Player)
        {
            
        }
        public bool CanJump = true;
        public void Jump()
        {
            if(CanJump)
            {
                Velocity.Y -= 10f;
                CanJump = false;
            }
        }
        public override void ReachedBottom()
        {
            Velocity.Y = 0;
            CanJump = true;
        }
    }
}
