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
        DateTime dateTime;
        FieldManager fieldManager;
        Cloud cloud;
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

        String menuState;
        //mainMenu, optionsMenu, optionsResolutionMenu, optionsKeybindingMenu, pauseMenu, debugMenu, saveSlotMenu
        String gameState;
        //Playing, MenuScreen, GameOver, Credits

        //Variables

        //Game
        String version = "Build V: 1.1.6.0"; // Major, Minor, Build, Revision #
        Boolean tripped = false;
        const int defualtWidth = 1080, defualtHeight = 720;

        //Keys
        string[] keyBindingInfo = new string[5];
        Boolean isPressedEsc;
        Texture2D gameMouseTexture;
        //Sounds
        //Song mainMenuMusic;
        Boolean songTripped;

        //Credits
        String[] creditsStringArray = new String[10] { "CREDITS", "Team", "Gavin Stewart - StarByte Designer & Lead Programer", "Kurtis Jones - Main Artist & Generation Programer", "Dalton Jones - Supporting Programer & Artist", "Josh Estrada - Programer & Tester", "A Special Thanks To", "Rb Witaker for helpful guides when we needed them", "AND", "To You The Player!" };

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
                dateTime = new DateTime();
                //Loads and sets option values
                GlobalVar.OptionsArray = optionsHandler.loadOptions();

                //Back Buffer Informaation
                GlobalVar.ScreenSize = new Vector2(Convert.ToInt32(GlobalVar.OptionsArray[0]), Convert.ToInt32(GlobalVar.OptionsArray[1]));
                graphics.PreferredBackBufferWidth = (int)GlobalVar.ScreenSize.X;
                graphics.PreferredBackBufferHeight = (int)GlobalVar.ScreenSize.Y;
                graphics.IsFullScreen = false;
                graphics.ApplyChanges();

                //Sets Keybinding Info from options Load
                for (int i = 0; i < keyBindingInfo.Length; i++)
                {
                    keyBindingInfo[i] = GlobalVar.OptionsArray[i + 2];
                }

                GlobalVar.ScaleSize = new Vector2(GlobalVar.ScreenSize.X / defualtWidth, GlobalVar.ScreenSize.Y / defualtHeight);
                GlobalVar.GameState = "SplashScreen";
                GlobalVar.Health = 100;
                GlobalVar.Sheild = 200;
                GlobalVar.Heat = 0;
                menuState = "MainMenu";
                //Load up classes we need
                fpsManager = new FPSManager();
                menus = new Menus(graphics);

                //TEMP Initilize
                cloud = new Cloud();
                fieldManager = new FieldManager();
                //Change to real values later
                splashScreenWaitTime = new float[2] { 1.0f, 1.0f };
                fieldManager.Intilize();
                cloud.Initialize();

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
                menus.SetMenu("MainMenu");

                //TEMP LOAD CONTENT
                cloud.LoadContent(this.Content);
                fieldManager.LoadContent(this.Content);
                splashScreenImages = new Texture2D[2];
                splashScreenImages[0] = Content.Load<Texture2D>("Sprites and pictures/splashScreen1");
                splashScreenImages[1] = Content.Load<Texture2D>("Sprites and pictures/splashScreen2");

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

                if (GlobalVar.OptionsArray[7] == "true")
                    this.IsMouseVisible = true;
                else
                {
                    this.IsMouseVisible = false;

                }

                //Handles Game States
                switch (gameState)
                {
                    case "MenuScreen":
                        if (tripped == false)
                        {
                            menus.SetMenu(menuState);
                            tripped = true;
                        }
                        if (menus.getMenuState() == "MainMenu")
                        {
                            //Reset Values
                            GlobalVar.Health = 100;
                            GlobalVar.Sheild = 200;
                            GlobalVar.Heat = 0;
                            GlobalVar.Score = 0;
                            GlobalVar.Currency = 0;
                        }
                        if (songTripped == false)
                        {
                            //MediaPlayer.Resume(mainMenuMusic, 
                            songTripped = true;
                        }
                        menus.Update(gameTime, mouseStateCurrent, mouseStatePrevious, milliScecondsElapsedGameTime);
                        cloud.Update(gameTime, milliScecondsElapsedGameTime);
                        break;

                    case "Playing":
                        //MediaPlayer.Pause(mainMenuMusic);
                        fieldManager.Update(milliScecondsElapsedGameTime);
                        cloud.Update(gameTime, milliScecondsElapsedGameTime);
                        tripped = false;
                        break;

                    case "Credits":
                        cloud.Update(gameTime, milliScecondsElapsedGameTime);
                        //MediaPlayer.Pause(mainMenuMusic);
                        tripped = false;
                        break;

                    case "SplashScreen":
                        //TEMP UPDATE
                        splashScreen.Update(milliScecondsElapsedGameTime, orginalSplashScreenStartTime);
                        if (splashScreen.getSplashScreenOver() == true)
                            GlobalVar.GameState = "MenuScreen";
                        break;
                }

                //Up Key Down what to do (Sets Boolean)  Menu Up
                if (keybState.IsKeyDown(Keys.Escape) && !isPressedEsc)
                {
                    isPressedEsc = true;
                    {
                        if (gameState == "Playing")
                        {
                            menuState = "PauseMenu";
                            GlobalVar.GameState = "MenuScreen";
                        }
                        if (gameState == "Credits")
                        {
                            menuState = "MainMenu";
                            GlobalVar.GameState = "MenuScreen";
                        }
                    }
                }
                //Up Key Up Logic (Resets Boolean)
                if (keybState.IsKeyUp(Keys.Escape))
                {
                    if (isPressedEsc)
                        isPressedEsc = false;
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
                GlobalVar.PreviousGameState = GlobalVar.GameState;
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
                
                GraphicsDevice.Clear(Color.SkyBlue);
                //GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                
                switch (gameState)
                {
                    case "Playing":
                        cloud.Draw(spriteBatch, SeqoeUIMonoNormal);
                        fieldManager.Draw(spriteBatch);
                        break;
                    case "GameOver":
                        break;
                    case "MenuScreen":
                        cloud.Draw(spriteBatch, SeqoeUIMonoNormal);
                        menus.Draw(spriteBatch);
                        break;
                    case "Credits":
                        cloud.Draw(spriteBatch, SeqoeUIMonoNormal);
                        credits.DrawCredits(spriteBatch);
                        break;
                    case "SplashScreen":
                        splashScreen.Draw(spriteBatch, new Vector2(0, 0), GlobalVar.ScaleSize, SeqoeUIMonoNormal);
                        break;          
                }

              

                //MOUSE DRAWING LOGIC
                if (gameState != "Credits" || gameState != "SplashScreen")
                    spriteBatch.Draw(gameMouseTexture, new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y), null, Color.White, 0f, new Vector2(0, 0), GlobalVar.ScaleSize, SpriteEffects.None, 1f);

                //Draws Version Info and Current FPS
                spriteBatch.DrawString(SeqoeUIMonoNormal, version, new Vector2((GlobalVar.ScreenSize.X / 2) - ((SeqoeUIMonoNormal.MeasureString(version).X / 2) * GlobalVar.ScaleSize.X), (10 * GlobalVar.ScaleSize.Y)), Color.White, 0f, new Vector2(0, 0), GlobalVar.ScaleSize, SpriteEffects.None, 1f);
                if (Convert.ToBoolean(GlobalVar.OptionsArray[8]) == true)
                {
                    if (!gameState.Equals("SplashScreen") && !gameState.Equals("Credits")) 
                        spriteBatch.DrawString(SeqoeUIMonoNormal, "FPS: " + fpsManager.getFPS(), new Vector2((GlobalVar.ScreenSize.X - (SeqoeUIMonoNormal.MeasureString("FPS: " + fpsManager.getFPS()).X) * GlobalVar.ScaleSize.X) - 10, (5 * GlobalVar.ScaleSize.Y)), Color.Black, 0f, new Vector2(0, 0), GlobalVar.ScaleSize, SpriteEffects.None, 1f);
                }

                //DEBUG USE: 
                if (Convert.ToBoolean(GlobalVar.OptionsArray[9]) == true)
                {
                    spriteBatch.DrawString(SeqoeUIMonoNormal, "GAME1 TIME: " + (milliScecondsElapsedGameTime / 1000).ToString() + "s", new Vector2(10 * GlobalVar.ScaleSize.X, GlobalVar.ScreenSize.Y - ((SeqoeUIMonoNormal.MeasureString("X").Y * 3 + 10) * GlobalVar.ScaleSize.Y)), Color.Black);
                    spriteBatch.DrawString(SeqoeUIMonoNormal, "GAME STATE: " + GlobalVar.GameState.ToString(), new Vector2(10 * GlobalVar.ScaleSize.X, GlobalVar.ScreenSize.Y - ((SeqoeUIMonoNormal.MeasureString("X").Y * 2 + 10) * GlobalVar.ScaleSize.Y)), Color.Black);
                    spriteBatch.DrawString(SeqoeUIMonoNormal, "MOUSE POS: " + "X-" + mouseStateCurrent.X.ToString() + " Y-" + mouseStateCurrent.X.ToString(), new Vector2(10 * GlobalVar.ScaleSize.X, GlobalVar.ScreenSize.Y - ((SeqoeUIMonoNormal.MeasureString("X").Y * 1 + 10) * GlobalVar.ScaleSize.Y)), Color.Black);
                }

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