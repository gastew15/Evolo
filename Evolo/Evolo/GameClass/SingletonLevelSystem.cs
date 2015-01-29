using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Evolo.GameClass
{
    public class SingletonLevelSystem
    {
        private static SingletonLevelSystem SinLevelSys = null;
        private String levelName;
        private Vector2 playerStartPos;
        private Vector2 startPlatPos;
        private Vector2 endPlatPos;
        private int levelMod;
        private int timer;
        private int lineToClear;

        private SingletonLevelSystem() { }

        public static SingletonLevelSystem getInstance()
        {
            if (null == SinLevelSys)
            {
                SinLevelSys = new SingletonLevelSystem();
            }

            return SinLevelSys;  
        }

        public void Update()
        {
            StreamReader sr = new StreamReader("Levels/Level" + levelName + ".dat");
            string[] paramaters = sr.ReadToEnd().Split(';');
            sr.Close();

            playerStartPos = new Vector2(Convert.ToInt32(paramaters[0].Split(',')[0]), Convert.ToInt32(paramaters[0].Split(',')[1]));
            startPlatPos = new Vector2(Convert.ToInt32(paramaters[1].Split(',')[0]), Convert.ToInt32(paramaters[1].Split(',')[1]));
            endPlatPos = new Vector2(Convert.ToInt32(paramaters[2].Split(',')[0]), Convert.ToInt32(paramaters[2].Split(',')[1]));
            levelMod = Convert.ToInt32(paramaters[3]);
            timer = Convert.ToInt32(paramaters[4]);
            lineToClear = Convert.ToInt32(paramaters[5]);
        }
        public void setLevel(String levelName)
        {
            this.levelName = levelName;
        }

        public Vector2 getPlayerPos()
        {
            return playerStartPos;
        }

        public Vector2 getStartPlatPos()
        {
            return startPlatPos;
        }

        public Vector2 getEndPlatPos()
        {
            return endPlatPos;
        }

        public int getTimer()
        {
            return timer;
        }

        public int getLevelMod()
        {
            return levelMod;
        }

        public int getLinesToClear()
        {
            return lineToClear;
        }
    }
}
