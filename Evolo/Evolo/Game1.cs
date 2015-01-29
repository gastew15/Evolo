using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System.Runtime.InteropServices;
using StarByte.io;
using StarByte.misc;
using StarByte.graphics;
using Evolo.GameClass;

namespace Evolo
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //SUPER IMPORTANT!!! DISABLE FOR ANY RELEASE VERSIONS!!
        private Boolean devErrorLoggingDisable = false;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern uint MessageBox(IntPtr hWnd, String text, String caption, uint type);

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont SeqoeUIMonoNormal, MenuFont;
        FPSManager fpsManager;
        OptionsHandler optionsHandler;

        //TEMP CLASSES
        FieldManager fieldManager;
        Background background;
        SplashScreenManager splashScreen;
        //TEMP VARIABLES
        Texture2D[] splashScreenImages;
        float[] splashScreenWaitTime;
        float orginalSplashScreenStartTime;
        float milliScecondsElapsedGameTime;

        //TEMP Save location, change over to user specific at a later data
        ErrorHandler errorHandler = new ErrorHandler(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Evolo");// "C:\\Users\\Public\\Saved Games", "Evolo");
        MouseState mouseStateCurrent, mouseStatePrevious;
        Menus menus;
        Credits credits;
        KeyboardState keybState;
        GameTime gameTime;

        String gameState;
        String holdingPreviousGameState;
        //Playing, MenuScreen, GameOver, Credits

        //Variables

        //Game
        String version = "Build V: 1.1.8.2"; // Major, Minor, Build, Revision #
        Boolean mainMenuTripped = false; //Boolean to chekc to see if the menus are active
        const int defualtWidth = 1280, defualtHeight = 720;

        //Keys
        string[] keyBindingInfo = new string[7];
        Boolean isPressedEsc;
        Boolean isPressedRightBtn;
        Texture2D gameMouseTexture;
        //Sounds
        //Song mainMenuMusic;
        Boolean songTripped;

        //Credits
        String[] creditsStringArray = new String[10] { "CREDITS", "Team", "G. Stewart - StarByte Designer & Lead Programer", "K. Jones - Main Artist & Generation Programer", "D. Jones - Supporting Programer & Artist", "J. Estrada - Programer & Tester", "A Special Thanks To", "D. Ely & The CACC", "AND", "To You The Player!" };

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IsFixedTimeStep = true;

            errorHandler.WriteError(1, 7, "Application Launch! ");
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            try
            {
                this.IsMouseVisible = true;
                var mouseState = Mouse.GetState();
                var mousePosition = new Point(mouseState.X, mouseState.Y);
                optionsHandler = new OptionsHandler(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Evolo");

                gameTime = new GameTime();
                //Loads and sets option values
                GlobalVar.OptionsArray = optionsHandler.loadOptions();

                //Back Buffer Informaation
                GlobalVar.ScreenSize = new Vector2(Convert.ToInt32(GlobalVar.OptionsArray[0]), Convert.ToInt32(GlobalVar.OptionsArray[1]));
                graphics.PreferredBackBufferWidth = (int)GlobalVar.ScreenSize.X;
                graphics.PreferredBackBufferHeight = (int)GlobalVar.ScreenSize.Y;
                graphics.IsFullScreen = Convert.ToBoolean(GlobalVar.OptionsArray[14]);
                graphics.ApplyChanges();

                //Sets Keybinding Info from options Load
                for (int i = 0; i < keyBindingInfo.Length; i++)
                {
                    keyBindingInfo[i] = "WIP";//GlobalVar.OptionsArray[i + 2];
                }

                GlobalVar.ScaleSize = new Vector2(GlobalVar.ScreenSize.X / defualtWidth, GlobalVar.ScreenSize.Y / defualtHeight);
                GlobalVar.GameState = "SplashScreen";
                //Load up classes we need
                fpsManager = new FPSManager();
                menus = new Menus(graphics);

                //TEMP Initilize
                background = new Background();
                fieldManager = new FieldManager();
                //Change to real values later
                splashScreenWaitTime = new float[3] { 2.0f, 2.0f, 4.0f };
                fieldManager.Initialize();
                background.Initialize();

                menus.Initialize(keyBindingInfo);
                base.Initialize();
            }
            catch (Exception e)
            {
                if (devErrorLoggingDisable != true)
                    errorHandler.WriteError(3, 123, "Initalize Failure! " + e);
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            try
            {
                // Create a new SpriteBatch, which can be used to draw textures.
                spriteBatch = new SpriteBatch(GraphicsDevice);

                //Content Loads
                //SeqoeUIMonoNormal = Content.Load<SpriteFont>("Fonts/SeqoeUIMonoNormal");
                SeqoeUIMonoNormal = Content.Load<SpriteFont>("Fonts/font");
                MenuFont = Content.Load<SpriteFont>("Fonts/MenuFont");
                //mainMenuMusic = Content.Load<Song>("Sounds/Music/spacesong2-8");

                menus.LoadContent(this.Content, MenuFont);
                menus.setLoadProfileFirstStartUp(true);
                menus.SetMenu("LoadProfileMenu");

                //TEMP LOAD CONTENT
                background.LoadContent(this.Content);
                fieldManager.LoadContent(this.Content);
                splashScreenImages = new Texture2D[3];
                splashScreenImages[0] = Content.Load<Texture2D>("Sprites and pictures/CognativeThought");
                splashScreenImages[1] = Content.Load<Texture2D>("Sprites and pictures/TwistedTransistors");
                splashScreenImages[2] = Content.Load<Texture2D>("Sprites and pictures/StarbyteLogo");

                gameMouseTexture = Content.Load<Texture2D>("Sprites and pictures/MouseWhite");

                credits = new Credits(creditsStringArray, SeqoeUIMonoNormal);

                //TEMP FINAL Initilize
                orginalSplashScreenStartTime = gameTime.ElapsedGameTime.Seconds;
                splashScreen = new SplashScreenManager(splashScreenImages, splashScreenWaitTime);
            }
            catch (Exception e)
            {
                if (devErrorLoggingDisable != true)
                    errorHandler.WriteError(3, 124, "LoadContent Failure! " + e);
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            try
            {

                mouseStateCurrent = Mouse.GetState();
                keybState = Keyboard.GetState();

                //GameState checking
                gameState = GlobalVar.GameState;

                milliScecondsElapsedGameTime += gameTime.ElapsedGameTime.Milliseconds; //(float)TimeSpan.FromMilliseconds(1.0).Milliseconds;//+=  

                //Handles Game States
                switch (gameState)
                {
                    case "MenuScreen":
                        if (menus.getMenuState() == "MainMenu")
                        {
                            //Reset Values
                            if (!mainMenuTripped)
                            {
                                GlobalVar.Score = 0;
                                fieldManager.resetGameVariables();
                                mainMenuTripped = true;
                                menus.setLoadProfileFirstStartUp(false);
                            }
                            GlobalVar.PreviousGameState = "MainMenu";
                            credits.resetCreditRoll();
                        }
                        if (songTripped == false)
                        {
                            //MediaPlayer.Resume(mainMenuMusic, 
                            songTripped = true;
                        }
                        menus.Update(gameTime, mouseStateCurrent, mouseStatePrevious, milliScecondsElapsedGameTime);
                        background.Update(gameTime, milliScecondsElapsedGameTime);
                        break;

                    case "Playing":
                        //MediaPlayer.Pause(mainMenuMusic);
                        fieldManager.Update(gameTime);
                        background.Update(gameTime, milliScecondsElapsedGameTime);
                        mainMenuTripped = false;
                        break;

                    case "Credits":
                        background.Update(gameTime, milliScecondsElapsedGameTime);
                        //MediaPlayer.Pause(mainMenuMusic);
                        break;

                    case "SplashScreen":
                        splashScreen.Update(milliScecondsElapsedGameTime, orginalSplashScreenStartTime);
                        if (splashScreen.getSplashScreenOver() == true)
                            GlobalVar.GameState = "MenuScreen";
                        background.Update(gameTime, milliScecondsElapsedGameTime);
                        break;
                    case "GameOver":
                        menus.Update(gameTime, mouseStateCurrent, mouseStatePrevious, milliScecondsElapsedGameTime);
                        fieldManager.resetGameVariables();
                        
                        if (fieldManager.getGameWin() == true)
                        {
                            menus.SetMenu("GameWinMenu");
                        }
                        else
                        {
                            menus.SetMenu("GameLoseMenu");
                        }
                        background.Update(gameTime, milliScecondsElapsedGameTime);
                        break;
                }

                Boolean backTrack = false;
                //Up Key Down what to do (Sets Boolean)  Menu Up
                if (keybState.IsKeyDown(Keys.Escape) && !isPressedEsc)
                {
                    isPressedEsc = true;
                    {
                        backTrack = true;
                    }
                }
                //Up Key Up Logic (Resets Boolean)
                if (keybState.IsKeyUp(Keys.Escape))
                {
                    if (isPressedEsc)
                        isPressedEsc = false;
                }
                //Left Mouse Button what to do (Sets Boolean)  Menu Up
                if (mouseStateCurrent.RightButton == ButtonState.Pressed && !isPressedRightBtn)
                {
                    isPressedRightBtn = true;
                    backTrack = true;
                }
                //Left Mouse Button release logic
                if (mouseStateCurrent.RightButton == ButtonState.Released)
                {
                    if (isPressedRightBtn)
                        isPressedRightBtn = false;
                }

                //sets back button logic (Menus & GameState)
                if (backTrack == true)                    
                {
                    if (gameState == "Playing")
                    {
                        menus.SetMenu("PauseMenu");
                        GlobalVar.GameState = "MenuScreen";
                    }
                    else if (gameState == "Credits")
                    {
                        menus.SetMenu("MainMenu");
                        GlobalVar.GameState = "MenuScreen";
                    }
                    else if (gameState == "MenuScreen")
                    {
                        if (menus.getMenuState() == "PauseMenu")
                        {
                            GlobalVar.GameState = "Playing";
                        }
                        else if (menus.getMenuState() == "OptionsMenu" || menus.getMenuState() == "LoadProfileMenu" || menus.getMenuState() == "LevelSelect")
                        {
                            if (GlobalVar.PreviousGameState == "Playing")
                                menus.SetMenu("PauseMenu");
                            else
                                menus.SetMenu("MainMenu");
                        }
                        else if (menus.getMenuState() == "OptionsResolutionMenu" || menus.getMenuState() == "OptionsKeybindingMenuPage1" || menus.getMenuState() == "OptionsKeybindingMenuPage2" || menus.getMenuState() == "debugMenu")
                        {
                            menus.SetMenu("OptionsMenu");
                        }
                        else if (menus.getMenuState() == "CustomLevelSelect")
                        {
                            menus.SetMenu("LevelSelect");
                        }
                    }
                }

                //Texture Scaling based on screenSize
                GlobalVar.ScaleSize = new Vector2(GlobalVar.ScreenSize.X / defualtWidth, GlobalVar.ScreenSize.Y / defualtHeight);

                //ExitLogic (Add in stuff for a graceful shutdown like prompts and such
                if (GlobalVar.ExitGame == true)
                {
                    this.Exit();
                }

                //Last to update

                fpsManager.Update(gameTime);
                mouseStatePrevious = mouseStateCurrent;

                if (holdingPreviousGameState != GlobalVar.GameState)
                {
                    GlobalVar.PreviousGameState = holdingPreviousGameState;
                }

                holdingPreviousGameState = GlobalVar.GameState;

                base.Update(gameTime);
            }
            catch (Exception e)
            {
                if (devErrorLoggingDisable != true)
                    errorHandler.WriteError(3, 134, "Update Failure! " + e);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            try
            {
                //Update logic for FPS
                fpsManager.updateFrameCount();
                
                GraphicsDevice.Clear(Color.Black);
                //GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                
                switch (gameState)
                {
                    case "Playing":
                        background.Draw(spriteBatch, SeqoeUIMonoNormal);
                        fieldManager.Draw(spriteBatch, SeqoeUIMonoNormal);
                        break;
                    case "GameOver":
                        background.Draw(spriteBatch, SeqoeUIMonoNormal);
                        fieldManager.Draw(spriteBatch, SeqoeUIMonoNormal);
                        menus.Draw(spriteBatch);
                        break;
                    case "MenuScreen":
                        background.Draw(spriteBatch, SeqoeUIMonoNormal);
                        if (menus.getMenuState() == "PauseMenu")
                        {
                            fieldManager.Draw(spriteBatch, SeqoeUIMonoNormal);
                        }
                        menus.Draw(spriteBatch);
                        break;
                    case "Credits":
                        background.Draw(spriteBatch, SeqoeUIMonoNormal);
                        credits.DrawCredits(spriteBatch);
                        break;
                    case "SplashScreen":
                        splashScreen.Draw(spriteBatch, new Vector2(0, 0), GlobalVar.ScaleSize, SeqoeUIMonoNormal);
                        break;
                }

                //if (menuState = "Paused")
              

                //MOUSE DRAWING LOGIC
                if (!(gameState == "Credits" || gameState == "SplashScreen" || gameState == "Playing"))
                {
                    if (GlobalVar.OptionsArray[9].Equals("false"))
                    {
                        spriteBatch.Draw(gameMouseTexture, new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y), null, Color.White, 0f, new Vector2(0, 0), GlobalVar.ScaleSize, SpriteEffects.None, 1f);
                        this.IsMouseVisible = false;

                    }
                    else
                    {
                        this.IsMouseVisible = true;
                    }
                }
                else
                {
                    this.IsMouseVisible = false;
                }

                //Draws Version Info and Current FPS
                spriteBatch.DrawString(SeqoeUIMonoNormal, version, new Vector2((GlobalVar.ScreenSize.X / 2) - ((SeqoeUIMonoNormal.MeasureString(version).X / 2) * GlobalVar.ScaleSize.X), (10 * GlobalVar.ScaleSize.Y)), Color.White, 0f, new Vector2(0, 0), GlobalVar.ScaleSize, SpriteEffects.None, 1f);
                if (Convert.ToBoolean(GlobalVar.OptionsArray[10]) == true)
                {
                    if (!gameState.Equals("SplashScreen") && !gameState.Equals("Credits")) 
                        spriteBatch.DrawString(SeqoeUIMonoNormal, "FPS: " + fpsManager.getFPS(), new Vector2((GlobalVar.ScreenSize.X - (SeqoeUIMonoNormal.MeasureString("FPS: " + fpsManager.getFPS()).X) * GlobalVar.ScaleSize.X) - 10, (5 * GlobalVar.ScaleSize.Y)), Color.White, 0f, new Vector2(0, 0), GlobalVar.ScaleSize, SpriteEffects.None, 1f);
                }

                //DEBUG USE: 
                if (Convert.ToBoolean(GlobalVar.OptionsArray[11]) == true)
                {
                    spriteBatch.DrawString(SeqoeUIMonoNormal, "GAME1 TIME: " + (milliScecondsElapsedGameTime / 1000).ToString() + "s", new Vector2(10 * GlobalVar.ScaleSize.X, GlobalVar.ScreenSize.Y - ((SeqoeUIMonoNormal.MeasureString("X").Y * 3 + 10) * GlobalVar.ScaleSize.Y)), Color.White, 0f, new Vector2(0, 0), GlobalVar.ScaleSize, SpriteEffects.None, 1f);
                    spriteBatch.DrawString(SeqoeUIMonoNormal, "GAME STATE: " + GlobalVar.GameState.ToString(), new Vector2(10 * GlobalVar.ScaleSize.X, GlobalVar.ScreenSize.Y - ((SeqoeUIMonoNormal.MeasureString("X").Y * 2 + 10) * GlobalVar.ScaleSize.Y)), Color.White, 0f, new Vector2(0, 0), GlobalVar.ScaleSize, SpriteEffects.None, 1f);
                    spriteBatch.DrawString(SeqoeUIMonoNormal, "MOUSE POS: " + "X-" + mouseStateCurrent.X.ToString() + " Y-" + mouseStateCurrent.Y.ToString(), new Vector2(10 * GlobalVar.ScaleSize.X, GlobalVar.ScreenSize.Y - ((SeqoeUIMonoNormal.MeasureString("X").Y * 1 + 10) * GlobalVar.ScaleSize.Y)), Color.White, 0f, new Vector2(0, 0), GlobalVar.ScaleSize, SpriteEffects.None, 1f);
                }
                
                //spriteBatch.DrawString(MenuFont, menus.getMenuState(), new Vector2(1, 23), Color.Black);

                spriteBatch.End();

                base.Draw(gameTime);
            }
            catch (Exception e)
            {
                if (devErrorLoggingDisable != true)
                    errorHandler.WriteError(3, 114, "Draw Failure! " + e);
            }
        }

        protected override void OnExiting(Object sender, EventArgs args)
        {
            base.OnExiting(sender, args);

        }
    }
}
