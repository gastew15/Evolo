﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

/**
 * Evolo Field Manager to handle most game related operations for the player and tetrominos
 * Author: Dalton, Josh, Gavin
 * Version: 4/23/15
 */

namespace Evolo.GameClass
{
    class FieldManager
    {
        #region Variables
  
        //Gamefield Variables
        private Boolean[,] gameField = new Boolean[26, 22];
        private Vector2 gridStartPos;
        private int linesToClear;
        private Boolean gameWin;
        private Boolean gameOver;
        private Tutorial tutorial = new Tutorial();
        private Boolean tutorialFirstUpdateTrip = false;

        //Timing Variables
        private int milisecondsElapsedTetrominoTime = 0;
        private int milisecondsTetrominoFallTime = 150;
        private int milisecondsTetrominoLockDelayTime = 400;
        private int milisecondsElapsedPlayerTime1 = 0;
        private int milisecondsElapsedPlayerTime2 = 0;
        private int milisecondsPlayerGravityTime1 = 200;
        private int milisecondsPlayerGravityTime2 = 450;
        private int timer;
        private int milisecondsElapsedTime = 0;
        private double playerMovementASecond = 150;

        //Keyboard Variables / Misc
        private bool keyLeftDown, keyRightDown, keyUpDown;
        private bool keyADown, keyDDown, keyWDown;
        private int milisecondExpirationKeyA = 200, milisecondExpirationKeyD = 200;
        private int milisecondExpirationKeyLeft = 200, milisecondExpirationKeyRight = 200;
        private int milisecondsElapsedKeyA = 0, milisecondsElapsedKeyD = 0, milisecondsElapsedKeyLeft = 0, milisecondsElapsedKeyRight = 0, milisecondsElapsedKeyUp = 0;
        private String debugStringData = "";
        private Random random = new Random();

        //Content Variables
        private Texture2D blockTexture, playerTexture, blankBlockTexture, fullBlockTexture;
        private SpriteFont font;

        //Sound Variables
        SoundEffect blockClear;
        SoundEffect blockLock;

        //Player Variables
        private SpriteEffects player1SpriteEffects;
        private bool player1Jump = false;
        private Player player1;
        private Vector2 player1GridPos, player1GridPosPrevious;
        private Boolean playerCanNotMoveRight, playerCanNotMoveLeft, playerCanNotMoveDown, playerCanNotMoveUp;

        //tetromino Variables
        private int tetristype;
        private int absTetrominoBlockFarthestLeft, absTetrominoBlockFarthestRight, absTetrominoBlockFarthestDown, absTetrominoBlockFarthestUp;
        private int[] farthestTetrominoBlockLeft, farthestTetrominoBlockRight, farthestTetrominoBlockDown, farthestTetrominoBlockUp;
        private int activeTetromino = 0;
        private int lastActiveTetromino = 0;
        private int tetrominoRotation = 0;
        private int tetrominoLastRotation = 0;
        private List<Tetromino> tetromino = new List<Tetromino>();
        private List<Vector2> tetrominoGridPos = new List<Vector2>();
        private int[] tetrominoHistory = new int[4];
        private Vector2 tetrominoLastGridPos = new Vector2();
        private Vector2[] tetrominoBlockLastPositions, tetrominoBlockPositions;
        private Boolean tetrominoCanNotMoveRight, tetrominoCanNotMoveLeft, tetrominoCanNotMoveDown;
        private Boolean tetrominoCanRotate;
        private Tetromino rotationTestTetromino;

        //Hud Variabels
        private Texture2D hudTexture;

        //Platform Variables
        private Texture2D platformTexture;
        private Vector2 endPlatformGridPos, startPlatformGridPos;

        //temp level variables
        private double levelModifier;

        //Level System variables
        private SingletonLevelSystem levels = SingletonLevelSystem.getInstance();

        #endregion

        public FieldManager()
        {

        }

        public void Initialize()
        {
            player1SpriteEffects = SpriteEffects.None;
            levels.setLevel("Level1");
        }

        public void LoadContent(ContentManager Content, SpriteFont font)
        {
            this.font = font;
            blockTexture = Content.Load<Texture2D>("Sprites and pictures/BasicBlock");
            fullBlockTexture = Content.Load<Texture2D>("Sprites and pictures/BasicBlock");
            blankBlockTexture = Content.Load<Texture2D>("Sprites and pictures/blankBlock");
            playerTexture = Content.Load<Texture2D>("Sprites and pictures/CharacterTest");
            platformTexture = Content.Load<Texture2D>("Sprites and pictures/Platform");
            hudTexture = Content.Load<Texture2D>("Sprites and pictures/GameHud");
            blockClear = Content.Load<SoundEffect>("Sounds/Sound Effects/Sound_BlockClear");
            blockLock= Content.Load<SoundEffect>("Sounds/Sound Effects/Sound_BlockLock");

            tutorial.LoadContent(Content, font);

            //Teromeno Set Up Reference
            resetGameVariables();
        }

