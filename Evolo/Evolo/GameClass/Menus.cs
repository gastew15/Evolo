﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using System.IO;
using StarByte.ui;
using StarByte.io;

namespace Evolo.GameClass
{
    class Menus
    {
        
        private int customMenuPageMod, fileAmount, customLevelFileCount; //for changing pages and displaying more levels
        private String[] customLevelList;
        private Boolean gameWin = false;
        private MenuHandler mainMenu, optionsMenu, optionsResolutionMenu, optionsKeybindingMenuPage1, optionsKeybindingMenuPage2, pauseMenu, debugMenu, loadProfileMenu, gameLoseMenu, gameWinMenu, levelSelectMenu, customLevelMenu;
        private WindowSizeManager windowSizeManager;
        private SpriteFont font;
        private Boolean pausedLast;
        private OptionsHandler optionsHandler;
        private SaveHandler saveHandler;
        private String menuState;
        private String previousMenuState;
        private String storedRealPreviousMenuState;
        //Playing, MenuScreen, GameOver

        //Temp string
        private String[] loadData = new string[] { "" };

        private String[] levelInfo = { "0,14;0,15;23,10;1;390;10", "0,19;0,20;23,10;2;450;15", "0,14;0,15;23,10;1;390;10", "0,19;0,20;23,10;2;450;15", "0,14;0,15;23,10;1;390;10" };

        //Variables
        private Texture2D optionsTitle, menuTitle, pauseTitle, debugTitle, keybindBlockTitle, keybindPlayerTitle, gameLoseTitle, gameWinTitle, levelSelectMenuTitle, customLevelMenuTitle, menuButtonBackground, menuButtonBorder7, menuButtonBorder6, menuButtonBorder4, menuButtonBorder2, menuButtonBorder3;
        private int mainMenuVerticalSpacing = 24;
        private Vector2 optionsCenterMenuSP, mainMenuSP, keybindingCenterMenuSP, pauseMenuSP, debugSP, loadProfileMenuSP, gameOverMenuSP, levelSelectMenuSp, customLevelMenuSp;
        private String[] mainMenuButtonText;
        private String[] optionsMenuButtonText;
        private String[] optionsResolutionMenuButtonText;
        private String[] optionsKeybindingMenuPage1ButtonText;
        private String[] optionsKeybindingMenuPage2ButtonText;
        private String[] pauseMenuButtonText;
        private String[] keyBindingInfo;
        private String[] debugMenuButtonText;
        private String[] gameLoseMenuText;
        private String[] gameWinMenuText;
        private String[] levelSelectMenuText;
        private String[] customLevelMenuText;
        private String[] loadProfileMenuButtonText;
        private Color[] mainMenuColors;
        private Color[] optionsMenuColors;
        private Color[] optionsResolutionMenuColors;
        private Color[] optionsKeybindingMenuColors;
        private Color[] pauseMenuColors;
        private Color[] debugMenuColors;
        private Color[] levelSelectMenuColors;
        private Color[] customLevelMenuColors;
        private Color[] loadProfileMenuColors;
        private GraphicsDeviceManager graphics;
        private SoundEffect menuHoverChangeSoundEffect, menuClickedSoundEffect;
        private SingletonLevelSystem levels = SingletonLevelSystem.getInstance();
        private Boolean loadProfileFirstTimeStartUp;



        public Menus(GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
        }

