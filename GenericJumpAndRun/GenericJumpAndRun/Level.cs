﻿using System;
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
        public string Name;
        public List<GameObject> GameObjects;
        public GameObject StartZone;
        public GameObject FinishZone;
        public bool Finished;
        public Level()
        {
            GameObjects = new List<GameObject>();
        }
        public string ToLevelString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (GameObject gobj in GameObjects)
            {
                if (gobj.Type == GameObject.ObjectType.Block)
                    sb.AppendLine(gobj.BoundingRectangle.X + "," + gobj.BoundingRectangle.Y + "," + gobj.Sprite.Name);
            }
            return sb.ToString();
        }
    }
}
