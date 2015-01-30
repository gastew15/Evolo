﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

    /**
    * Evolo Level System: uses a Singleton pattern to create a level system that will read dat files that contain data on the layout of each level
    * Author: Dalton
    * Version: 1/30/15
    */

namespace Evolo.GameClass
{

    public class SingletonLevelSystem
    {
        private static SingletonLevelSystem SinLevelSys = null;
        private String levelName;
        private Vector2 playerStartPos;
        private Vector2 startPlatPos;
        private Vector2 endPlatPos;
        private double levelMod;
        private int timer;
        private int lineToClear;
        

        //Private constructor so that the only class that can intitialize the level system is itself
        private SingletonLevelSystem() { }

        //Method to check 
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
            try
            {
                StreamReader sr = new StreamReader("Levels/" + levelName + ".dat");
                string[] paramaters = sr.ReadToEnd().Split(';');
                sr.Close();

                playerStartPos = new Vector2(Convert.ToInt32(paramaters[0].Split(',')[0]), Convert.ToInt32(paramaters[0].Split(',')[1]));
                startPlatPos = new Vector2(Convert.ToInt32(paramaters[1].Split(',')[0]), Convert.ToInt32(paramaters[1].Split(',')[1]));
                endPlatPos = new Vector2(Convert.ToInt32(paramaters[2].Split(',')[0]), Convert.ToInt32(paramaters[2].Split(',')[1]));
                levelMod = Convert.ToDouble(paramaters[3]);
                timer = Convert.ToInt32(paramaters[4]);
                lineToClear = Convert.ToInt32(paramaters[5]);
            }
            catch
            {
                if (!File.Exists("Levels/" + levelName + ".dat"))
                {
                    StreamWriter stream = new StreamWriter("Levels/" + levelName + ".dat");

                    switch (levelName)
                    {
                        case "Level1":
                            stream.Write("0,16;0,17;23,15;1;330;10");
                            break;
                        case "Level2":
                            stream.Write("0,13;0,14;23,18;1.25;420;15");
                            break;
                        case "Level3":
                            stream.Write("0,14;0,15;23,10;1.5;590;20");
                            break;
                        case "Level4":
                            stream.Write("0,9;0,10;23,15;1.75;600;25");
                            break;
                        case "Level5":
                            stream.Write("0,14;0,15;23,8;2.5;800;40");
                            break;
                    }

                    stream.Close();
                }

            }
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

        public double getLevelMod()
        {
            return levelMod;
        }

        public int getLinesToClear()
        {
            return lineToClear;
        }
    }
}
