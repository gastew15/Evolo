﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using StarByte.ui;
using StarByte.io;

namespace Evolo.GameClass
{
    class Menus
    {

        //Background TEST
        public float milliScecondsElapsedGameTime;
        public Vector2 backgroundMoving;
        public Boolean down = true;
        private Texture2D menuBackground;

        private MenuHandler mainMenu, optionsMenu, optionsResolutionMenu, optionsKeybindingMenuPage1, optionsKeybindingMenuPage2, pauseMenu, debugMenu, saveSlotMenu;
        private WindowSizeManager windowSizeManager;
        private SpriteFont font;
        private Boolean isFullScreen, pausedLast;
        private OptionsHandler optionsHandler;
        private SaveHandler saveHandler;

        private String menuState;
        //Playing, MenuScreen, GameOver

        //Temp string
        private String[] loadData = new string[]{""};

        //Variables
        private Texture2D menuButtonBackground, optionsTitle, menuButtonBorder7, menuButtonBorder6, menuButtonBorder4, menuIndicatorLight, menuTitle, pauseTitle, debugTitle, keybindBlockTitle, keybindPlayerTitle;
        private int mainMenuVerticalSpacing = 24;
        private Vector2 optionsCenterMenuSP, mainMenuSP, keybindingCenterMenuSP, pauseMenuSP, debugSP, saveSlotMenuSP;
        private String[] mainMenuButtonText;
        private String[] optionsMenuButtonText;
        private String[] optionsResolutionMenuButtonText;
        private String[] optionsKeybindingMenuPage1ButtonText;
        private String[] optionsKeybindingMenuPage2ButtonText;
        private String[] pauseMenuButtonText;
        private String[] keyBindingInfo;
        private String[] debugMenuButtonText;
        private String[] saveSlotMenuButtonText;
        private Color[] mainMenuColors;
        private Color[] optionsMenuColors;
        private Color[] optionsResolutionMenuColors;
        private Color[] optionsKeybindingMenuColors;
        private Color[] pauseMenuColors;
        private Color[] debugMenuColors;
        private Color[] saveSlotMenuColors;
        private GraphicsDeviceManager graphics;
        private String previousMenuState;
        private SoundEffect menuHoverChangeSoundEffect, menuClickedSoundEffect;

        //Game Over Variables
        private Texture2D gameoverScreen;
        private bool gameOver = false;
        private Vector2 gameoverPosition;

        public Menus(GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
        }