        public void Update(GameTime gameTime, MouseState currentMouseState, MouseState previousMouseState)
        {
            if ((GlobalVar.CurrentLevel.Equals("6") && !tutorial.getIsActive()) || !GlobalVar.CurrentLevel.Equals("6") || !tutorialFirstUpdateTrip)
            {
                #region Local Variable Reseting

                //Adds time since last update to the elapsed time for the tetromino
                milisecondsElapsedTetrominoTime += gameTime.ElapsedGameTime.Milliseconds;
                milisecondsElapsedPlayerTime1 += gameTime.ElapsedGameTime.Milliseconds;
                milisecondsElapsedTime += gameTime.ElapsedGameTime.Milliseconds;
                milisecondsElapsedKeyA += gameTime.ElapsedGameTime.Milliseconds;
                milisecondsElapsedKeyD += gameTime.ElapsedGameTime.Milliseconds;
                milisecondsElapsedKeyLeft += gameTime.ElapsedGameTime.Milliseconds;
                milisecondsElapsedKeyRight += gameTime.ElapsedGameTime.Milliseconds;
                milisecondsElapsedKeyUp += gameTime.ElapsedGameTime.Milliseconds;

                //Adjusts the grid Starting Postion for resolution changes & such
                gridStartPos = new Vector2((GlobalVar.ScreenSize.X / 2) - (((blockTexture.Width * GlobalVar.ScaleSize.X) * gameField.GetLength(0)) / 2), -1 * (blockTexture.Height * GlobalVar.ScaleSize.Y));

                //Resets tetromino Movement Booleans
                tetrominoCanNotMoveRight = false;
                tetrominoCanNotMoveLeft = false;
                tetrominoCanNotMoveDown = false;
                tetrominoCanRotate = true;

                playerCanNotMoveRight = false;
                playerCanNotMoveLeft = false;
                playerCanNotMoveDown = false;
                playerCanNotMoveUp = false;

                //Resets absoluote tetromino Positions
                absTetrominoBlockFarthestLeft = gameField.GetLength(0);
                absTetrominoBlockFarthestUp = gameField.GetLength(1);
                absTetrominoBlockFarthestRight = 0;
                absTetrominoBlockFarthestDown = 0;
                tetrominoLastRotation = tetrominoRotation;

                if (activeTetromino >= 0 && activeTetromino < tetrominoGridPos.Count)
                    tetrominoLastGridPos = tetrominoGridPos[activeTetromino];

                #endregion

                #region tetromino Collision Box Logic

                //Temporary Holding Variables
                int tempX, tempY;

                for (int p = 0; p < tetromino[activeTetromino].getPositions().Length; p++)
                {
                    //Clears the hit boxes from the previous Positions
                    if (lastActiveTetromino == activeTetromino)
                    {
                        gameField[(int)tetrominoBlockLastPositions[p].X, (int)(tetrominoBlockLastPositions[p].Y)] = false;
                    }

                    //Sets holding X value to the input position respective from the tetromino Object, and if less than 0 corrects the issue
                    if ((int)(tetromino[activeTetromino].getPositions()[p].X) < 0)
                    {
                        tempX = 0;
                    }
                    else
                    {
                        tempX = (int)(tetromino[activeTetromino].getPositions()[p].X);
                    }

                    //Sets holding Y value to the input position respective from the tetromino Object, and if less than 0 corrects the issue
                    if ((int)(tetromino[activeTetromino].getPositions()[p].Y) < 0)
                    {
                        tempY = 0;
                    }
                    else
                    {
                        tempY = (int)(tetromino[activeTetromino].getPositions()[p].Y);
                    }

                    //Sets the corrected local holding array for the postions of the blocks from the tetromino
                    tetrominoBlockPositions[p] = new Vector2(tempX, tempY);

                    //Sets the hit boxes for the tetromino at the current Positon
                    gameField[(int)tetrominoBlockPositions[p].X, (int)tetrominoBlockPositions[p].Y] = true;

                    //Resets the variable
                    tetrominoBlockLastPositions[p] = new Vector2(tempX, tempY);
                }

                //Resets the variable
                lastActiveTetromino = activeTetromino;

                #endregion

                #region tetromino Block Constraint Information

                //Finds the parts of the axis relative to their opposite

                #region X's On Y Values

                //Local Varaibles used in calculations
                Boolean yPosAlreadyInList = false;
                List<int> yValuesUsedForX = new List<int>();
                List<List<int>> xValuesUsed = new List<List<int>>();

                //Finds all the unqiue Y Values in tetromino's indivdual blocks
                for (int w = 0; w < tetrominoBlockPositions.Length; w++)
                {
                    List<int> yPosSublist = new List<int>();

                    if (yValuesUsedForX != null)
                    {
                        for (int j = 0; j < yValuesUsedForX.Count; j++)
                        {
                            if (tetrominoBlockPositions[w].Y == yValuesUsedForX[j])
                            {
                                yPosAlreadyInList = true;
                            }
                        }
                        if (yPosAlreadyInList == false)
                        {
                            yValuesUsedForX.Add((int)tetrominoBlockPositions[w].Y);

                        }
                    }
                    else
                    {
                        yValuesUsedForX.Add((int)tetrominoBlockPositions[w].Y);

                    }
                    yPosAlreadyInList = false;
                }

                yValuesUsedForX.Sort();

                //Finds the X values that exist relative to the Y values in the Tetrimino
                for (int k = 0; k < yValuesUsedForX.Count; k++)
                {
                    List<int> xHoldingValues = new List<int>();

                    for (int i = 0; i < tetrominoBlockPositions.Length; i++)
                    {
                        if (tetrominoBlockPositions[i].Y == yValuesUsedForX[k])
                        {
                            xHoldingValues.Add((int)tetrominoBlockPositions[i].X);
                        }
                    }
                    xValuesUsed.Add(xHoldingValues);
                }

                #endregion

                #region Y's On X Values

                //Local Varaibles used in calculations
                Boolean xPosAlreadyInList = false;
                List<int> xValuesUsedForY = new List<int>();
                List<List<int>> yValuesUsed = new List<List<int>>();

                //Finds all the unqiue X Values in tetromino's indivdual blocks
                for (int g = 0; g < tetrominoBlockPositions.Length; g++)
                {
                    List<int> xPosSublist = new List<int>();

                    if (xValuesUsedForY != null)
                    {
                        for (int j = 0; j < xValuesUsedForY.Count; j++)
                        {
                            if (tetrominoBlockPositions[g].X == xValuesUsedForY[j])
                            {
                                xPosAlreadyInList = true;
                            }
                        }
                        if (xPosAlreadyInList == false)
                        {
                            xValuesUsedForY.Add((int)tetrominoBlockPositions[g].X);

                        }
                    }
                    else
                    {
                        xValuesUsedForY.Add((int)tetrominoBlockPositions[g].X);

                    }
                    xPosAlreadyInList = false;
                }

                xValuesUsedForY.Sort();

                //Finds the Y values that exist relative to the X values in the Tetrimino
                for (int q = 0; q < xValuesUsedForY.Count; q++)
                {
                    List<int> yHoldingValues = new List<int>();

                    for (int i = 0; i < tetrominoBlockPositions.Length; i++)
                    {
                        if (tetrominoBlockPositions[i].X == xValuesUsedForY[q])
                        {
                            yHoldingValues.Add((int)tetrominoBlockPositions[i].Y);
                        }
                    }
                    yValuesUsed.Add(yHoldingValues);
                }

                #endregion

                //Debug Info Write
                debugStringData = "\n" + "yValuesUsed:";
                for (int m = 0; m < yValuesUsedForX.Count; m++)
                {
                    xValuesUsed[m].Sort();

                    debugStringData += "\nY" + (m + 1) + ": " + yValuesUsedForX[m];

                    debugStringData += " X's:";
                    for (int j = 0; j < xValuesUsed[m].Count; j++)
                    {
                        debugStringData += " " + xValuesUsed[m][j] + ",";
                    }

                }

                //Sets array index size for the farthest blocks
                farthestTetrominoBlockLeft = new int[xValuesUsed.Count];
                farthestTetrominoBlockRight = new int[xValuesUsed.Count];
                farthestTetrominoBlockDown = new int[yValuesUsed.Count];
                farthestTetrominoBlockUp = new int[yValuesUsed.Count];

                #region extremas of X Values on the Y axis

                //Checks the farthest x Values for their repsective Y's in the List
                for (int p = 0; p < xValuesUsed.Count; p++)
                {
                    int mostRightXOnY = 0;
                    int mostLeftXOnY = gameField.GetLength(0);

                    //Checks for Max X values On Y
                    for (int i = 0; i < xValuesUsed[p].Count; i++)
                    {
                        if (xValuesUsed[p][i] > mostRightXOnY)
                        {
                            mostRightXOnY = xValuesUsed[p][i];
                        }

                        if (xValuesUsed[p][i] < mostLeftXOnY)
                        {
                            mostLeftXOnY = xValuesUsed[p][i];
                        }
                    }

                    farthestTetrominoBlockLeft[p] = mostLeftXOnY;
                    farthestTetrominoBlockRight[p] = mostRightXOnY;

                    //Checks for Overall Max X Values
                    if (mostRightXOnY > absTetrominoBlockFarthestRight)
                    {
                        absTetrominoBlockFarthestRight = mostRightXOnY;
                    }

                    if (mostLeftXOnY < absTetrominoBlockFarthestLeft)
                    {
                        absTetrominoBlockFarthestLeft = mostLeftXOnY;
                    }
                }

                #endregion

                #region extremas of Y Values on the X axis

                //Checks the farthest Y Values for their repsective X's in the List
                for (int v = 0; v < yValuesUsed.Count; v++)
                {
                    int mostUpYOnX = gameField.GetLength(1);
                    int mostDownYOnX = 0;

                    //Checks for Max Y values On X
                    for (int i = 0; i < yValuesUsed[v].Count; i++)
                    {
                        if (yValuesUsed[v][i] < mostUpYOnX)
                        {
                            mostUpYOnX = yValuesUsed[v][i];
                        }

                        if (yValuesUsed[v][i] > mostDownYOnX)
                        {
                            mostDownYOnX = yValuesUsed[v][i];
                        }
                    }

                    farthestTetrominoBlockDown[v] = mostDownYOnX;
                    farthestTetrominoBlockUp[v] = mostUpYOnX;

                    //Checks for Overall Max Y Values
                    if (mostUpYOnX < absTetrominoBlockFarthestUp)
                    {
                        absTetrominoBlockFarthestUp = mostUpYOnX;
                    }

                    if (mostDownYOnX > absTetrominoBlockFarthestDown)
                    {
                        absTetrominoBlockFarthestDown = mostDownYOnX;
                    }
                }

                #endregion

                #endregion

                #region tetromino Collision Detection

                //(Left / Right)
                for (int i = 0; i < farthestTetrominoBlockLeft.Length; i++)
                {
                    if (farthestTetrominoBlockLeft[i] > 0)
                    {
                        if (gameField[farthestTetrominoBlockLeft[i] - 1, yValuesUsedForX[i]] == true)
                        {
                            tetrominoCanNotMoveLeft = true;
                        }
                    }

                    if (farthestTetrominoBlockRight[i] < gameField.GetLength(0))
                    {
                        if (gameField[farthestTetrominoBlockRight[i] + 1, yValuesUsedForX[i]] == true)
                        {
                            tetrominoCanNotMoveRight = true;
                        }
                    }
                }

                //(Up / Down)
                for (int i = 0; i < farthestTetrominoBlockUp.Length; i++)
                {
                    if (farthestTetrominoBlockDown[i] < gameField.GetLength(1) - 1)
                    {
                        if (gameField[xValuesUsedForY[i], farthestTetrominoBlockDown[i] + 1] == true)
                        {
                            tetrominoCanNotMoveDown = true;
                        }
                    }
                }

                #endregion

                #region Player Collision Detection

                //Fail safe for player falling out of array
                if (player1GridPos.Y > gameField.GetLength(1) - 1)
                {
                    player1GridPos.Y = player1GridPosPrevious.Y - 25;
                }

                //Check
                if (player1GridPos.X - 1 >= 0 && player1GridPos.X + 1 < gameField.GetLength(0))
                {
                    //Right
                    if (gameField[(int)player1GridPos.X + 1, (int)player1GridPos.Y] == true)
                    {
                        playerCanNotMoveRight = true;
                    }

                    //Left
                    if (gameField[(int)player1GridPos.X - 1, (int)player1GridPos.Y] == true)
                    {
                        playerCanNotMoveLeft = true;
                    }
                }

                //Y Check
                //Down
                if (player1GridPos.Y < gameField.GetLength(1) - 1 && gameField[(int)player1GridPos.X, (int)player1GridPos.Y + 1] == true)
                {
                    playerCanNotMoveDown = true;
                }
                //Up
                if (player1GridPos.Y > 0 && gameField[(int)player1GridPos.X, (int)player1GridPos.Y - 1] == true)
                {
                    playerCanNotMoveUp = true;
                }

                //Block Falling on Player Check
                if (gameField[(int)player1GridPos.X, (int)player1GridPos.Y] == true)
                {
                    gameWin = false;
                    gameOver = true;
                }

                //Player Reaching End Platform Check
                if ((player1GridPos.Y == (endPlatformGridPos.Y - 1) && (player1GridPos.X == endPlatformGridPos.X)) && linesToClear == 0)
                {
                    gameWin = true;

                    GlobalVar.GameState = "GameOver";

                    GlobalVar.Score = (int)(((timer - milisecondsElapsedTime) * .75 * levelModifier));


                    gameOver = true;

                }
                #endregion

                #region tetromino Keyboard Input

                //Keyboard Input (Left Key)
                if (absTetrominoBlockFarthestLeft > 3 && tetrominoCanNotMoveLeft == false)
                {
                    if (keyLeftDown == true)
                    {
                        if (Keyboard.GetState().IsKeyDown((Keys)Enum.Parse(typeof(Keys), GlobalVar.OptionsArray[5], true)))
                        {
                            keyLeftDown = false;
                            tetrominoGridPos[activeTetromino] = new Vector2(tetrominoGridPos[activeTetromino].X - 1, tetrominoGridPos[activeTetromino].Y);
                        }
                    }
                    if ((milisecondsElapsedKeyLeft - milisecondExpirationKeyLeft) >= 0)
                    {
                        if (keyLeftDown == false)
                        {
                            keyLeftDown = true;
                        }

                        milisecondsElapsedKeyLeft -= milisecondExpirationKeyLeft;
                    }
                    else if (Keyboard.GetState().IsKeyUp((Keys)Enum.Parse(typeof(Keys), GlobalVar.OptionsArray[5], true)))
                    {
                        if (keyLeftDown == false)
                        {
                            keyLeftDown = true;
                        }
                    }
                }

                //Keyboard Input (Right Key)
                if (absTetrominoBlockFarthestRight < gameField.GetLength(0) - 4 && tetrominoCanNotMoveRight == false)
                {
                    if (keyRightDown == true)
                    {
                        if (Keyboard.GetState().IsKeyDown((Keys)Enum.Parse(typeof(Keys), GlobalVar.OptionsArray[6], true)))
                        {
                            keyRightDown = false;
                            tetrominoGridPos[activeTetromino] = new Vector2(tetrominoGridPos[activeTetromino].X + 1, tetrominoGridPos[activeTetromino].Y);
                        }
                    }
                    if ((milisecondsElapsedKeyRight - milisecondExpirationKeyRight) >= 0)
                    {
                        if (keyRightDown == false)
                        {
                            keyRightDown = true;
                        }

                        milisecondsElapsedKeyRight -= milisecondExpirationKeyRight;
                    }
                    else if (Keyboard.GetState().IsKeyUp((Keys)Enum.Parse(typeof(Keys), GlobalVar.OptionsArray[6], true)))
                    {
                        if (keyRightDown == false)
                        {
                            keyRightDown = true;
                        }
                    }
                }

                //Keyboard Input (Down Key) *Speed Up
                if (Keyboard.GetState().IsKeyDown((Keys)Enum.Parse(typeof(Keys), GlobalVar.OptionsArray[8], true)))
                {
                    milisecondsTetrominoFallTime = 100;

                }
                else if (Keyboard.GetState().IsKeyUp((Keys)Enum.Parse(typeof(Keys), GlobalVar.OptionsArray[8], true)))
                {
                    milisecondsTetrominoFallTime = 300;
                }

                //Keyboard Input (Up Key) *Rotation
                if (tetromino[activeTetromino].getTetrisType() != 5)
                {
                    //Sets test block rotation one ahead of the current rotation
                    if (tetrominoRotation < 3)
                        rotationTestTetromino.setRotation(tetrominoRotation + 1);
                    else
                        rotationTestTetromino.setRotation(0);

                    if (keyUpDown == true)
                    {
                        if (Keyboard.GetState().IsKeyDown((Keys)Enum.Parse(typeof(Keys), GlobalVar.OptionsArray[7], true)))
                        {
                            keyUpDown = false;

                            //Rotation Checking
                            for (int p = 0; p < rotationTestTetromino.getPositions().Length; p++)
                            {
                                //check if the rotation would be inside the game field bounds
                                if (((int)rotationTestTetromino.getPositions()[p].X > 2 && (int)rotationTestTetromino.getPositions()[p].X < gameField.GetLength(0) - 3) && ((int)rotationTestTetromino.getPositions()[p].Y > 0 && (int)rotationTestTetromino.getPositions()[p].Y < gameField.GetLength(1)))
                                {
                                    //checks if the rotation will collide with any blocks set to active                           
                                    if (gameField[(int)rotationTestTetromino.getPositions()[p].X, (int)rotationTestTetromino.getPositions()[p].Y] == true)
                                    {
                                        //checks if the postions that are true are part of the main active tetromino before setting to true.
                                        Boolean tripped = false;
                                        for (int q = 0; q < tetromino[activeTetromino].getPositions().Length; q++)
                                        {
                                            if (rotationTestTetromino.getPositions()[p] == tetromino[activeTetromino].getPositions()[q])
                                            {
                                                tripped = true;
                                            }
                                        }

                                        if (tripped == false)
                                        {
                                            tetrominoCanRotate = false;
                                        }
                                    }
                                }
                                else
                                {
                                    tetrominoCanRotate = false;
                                }
                            }

                            if (tetrominoCanRotate == true)
                            {
                                //Sets current rotation value based off 0 - 3
                                if (tetrominoRotation < 3)
                                    tetrominoRotation++;
                                else
                                    tetrominoRotation = 0;

                                //Sends rotation value to the currently active tetromino
                                tetromino[activeTetromino].setRotation(tetrominoRotation);
                            }
                        }
                    }

                    else if (Keyboard.GetState().IsKeyUp((Keys)Enum.Parse(typeof(Keys), GlobalVar.OptionsArray[7], true)))
                    {
                        if (keyUpDown == false)
                        {
                            keyUpDown = true;
                        }
                    }
                }

                #endregion

                #region tetromino Movement Down

                //Checks to see if the block is able to move down 1 gridPos or not
                if (absTetrominoBlockFarthestDown < gameField.GetLength(1) - 1 && tetrominoCanNotMoveDown == false)
                {
                    //Checks if the block can move down based off the elapsed time, and makes up for any lost movement since the last update
                    if (milisecondsElapsedTetrominoTime - milisecondsTetrominoFallTime >= 1)
                    {
                        //Extra Check if it can move while moving through the while loop (Speed & Droped frames protection mostly)
                        if (absTetrominoBlockFarthestDown < gameField.GetLength(1) - 1 && tetrominoCanNotMoveDown == false)
                        {
                            //Move the tetromino down one and takes away 1 movement worth of time from the elapsed time
                            tetrominoGridPos[activeTetromino] = new Vector2(tetrominoGridPos[activeTetromino].X, tetrominoGridPos[activeTetromino].Y + 1);
                            milisecondsElapsedTetrominoTime -= milisecondsTetrominoFallTime;

                            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                            {
                                GlobalVar.Score += 2;
                            }
                        }
                    }
                }
                else
                {
                    //Checks for block lock delay before locking in place and spawning new block
                    if (milisecondsElapsedTetrominoTime - milisecondsTetrominoLockDelayTime >= 1)
                    {
                        if (Convert.ToBoolean(GlobalVar.OptionsArray[12]))
                        {
                            blockLock.Play();
                        }

                        //New Boolean Array to tell the game which lines are filled
                        Boolean[] isfilled = new Boolean[4];
                        for (int o = 0; o < isfilled.Length; o++)
                        {
                            isfilled[o] = true;
                        }
                        //Checking the lines from the Lowest block that just feel to 4 lines above to make sure all lines that could be filled are accounted for
                        for (int l = 0; l < 4; l++)
                        {
                            for (int i = 3; i < gameField.GetLength(0) - 3; i++)
                            {
                                if ((i >= 0 && i < gameField.GetLength(0)) && ((absTetrominoBlockFarthestDown - 3) + l >= 0 && (absTetrominoBlockFarthestDown - 3) + l < gameField.GetLength(1)))
                                {
                                    //Where the loop checks to see if the line is not filled
                                    if (gameField[i, (absTetrominoBlockFarthestDown - 3) + l] == false)
                                    {
                                        isfilled[l] = false;
                                    }
                                }
                                else
                                {
                                    isfilled[l] = false;
                                }
                            }
                        }

                        //Tetromino Line Clearing (DISABLED)
                        /*
                        //Acessing all the tetromino Y values that are within the lines needed to be clear
                        for (int i = 0; i < tetromino.Count - 1; i++)
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                //Checks which tetromino's are within the playing field
                                if ((int)tetromino[i].getPositions()[j].Y >= 3 && (int)tetromino[i].getPositions()[j].Y < gameField.GetLength(1))
                                {
                                    for (int n = 0; n < isfilled.Length; n++)
                                    {
                                        //Checking to see if the Y positons of the tetromino is within 4 blocks from the tetromino that just landed and to see if they are within a line that is needed to be cleared
                                        if (isfilled[n] == true && (int)tetromino[i].getPositions()[j].Y == (absTetrominoBlockFarthestDown - 3) + n)
                                        {
                                            // temp boolean to later set the Active Pos to false or true.
                                            Boolean[] tempHolding = new Boolean[4];

                                            for (int a = 0; a < tempHolding.Length; a++)
                                            {
                                                //Seeing which Y pos of the blocks are apart of the temp boolean array
                                                if (a == j)
                                                {
                                                    //Sets the boolean to false to then later set the whether the block is Active or not to false
                                                    tempHolding[a] = false;
                                                }
                                                else
                                                {
                                                    //Leaves the Active Pos as is.
                                                    tempHolding[a] = tetromino[i].getBlockPosActive()[a];
                                                }
                                            }
                                            //Sets the tetromino PosActive to the temp Holding array
                                            tetromino[i].setBlockPosActive(tempHolding);
                                        }
                                    }
                                }
                            }
                            //If Tetromino Positions array is empty it's removed from the list
                            if (tetromino[i].getPositions().Length == 0)
                            {
                                tetromino.RemoveAt(i);
                                tetrominoGridPos.RemoveAt(i);
                            }
                        }

                        //Line Down For What?
                        for (int a = 0; a < 4; a++)
                        {
                            for (int i = 0; i < tetrominoGridPos.Count - 1; i++)
                            {
                                if (tetrominoGridPos[i].Y <= (absTetrominoBlockFarthestDown - 3) + a && isfilled[a] == true)
                                {
                                    tetrominoGridPos[i] = new Vector2(tetrominoGridPos[i].X, tetrominoGridPos[i].Y + 1);

                                    tetromino[i].Update(tetrominoGridPos[i], gridStartPos, GlobalVar.ScaleSize);

                                    for (int j = 0; j < 4; j++)
                                    {
                                        if (tetromino[i].getRawBlockPositions()[j].Y > (absTetrominoBlockFarthestDown - 3) + a)
                                        {
                                            Vector2[] tempHolding = new Vector2[4];

                                            for (int k = 0; k < tempHolding.Length; k++)
                                            {
                                                if (k == j)
                                                {
                                                    tempHolding[k] = new Vector2(tetromino[i].getRawBlockPositions()[k].X, tetromino[i].getRawBlockPositions()[k].Y - 1);
                                                }
                                                else
                                                {
                                                    tempHolding[k] = tetromino[i].getRawBlockPositions()[k];
                                                }
                                            }

                                            tetromino[i].setBlockPositions(tempHolding);
                                        }
                                    }
                                }
                            }
                        }
                         */

                        //Hit box clearing
                        int lineClearCount = 0;
                        for (int n = 0; n < isfilled.Length; n++)
                        {
                            //Much simple virsion of the line clearing above
                            if (isfilled[n] == true)
                            {
                                lineClearCount++;
                                linesToClear -= 1;
                                if (linesToClear <= 0)
                                {
                                    linesToClear = 0;
                                }
                                for (int i = 3; i < gameField.GetLength(0) - 3; i++)
                                {
                                    gameField[i, (absTetrominoBlockFarthestDown - 3) + n] = false;
                                }

                                //Moves all the blocks down
                                for (int j = (absTetrominoBlockFarthestDown - 3) + n; j > 2; j--)
                                {
                                    for (int e = 3; e < gameField.GetLength(0) - 3; e++)
                                    {
                                        gameField[e, j] = gameField[e, j - 1];
                                    }
                                }
                                if (Convert.ToBoolean(GlobalVar.OptionsArray[12]))
                                {
                                    blockClear.Play();
                                }
                            }        
                        }
                        //Adds Points for the lines cleared
                        GlobalVar.Score += (int)(750 * lineClearCount * 1.5 * levelModifier);

                        //Setting various variables required to spawn a new clean tetromino and checks to see if the new tetromino is not part of the History Array then trys to spawn a new block atlest 2 times
                        bool repeat = false;
                        int index = 0;
                        do
                        {

                            tetristype = random.Next(1, 8);
                            repeat = false;
                            if (tetrominoHistory.Contains<int>(tetristype))
                            {
                                repeat = true;
                            }
                            index++;
                        }
                        while (repeat == true && index != 3);

                        tetrominoHistoryAddItem(tetristype);

                        activeTetromino += 1;
                        //Sets new Tetromino in the Preview
                        tetromino.Add(new Tetromino(tetristype, blockTexture));
                        tetrominoGridPos.Add(new Vector2(28.5f, 4));

                        //Moves previous Tetromino from the Preview to the game field
                        tetrominoGridPos[activeTetromino] = new Vector2(13, 0);

                        tetrominoGridPos[activeTetromino] = new Vector2(12, 0);

                        tetrominoRotation = 0;
                        tetrominoLastRotation = 0;

                        if (tetrominoGridPos[activeTetromino - 1].Y == 0)
                        {
                            gameOver = true;
                            gameWin = false;
                        }

                        rotationTestTetromino.setTetrisType(tetromino[activeTetromino].getTetrisType());
                        rotationTestTetromino.setRotation(0);

                        milisecondsElapsedTetrominoTime -= milisecondsTetrominoLockDelayTime;
                    }
                }

                #endregion

                #region Player Movement / Key Input

                //Left
                if (keyADown == true && playerCanNotMoveLeft == false && player1GridPos.X > 0)
                {
                    if (Keyboard.GetState().IsKeyDown((Keys)Enum.Parse(typeof(Keys), GlobalVar.OptionsArray[2], true)))
                    {
                        keyADown = false;
                        player1SpriteEffects = SpriteEffects.FlipHorizontally;
                        player1GridPos.X -= 1;
                    }
                }
                if ((milisecondsElapsedKeyA - milisecondExpirationKeyA) >= 0)
                {
                    if (keyADown == false)
                    {
                        keyADown = true;
                    }

                    milisecondsElapsedKeyA -= milisecondExpirationKeyA;
                }
                else if (Keyboard.GetState().IsKeyUp((Keys)Enum.Parse(typeof(Keys), GlobalVar.OptionsArray[2], true)))
                {
                    if (keyADown == false)
                    {
                        keyADown = true;
                    }
                }

                //Right
                if (keyDDown == true && playerCanNotMoveRight == false && player1GridPos.X < gameField.GetLength(0) - 1)
                {
                    if (Keyboard.GetState().IsKeyDown((Keys)Enum.Parse(typeof(Keys), GlobalVar.OptionsArray[3], true)))
                    {
                        keyDDown = false;
                        player1SpriteEffects = SpriteEffects.None;
                        player1GridPos.X += 1;
                    }
                }
                if ((milisecondsElapsedKeyD - milisecondExpirationKeyD) >= 0)
                {
                    if (keyDDown == false)
                    {
                        keyDDown = true;
                    }

                    milisecondsElapsedKeyD -= milisecondExpirationKeyD;
                }
                else if (Keyboard.GetState().IsKeyUp((Keys)Enum.Parse(typeof(Keys), GlobalVar.OptionsArray[3], true)))
                {
                    if (keyDDown == false)
                    {
                        keyDDown = true;
                    }
                }

                //Up
                if (keyWDown == true && playerCanNotMoveUp == false)
                {
                    if (Keyboard.GetState().IsKeyDown((Keys)Enum.Parse(typeof(Keys), GlobalVar.OptionsArray[4], true)))
                    {
                        if (player1Jump == false)
                        {
                            keyWDown = false;
                            player1GridPos.Y -= 1;
                            player1Jump = true;
                        }
                    }
                }
                else if (Keyboard.GetState().IsKeyUp((Keys)Enum.Parse(typeof(Keys), GlobalVar.OptionsArray[4], true)))
                {
                    if (keyWDown == false)
                    {
                        keyWDown = true;
                    }
                }

                //Player Jump Logic
                if (player1Jump == true && !playerCanNotMoveUp)
                {
                    if (milisecondsElapsedPlayerTime2 - milisecondsPlayerGravityTime2 >= 0)
                    {
                        if (!playerCanNotMoveDown && player1GridPos.Y < gameField.GetLength(1) - 1)
                        {
                            player1GridPos.Y += 1;
                        }
                        milisecondsElapsedPlayerTime2 = 0;
                        player1Jump = false;
                    }
                    milisecondsElapsedPlayerTime2 += gameTime.ElapsedGameTime.Milliseconds;
                }
                else
                {
                    if (player1GridPos.Y < gameField.GetLength(1) && !playerCanNotMoveDown)
                    {
                        if (milisecondsElapsedPlayerTime1 - milisecondsPlayerGravityTime1 >= 0)
                        {
                            if (player1GridPos.Y < gameField.GetLength(1) - 1 && !playerCanNotMoveDown)
                            {
                                player1GridPos.Y += 1;
                            }
                            milisecondsElapsedPlayerTime1 -= milisecondsPlayerGravityTime1;
                        }
                    }
                }

                #endregion

                //Tetromino Updates
                for (int k = 0; k < tetromino.Count - 1; k++)
                {
                    tetromino[k].Update(tetrominoGridPos[k], gridStartPos, GlobalVar.ScaleSize);
                }

                //Block Preview Position
                float farthestLeft = float.MaxValue, farthestRight = float.MinValue;

                for (int p = 0; p < tetromino[tetromino.Count - 1].getPositions().Length; p++)
                {
                    if (tetromino[tetromino.Count - 1].getPositions()[p].X < farthestLeft)
                    {
                        farthestLeft = tetromino[tetromino.Count - 1].getPositions()[p].X;
                    }
                    if (tetromino[tetromino.Count - 1].getPositions()[p].X > farthestRight)
                    {
                        farthestRight = tetromino[tetromino.Count - 1].getPositions()[p].X;
                    }
                }
                tetrominoGridPos[tetromino.Count - 1] = new Vector2((GlobalVar.ScreenSize.X - (((224 * GlobalVar.ScaleSize.X) / 2) + ((((farthestRight - farthestLeft) + 1) * (blockTexture.Width * GlobalVar.ScaleSize.X)) / 2))) / (blockTexture.Width * GlobalVar.ScaleSize.X), 4);
                tetromino[tetromino.Count - 1].Update(tetrominoGridPos[tetromino.Count - 1], new Vector2(0, gridStartPos.Y), GlobalVar.ScaleSize);

                rotationTestTetromino.Update(tetrominoGridPos[activeTetromino], gridStartPos, GlobalVar.ScaleSize);

                //Player Update
                player1.Update(new Vector2(gridStartPos.X + (player1GridPos.X * (blockTexture.Width * GlobalVar.ScaleSize.X)), gridStartPos.Y + (player1GridPos.Y * (blockTexture.Height * GlobalVar.ScaleSize.Y))), GlobalVar.ScaleSize, Color.White);

                //Tutorial Update (A little ghetto)
                if (GlobalVar.CurrentLevel.Equals("6"))
                {
                    tutorial.Update(gameTime, currentMouseState, previousMouseState);
                }

                //Store in Variable Last
                player1GridPosPrevious = player1GridPos;

                //checking to see if timer is 0
                if ((timer - milisecondsElapsedTime) / 1000 <= 0)
                {
                    gameOver = true;
                    gameWin = false;
                }

                tutorialFirstUpdateTrip = true;
            }
            else
            {
                tutorial.Update(gameTime, currentMouseState, previousMouseState);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Color backdropColor;

            //Hud Drawing
            spriteBatch.Draw(hudTexture, new Vector2(0, 0), null, Color.White, 0, new Vector2(0, 0), GlobalVar.ScaleSize, SpriteEffects.None, 0);
            //For Loop for drawing blocks to the screen
            for (int j = 0; j < gameField.GetLength(0); j++)
            {
                for (int i = 0; i < gameField.GetLength(1); i++)
                {
                    if (j < 3 || j > 22)
                    {
                        backdropColor = Color.Blue;
                    }
                    else
                    {
                        backdropColor = Color.White;
                    }

                    if (gameField[j, i] == false)
                    {
                        blockTexture = blankBlockTexture;
                        backdropColor.A = 50;
                    }
                    else
                    {
                        //HitBox Debug
                        backdropColor = Color.Blue;
                        blockTexture = fullBlockTexture;
                        //backdropColor.A = 25;
                    }

                    //Draws the block to the screen at the specified point based on the for loop
                    spriteBatch.Draw(blockTexture, new Vector2(gridStartPos.X + ((blockTexture.Width * GlobalVar.ScaleSize.X) * j), gridStartPos.Y + ((blockTexture.Height * GlobalVar.ScaleSize.Y) * i)), null, backdropColor, 0, new Vector2(0), GlobalVar.ScaleSize, SpriteEffects.None, 0);
                }
           
                spriteBatch.DrawString(font, "Next Block:" , new Vector2(1090 * GlobalVar.ScaleSize.X, 35 * GlobalVar.ScaleSize.Y), Color.SpringGreen, 0f, new Vector2(0, 0), GlobalVar.ScaleSize, SpriteEffects.None, 1f);

                //HighScore Draw
                if (!GlobalVar.CustomLevel)
                {
                    if (GlobalVar.HighScore[Int32.Parse(GlobalVar.CurrentLevel) - 1] > GlobalVar.Score)
                    {
                        spriteBatch.DrawString(font, "HIGH SCORE " + GlobalVar.HighScore[Int32.Parse(GlobalVar.CurrentLevel) - 1], new Vector2(1120 * GlobalVar.ScaleSize.X, 265 * GlobalVar.ScaleSize.Y), Color.SpringGreen, 0f, new Vector2(0, 0), GlobalVar.ScaleSize, SpriteEffects.None, 1f);
                    }
                    else
                    {
                        spriteBatch.DrawString(font, "HIGH SCORE " + GlobalVar.Score, new Vector2(1120 * GlobalVar.ScaleSize.X, 265 * GlobalVar.ScaleSize.Y), Color.SpringGreen, 0f, new Vector2(0, 0), GlobalVar.ScaleSize, SpriteEffects.None, 1f);
                    }
                }
                else
                {
                    spriteBatch.DrawString(font, "HIGH SCORE " + "N/A", new Vector2(1120 * GlobalVar.ScaleSize.X, 265 * GlobalVar.ScaleSize.Y), Color.SpringGreen, 0f, new Vector2(0, 0), GlobalVar.ScaleSize, SpriteEffects.None, 1f);
                }

                spriteBatch.DrawString(font, "Score: " + GlobalVar.Score, new Vector2(1120 * GlobalVar.ScaleSize.X, 305 * GlobalVar.ScaleSize.Y), Color.SpringGreen, 0f, new Vector2(0, 0), GlobalVar.ScaleSize, SpriteEffects.None, 1f);
                spriteBatch.DrawString(font, "Lines left: " + linesToClear, new Vector2(1120 * GlobalVar.ScaleSize.X, 345 * GlobalVar.ScaleSize.Y), Color.SpringGreen, 0f, new Vector2(0, 0), GlobalVar.ScaleSize, SpriteEffects.None, 1f);
                spriteBatch.DrawString(font, "Time left: " + (timer - milisecondsElapsedTime) / 1000, new Vector2(1120 * GlobalVar.ScaleSize.X, 385 * GlobalVar.ScaleSize.Y), Color.SpringGreen, 0f, new Vector2(0, 0), GlobalVar.ScaleSize, SpriteEffects.None, 1f);

                //Prints out Debug Info About the Block
                if (Boolean.Parse(GlobalVar.OptionsArray[11]) == true)
                {
                    spriteBatch.DrawString(font, "AbsLeft: " + absTetrominoBlockFarthestLeft.ToString() + "\n" + "AbsRight: " + absTetrominoBlockFarthestRight.ToString() + "\n" + "AbsDown: " + absTetrominoBlockFarthestDown.ToString() + debugStringData + "\nMove Left: " + !tetrominoCanNotMoveLeft + "\nMove Right: " + !tetrominoCanNotMoveRight + "\nMove Down: " + !tetrominoCanNotMoveDown, new Vector2(10 * GlobalVar.ScaleSize.X, 10 * GlobalVar.ScaleSize.Y), Color.White, 0f, new Vector2(0, 0), GlobalVar.ScaleSize, SpriteEffects.None, 1f);
                    spriteBatch.DrawString(font, "Player Pos: " + "X: " + player1GridPos.X + " Y: " + player1GridPos.Y + "\nMove Left: " + !playerCanNotMoveLeft + "\nMove Right: " + !playerCanNotMoveRight + "\nMove Down: " + !playerCanNotMoveDown + "\nMove Up: " + !playerCanNotMoveUp, new Vector2(10 * GlobalVar.ScaleSize.X, 225 * GlobalVar.ScaleSize.Y), Color.White, 0f, new Vector2(0, 0), GlobalVar.ScaleSize, SpriteEffects.None, 1f);

                    for (int p = 0; p < tetrominoHistory.Length; p++)
                    {
                        spriteBatch.DrawString(font, tetrominoHistory[p].ToString(), new Vector2((10 + (10 * p)) * GlobalVar.ScaleSize.X, 400 * GlobalVar.ScaleSize.Y), Color.White, 0f, new Vector2(0, 0), GlobalVar.ScaleSize, SpriteEffects.None, 1f);
                    }
                }

                //Debuging information for rotation

                //ghost rotationTestTetromino collsion Block Debug drawing
                //rotationTestTetromino.setColorTemp(Color.Black);
                //rotationTestTetromino.Draw(spriteBatch);

                //tetromino Draw
                blockTexture = fullBlockTexture;

                tetromino[tetromino.Count - 1].Draw(spriteBatch);

                //(DISABLED)
                /*
                for (int k = 0; k < tetromino.Count; k++)
                {
                   tetromino[k].Draw(spriteBatch);
                }
                 */

                player1.Draw(spriteBatch, player1SpriteEffects);

                spriteBatch.Draw(platformTexture, new Vector2(gridStartPos.X + (endPlatformGridPos.X * (blockTexture.Width * GlobalVar.ScaleSize.X)), gridStartPos.Y + (endPlatformGridPos.Y * (blockTexture.Height * GlobalVar.ScaleSize.Y))), null, Color.White, 0f, new Vector2(0), GlobalVar.ScaleSize, SpriteEffects.None, 1f);
                spriteBatch.Draw(platformTexture, new Vector2(gridStartPos.X + (startPlatformGridPos.X * (blockTexture.Width * GlobalVar.ScaleSize.X)), gridStartPos.Y + (startPlatformGridPos.Y * (blockTexture.Height * GlobalVar.ScaleSize.Y))), null, Color.White, 0f, new Vector2(0), GlobalVar.ScaleSize, SpriteEffects.None, 1f);

                //Tutorial Draw (A little ghetto)
                if (GlobalVar.CurrentLevel.Equals("6"))
                {
                    tutorial.Draw(spriteBatch);
                }
            }
        }

        public Boolean[,] getGameField()
        {
            return gameField;
        }

        public Boolean getGameWin()
        {
            return gameWin;
        }

        public Boolean getGameOver()
        {
            return gameOver;
        }

        public Boolean getTutorialActive()
        {
            if (GlobalVar.CurrentLevel.Equals("6"))
            {
                return tutorial.getIsActive();
            }
            else
            {
                return false;
            }
        }

        private void tetrominoHistoryAddItem(int tetrominoType)
        {
            for (int i = 3; i > 0; i--)
            {
                tetrominoHistory[i] = tetrominoHistory[i - 1];
            }
            tetrominoHistory[0] = tetrominoType;
        }

        public void resetGameVariables()
        {
            //Resets Timers
            milisecondsElapsedTetrominoTime = 0;
            milisecondsElapsedPlayerTime1 = 0;
            milisecondsElapsedTime = 0;

            //Reset Tutorial
            tutorialFirstUpdateTrip = false;
            tutorial.resetTutorial();

            //Level System Values
            levels.Update();
            levelModifier = levels.getLevelMod();
            timer = levels.getTimer() * 1000;
            linesToClear = levels.getLinesToClear();
            player1GridPos = levels.getPlayerPos();
            endPlatformGridPos = levels.getEndPlatPos();
            startPlatformGridPos = levels.getStartPlatPos();

            //Setting win/losing states
            gameWin = false;
            gameOver = false;
            gameField = new Boolean[26, 22];

            //Resting Player and Tetromino information
            player1Jump = false;
            activeTetromino = 0;
            lastActiveTetromino = 0;
            tetrominoRotation = 0;
            tetrominoLastRotation = 0;
            tetromino = new List<Tetromino>();
            tetrominoGridPos = new List<Vector2>();
            tetrominoHistory = new int[4];
            player1SpriteEffects = SpriteEffects.None;
            player1GridPosPrevious = player1GridPos;

            //Teromeno Set Up Reference 
            tetristype = random.Next(1, 8);
            tetrominoHistoryAddItem(tetristype);
            tetromino.Add(new Tetromino(tetristype, blockTexture));
            tetrominoGridPos.Add(new Vector2(13, 0));
            tetrominoGridPos.Add(new Vector2(12, 0));

            //Rotation Tetromino Test Set Up
            rotationTestTetromino = new Tetromino(tetristype, blockTexture);

            //Next Teromino Set Up Reference 
            tetristype = random.Next(1, 8);
            if (tetristype == tetrominoHistory[0])
            {
                tetristype = random.Next(1, 8);
            }
            tetrominoHistoryAddItem(tetristype);
            tetromino.Add(new Tetromino(tetristype, blockTexture));
            tetrominoGridPos.Add(new Vector2(28.5f, 4));

            //Temp levelSP
            //levelStartPoint = new Vector2(0, startPlatformGridPos.Y - 1);

            tetrominoBlockLastPositions = new Vector2[tetromino[activeTetromino].getPositions().Length];
            tetrominoBlockPositions = new Vector2[tetromino[activeTetromino].getPositions().Length];
            lastActiveTetromino = activeTetromino;

            //Player Set Up
            player1 = new Player(playerTexture);

            for (int i = 0; i < 3; i++)
            {
                gameField[(int)endPlatformGridPos.X + i, (int)endPlatformGridPos.Y] = true;
                gameField[(int)startPlatformGridPos.X + i, (int)startPlatformGridPos.Y] = true;
            }

        }
    }
}
