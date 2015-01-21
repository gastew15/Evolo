﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Evolo.GameClass
{
    /// <summary>
    /// Reads .lvl files into game and takes info from it, to determine the placements and values in the game
    /// </summary>
    class LevelSystem
    {
        private int levelNum = 1;
        private Vector2 playerStartPos;
        private Vector2 startPlatPos;
        private Vector2 endPlatPos;
        private int levelMod;
        private int timer;
        private int lineToClear;

        public LevelSystem()
        {

        }

        public void Update()
        {
            StreamReader sr = new StreamReader("Levels/Level" + levelNum + ".dat");
            string[] paramaters = sr.ReadToEnd().Split(';');
            sr.Close();

            playerStartPos = new Vector2(Convert.ToInt32(paramaters[0].Split(',')[0]), Convert.ToInt32(paramaters[0].Split(',')[1]));
            startPlatPos = new Vector2(Convert.ToInt32(paramaters[1].Split(',')[0]), Convert.ToInt32(paramaters[1].Split(',')[1]));
            endPlatPos = new Vector2(Convert.ToInt32(paramaters[2].Split(',')[0]), Convert.ToInt32(paramaters[2].Split(',')[1]));
            levelMod = Convert.ToInt32(paramaters[3]);
            timer = Convert.ToInt32(paramaters[4]);
            lineToClear = Convert.ToInt32(paramaters[5]);

        }

        public void setLevel(int levelNum)
        {
            this.levelNum = levelNum;
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