        public void Initialize(String[] keyBindingInfo)
        {
            GlobalVar.HighScore = new int[5];
            optionsHandler = new OptionsHandler(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Evolo");
            saveHandler = new SaveHandler(6, new String[] { "1", "0", "0", "0", "0", "0" }, Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Evolo", "Save.dat");

            //2 pages to contain all of the key options, treat as seperate menus
            optionsKeybindingMenuPage1ButtonText = new String[6] { "PlayerLeft: " + "left", "PlayerRight: " + keyBindingInfo[1], "PlayerJump: " + keyBindingInfo[2], "Nothing At All", "Next Page ->", "Back" };
            optionsKeybindingMenuPage2ButtonText = new String[6] { "BlockLeft: " + keyBindingInfo[3], "BlockRight: " + keyBindingInfo[4], "BlockRotate: " + keyBindingInfo[5], "BlockDown: " + keyBindingInfo[6], "<- Previous Page", "Back" };
            optionsMenuButtonText = new String[7] { "Resolution", "Key Bindings", "Debug Options", "Sound: OFF", "Music: OFF", "Exit & Save", "Exit W/O Saving" };
            optionsResolutionMenuButtonText = new String[7] { "Full Screen", "800 x 600", "1280 x 720", "1366 x 768", "1600 x 900", "1920 x 1080", "Back" };
            mainMenuButtonText = new String[6] { "Level Select", "Load Profile", "Options", "Credits", "Help", "Quit" };
            pauseMenuButtonText = new String[3] { "Resume", "Options", "Exit" };
            debugMenuButtonText = new String[4] { "Cursor: Game", "FPS: Off", "Debug Info: Off", "Back" };
            loadProfileMenuButtonText = new String[7] { "Profile 1", "Profile 2", "Profile 3", "Profile 4", "Profile 5", "Profile 6", "Back" };
            gameLoseMenuText = new String[2] { "Restart", "Main Menu" };
            gameWinMenuText = new String[2] { "Restart", "Main Menu" };
            levelSelectMenuText = new String[7] { "Level 1", "Level 2", "Level 3", "Level 4", "Level 5", "Custom Level", "Back" };
            customLevelMenuText = new String[7] { "(Blank)", "(Blank)", "(Blank)", "(Blank)", "Previous", "Next", "Back" };
            #region Menu Text Colours
            mainMenuColors = new Color[] { Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray };
            optionsMenuColors = new Color[] { Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray };
            optionsResolutionMenuColors = new Color[] { Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray };
            optionsKeybindingMenuColors = new Color[] { Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray };
            pauseMenuColors = new Color[] { Color.DarkGray, Color.DarkGray, Color.DarkGray };
            debugMenuColors = new Color[] { Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray };
            loadProfileMenuColors = new Color[] { Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray };
            levelSelectMenuColors = new Color[] { Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray };
            customLevelMenuColors = new Color[] { Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray };
            #endregion

            #region Level File Checking
            try
            {
                DirectoryInfo dInfo = new DirectoryInfo("Levels/CustomLevels");
                customLevelFileCount = dInfo.GetFiles().Length - 1;
                customLevelList = new String[customLevelFileCount];

                foreach (FileInfo file in dInfo.GetFiles())
                {
                    if (file.ToString() != "Level Template.txt")
                    {
                        customLevelList[fileAmount] = file.ToString().Substring(0, (file.ToString().Length - 4));
                        fileAmount++;
                    }
                }
            }
            catch
            {
                if (!Directory.Exists("Levels"))
                {
                    Directory.CreateDirectory("Levels");
                    Directory.CreateDirectory("Levels/CustomLevels");

                    StreamWriter sw = new StreamWriter("Levels/CustomLevels/Level Template.txt", false, Encoding.ASCII);
                    sw.Write("Player Start Position;Start Platform Position;End Platform Position;Level Modifier;Timer(Seconds);Lines to Clear");
                    sw.WriteLine("");
                    sw.WriteLine("Make sure there are no spaces in the cordinates");
                    sw.WriteLine("");
                    sw.WriteLine("0, 14;0, 15;23,10;1;390;10 NO");
                    sw.WriteLine("");
                    sw.WriteLine("0,14;0,15;23,10;1;390;10 YES");
                    sw.Close();

                    for (int i = 0; i < 5; i++)
                    {
                        StreamWriter sr = new StreamWriter("Levels/Level" + (i + 1) + ".dat");
                        sr.Write(levelInfo[i]);
                        sr.Close();
                    }
                }

                else if(!Directory.Exists("Levels/CustomLevels"))
                {
                    Directory.CreateDirectory("Levels/CustomLevels");
                    StreamWriter sw = new StreamWriter("Levels/CustomLevels/Level Template.txt", false, Encoding.ASCII);
                    sw.Write("Player Start Position;Start Platform Position;End Platform Position;Level Modifier;Timer(Seconds);Lines to Clear");
                    sw.WriteLine("");
                    sw.WriteLine("Make sure there are no spaces in the cordinates");
                    sw.WriteLine("");
                    sw.WriteLine("0, 14;0, 15;23,10;1;390;10 NO");
                    sw.WriteLine("");
                    sw.WriteLine("0,14;0,15;23,10;1;390;10 YES");
                    sw.Close();

                }

                DirectoryInfo dInfo = new DirectoryInfo("Levels/CustomLevels");
                customLevelFileCount = dInfo.GetFiles().Length - 1;
                customLevelList = new String[customLevelFileCount];

                foreach (FileInfo file in dInfo.GetFiles())
                {
                    customLevelList[fileAmount] = file.ToString().Substring(0, (file.ToString().Length - 4));
                    fileAmount++;
                }

            }
            #endregion

            var mouseState = Mouse.GetState();
            var mousePosition = new Point(mouseState.X, mouseState.Y);
            this.keyBindingInfo = keyBindingInfo;
            windowSizeManager = new WindowSizeManager(graphics);
        }

        public void LoadContent(ContentManager Content, SpriteFont font)
        {
            this.font = font;
            #region Sound/Sprite Content Loading
            menuButtonBorder3 = Content.Load<Texture2D>("Sprites and Pictures/ButtonBorder3");
            menuButtonBorder4 = Content.Load<Texture2D>("Sprites and Pictures/ButtonBorder4");
            menuButtonBorder6 = Content.Load<Texture2D>("Sprites and Pictures/ButtonBorder6");
            menuButtonBorder7 = Content.Load<Texture2D>("Sprites and Pictures/ButtonBorder7");
            menuButtonBorder2 = Content.Load<Texture2D>("Sprites and Pictures/ButtonBorder2");

            menuButtonBackground = Content.Load<Texture2D>("Sprites and Pictures/ButtonBackground");

            keybindBlockTitle = Content.Load<Texture2D>("Sprites and Pictures/Logo_KeybindBlock");
            keybindPlayerTitle = Content.Load<Texture2D>("Sprites and Pictures/Logo_KeybindPlayer");

            debugTitle = Content.Load<Texture2D>("Sprites and Pictures/Logo_Debug");
            pauseTitle = Content.Load<Texture2D>("Sprites and Pictures/Logo_Pause");
            menuTitle = Content.Load<Texture2D>("Sprites and Pictures/Logo_MainMenu");
            optionsTitle = Content.Load<Texture2D>("Sprites and Pictures/Logo_options");
            gameLoseTitle = Content.Load<Texture2D>("Sprites and Pictures/Logo_YouLose");
            gameWinTitle = Content.Load<Texture2D>("Sprites and Pictures/Logo_YouWin");
            levelSelectMenuTitle = Content.Load<Texture2D>("Sprites and Pictures/Logo_LevelSelect");
            customLevelMenuTitle = Content.Load<Texture2D>("Sprites and Pictures/Logo_CustomLevel");
            menuHoverChangeSoundEffect = Content.Load<SoundEffect>("Sounds/Sound Effects/menuHoverChangeEffect");
            menuClickedSoundEffect = Content.Load<SoundEffect>("Sounds/Sound Effects/menuClickedEffect");
            #endregion
            #region Menu Starting Positions
            //SP = Starting Position
            mainMenuSP = new Vector2();
            optionsCenterMenuSP = new Vector2((GlobalVar.ScreenSize.X / 2) - (menuButtonBackground.Width / 2), (GlobalVar.ScreenSize.Y / 2) - (((menuButtonBackground.Height * optionsMenuButtonText.Length) + (mainMenuVerticalSpacing * (optionsMenuButtonText.Length - 1))) / 2));
            keybindingCenterMenuSP = new Vector2((GlobalVar.ScreenSize.X / 2) - (menuButtonBackground.Width / 2), (GlobalVar.ScreenSize.Y / 2) - (((menuButtonBackground.Height * optionsKeybindingMenuPage1ButtonText.Length) + (mainMenuVerticalSpacing * (optionsMenuButtonText.Length - 1))) / 2));
            pauseMenuSP = new Vector2((GlobalVar.ScreenSize.X / 2) - (menuButtonBackground.Width / 2), (GlobalVar.ScreenSize.Y / 2) - (((menuButtonBackground.Height * pauseMenuButtonText.Length) + (mainMenuVerticalSpacing * (pauseMenuButtonText.Length - 1))) / 2));
            debugSP = new Vector2((GlobalVar.ScreenSize.X / 2) - (menuButtonBackground.Width / 2), (GlobalVar.ScreenSize.Y / 2) - (((menuButtonBackground.Height * debugMenuButtonText.Length) + (mainMenuVerticalSpacing * (debugMenuButtonText.Length - 1))) / 2));
            loadProfileMenuSP = new Vector2((GlobalVar.ScreenSize.X / 2) - (menuButtonBackground.Width / 2), (GlobalVar.ScreenSize.Y / 2) - (((menuButtonBackground.Height * loadProfileMenuButtonText.Length) + (mainMenuVerticalSpacing * (loadProfileMenuButtonText.Length - 1))) / 2));
            #endregion
            #region Menu Handlers
            mainMenu = new MenuHandler(menuTitle, menuButtonBorder6, GlobalVar.ScaleSize, new Vector2((GlobalVar.ScreenSize.X / 2) - ((menuButtonBorder6.Width * GlobalVar.ScaleSize.X) / 2), 0), new Vector2((GlobalVar.ScreenSize.X / 2) - ((menuTitle.Width * GlobalVar.ScaleSize.X) / 2), 0), mainMenuVerticalSpacing, menuButtonBackground, mainMenuButtonText, font, menuHoverChangeSoundEffect, menuClickedSoundEffect, GlobalVar.ScreenSize);
            pauseMenu = new MenuHandler(pauseTitle, menuButtonBorder3, GlobalVar.ScaleSize, new Vector2((GlobalVar.ScreenSize.X / 2) - ((menuButtonBorder3.Width * GlobalVar.ScaleSize.X) / 2), 0), new Vector2((GlobalVar.ScreenSize.X / 2) - ((pauseTitle.Width * GlobalVar.ScaleSize.X) / 2), 0), mainMenuVerticalSpacing, menuButtonBackground, pauseMenuButtonText, font, menuHoverChangeSoundEffect, menuClickedSoundEffect, GlobalVar.ScreenSize);
            gameLoseMenu = new MenuHandler(gameLoseTitle, menuButtonBorder2, GlobalVar.ScaleSize, new Vector2((GlobalVar.ScreenSize.X / 2) - ((menuButtonBorder2.Width * GlobalVar.ScaleSize.X) / 2), 0), new Vector2((GlobalVar.ScreenSize.X / 2) - ((gameLoseTitle.Width * GlobalVar.ScaleSize.X) / 2), 0), mainMenuVerticalSpacing, menuButtonBackground, gameLoseMenuText, font, menuHoverChangeSoundEffect, menuClickedSoundEffect, GlobalVar.ScreenSize);
            gameWinMenu = new MenuHandler(gameWinTitle, menuButtonBorder2, GlobalVar.ScaleSize, new Vector2((GlobalVar.ScreenSize.X / 2) - ((menuButtonBorder2.Width * GlobalVar.ScaleSize.X) / 2), 0), new Vector2((GlobalVar.ScreenSize.X / 2) - ((gameWinTitle.Width * GlobalVar.ScaleSize.X) / 2), 0), mainMenuVerticalSpacing, menuButtonBackground, gameWinMenuText, font, menuHoverChangeSoundEffect, menuClickedSoundEffect, GlobalVar.ScreenSize);
            levelSelectMenu = new MenuHandler(levelSelectMenuTitle, menuButtonBorder7, GlobalVar.ScaleSize, new Vector2((GlobalVar.ScreenSize.X / 2) - ((menuButtonBorder7.Width * GlobalVar.ScaleSize.X) / 2), 0), new Vector2((GlobalVar.ScreenSize.X / 2) - ((levelSelectMenuTitle.Width * GlobalVar.ScaleSize.X) / 2), 0), mainMenuVerticalSpacing, menuButtonBackground, levelSelectMenuText, font, menuHoverChangeSoundEffect, menuClickedSoundEffect, GlobalVar.ScreenSize);
            customLevelMenu = new MenuHandler(customLevelMenuTitle, menuButtonBorder7, GlobalVar.ScaleSize, new Vector2((GlobalVar.ScreenSize.X / 2) - ((menuButtonBorder7.Width * GlobalVar.ScaleSize.X) / 2), 0), new Vector2((GlobalVar.ScreenSize.X / 2) - ((customLevelMenuTitle.Width * GlobalVar.ScaleSize.X) / 2), 0), mainMenuVerticalSpacing, menuButtonBackground, customLevelMenuText, font, menuHoverChangeSoundEffect, menuClickedSoundEffect, GlobalVar.ScreenSize);

            optionsMenu = new MenuHandler(optionsTitle, menuButtonBorder7, GlobalVar.ScaleSize, new Vector2((GlobalVar.ScreenSize.X / 2) - ((menuButtonBorder7.Width * GlobalVar.ScaleSize.X) / 2), 0), new Vector2((GlobalVar.ScreenSize.X / 2) - ((optionsTitle.Width * GlobalVar.ScaleSize.X) / 2), 0), mainMenuVerticalSpacing, menuButtonBackground, optionsMenuButtonText, font, menuHoverChangeSoundEffect, menuClickedSoundEffect, GlobalVar.ScreenSize);
            optionsResolutionMenu = new MenuHandler(optionsTitle, menuButtonBorder7, GlobalVar.ScaleSize, new Vector2((GlobalVar.ScreenSize.X / 2) - ((menuButtonBorder7.Width * GlobalVar.ScaleSize.X) / 2), 0), new Vector2((GlobalVar.ScreenSize.X / 2) - ((optionsTitle.Width * GlobalVar.ScaleSize.X) / 2), 0), mainMenuVerticalSpacing, menuButtonBackground, optionsResolutionMenuButtonText, font, menuHoverChangeSoundEffect, menuClickedSoundEffect, GlobalVar.ScreenSize);
            optionsKeybindingMenuPage1 = new MenuHandler(keybindPlayerTitle, menuButtonBorder6, GlobalVar.ScaleSize, new Vector2((GlobalVar.ScreenSize.X / 2) - ((menuButtonBorder7.Width * GlobalVar.ScaleSize.X) / 2), 0), new Vector2((GlobalVar.ScreenSize.X / 2) - ((keybindPlayerTitle.Width * GlobalVar.ScaleSize.X) / 2), 0), mainMenuVerticalSpacing, menuButtonBackground, optionsKeybindingMenuPage1ButtonText, font, menuHoverChangeSoundEffect, menuClickedSoundEffect, GlobalVar.ScreenSize);
            optionsKeybindingMenuPage2 = new MenuHandler(keybindBlockTitle, menuButtonBorder6, GlobalVar.ScaleSize, new Vector2((GlobalVar.ScreenSize.X / 2) - ((menuButtonBorder7.Width * GlobalVar.ScaleSize.X) / 2), 0), new Vector2((GlobalVar.ScreenSize.X / 2) - ((keybindBlockTitle.Width * GlobalVar.ScaleSize.X) / 2), 0), mainMenuVerticalSpacing, menuButtonBackground, optionsKeybindingMenuPage2ButtonText, font, menuHoverChangeSoundEffect, menuClickedSoundEffect, GlobalVar.ScreenSize);

            debugMenu = new MenuHandler(debugTitle, menuButtonBorder4, GlobalVar.ScaleSize, new Vector2((GlobalVar.ScreenSize.X / 2) - ((menuButtonBorder4.Width * GlobalVar.ScaleSize.X) / 2), 0), new Vector2((GlobalVar.ScreenSize.X / 2) - ((debugTitle.Width * GlobalVar.ScaleSize.X) / 2), 0), mainMenuVerticalSpacing, menuButtonBackground, debugMenuButtonText, font, menuHoverChangeSoundEffect, menuClickedSoundEffect, GlobalVar.ScreenSize);

            //debugMenu = new MenuHandler(optionsTitle, debugSP, new Vector2(), mainMenuVerticalSpacing, menuButtonBackground, debugMenuButtonText, font, menuHoverChangeSoundEffect, menuClickedSoundEffect, GlobalVar.ScreenSize);
            loadProfileMenu = new MenuHandler(menuTitle, menuButtonBorder7, GlobalVar.ScaleSize, new Vector2((GlobalVar.ScreenSize.X / 2) - ((menuButtonBorder7.Width * GlobalVar.ScaleSize.X) / 2), 0), new Vector2((GlobalVar.ScreenSize.X / 2) - ((menuTitle.Width * GlobalVar.ScaleSize.X) / 2), 0), mainMenuVerticalSpacing, menuButtonBackground, loadProfileMenuButtonText, font, menuHoverChangeSoundEffect, menuClickedSoundEffect, GlobalVar.ScreenSize);

            if (Boolean.Parse(GlobalVar.OptionsArray[12]) == true)
                optionsMenuButtonText[3] = "Sound: ON";
            if (Boolean.Parse(GlobalVar.OptionsArray[13]) == true)
                optionsMenuButtonText[4] = "Music: ON";
            if (Boolean.Parse(GlobalVar.OptionsArray[9]) == true)
                debugMenuButtonText[0] = "Cursor: Hardware";
            if (Boolean.Parse(GlobalVar.OptionsArray[10]) == true)
                debugMenuButtonText[1] = "FPS: On";
            if (Boolean.Parse(GlobalVar.OptionsArray[11]) == true)
                debugMenuButtonText[2] = "Debug Info: On";

            #endregion
        }

        public void Update(GameTime gameTime, MouseState mouseStateCurrent, MouseState mouseStatePrevious, float millisecondsElapsedGametime)
        {
            //Handles Menu States
            switch (menuState)
            {
                #region Main Menu Update
                case "MainMenu":
                    mainMenu.Update(gameTime,
                        mouseStateCurrent,
                        mouseStatePrevious,
                        GlobalVar.ScreenSize,
                        new Vector2(((GlobalVar.ScreenSize.X / 2) - (menuButtonBackground.Width * GlobalVar.ScaleSize.X) / 2), ((GlobalVar.ScreenSize.Y / 2) - (((menuTitle.Height + menuButtonBorder6.Height) * GlobalVar.ScaleSize.Y)) / 2) + menuTitle.Height * GlobalVar.ScaleSize.Y),
                        new Vector2((GlobalVar.ScreenSize.X / 2) - ((menuButtonBorder6.Width * GlobalVar.ScaleSize.X) / 2), ((GlobalVar.ScreenSize.Y / 2) - (((menuTitle.Height + menuButtonBorder6.Height) * GlobalVar.ScaleSize.Y)) / 2) + menuTitle.Height * GlobalVar.ScaleSize.Y),
                        new Vector2((GlobalVar.ScreenSize.X / 2) - ((menuTitle.Width * GlobalVar.ScaleSize.X) / 2), ((GlobalVar.ScreenSize.Y / 2) - (((menuTitle.Height + menuButtonBorder6.Height) * GlobalVar.ScaleSize.Y)) / 2)),
                        GlobalVar.ScaleSize,
                        mainMenuColors,
                        Convert.ToBoolean(GlobalVar.OptionsArray[12]));
                    //Example for menu to gamemode
                    pausedLast = false;
                    if (mainMenu.menuNumberSelection() == 1)
                    {
                        menuState = "LevelSelect";
                    }
                    else if (mainMenu.menuNumberSelection() == 2)
                    {
                        menuState = "LoadProfileMenu";
                    }
                    else if (mainMenu.menuNumberSelection() == 3)
                        menuState = "OptionsMenu";
                    else if (mainMenu.menuNumberSelection() == 4)
                    {
                        GlobalVar.GameState = "Credits";
                    }
                    else if (mainMenu.menuNumberSelection() == 4)
                    {
                        //Put Help Screen Stuff Here
                    }
                    else if (mainMenu.menuNumberSelection() == 6)
                    {
                        GlobalVar.ExitGame = true;
                    }

                    break;
                #endregion
                #region Pause Menu Update
                case "PauseMenu":
                    pauseMenu.Update(gameTime,
                        mouseStateCurrent,
                        mouseStatePrevious,
                        GlobalVar.ScreenSize,
                        new Vector2(((GlobalVar.ScreenSize.X / 2) - (menuButtonBackground.Width * GlobalVar.ScaleSize.X) / 2), ((GlobalVar.ScreenSize.Y / 2) - (((pauseTitle.Height + menuButtonBorder3.Height) * GlobalVar.ScaleSize.Y)) / 2) + pauseTitle.Height * GlobalVar.ScaleSize.Y),
                        new Vector2((GlobalVar.ScreenSize.X / 2) - ((menuButtonBorder3.Width * GlobalVar.ScaleSize.X) / 2), ((GlobalVar.ScreenSize.Y / 2) - (((pauseTitle.Height + menuButtonBorder3.Height) * GlobalVar.ScaleSize.Y)) / 2) + pauseTitle.Height * GlobalVar.ScaleSize.Y),
                        new Vector2((GlobalVar.ScreenSize.X / 2) - ((pauseTitle.Width * GlobalVar.ScaleSize.X) / 2), ((GlobalVar.ScreenSize.Y / 2) - (((pauseTitle.Height + menuButtonBorder3.Height) * GlobalVar.ScaleSize.Y)) / 2)),
                        GlobalVar.ScaleSize,
                        pauseMenuColors,
                        Convert.ToBoolean(GlobalVar.OptionsArray[12]));
                    pausedLast = true;
                    if (pauseMenu.menuNumberSelection() == 1)
                    {
                        pauseMenu.setMenuHoverNumber(1);
                        GlobalVar.GameState = "Playing";
                    }
                    else if (pauseMenu.menuNumberSelection() == 2)
                        menuState = "OptionsMenu";
                    else if (pauseMenu.menuNumberSelection() == 3)
                        menuState = "MainMenu";
                    break;
                #endregion
                #region Options Menu Update
                case "OptionsMenu":
                    optionsMenu.Update(gameTime,
                        mouseStateCurrent,
                        mouseStatePrevious,
                        GlobalVar.ScreenSize,
                        new Vector2(((GlobalVar.ScreenSize.X / 2) - (menuButtonBackground.Width * GlobalVar.ScaleSize.X) / 2), ((GlobalVar.ScreenSize.Y / 2) - (((optionsTitle.Height + menuButtonBorder7.Height) * GlobalVar.ScaleSize.Y)) / 2) + optionsTitle.Height * GlobalVar.ScaleSize.Y),
                        new Vector2((GlobalVar.ScreenSize.X / 2) - ((menuButtonBorder7.Width * GlobalVar.ScaleSize.X) / 2), ((GlobalVar.ScreenSize.Y / 2) - (((optionsTitle.Height + menuButtonBorder7.Height) * GlobalVar.ScaleSize.Y)) / 2) + optionsTitle.Height * GlobalVar.ScaleSize.Y),
                        new Vector2((GlobalVar.ScreenSize.X / 2) - ((optionsTitle.Width * GlobalVar.ScaleSize.X) / 2), ((GlobalVar.ScreenSize.Y / 2) - (((optionsTitle.Height + menuButtonBorder7.Height) * GlobalVar.ScaleSize.Y)) / 2)),
                        GlobalVar.ScaleSize,
                        optionsMenuColors,
                        Convert.ToBoolean(GlobalVar.OptionsArray[12]));
                    //Example for menu to gamemode
                    if (optionsMenu.menuNumberSelection() == 1)
                        menuState = "OptionsResolutionMenu";
                    else if (optionsMenu.menuNumberSelection() == 2)
                        menuState = "OptionsKeybindingMenuPage1";
                    else if (optionsMenu.menuNumberSelection() == 3)
                        menuState = "debugMenu";
                    else if (optionsMenu.menuNumberSelection() == 4)
                    {
                        if (Convert.ToBoolean(GlobalVar.OptionsArray[12]) == false)
                        {
                            GlobalVar.OptionsArray[12] = "true";
                            optionsMenuButtonText[3] = "Sound: ON";
                        }
                        else
                        {
                            GlobalVar.OptionsArray[12] = "false";
                            optionsMenuButtonText[3] = "Sound: OFF";
                        }
                    }
                    else if (optionsMenu.menuNumberSelection() == 5)
                    {
                        if (Convert.ToBoolean(GlobalVar.OptionsArray[13]) == false)
                        {
                            GlobalVar.OptionsArray[13] = "true";
                            optionsMenuButtonText[4] = "Music: ON";
                        }
                        else
                        {
                            GlobalVar.OptionsArray[13] = "false";
                            optionsMenuButtonText[4] = "Music: OFF";
                        }
                    }
                    else if (optionsMenu.menuNumberSelection() == 6)
                    {
                        //Exit & Save (Write options file to the Global Values)
                        optionsHandler.writeOptions(GlobalVar.OptionsArray);
                        GlobalVar.ScreenSize = new Vector2(Convert.ToInt32(GlobalVar.OptionsArray[0]), Convert.ToInt32(GlobalVar.OptionsArray[1]));
                        windowSizeManager.SetScreenSize(new Vector2(GlobalVar.ScreenSize.X, GlobalVar.ScreenSize.Y), Convert.ToBoolean(GlobalVar.OptionsArray[14]));
                        if (pausedLast == true)
                            menuState = "PauseMenu";
                        else
                            menuState = "MainMenu";
                    }
                    else if (optionsMenu.menuNumberSelection() == 7)
                    {
                        //Exit W/ Saving (Set Global Values to the options Files)
                        GlobalVar.OptionsArray = optionsHandler.loadOptions();
                        GlobalVar.ScreenSize = new Vector2(Convert.ToInt32(GlobalVar.OptionsArray[0]), Convert.ToInt32(GlobalVar.OptionsArray[1]));
                        windowSizeManager.SetScreenSize(new Vector2(GlobalVar.ScreenSize.X, GlobalVar.ScreenSize.Y), Convert.ToBoolean(GlobalVar.OptionsArray[14]));
                        if (pausedLast == true)
                            menuState = "PauseMenu";
                        else
                            menuState = "MainMenu";
                    }
                    break;
                #endregion
                #region Options Resolution Menu Update
                case "OptionsResolutionMenu":
                    optionsResolutionMenu.Update(gameTime,
                        mouseStateCurrent,
                        mouseStatePrevious,
                        GlobalVar.ScreenSize,
                        new Vector2(((GlobalVar.ScreenSize.X / 2) - (menuButtonBackground.Width * GlobalVar.ScaleSize.X) / 2), ((GlobalVar.ScreenSize.Y / 2) - (((optionsTitle.Height + menuButtonBorder7.Height) * GlobalVar.ScaleSize.Y)) / 2) + optionsTitle.Height * GlobalVar.ScaleSize.Y),
                        new Vector2((GlobalVar.ScreenSize.X / 2) - ((menuButtonBorder7.Width * GlobalVar.ScaleSize.X) / 2), ((GlobalVar.ScreenSize.Y / 2) - (((optionsTitle.Height + menuButtonBorder7.Height) * GlobalVar.ScaleSize.Y)) / 2) + optionsTitle.Height * GlobalVar.ScaleSize.Y),
                        new Vector2((GlobalVar.ScreenSize.X / 2) - ((optionsTitle.Width * GlobalVar.ScaleSize.X) / 2), ((GlobalVar.ScreenSize.Y / 2) - (((optionsTitle.Height + menuButtonBorder7.Height) * GlobalVar.ScaleSize.Y)) / 2)),
                        GlobalVar.ScaleSize,
                        optionsResolutionMenuColors,
                        Convert.ToBoolean(GlobalVar.OptionsArray[12]));
                    //Example for menu to gamemode
                    if (optionsResolutionMenu.menuNumberSelection() == 1)
                    {
                        if (Convert.ToBoolean(GlobalVar.OptionsArray[14]) == false)
                        {
                            GlobalVar.OptionsArray[14] = "true";
                        }
                        else if (Convert.ToBoolean(GlobalVar.OptionsArray[14]) == true)
                        {
                            GlobalVar.OptionsArray[14] = "false";
                        }
                        windowSizeManager.SetScreenSize(new Vector2(GlobalVar.ScreenSize.X, GlobalVar.ScreenSize.Y), Convert.ToBoolean(GlobalVar.OptionsArray[14]));
                    }
                    else if (optionsResolutionMenu.menuNumberSelection() == 2)
                    {
                        GlobalVar.OptionsArray[0] = "800";
                        GlobalVar.OptionsArray[1] = "600";
                        GlobalVar.ScreenSize = new Vector2(Convert.ToInt32(GlobalVar.OptionsArray[0]), Convert.ToInt32(GlobalVar.OptionsArray[1]));
                        windowSizeManager.SetScreenSize(new Vector2(GlobalVar.ScreenSize.X, GlobalVar.ScreenSize.Y), Convert.ToBoolean(GlobalVar.OptionsArray[14]));
                    }
                    else if (optionsResolutionMenu.menuNumberSelection() == 3)
                    {
                        GlobalVar.OptionsArray[0] = "1280";
                        GlobalVar.OptionsArray[1] = "720";
                        GlobalVar.ScreenSize = new Vector2(Convert.ToInt32(GlobalVar.OptionsArray[0]), Convert.ToInt32(GlobalVar.OptionsArray[1]));
                        windowSizeManager.SetScreenSize(new Vector2(GlobalVar.ScreenSize.X, GlobalVar.ScreenSize.Y), Convert.ToBoolean(GlobalVar.OptionsArray[14]));
                    }
                    else if (optionsResolutionMenu.menuNumberSelection() == 4)
                    {
                        GlobalVar.OptionsArray[0] = "1366";
                        GlobalVar.OptionsArray[1] = "768";
                        GlobalVar.ScreenSize = new Vector2(Convert.ToInt32(GlobalVar.OptionsArray[0]), Convert.ToInt32(GlobalVar.OptionsArray[1]));
                        windowSizeManager.SetScreenSize(new Vector2(GlobalVar.ScreenSize.X, GlobalVar.ScreenSize.Y), Convert.ToBoolean(GlobalVar.OptionsArray[14]));
                    }
                    else if (optionsResolutionMenu.menuNumberSelection() == 5)
                    {
                        GlobalVar.OptionsArray[0] = "1600";
                        GlobalVar.OptionsArray[1] = "900";
                        GlobalVar.ScreenSize = new Vector2(Convert.ToInt32(GlobalVar.OptionsArray[0]), Convert.ToInt32(GlobalVar.OptionsArray[1]));
                        windowSizeManager.SetScreenSize(new Vector2(GlobalVar.ScreenSize.X, GlobalVar.ScreenSize.Y), Convert.ToBoolean(GlobalVar.OptionsArray[14]));
                    }
                    else if (optionsResolutionMenu.menuNumberSelection() == 6)
                    {
                        GlobalVar.OptionsArray[0] = "1920";
                        GlobalVar.OptionsArray[1] = "1080";
                        GlobalVar.ScreenSize = new Vector2(Convert.ToInt32(GlobalVar.OptionsArray[0]), Convert.ToInt32(GlobalVar.OptionsArray[1]));
                        windowSizeManager.SetScreenSize(new Vector2(GlobalVar.ScreenSize.X, GlobalVar.ScreenSize.Y), Convert.ToBoolean(GlobalVar.OptionsArray[14]));
                    }
                    else if (optionsResolutionMenu.menuNumberSelection() == 7)
                        menuState = "OptionsMenu";
                    break;
                #endregion
                #region Options Key Binding Menu Page1 Update
                case "OptionsKeybindingMenuPage1":
                    optionsKeybindingMenuPage1.Update(gameTime,
                        mouseStateCurrent,
                        mouseStatePrevious,
                        GlobalVar.ScreenSize,
                        new Vector2(((GlobalVar.ScreenSize.X / 2) - (menuButtonBackground.Width * GlobalVar.ScaleSize.X) / 2), ((GlobalVar.ScreenSize.Y / 2) - (((keybindPlayerTitle.Height + menuButtonBorder6.Height) * GlobalVar.ScaleSize.Y)) / 2) + keybindPlayerTitle.Height * GlobalVar.ScaleSize.Y),
                        new Vector2((GlobalVar.ScreenSize.X / 2) - ((menuButtonBorder6.Width * GlobalVar.ScaleSize.X) / 2), ((GlobalVar.ScreenSize.Y / 2) - (((keybindPlayerTitle.Height + menuButtonBorder6.Height) * GlobalVar.ScaleSize.Y)) / 2) + keybindPlayerTitle.Height * GlobalVar.ScaleSize.Y),
                        new Vector2((GlobalVar.ScreenSize.X / 2) - ((keybindPlayerTitle.Width * GlobalVar.ScaleSize.X) / 2), ((GlobalVar.ScreenSize.Y / 2) - (((keybindPlayerTitle.Height + menuButtonBorder6.Height) * GlobalVar.ScaleSize.Y)) / 2)),
                        GlobalVar.ScaleSize,
                        optionsKeybindingMenuColors,
                        Convert.ToBoolean(GlobalVar.OptionsArray[12]));
                    if (optionsKeybindingMenuPage1.menuNumberSelection() == 4)
                    {

                    }
                    if (optionsKeybindingMenuPage1.menuNumberSelection() == 5)
                        menuState = "OptionsKeybindingMenuPage2";
                    if (optionsKeybindingMenuPage1.menuNumberSelection() == 6)
                        menuState = "OptionsMenu";
                    break;
                #endregion
                #region Options Key Binding Menu Page2 Update
                case "OptionsKeybindingMenuPage2":
                    optionsKeybindingMenuPage2.Update(gameTime,
                        mouseStateCurrent,
                        mouseStatePrevious,
                        GlobalVar.ScreenSize,
                        new Vector2(((GlobalVar.ScreenSize.X / 2) - (menuButtonBackground.Width * GlobalVar.ScaleSize.X) / 2), ((GlobalVar.ScreenSize.Y / 2) - (((keybindBlockTitle.Height + menuButtonBorder6.Height) * GlobalVar.ScaleSize.Y)) / 2) + keybindBlockTitle.Height * GlobalVar.ScaleSize.Y),
                        new Vector2((GlobalVar.ScreenSize.X / 2) - ((menuButtonBorder6.Width * GlobalVar.ScaleSize.X) / 2), ((GlobalVar.ScreenSize.Y / 2) - (((keybindBlockTitle.Height + menuButtonBorder6.Height) * GlobalVar.ScaleSize.Y)) / 2) + keybindBlockTitle.Height * GlobalVar.ScaleSize.Y),
                        new Vector2((GlobalVar.ScreenSize.X / 2) - ((keybindBlockTitle.Width * GlobalVar.ScaleSize.X) / 2), ((GlobalVar.ScreenSize.Y / 2) - (((keybindBlockTitle.Height + menuButtonBorder6.Height) * GlobalVar.ScaleSize.Y)) / 2)),
                        GlobalVar.ScaleSize,
                        optionsKeybindingMenuColors,
                        Convert.ToBoolean(GlobalVar.OptionsArray[12]));
                    if (optionsKeybindingMenuPage2.menuNumberSelection() == 5)
                        menuState = "OptionsKeybindingMenuPage1";
                    if (optionsKeybindingMenuPage2.menuNumberSelection() == 6)
                        menuState = "OptionsMenu";
                    break;
                #endregion
                #region Debug Menu Update
                case "debugMenu":
                    debugMenu.Update(gameTime,
                        mouseStateCurrent,
                        mouseStatePrevious,
                        GlobalVar.ScreenSize,
                        new Vector2(((GlobalVar.ScreenSize.X / 2) - (menuButtonBackground.Width * GlobalVar.ScaleSize.X) / 2), ((GlobalVar.ScreenSize.Y / 2) - (((debugTitle.Height + menuButtonBorder4.Height) * GlobalVar.ScaleSize.Y)) / 2) + debugTitle.Height * GlobalVar.ScaleSize.Y),
                        new Vector2((GlobalVar.ScreenSize.X / 2) - ((menuButtonBorder4.Width * GlobalVar.ScaleSize.X) / 2), ((GlobalVar.ScreenSize.Y / 2) - (((debugTitle.Height + menuButtonBorder4.Height) * GlobalVar.ScaleSize.Y)) / 2) + debugTitle.Height * GlobalVar.ScaleSize.Y),
                        new Vector2((GlobalVar.ScreenSize.X / 2) - ((debugTitle.Width * GlobalVar.ScaleSize.X) / 2), ((GlobalVar.ScreenSize.Y / 2) - (((debugTitle.Height + menuButtonBorder4.Height) * GlobalVar.ScaleSize.Y)) / 2)),
                        GlobalVar.ScaleSize,
                        debugMenuColors,
                        Convert.ToBoolean(GlobalVar.OptionsArray[12]));
                    if (debugMenu.menuNumberSelection() == 1)
                    {
                        if (Convert.ToBoolean(GlobalVar.OptionsArray[9]) == false)
                        {
                            GlobalVar.OptionsArray[9] = "true";
                            debugMenuButtonText[0] = "Cursor: Hardware";
                        }
                        else
                        {
                            GlobalVar.OptionsArray[9] = "false";
                            debugMenuButtonText[0] = "Cursor: Game";
                        }
                    }
                    else if (debugMenu.menuNumberSelection() == 2)
                    {
                        if (Convert.ToBoolean(GlobalVar.OptionsArray[10]) == false)
                        {
                            GlobalVar.OptionsArray[10] = "true";
                            debugMenuButtonText[1] = "FPS: On";
                        }
                        else
                        {
                            GlobalVar.OptionsArray[10] = "false";
                            debugMenuButtonText[1] = "FPS: Off";
                        }
                    }
                    else if (debugMenu.menuNumberSelection() == 3)
                    {
                        if (Convert.ToBoolean(GlobalVar.OptionsArray[11]) == false)
                        {
                            GlobalVar.OptionsArray[11] = "true";
                            debugMenuButtonText[2] = "Debug Info: On";

                        }
                        else
                        {
                            GlobalVar.OptionsArray[11] = "false";
                            debugMenuButtonText[2] = "Debug Info: Off";
                        }
                    }
                    else if (debugMenu.menuNumberSelection() == 4)
                        menuState = "OptionsMenu";
                    break;
                #endregion
                #region Load Profile Menu Update
                case "LoadProfileMenu":
                    if (loadProfileFirstTimeStartUp)
                        loadProfileMenuButtonText[6] = "Quit";
                    else
                        loadProfileMenuButtonText[6] = "Back";

                    loadProfileMenu.Update(gameTime,
                         mouseStateCurrent,
                         mouseStatePrevious,
                         GlobalVar.ScreenSize,
                         new Vector2(((GlobalVar.ScreenSize.X / 2) - (menuButtonBackground.Width * GlobalVar.ScaleSize.X) / 2), ((GlobalVar.ScreenSize.Y / 2) - (((menuTitle.Height + menuButtonBorder7.Height) * GlobalVar.ScaleSize.Y)) / 2) + menuTitle.Height * GlobalVar.ScaleSize.Y),
                         new Vector2((GlobalVar.ScreenSize.X / 2) - ((menuButtonBorder7.Width * GlobalVar.ScaleSize.X) / 2), ((GlobalVar.ScreenSize.Y / 2) - (((menuTitle.Height + menuButtonBorder7.Height) * GlobalVar.ScaleSize.Y)) / 2) + menuTitle.Height * GlobalVar.ScaleSize.Y),
                         new Vector2((GlobalVar.ScreenSize.X / 2) - ((menuTitle.Width * GlobalVar.ScaleSize.X) / 2), ((GlobalVar.ScreenSize.Y / 2) - (((menuTitle.Height + menuButtonBorder7.Height) * GlobalVar.ScaleSize.Y)) / 2)),
                         GlobalVar.ScaleSize,
                         loadProfileMenuColors,
                         Convert.ToBoolean(GlobalVar.OptionsArray[12]));

                    if (loadProfileMenu.menuNumberSelection() == 7)
                    {
                        if (!loadProfileFirstTimeStartUp)
                            menuState = "MainMenu";
                        else
                            GlobalVar.ExitGame = true;
                    }
                    else if (loadProfileMenu.menuNumberSelection() != 0)
                    {
                        GlobalVar.PlayerProfile = loadProfileMenu.menuNumberSelection();
                        if (saveHandler.loadData(GlobalVar.PlayerProfile) != null)
                        {
                            GlobalVar.HighestLevel = Int32.Parse(saveHandler.loadData(GlobalVar.PlayerProfile)[0]);
                            GlobalVar.HighScore[0] = Int32.Parse(saveHandler.loadData(GlobalVar.PlayerProfile)[1]);
                            GlobalVar.HighScore[1] = Int32.Parse(saveHandler.loadData(GlobalVar.PlayerProfile)[2]);
                            GlobalVar.HighScore[2] = Int32.Parse(saveHandler.loadData(GlobalVar.PlayerProfile)[3]);
                            GlobalVar.HighScore[3] = Int32.Parse(saveHandler.loadData(GlobalVar.PlayerProfile)[4]);
                            GlobalVar.HighScore[4] = Int32.Parse(saveHandler.loadData(GlobalVar.PlayerProfile)[5]);
                        }
                        if (loadProfileFirstTimeStartUp)
                            menuState = "MainMenu";
                    }

                    break;
                #endregion
                #region Game Lose Menu Update
                case "GameLoseMenu":
                    gameLoseMenu.Update(gameTime,
                        mouseStateCurrent,
                        mouseStatePrevious,
                        GlobalVar.ScreenSize,
                        new Vector2(((GlobalVar.ScreenSize.X / 2) - (menuButtonBackground.Width * GlobalVar.ScaleSize.X) / 2), ((GlobalVar.ScreenSize.Y / 2) - (((gameLoseTitle.Height + menuButtonBorder2.Height) * GlobalVar.ScaleSize.Y)) / 2) + gameLoseTitle.Height * GlobalVar.ScaleSize.Y),
                        new Vector2((GlobalVar.ScreenSize.X / 2) - ((menuButtonBorder2.Width * GlobalVar.ScaleSize.X) / 2), ((GlobalVar.ScreenSize.Y / 2) - (((gameLoseTitle.Height + menuButtonBorder2.Height) * GlobalVar.ScaleSize.Y)) / 2) + gameLoseTitle.Height * GlobalVar.ScaleSize.Y),
                        new Vector2((GlobalVar.ScreenSize.X / 2) - ((gameLoseTitle.Width * GlobalVar.ScaleSize.X) / 2), ((GlobalVar.ScreenSize.Y / 2) - (((gameLoseTitle.Height + menuButtonBorder2.Height) * GlobalVar.ScaleSize.Y)) / 2)),
                        GlobalVar.ScaleSize,
                        mainMenuColors,
                        Convert.ToBoolean(GlobalVar.OptionsArray[12]));
                    if (gameLoseMenu.menuNumberSelection() != 0)
                    {
                        if (!GlobalVar.CustomLevel)
                        {
                            if (GlobalVar.Score > GlobalVar.HighScore[Int32.Parse(GlobalVar.CurrentLevel) - 1])
                            {
                                GlobalVar.HighScore[Int32.Parse(GlobalVar.CurrentLevel) - 1] = GlobalVar.Score;
                            }
                            saveHandler.saveData(new String[] { GlobalVar.HighestLevel.ToString(), GlobalVar.HighScore[0].ToString(), GlobalVar.HighScore[1].ToString(), GlobalVar.HighScore[2].ToString(), GlobalVar.HighScore[3].ToString(), GlobalVar.HighScore[4].ToString() }, GlobalVar.PlayerProfile);
                        }
                    }
                    if (gameLoseMenu.menuNumberSelection() == 1)
                    {
                        GlobalVar.Score = 0;
                        GlobalVar.ResetGameField = true;
                        GlobalVar.GameState = "Playing";
                    }
                    else if (gameLoseMenu.menuNumberSelection() == 2)
                    {
                        menuState = "MainMenu";
                    }
                    break;
                #endregion
                #region Game Win Menu Update
                case "GameWinMenu":
                    gameWinMenu.Update(gameTime,
                        mouseStateCurrent,
                        mouseStatePrevious,
                        GlobalVar.ScreenSize,
                        new Vector2(((GlobalVar.ScreenSize.X / 2) - (menuButtonBackground.Width * GlobalVar.ScaleSize.X) / 2), ((GlobalVar.ScreenSize.Y / 2) - (((gameWinTitle.Height + menuButtonBorder2.Height) * GlobalVar.ScaleSize.Y)) / 2) + gameWinTitle.Height * GlobalVar.ScaleSize.Y),
                        new Vector2((GlobalVar.ScreenSize.X / 2) - ((menuButtonBorder2.Width * GlobalVar.ScaleSize.X) / 2), ((GlobalVar.ScreenSize.Y / 2) - (((gameWinTitle.Height + menuButtonBorder2.Height) * GlobalVar.ScaleSize.Y)) / 2) + gameWinTitle.Height * GlobalVar.ScaleSize.Y),
                        new Vector2((GlobalVar.ScreenSize.X / 2) - ((gameWinTitle.Width * GlobalVar.ScaleSize.X) / 2), ((GlobalVar.ScreenSize.Y / 2) - (((gameWinTitle.Height + menuButtonBorder2.Height) * GlobalVar.ScaleSize.Y)) / 2)),
                        GlobalVar.ScaleSize,
                        mainMenuColors,
                        Convert.ToBoolean(GlobalVar.OptionsArray[12]));
                    //Sets profile stats
                    if (gameWinMenu.menuNumberSelection() != 0)
                    {
                        if (!GlobalVar.CustomLevel)
                        {
                            if (GlobalVar.Score > GlobalVar.HighScore[Int32.Parse(GlobalVar.CurrentLevel) - 1])
                            {
                                GlobalVar.HighScore[Int32.Parse(GlobalVar.CurrentLevel) - 1] = GlobalVar.Score;
                            }
                            if (Int32.Parse(GlobalVar.CurrentLevel) + 1 > GlobalVar.HighestLevel)
                            {
                                GlobalVar.HighestLevel = Int32.Parse(GlobalVar.CurrentLevel) + 1;
                            }
                            saveHandler.saveData(new String[] { GlobalVar.HighestLevel.ToString(), GlobalVar.HighScore[0].ToString(), GlobalVar.HighScore[1].ToString(), GlobalVar.HighScore[2].ToString(), GlobalVar.HighScore[3].ToString(), GlobalVar.HighScore[4].ToString() }, GlobalVar.PlayerProfile);
                        }
                    }
                    //Does button presses
                    if (gameWinMenu.menuNumberSelection() == 1)
                    {
                        GlobalVar.Score = 0;
                        GlobalVar.ResetGameField = true;
                        GlobalVar.GameState = "Playing";
                    }
                    else if (gameWinMenu.menuNumberSelection() == 2)
                    {
                        menuState = "MainMenu";
                    }
                    break;
                #endregion
                #region Level Select Menu Update
                case "LevelSelect":
                    levelSelectMenu.Update(gameTime,
                        mouseStateCurrent,
                        mouseStatePrevious,
                        GlobalVar.ScreenSize,
                        new Vector2(((GlobalVar.ScreenSize.X / 2) - (menuButtonBackground.Width * GlobalVar.ScaleSize.X) / 2), ((GlobalVar.ScreenSize.Y / 2) - (((levelSelectMenuTitle.Height + menuButtonBorder7.Height) * GlobalVar.ScaleSize.Y)) / 2) + levelSelectMenuTitle.Height * GlobalVar.ScaleSize.Y),
                        new Vector2((GlobalVar.ScreenSize.X / 2) - ((menuButtonBorder7.Width * GlobalVar.ScaleSize.X) / 2), ((GlobalVar.ScreenSize.Y / 2) - (((levelSelectMenuTitle.Height + menuButtonBorder7.Height) * GlobalVar.ScaleSize.Y)) / 2) + levelSelectMenuTitle.Height * GlobalVar.ScaleSize.Y),
                        new Vector2((GlobalVar.ScreenSize.X / 2) - ((levelSelectMenuTitle.Width * GlobalVar.ScaleSize.X) / 2), ((GlobalVar.ScreenSize.Y / 2) - (((levelSelectMenuTitle.Height + menuButtonBorder7.Height) * GlobalVar.ScaleSize.Y)) / 2)),
                        GlobalVar.ScaleSize,
                        levelSelectMenuColors,
                        Convert.ToBoolean(GlobalVar.OptionsArray[12]));

                    if (levelSelectMenu.menuNumberSelection() == 6)
                    {
                        menuState = "CustomLevelSelect";
                        customMenuPageMod = 0;
                    }
                    else if (levelSelectMenu.menuNumberSelection() == 7)
                    {
                        menuState = "MainMenu";
                        GlobalVar.GameState = "MenuScreen";
                    }
                    else if (levelSelectMenu.menuNumberSelection() != 0 && GlobalVar.HighestLevel >= levelSelectMenu.menuNumberSelection())
                    {
                        GlobalVar.CustomLevel = false;
                        GlobalVar.CurrentLevel =  levelSelectMenu.menuNumberSelection().ToString();
                        levels.setLevel("Level" + GlobalVar.CurrentLevel);
                        GlobalVar.ResetGameField = true;
                        GlobalVar.GameState = "Playing";
                    }
                    break;
                #endregion
                #region Custom Level Menu Update
                case "CustomLevelSelect":

                    for (int i = 0; i < 4; i++)
                    {
                        if ((i + (customMenuPageMod * 4)) < customLevelFileCount)
                            customLevelMenuText[i] = customLevelList[i + (customMenuPageMod * 4)];
                        else
                            customLevelMenuText[i] = "Blank";
                    }
                    if (customMenuPageMod > 0)
                        customLevelMenuText[4] = "Pg" + (customMenuPageMod) + "<-Previous";
                    else
                        customLevelMenuText[4] = " On First Page";

                    if (customMenuPageMod < (Math.Ceiling((double)fileAmount / 4f)) - 1)
                        customLevelMenuText[5] = "Next->Pg" + (customMenuPageMod + 2);
                    else
                        customLevelMenuText[5] = "On Last Page";

                    customLevelMenu.Update(gameTime,
                        mouseStateCurrent,
                        mouseStatePrevious,
                        GlobalVar.ScreenSize,
                        new Vector2(((GlobalVar.ScreenSize.X / 2) - (menuButtonBackground.Width * GlobalVar.ScaleSize.X) / 2), ((GlobalVar.ScreenSize.Y / 2) - (((customLevelMenuTitle.Height + menuButtonBorder7.Height) * GlobalVar.ScaleSize.Y)) / 2) + customLevelMenuTitle.Height * GlobalVar.ScaleSize.Y),
                        new Vector2((GlobalVar.ScreenSize.X / 2) - ((menuButtonBorder7.Width * GlobalVar.ScaleSize.X) / 2), ((GlobalVar.ScreenSize.Y / 2) - (((customLevelMenuTitle.Height + menuButtonBorder7.Height) * GlobalVar.ScaleSize.Y)) / 2) + customLevelMenuTitle.Height * GlobalVar.ScaleSize.Y),
                        new Vector2((GlobalVar.ScreenSize.X / 2) - ((customLevelMenuTitle.Width * GlobalVar.ScaleSize.X) / 2), ((GlobalVar.ScreenSize.Y / 2) - (((customLevelMenuTitle.Height + menuButtonBorder7.Height) * GlobalVar.ScaleSize.Y)) / 2)),
                        GlobalVar.ScaleSize,
                        customLevelMenuColors,
                        Convert.ToBoolean(GlobalVar.OptionsArray[12]));

                    if (customLevelMenu.menuNumberSelection() == 5)
                    {
                        if (customMenuPageMod > 0)
                            customMenuPageMod--;
                    }
                    else if (customLevelMenu.menuNumberSelection() == 6)
                    {
                        if (customMenuPageMod < (Math.Ceiling((double)fileAmount / 4f)) - 1)
                            customMenuPageMod++;
                    }
                    else if (customLevelMenu.menuNumberSelection() == 7)
                    {
                        menuState = "LevelSelect";
                    }
                    else if (customLevelMenu.menuNumberSelection() != 0 && !(customLevelMenuText[customLevelMenu.menuNumberSelection() - 1].Equals("Blank")))
                    {
                        GlobalVar.CustomLevel = true;
                        GlobalVar.CurrentLevel = customLevelMenuText[customLevelMenu.menuNumberSelection() - 1];
                        levels.setLevel("CustomLevels/" + GlobalVar.CurrentLevel);
                        GlobalVar.ResetGameField = true;
                        GlobalVar.GameState = "Playing";
                    }
                    break;
            }
            #endregion
                #region Toggle Button Color Changer
            //Checks for color updates from toggles for menus

            if (Convert.ToBoolean(GlobalVar.OptionsArray[9]) == true)
                debugMenuColors[0] = Color.LimeGreen;
            else
                debugMenuColors[0] = Color.DimGray;
            if (Convert.ToBoolean(GlobalVar.OptionsArray[10]) == true)
                debugMenuColors[1] = Color.Gold;
            else
                debugMenuColors[1] = Color.DimGray;
            if (Convert.ToBoolean(GlobalVar.OptionsArray[11]) == true)
                debugMenuColors[2] = Color.Firebrick;
            else
                debugMenuColors[2] = Color.DimGray;
            //Terrible, but still as effective as a switch case...
            if (Convert.ToBoolean(GlobalVar.OptionsArray[14]) == true)
                optionsResolutionMenuColors[0] = Color.LimeGreen;
            else
                optionsResolutionMenuColors[0] = Color.DimGray;
            if (GlobalVar.ScreenSize.Y == 600)
                optionsResolutionMenuColors[1] = Color.LimeGreen;
            else
                optionsResolutionMenuColors[1] = Color.DimGray;
            if (GlobalVar.ScreenSize.Y == 720)
                optionsResolutionMenuColors[2] = Color.LimeGreen;
            else
                optionsResolutionMenuColors[2] = Color.DimGray;
            if (GlobalVar.ScreenSize.Y == 768)
                optionsResolutionMenuColors[3] = Color.LimeGreen;
            else
                optionsResolutionMenuColors[3] = Color.DimGray;
            if (GlobalVar.ScreenSize.Y == 900)
                optionsResolutionMenuColors[4] = Color.LimeGreen;
            else
                optionsResolutionMenuColors[4] = Color.DimGray;
            if (GlobalVar.ScreenSize.Y == 1080)
                optionsResolutionMenuColors[5] = Color.LimeGreen;
            else
                optionsResolutionMenuColors[5] = Color.DimGray;

            //Load Profile color
            if (!loadProfileFirstTimeStartUp)
            {
                for (int p = 0; p < loadProfileMenuColors.Length; p++)
                {
                    if (p == GlobalVar.PlayerProfile - 1)
                        loadProfileMenuColors[p] = Color.LimeGreen;
                    else
                        loadProfileMenuColors[p] = Color.DimGray;
                }
            }

            //Level Color
            for (int u = 0; u < levelSelectMenuText.Length; u++)
            {
                if (u < GlobalVar.HighestLevel - 1)
                    levelSelectMenuColors[u] = Color.LimeGreen;
                else if (u == GlobalVar.HighestLevel - 1)
                    levelSelectMenuColors[u] = Color.LightBlue;
                else
                    levelSelectMenuColors[u] = Color.DimGray;
            }

            //Checks if the menuState changed and if so set the menu hover back to 1
            if (previousMenuState != menuState)
            {
                mainMenu.setMenuHoverNumber(1);
                optionsKeybindingMenuPage1.setMenuHoverNumber(1);
                optionsResolutionMenu.setMenuHoverNumber(1);
                optionsMenu.setMenuHoverNumber(1);
                pauseMenu.setMenuHoverNumber(1);
                debugMenu.setMenuHoverNumber(1);
                loadProfileMenu.setMenuHoverNumber(1);
            }

            if (menuState != previousMenuState)
            {
                storedRealPreviousMenuState = previousMenuState;
            }

            previousMenuState = menuState;

            #endregion
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            //spriteBatch.Draw(menuBackground, backgroundMoving, null, Color.White, 0f, new Vector2(0, 0), GlobalVar.ScaleSize, SpriteEffects.None, .5f);

            //Handles Menu States
            switch (menuState)
            {
                case "MainMenu":
                    mainMenu.Draw(spriteBatch);
                    break;
                case "PauseMenu":
                    pauseMenu.Draw(spriteBatch);
                    break;
                case "OptionsMenu":
                    optionsMenu.Draw(spriteBatch);
                    break;
                case "OptionsResolutionMenu":
                    optionsResolutionMenu.Draw(spriteBatch);
                    break;
                case "OptionsKeybindingMenuPage1":
                    optionsKeybindingMenuPage1.Draw(spriteBatch);
                    break;
                case "OptionsKeybindingMenuPage2":
                    optionsKeybindingMenuPage2.Draw(spriteBatch);
                    break;
                case "debugMenu":
                    debugMenu.Draw(spriteBatch);
                    break;
                case "LoadProfileMenu":
                    loadProfileMenu.Draw(spriteBatch);
                    break;
                case "GameLoseMenu":
                    gameLoseMenu.Draw(spriteBatch);
                    break;
                case "GameWinMenu":
                    gameWinMenu.Draw(spriteBatch);
                    break;
                case "LevelSelect":
                    levelSelectMenu.Draw(spriteBatch);
                    break;
                case "CustomLevelSelect":
                    customLevelMenu.Draw(spriteBatch);
                    break;
            }

        }

        public void SetMenu(String menuState)
        {
            this.menuState = menuState;
        }

        public void SetGameOver(Boolean gameWin)
        {
            this.gameWin = gameWin;
        }

        public void SetKeybindingInfo(String[] keyBindingInfo)
        {
            //In the future send it back too?
            this.keyBindingInfo = keyBindingInfo;
        }

        public String getMenuState()
        {
            return menuState;
        }

        public String getPreviousMenuState()
        {
            return storedRealPreviousMenuState;
        }

        public void setLoadProfileFirstStartUp(Boolean state)
        {
            this.loadProfileFirstTimeStartUp = state;
        }

        public Boolean getLoadProfileFirstStartUp()
        {
            return loadProfileFirstTimeStartUp;
        }
    }
}
