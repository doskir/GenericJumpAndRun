using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GenericJumpAndRun
{
    class Level
    {
        public List<GameObject> GameObjects;
        public Level()
        {
            GameObjects = new List<GameObject>();
        }
    }
}
