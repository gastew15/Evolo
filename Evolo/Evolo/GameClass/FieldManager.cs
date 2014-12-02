using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

/**
 * Evolo Field Manager to handle most game related operations for the player and tetrominos
 * Author: Dalton, Josh, Gavin
 * Version: 11/24/14
 */

namespace Evolo.GameClass
{
    class FieldManager
    {
        #region Variables

        //Gamefield Variables
        private Boolean[,] gameField = new Boolean[26, 22];
        private Vector2 gridStartPos, levelStartPoint, levelEndPoint;

        //Timing Variables
        private int milisecondsElapsedtetrominoTime = 0;
        private int milisecondstetrominoFallTime = 300;
        private int milisecondstetrominoLockDelayTime = 400;
        private int milisecondsElapsedPlayerTime = 0;
        private int milisecondsPlayerGravityTime = 200;

        //Keyboard Variables / Misc
        private bool keyLeftDown, keyRightDown, keyUpDown;
        private bool keyADown, keyDDown, keyWDown;
        private String debugStringData = "";
        private Random random = new Random();

        //Content Variables
        private Texture2D blockTexture, playerTexture;

        //Player Variables
        private SpriteEffects player1SpriteEffects;
        private bool player1Jump = false;
        private Player player1;
        private Vector2 player1GridPos, player1GridPosPrevious;
        private Boolean playerCanNotMoveRight, playerCanNotMoveLeft, playerCanNotMoveDown, playerCanNotMoveUp;

        //tetromino Variables
        private int tetristype = 5;
        private int absTetrominoBlockFarthestLeft, absTetrominoBlockFarthestRight, absTetrominoBlockFarthestDown, absTetrominoBlockFarthestUp;
        private int[] farthestTetrominoBlockLeft, farthestTetrominoBlockRight, farthestTetrominoBlockDown, farthestTetrominoBlockUp;
        private int activeTetromino = 0;
        private int lastActiveTetromino = 0;
        int tetrominoRotation = 0;
        int tetrominoLastRotation = 0;
        private List<Tetromino> tetromino = new List<Tetromino>();
        private List<Vector2> tetrominoGridPos = new List<Vector2>();
        private int[] tetrominoHistory = new int[4];
        private Vector2 tetrominoLastGridPos = new Vector2();
        private Vector2[] tetrominoBlockLastPositions, tetrominoBlockPositions;
        private Boolean tetrominoCanNotMoveRight, tetrominoCanNotMoveLeft, tetrominoCanNotMoveDown, tetrominoCanNotMoveUp;
        private Boolean tetrominoCanRotate;

        #endregion

        public FieldManager()
        {

        }

        public void Intilize()
        {
            player1SpriteEffects = SpriteEffects.None;

            player1GridPosPrevious = player1GridPos;

        }

        public void LoadContent(ContentManager Content)
        {
            blockTexture = Content.Load<Texture2D>("Sprites and pictures/BasicBlock2");
            playerTexture = Content.Load<Texture2D>("Sprites and pictures/CharacterTest");

            //Teromeno Set Up Reference 
            tetristype = random.Next(1, 8);
            tetrominoHistoryAddItem(tetristype);
            tetromino.Add(new Tetromino(tetristype, blockTexture));
            tetrominoGridPos.Add(new Vector2(13, 0));
            tetristype = random.Next(1, 8);
            if (tetristype == tetrominoHistory[0])
            {
                tetristype = random.Next(1, 8);
            }

            tetrominoHistoryAddItem(tetristype);
            tetromino.Add(new Tetromino(tetristype, blockTexture));
            tetrominoGridPos.Add(new Vector2(28.5f, 4));

            //Temp levelSP
            levelStartPoint = new Vector2(1, 17);

            tetrominoBlockLastPositions = new Vector2[tetromino[activeTetromino].getPositions().Length];
            tetrominoBlockPositions = new Vector2[tetromino[activeTetromino].getPositions().Length];

            lastActiveTetromino = activeTetromino;

            //Player Set Up
            player1 = new Player(playerTexture);
            player1GridPos = levelStartPoint;

        }