        public void Initialize(String[] keyBindingInfo)
        {
            


            optionsHandler = new OptionsHandler(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Evolo");
            //saveHandler = new SaveHandler(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Evolo\\Save.dat", 7);

            //2 pages to contain all of the key options, treat as seperate menus
            optionsKeybindingMenuPage1ButtonText = new String[6] { "PlayerLeft: " + "left", "PlayerRight: " + keyBindingInfo[1], "PlayerJump: " + keyBindingInfo[2], "Nothing At All" , "Next Page ->", "Back" };
            optionsKeybindingMenuPage2ButtonText = new String[6] {  "BlockLeft: " + keyBindingInfo[3], "BlockRight: " + keyBindingInfo[4], "BlockRotate: " + keyBindingInfo[5], "BlockDown: " + keyBindingInfo[6], "<- Previous Page", "Back" };

            optionsMenuButtonText = new String[7] { "Resolution", "Key Bindings", "Debug Options", "Sound: OFF", "Music: OFF", "Exit & Save", "Exit W/O Saving" };
            optionsResolutionMenuButtonText = new String[7] { "Full Screen", "800 x 600", "1280 x 720", "1366 x 768", "1600 x 900", "1920 x 1080", "Back" };
            mainMenuButtonText = new String[6] { "Start New Game", "Load Game", "Options","Credits","Help", "Quit" };
            pauseMenuButtonText = new String[4] { "Resume", "Save", "Options", "Exit" };
            debugMenuButtonText = new String[4] { "Cursor: Game", "FPS: Off", "Debug Info: Off", "Back"};
            saveSlotMenuButtonText = new String[8] { "Slot 1", "Slot 2", "Slot 3", "Slot 4", "Slot 5", "Slot 6", "Slot 7", "Back"};
            #region Menu Text Colours
            mainMenuColors = new Color[] { Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray };
            optionsMenuColors = new Color[] { Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray};
            optionsResolutionMenuColors = new Color[] { Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray };
            optionsKeybindingMenuColors = new Color[] { Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray };
            pauseMenuColors = new Color[] { Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray };
            debugMenuColors = new Color[] { Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray };
            saveSlotMenuColors = new Color[] { Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray, Color.DarkGray };
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
            menuButtonBorder4 = Content.Load<Texture2D>("Sprites and Pictures/ButtonBorder4");
            menuButtonBorder6 = Content.Load<Texture2D>("Sprites and Pictures/ButtonBorder6");
            menuButtonBorder7 = Content.Load<Texture2D>("Sprites and Pictures/ButtonBorder7");

            menuButtonBackground = Content.Load<Texture2D>("Sprites and Pictures/ButtonBackground");

            keybindBlockTitle = Content.Load<Texture2D>("Sprites and Pictures/Logo_KeybindBlock");
            keybindPlayerTitle = Content.Load<Texture2D>("Sprites and Pictures/Logo_KeybindPlayer");

            menuIndicatorLight = Content.Load<Texture2D>("Sprites and Pictures/menuIndicatorLight");

            debugTitle = Content.Load<Texture2D>("Sprites and Pictures/Logo_Debug");
            pauseTitle = Content.Load<Texture2D>("Sprites and Pictures/Logo_Pause");
            menuTitle = Content.Load<Texture2D>("Sprites and Pictures/Logo_MainMenu");
            optionsTitle = Content.Load<Texture2D>("Sprites and Pictures/Logo_options");

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
            saveSlotMenuSP = new Vector2((GlobalVar.ScreenSize.X / 2) - (menuButtonBackground.Width / 2), (GlobalVar.ScreenSize.Y / 2) - (((menuButtonBackground.Height * saveSlotMenuButtonText.Length) + (mainMenuVerticalSpacing * (saveSlotMenuButtonText.Length - 1))) / 2));
            #endregion
            #region Menu Handlers
            mainMenu = new MenuHandler(menuTitle, menuButtonBorder6, GlobalVar.ScaleSize, new Vector2((GlobalVar.ScreenSize.X / 2) - ((menuButtonBorder6.Width * GlobalVar.ScaleSize.X) / 2), 0), new Vector2((GlobalVar.ScreenSize.X / 2) - ((menuTitle.Width * GlobalVar.ScaleSize.X) / 2), 0), mainMenuVerticalSpacing, menuButtonBackground, mainMenuButtonText, font, menuHoverChangeSoundEffect, menuClickedSoundEffect, GlobalVar.ScreenSize);
            pauseMenu = new MenuHandler(pauseTitle, menuButtonBorder4, GlobalVar.ScaleSize, new Vector2((GlobalVar.ScreenSize.X / 2) - ((menuButtonBorder4.Width * GlobalVar.ScaleSize.X) / 2), 0), new Vector2((GlobalVar.ScreenSize.X / 2) - ((pauseTitle.Width * GlobalVar.ScaleSize.X) / 2), 0), mainMenuVerticalSpacing, menuButtonBackground, pauseMenuButtonText, font, menuHoverChangeSoundEffect, menuClickedSoundEffect, GlobalVar.ScreenSize);
            
            optionsMenu = new MenuHandler(optionsTitle, menuButtonBorder7, GlobalVar.ScaleSize, new Vector2((GlobalVar.ScreenSize.X / 2) - ((menuButtonBorder7.Width * GlobalVar.ScaleSize.X) / 2), 0), new Vector2((GlobalVar.ScreenSize.X / 2) - ((optionsTitle.Width * GlobalVar.ScaleSize.X) / 2), 0), mainMenuVerticalSpacing, menuButtonBackground, optionsMenuButtonText, font, menuHoverChangeSoundEffect, menuClickedSoundEffect, GlobalVar.ScreenSize);
            optionsResolutionMenu = new MenuHandler(optionsTitle, menuButtonBorder7, GlobalVar.ScaleSize, new Vector2((GlobalVar.ScreenSize.X / 2) - ((menuButtonBorder7.Width * GlobalVar.ScaleSize.X) / 2), 0), new Vector2((GlobalVar.ScreenSize.X / 2) - ((optionsTitle.Width * GlobalVar.ScaleSize.X) / 2), 0), mainMenuVerticalSpacing, menuButtonBackground, optionsResolutionMenuButtonText, font, menuHoverChangeSoundEffect, menuClickedSoundEffect, GlobalVar.ScreenSize);       
            optionsKeybindingMenuPage1 = new MenuHandler(keybindPlayerTitle, menuButtonBorder6, GlobalVar.ScaleSize, new Vector2((GlobalVar.ScreenSize.X / 2) - ((menuButtonBorder7.Width * GlobalVar.ScaleSize.X) / 2), 0), new Vector2((GlobalVar.ScreenSize.X / 2) - ((keybindPlayerTitle.Width * GlobalVar.ScaleSize.X) / 2), 0), mainMenuVerticalSpacing, menuButtonBackground, optionsKeybindingMenuPage1ButtonText, font, menuHoverChangeSoundEffect, menuClickedSoundEffect, GlobalVar.ScreenSize);
            optionsKeybindingMenuPage2 = new MenuHandler(keybindBlockTitle, menuButtonBorder6, GlobalVar.ScaleSize, new Vector2((GlobalVar.ScreenSize.X / 2) - ((menuButtonBorder7.Width * GlobalVar.ScaleSize.X) / 2), 0), new Vector2((GlobalVar.ScreenSize.X / 2) - ((keybindBlockTitle.Width * GlobalVar.ScaleSize.X) / 2), 0), mainMenuVerticalSpacing, menuButtonBackground, optionsKeybindingMenuPage2ButtonText, font, menuHoverChangeSoundEffect, menuClickedSoundEffect, GlobalVar.ScreenSize);

            debugMenu = new MenuHandler(debugTitle, menuButtonBorder4, GlobalVar.ScaleSize, new Vector2((GlobalVar.ScreenSize.X / 2) - ((menuButtonBorder4.Width * GlobalVar.ScaleSize.X) / 2), 0), new Vector2((GlobalVar.ScreenSize.X / 2) - ((debugTitle.Width * GlobalVar.ScaleSize.X) / 2), 0), mainMenuVerticalSpacing, menuButtonBackground, debugMenuButtonText, font, menuHoverChangeSoundEffect, menuClickedSoundEffect, GlobalVar.ScreenSize);
          
            //debugMenu = new MenuHandler(optionsTitle, debugSP, new Vector2(), mainMenuVerticalSpacing, menuButtonBackground, debugMenuButtonText, font, menuHoverChangeSoundEffect, menuClickedSoundEffect, GlobalVar.ScreenSize);
            saveSlotMenu = new MenuHandler(menuTitle, saveSlotMenuSP, new Vector2(), mainMenuVerticalSpacing, menuButtonBackground, saveSlotMenuButtonText, font, menuHoverChangeSoundEffect, menuClickedSoundEffect, GlobalVar.ScreenSize);

            if(Boolean.Parse(GlobalVar.OptionsArray[12]) == true)
                optionsMenuButtonText[3] = "Sound: ON";
            if(Boolean.Parse(GlobalVar.OptionsArray[13]) == true)
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
      
            if (down == true)
            {
                /*if (backgroundMoving.Y > -2880)
                {
                    backgroundMoving.Y -= ((0.01f) * ((backgroundMoving.Y + 2881)/2));
                }*/
                if (backgroundMoving.Y > -2880)
                {
                    backgroundMoving.Y -= (0.5f);
                }
                else
                    down = false;
            }
            if (down == false)
            {
                if (backgroundMoving.Y < 0)
                {
                    backgroundMoving.Y += (0.5f);
                }
                else
                    down = true;
            }

            //Handles Menu States
            switch (menuState)
            {
                #region Main Menu Update
                case "MainMenu":
                    mainMenu.Update(gameTime, 
                        mouseStateCurrent, 
                        mouseStatePrevious, 
                        GlobalVar.ScreenSize, 
                        new Vector2(((GlobalVar.ScreenSize.X / 2) - (menuButtonBackground.Width * GlobalVar.ScaleSize.X) /2), ((GlobalVar.ScreenSize.Y / 2) - (((menuTitle.Height + menuButtonBorder6.Height) * GlobalVar.ScaleSize.Y)) / 2) + menuTitle.Height * GlobalVar.ScaleSize.Y), 
                        new Vector2((GlobalVar.ScreenSize.X / 2) - ((menuButtonBorder6.Width * GlobalVar.ScaleSize.X) / 2), ((GlobalVar.ScreenSize.Y / 2) - (((menuTitle.Height + menuButtonBorder6.Height) * GlobalVar.ScaleSize.Y)) / 2) + menuTitle.Height * GlobalVar.ScaleSize.Y),
                        new Vector2((GlobalVar.ScreenSize.X / 2) - ((menuTitle.Width * GlobalVar.ScaleSize.X) / 2), ((GlobalVar.ScreenSize.Y / 2) - (((menuTitle.Height + menuButtonBorder6.Height) * GlobalVar.ScaleSize.Y)) / 2)),
                        GlobalVar.ScaleSize, 
                        mainMenuColors, 
                        Convert.ToBoolean(GlobalVar.OptionsArray[12]));
                    //Example for menu to gamemode
                    pausedLast = false;
                    if (mainMenu.menuNumberSelection() == 1)
                    {
                        GlobalVar.GameState = "Playing";
                    }
                    else if (mainMenu.menuNumberSelection() == 2)
                    {
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
                        new Vector2(((GlobalVar.ScreenSize.X / 2) - (menuButtonBackground.Width * GlobalVar.ScaleSize.X) / 2), ((GlobalVar.ScreenSize.Y / 2) - (((pauseTitle.Height + menuButtonBorder4.Height) * GlobalVar.ScaleSize.Y)) / 2) + pauseTitle.Height * GlobalVar.ScaleSize.Y),
                        new Vector2((GlobalVar.ScreenSize.X / 2) - ((menuButtonBorder4.Width * GlobalVar.ScaleSize.X) / 2), ((GlobalVar.ScreenSize.Y / 2) - (((pauseTitle.Height + menuButtonBorder4.Height) * GlobalVar.ScaleSize.Y)) / 2) + pauseTitle.Height * GlobalVar.ScaleSize.Y),
                        new Vector2((GlobalVar.ScreenSize.X / 2) - ((pauseTitle.Width * GlobalVar.ScaleSize.X) / 2), ((GlobalVar.ScreenSize.Y / 2) - (((pauseTitle.Height + menuButtonBorder4.Height) * GlobalVar.ScaleSize.Y)) / 2)),
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
                    {
                        menuState = "SaveSlotMenu";
                    }
                    else if (pauseMenu.menuNumberSelection() == 3)
                        menuState = "OptionsMenu";
                    else if (pauseMenu.menuNumberSelection() == 4)
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
                        windowSizeManager.SetScreenSize(new Vector2(GlobalVar.ScreenSize.X, GlobalVar.ScreenSize.Y), this.isFullScreen);
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
                        windowSizeManager.SetScreenSize(new Vector2(GlobalVar.ScreenSize.X, GlobalVar.ScreenSize.Y), this.isFullScreen);
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
                        /*
                        if (graphics.IsFullScreen == false)
                        {
                            this.isFullScreen = true;
                            GlobalVar.ScreenSize = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
                            windowSizeManager.SetScreenSize(new Vector2(GlobalVar.ScreenSize.X, GlobalVar.ScreenSize.Y), true);
                        }
                        else if (graphics.IsFullScreen == true)
                        {
                            this.isFullScreen = false;
                            //Read Options file for the last set resolution, for now it defualts to 800 x 600
                            GlobalVar.ScreenSize = new Vector2(800, 600);
                            windowSizeManager.SetScreenSize(new Vector2(GlobalVar.ScreenSize.X, GlobalVar.ScreenSize.Y), true);
                            windowSizeManager.SetScreenSize(new Vector2(GlobalVar.ScreenSize.X, GlobalVar.ScreenSize.Y), false);
                        }
                        */
                    }
                    else if (optionsResolutionMenu.menuNumberSelection() == 2)
                    {

                        if (!(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width < 800) && !(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height < 600))
                        {
                            GlobalVar.OptionsArray[0] = "800";
                            GlobalVar.OptionsArray[1] = "600";
                            GlobalVar.ScreenSize = new Vector2(Convert.ToInt32(GlobalVar.OptionsArray[0]), Convert.ToInt32(GlobalVar.OptionsArray[1]));
                            windowSizeManager.SetScreenSize(new Vector2(GlobalVar.ScreenSize.X, GlobalVar.ScreenSize.Y), this.isFullScreen);
                        }
                    }
                    else if (optionsResolutionMenu.menuNumberSelection() == 3)
                    {
                        if (!(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width < 1080) && !(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height < 720))
                        {
                            GlobalVar.OptionsArray[0] = "1280";
                            GlobalVar.OptionsArray[1] = "720";
                            GlobalVar.ScreenSize = new Vector2(Convert.ToInt32(GlobalVar.OptionsArray[0]), Convert.ToInt32(GlobalVar.OptionsArray[1]));
                            windowSizeManager.SetScreenSize(new Vector2(GlobalVar.ScreenSize.X, GlobalVar.ScreenSize.Y), this.isFullScreen);
                        }
                    }
                    else if (optionsResolutionMenu.menuNumberSelection() == 4)
                    {
                        if (!(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width < 1366) && !(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height < 768))
                        {
                            GlobalVar.OptionsArray[0] = "1366";
                            GlobalVar.OptionsArray[1] = "768";
                            GlobalVar.ScreenSize = new Vector2(Convert.ToInt32(GlobalVar.OptionsArray[0]), Convert.ToInt32(GlobalVar.OptionsArray[1]));
                            windowSizeManager.SetScreenSize(new Vector2(GlobalVar.ScreenSize.X, GlobalVar.ScreenSize.Y), this.isFullScreen);
                        }
                    }
                    else if (optionsResolutionMenu.menuNumberSelection() == 5)
                    {
                        if (!(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width < 1600) && !(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height < 900))
                        {
                            GlobalVar.OptionsArray[0] = "1600";
                            GlobalVar.OptionsArray[1] = "900";
                            GlobalVar.ScreenSize = new Vector2(Convert.ToInt32(GlobalVar.OptionsArray[0]), Convert.ToInt32(GlobalVar.OptionsArray[1]));
                            windowSizeManager.SetScreenSize(new Vector2(GlobalVar.ScreenSize.X, GlobalVar.ScreenSize.Y), this.isFullScreen);
                        }
                    }
                    else if (optionsResolutionMenu.menuNumberSelection() == 6)
                    {
                        if (!(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width < 1920) && !(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height < 1080))
                        {
                            GlobalVar.OptionsArray[0] = "1920";
                            GlobalVar.OptionsArray[1] = "1080";
                            GlobalVar.ScreenSize = new Vector2(Convert.ToInt32(GlobalVar.OptionsArray[0]), Convert.ToInt32(GlobalVar.OptionsArray[1]));
                            windowSizeManager.SetScreenSize(new Vector2(GlobalVar.ScreenSize.X, GlobalVar.ScreenSize.Y), this.isFullScreen);
                        }
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
                #region Save Slot Menu Update
                case "SaveSlotMenu":
                    saveSlotMenu.Update(gameTime, mouseStateCurrent, mouseStatePrevious, GlobalVar.ScreenSize, new Vector2((GlobalVar.ScreenSize.X / 2) - ((menuButtonBackground.Width * GlobalVar.ScaleSize.X) / 2), (GlobalVar.ScreenSize.Y / 2) - ((((menuButtonBackground.Height * GlobalVar.ScaleSize.Y) * saveSlotMenuButtonText.Length) + ((mainMenuVerticalSpacing * GlobalVar.ScaleSize.Y) * (saveSlotMenuButtonText.Length - 1))) / 2)),  new Vector2((GlobalVar.ScreenSize.X / 2) - ((menuTitle.Width * GlobalVar.ScaleSize.X) / 2), 0), GlobalVar.ScaleSize, saveSlotMenuColors, Convert.ToBoolean(GlobalVar.OptionsArray[12]));
                    if (saveSlotMenu.menuNumberSelection() == 1)
                    {
                        //saveHandler.SaveData(new String[] { "Test 1", "Test 2", "Test 3" }, 3);
                        //saveHandler.SaveDataTemp(new String[] { "Test 1", "Test 2", "Test 3" });
                    }
                    else if (saveSlotMenu.menuNumberSelection() == 2)
                    {
                        //loadData = saveHandler.LoadData(3);
                        //loadData = saveHandler.LoadDataTemp();
                    }
                    else if (saveSlotMenu.menuNumberSelection() == 8)
                    {
                        menuState = "PauseMenu";
                    }
                    break;
                #endregion
            }
            #region Toggle Button Color Changer
            //Checks for color updates from toggles for menus

            //Colors for Sound and Music Toggle
            /*
            if (Convert.ToBoolean(GlobalVar.OptionsArray[10]) == true)
                optionsMenuColors[3] = Color.DarkOrchid;
            else
                optionsMenuColors[3] = Color.DimGray;
            if (Convert.ToBoolean(GlobalVar.OptionsArray[11]) == true)
                optionsMenuColors[4] = Color.DarkTurquoise;
            else
                optionsMenuColors[4] = Color.DimGray;
             */
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
            if (GlobalVar.ScreenSize.Y == 1200)
                optionsResolutionMenuColors[5] = Color.LimeGreen;
            else
                optionsResolutionMenuColors[5] = Color.DimGray;
            if (GlobalVar.ScreenSize.Y == 1080)
                optionsResolutionMenuColors[6] = Color.LimeGreen;
            else
                optionsResolutionMenuColors[6] = Color.DimGray;

            //Checks if the menuState changed and if so set the menu hover back to 1
            if (previousMenuState != menuState)
            {
                mainMenu.setMenuHoverNumber(1);
                optionsKeybindingMenuPage1.setMenuHoverNumber(1);
                optionsResolutionMenu.setMenuHoverNumber(1);
                optionsMenu.setMenuHoverNumber(1);
                pauseMenu.setMenuHoverNumber(1);
                debugMenu.setMenuHoverNumber(1);
                saveSlotMenu.setMenuHoverNumber(1);
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
                case "SaveSlotMenu":
                    saveSlotMenu.Draw(spriteBatch);

                    for (int j = 0; j < loadData.Length; j++)
                    {
                        spriteBatch.DrawString(font, loadData[j], new Vector2(50, 250 + (30 * j)), Color.White);
                    }

                    break;
            }

        }

        public void SetMenu(String menuState)
        {
            this.menuState = menuState;
        }

        public String GetMenu()
        {
            return menuState;
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
    }
}
