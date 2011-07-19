using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;

namespace GenericJumpAndRun
{
    class Level
    {
        public string Name;
        public List<GameObject> GameObjects;
        public GameObject StartZone;
        public GameObject FinishZone;
        public bool Finished;
        public DateTime LevelFinishTime;
        public Level()
        {
            GameObjects = new List<GameObject>();
        }
        public string ToLevelString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(StartZone.BoundingRectangle.X + "," + StartZone.BoundingRectangle.Y + ","
                          + StartZone.Sprite.Name);
            sb.AppendLine(FinishZone.BoundingRectangle.X + "," + FinishZone.BoundingRectangle.Y + ","
                          + FinishZone.Sprite.Name);
            foreach (GameObject gobj in GameObjects)
            {
                if (gobj.Type == GameObject.ObjectType.Block)
                    sb.AppendLine(gobj.BoundingRectangle.X + "," + gobj.BoundingRectangle.Y + "," + gobj.Sprite.Name);
                if(gobj.Type == GameObject.ObjectType.Enemy)
                {
                    var enemy = (Enemy) gobj;
                    sb.AppendLine(enemy.SpawnLocation.X + "," + enemy.SpawnLocation.Y + "," + enemy.Sprite.Name);
                }
            }
            return sb.ToString();
        }
        public void SaveLevelToFile(string filename)
        {
            using(var sw = new StreamWriter(filename))
            {
                sw.Write(ToLevelString());
            }
        }

    }
}
