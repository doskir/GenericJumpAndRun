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
    }
}