        public void Update(GameTime gameTime)
        {

            #region Local Variable Reseting

            //Adds time since last update to the elapsed time for the tetromino
            milisecondsElapsedtetrominoTime += gameTime.ElapsedGameTime.Milliseconds;
            milisecondsElapsedPlayerTime += gameTime.ElapsedGameTime.Milliseconds;

            //Adjusts the grid Starting Postion for resolution changes & such
            gridStartPos = new Vector2((GlobalVar.ScreenSize.X / 2) - (((blockTexture.Width * GlobalVar.ScaleSize.X) * gameField.GetLength(0)) / 2), (GlobalVar.ScreenSize.Y / 2) - (((blockTexture.Height * GlobalVar.ScaleSize.Y) * (gameField.GetLength(1) + 2)) / 2));

            //Resets tetromino Movement Booleans
            tetrominoCanNotMoveRight = false;
            tetrominoCanNotMoveLeft = false;
            tetrominoCanNotMoveDown = false;
            tetrominoCanNotMoveUp = false;
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
                if (farthestTetrominoBlockUp[i] > 0)
                {
                    if (gameField[xValuesUsedForY[i], farthestTetrominoBlockUp[i] - 1] == true)
                    {
                        tetrominoCanNotMoveUp = true;
                    }
                }

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
                player1GridPos.Y = player1GridPosPrevious.Y;
            }

            //Check
            if (player1GridPos.X - 1 >= 0 && player1GridPos.X + 1 < gameField.GetLength(0) && (player1GridPos.Y >= 0 && player1GridPos.Y < gameField.GetLength(1)))
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
            if (player1GridPos.X >= 0 && player1GridPos.X + 1 < gameField.GetLength(0) && (player1GridPos.Y - 1 >= 0 && player1GridPos.Y + 1 < gameField.GetLength(1)))
            {
                //Down
                if (gameField[(int)player1GridPos.X, (int)player1GridPos.Y + 1] == true)
                {
                    playerCanNotMoveDown = true;
                }

                //Up
                if (gameField[(int)player1GridPos.X, (int)player1GridPos.Y - 1] == true)
                {
                    playerCanNotMoveUp = true;
                }
            }

            #endregion

            #region tetromino Keyboard Input

            //Keyboard Input (Left Key)
            if (absTetrominoBlockFarthestLeft > 3 && tetrominoCanNotMoveLeft == false)
            {
                if (keyLeftDown == true)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    {
                        keyLeftDown = false;
                        tetrominoGridPos[activeTetromino] = new Vector2(tetrominoGridPos[activeTetromino].X - 1, tetrominoGridPos[activeTetromino].Y);
                    }
                }
                else if (Keyboard.GetState().IsKeyUp(Keys.Left))
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
                    if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    {
                        keyRightDown = false;
                        tetrominoGridPos[activeTetromino] = new Vector2(tetrominoGridPos[activeTetromino].X + 1, tetrominoGridPos[activeTetromino].Y);
                    }
                }
                else if (Keyboard.GetState().IsKeyUp(Keys.Right))
                {
                    if (keyRightDown == false)
                    {
                        keyRightDown = true;
                    }
                }
            }

            //Keyboard Input (Up Key) *Rotation
            if (tetromino[activeTetromino].getTetrisType() != 5)
            {
                if (keyUpDown == true)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    {
                        keyUpDown = false;

                        //Sets current roation value based off 0 - 3
                        if (tetrominoRotation < 3)
                            tetrominoRotation++;
                        else
                            tetrominoRotation = 0;

                        //Sends roation value to the currently active tetromino
                        tetromino[activeTetromino].setRotation(tetrominoRotation);
                    }
                }
                else if (Keyboard.GetState().IsKeyUp(Keys.Up))
                {
                    if (keyUpDown == false)
                    {
                        keyUpDown = true;
                    }
                }
            }

            //Checks for rotation out of array
            for (int a = 0; a < tetromino[activeTetromino].getPositions().Length; a++)
            {
                if ((tetromino[activeTetromino].getPositions()[a].X >= 3 && tetromino[activeTetromino].getPositions()[a].X <= gameField.GetLength(0) - 4) && (tetromino[activeTetromino].getPositions()[a].Y >= 0 && tetromino[activeTetromino].getPositions()[a].Y <= gameField.GetLength(1) - 1))
                {
                    if (!(gameField[(int)tetromino[activeTetromino].getPositions()[a].X, (int)tetromino[activeTetromino].getPositions()[a].Y] == true))
                    {
                        tetrominoCanRotate = false;
                    }
                }
                else
                {
                    tetrominoCanRotate = false;
                }
            }

            //Fixes probelm if it occurs
            if (tetrominoCanRotate == false)
            {
                tetrominoGridPos[activeTetromino] = tetrominoLastGridPos;
                tetrominoRotation = tetrominoLastRotation;
            }

            #endregion

            #region tetromino Movement Down

            //Checks to see if the block is able to move down 1 gridPos or not
            if (absTetrominoBlockFarthestDown < gameField.GetLength(1) - 1 && tetrominoCanNotMoveDown == false)
            {
                //Checks if the block can move down based off the elapsed time, and makes up for any lost movement since the last update
                while (milisecondsElapsedtetrominoTime - milisecondstetrominoFallTime >= 1)
                {
                    //Move the tetromino down one and takes away 1 movement worth of time from the elapsed time
                    tetrominoGridPos[activeTetromino] = new Vector2(tetrominoGridPos[activeTetromino].X, tetrominoGridPos[activeTetromino].Y + 1);
                    milisecondsElapsedtetrominoTime -= milisecondstetrominoFallTime;
                }
            }
            else
            {
                //Checks for block lock delay before locking in place and spawning new block
                if (milisecondsElapsedtetrominoTime - milisecondstetrominoLockDelayTime >= 1)
                {
                    //Setting various variables required to spawn a new clean tetromin

                    bool repeat = false;
                    int index = 0;
                    do
                    {
                        tetristype = random.Next(1, 8);
                        for (int i = 0; i < tetrominoHistory.Length; i++)
                        {
                            if (tetristype == tetrominoHistory[i])
                                repeat = true;

                        }
                        index++;
                    }
                    while (repeat == true && index != 3);

                    tetrominoHistoryAddItem(tetristype);

                    activeTetromino += 1;
                    tetromino.Add(new Tetromino(tetristype, blockTexture));
                    tetrominoGridPos.Add(new Vector2(28.5f, 4));
                    tetrominoGridPos[activeTetromino] = new Vector2(13, 0);
                    milisecondsElapsedtetrominoTime -= milisecondstetrominoLockDelayTime;
                }
            }

            #endregion

            #region Player Movement / Key Input

            //Left
            if (keyADown == true && playerCanNotMoveLeft == false && player1GridPos.X > 0)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    keyADown = false;
                    player1SpriteEffects = SpriteEffects.FlipHorizontally;
                    player1GridPos.X -= 1;
                }
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.A))
            {
                if (keyADown == false)
                {
                    keyADown = true;
                }
            }

            //Right
            if (keyDDown == true && playerCanNotMoveRight == false && player1GridPos.X < gameField.GetLength(0) - 1)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    keyDDown = false;
                    player1SpriteEffects = SpriteEffects.None;
                    player1GridPos.X += 1;
                }
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.D))
            {
                if (keyDDown == false)
                {
                    keyDDown = true;
                }
            }

            //Up
            if (keyWDown == true && playerCanNotMoveUp == false)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    if (player1Jump == false)
                    {
                        keyWDown = false;
                        player1GridPos.Y -= 1;
                        player1Jump = true;
                    }
                }
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.W))
            {
                if (keyWDown == false)
                {
                    keyWDown = true;
                }
            }

            //Player Jump Logic
            if (player1Jump == true)
            {
                if (gameTime.TotalGameTime.Milliseconds % 220 == 0)
                {
                    if (playerCanNotMoveDown == false)
                    {
                        player1GridPos.Y += 1;
                    }
                    player1Jump = false;

                    //milisecondsElapsedPlayerTime -= milisecondsPlayerJumpTime;
                }
            }
            else
            {
                if (player1GridPos.Y < gameField.GetLength(1) - 1 && playerCanNotMoveDown == false)
                {
                    while (milisecondsElapsedPlayerTime - milisecondsPlayerGravityTime >= 0)
                    {
                        player1GridPos.Y += 1;

                        milisecondsElapsedPlayerTime -= milisecondsPlayerGravityTime;
                    }
                }
            }

            #endregion

            //Termeno Updates
            for (int k = 0; k < tetromino.Count; k++)
            {
                tetromino[k].Update(tetrominoGridPos[k], gridStartPos, GlobalVar.ScaleSize);
            }

            //Player Update
            player1.Update(new Vector2(gridStartPos.X + (player1GridPos.X * (blockTexture.Width * GlobalVar.ScaleSize.X)), gridStartPos.Y + (player1GridPos.Y * (blockTexture.Height * GlobalVar.ScaleSize.Y))), GlobalVar.ScaleSize, Color.White);
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {

            //TEMP BACKDROP FOR POS TESTING
            Color backdropColor;
            //For Loop for drawing blocks to the screen
            for (int j = 0; j < gameField.GetLength(0); j++)
            {
                for (int i = 0; i < gameField.GetLength(1); i++)
                {
                    if (j < 3 || j > 22)
                        backdropColor = Color.LightGray;
                    else if (i < 2)
                        backdropColor = Color.DarkGray;
                    else
                        backdropColor = Color.White;

                    //HitBox Debug
                    /*
                    if (gameField[j, i] == true)
                    {
                        backdropColor = Color.Blue;
                    }
                    */

                    //Draws the block to the screen at the specified point based on the for loop
                    spriteBatch.Draw(blockTexture, new Vector2(gridStartPos.X + ((blockTexture.Width * GlobalVar.ScaleSize.X) * j), gridStartPos.Y + ((blockTexture.Height * GlobalVar.ScaleSize.Y) * i)), null, backdropColor, 0, new Vector2(0), GlobalVar.ScaleSize, SpriteEffects.None, 0);

                    //spriteBatch.DrawString(SeqoeUIMonoNormal, "FPS: " + fpsManager.getFPS(), new Vector2((GlobalVar.ScreenSize.X - (SeqoeUIMonoNormal.MeasureString("FPS: " + fpsManager.getFPS()).X) * GlobalVar.ScaleSize.X) - 10, (5 * GlobalVar.ScaleSize.Y)), Color.White, 0f, new Vector2(0, 0), GlobalVar.ScaleSize, SpriteEffects.None, 1f);

                    //Prints out Debug Info About the Block
                    if (Boolean.Parse(GlobalVar.OptionsArray[9]) == true)
                    {
                        spriteBatch.DrawString(font, "AbsLeft: " + absTetrominoBlockFarthestLeft.ToString() + "\n" + "AbsRight: " + absTetrominoBlockFarthestRight.ToString() + "\n" + "AbsDown: " + absTetrominoBlockFarthestDown.ToString() + debugStringData + "\nMove Left: " + !tetrominoCanNotMoveLeft + "\nMove Right: " + !tetrominoCanNotMoveRight + "\nMove Down: " + !tetrominoCanNotMoveDown, new Vector2(10 * GlobalVar.ScaleSize.X, 10 * GlobalVar.ScaleSize.Y), Color.White, 0f, new Vector2(0, 0), GlobalVar.ScaleSize, SpriteEffects.None, 1f);
                        spriteBatch.DrawString(font, "Player Pos: " + "X: " + player1GridPos.X + " Y: " + player1GridPos.Y + "\nMove Left: " + !playerCanNotMoveLeft + "\nMove Right: " + !playerCanNotMoveRight + "\nMove Down: " + !playerCanNotMoveDown + "\nMove Up: " + !playerCanNotMoveUp, new Vector2(10 * GlobalVar.ScaleSize.X, 225 * GlobalVar.ScaleSize.Y), Color.White, 0f, new Vector2(0, 0), GlobalVar.ScaleSize, SpriteEffects.None, 1f);
                    }
                }
            }

            //tetromino Draw
            for (int k = 0; k < tetromino.Count; k++)
            {
                tetromino[k].Draw(spriteBatch);
            }

            player1.Draw(spriteBatch, player1SpriteEffects);

            //Store in Variable Last
            player1GridPosPrevious = player1GridPos;
        }

        public Boolean[,] getGameField()
        {
            return gameField;
        }

        private void tetrominoHistoryAddItem(int tetromino)
        {
            for(int i = 3; i > 0; i--)
            {
                tetrominoHistory[i] = tetrominoHistory[i - 1];
            }
            tetrominoHistory[0] = tetromino;
        }

    }
}
