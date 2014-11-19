using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

/**
 * Evolo Field Manager to handle most game related operations for the player and tetromenos
 * Author: Dalton, Josh, Gavin
 * Version: 11/16/14
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
        private int milisecondsElapsedTetromenoTime = 0;
        private int milisecondsTetromenoFallTime = 300;
        private int milisecondsTetromenoLockDelayTime = 400;
        private int milisecondsElapsedPlayerTime = 0;
        private int milisecondsPlayerJumpTime = 500;
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

        //Tetromeno Variables
        private int tetristype = 5;
        private int absTetromenoBlockFarthestLeft, absTetromenoBlockFarthestRight, absTetromenoBlockFarthestDown;
        private int[] farthestTetromenoBlockLeft, farthestTetromenoBlockRight, farthestTetromenoBlockDown;
        private int activeTetromeno = 0;
        private int lastActiveTetromeno = 0;
        int tetromenoRotation = 0;
        private List<Tetromeno> tetromeno = new List<Tetromeno>();
        private List<Vector2> tetromenoGridPos = new List<Vector2>();
        private Vector2[] tetromenoBlocklastPositions, tetromenoBlockPositions;
        private Boolean tetromenoCanNotMoveRight, tetromenoCanNotMoveLeft, tetromenoCanNotMoveDown;
        private Boolean playerCanNotMoveRight, playerCanNotMoveLeft, playerCanNotMoveDown, playerCanNotMoveUp;

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
            tetromeno.Add(new Tetromeno(tetristype, blockTexture));
            tetromenoGridPos.Add(new Vector2(13, 0));

            //Temp levelSP
            levelStartPoint = new Vector2(1, 17);

            tetromenoBlocklastPositions = new Vector2[tetromeno[activeTetromeno].getPositions().Length];
            tetromenoBlockPositions = new Vector2[tetromeno[activeTetromeno].getPositions().Length];

            lastActiveTetromeno = activeTetromeno;

            //Player Set Up
            player1 = new Player(playerTexture);
            player1GridPos = levelStartPoint;

        }

        public void Update(GameTime gameTime)
        {

            #region Local Variable Reseting

            //Adds time since last update to the elapsed time for the tetromeno
            milisecondsElapsedTetromenoTime += gameTime.ElapsedGameTime.Milliseconds;
            milisecondsElapsedPlayerTime += gameTime.ElapsedGameTime.Milliseconds;

            //Adjusts the grid Starting Postion for resolution changes & such
            gridStartPos = new Vector2((GlobalVar.ScreenSize.X / 2) - (((blockTexture.Width * GlobalVar.ScaleSize.X) * gameField.GetLength(0)) / 2), (GlobalVar.ScreenSize.Y / 2) - (((blockTexture.Height * GlobalVar.ScaleSize.Y) * (gameField.GetLength(1) + 2)) / 2));

            //Resets tetromeno Movement Booleans
            tetromenoCanNotMoveRight = false;
            tetromenoCanNotMoveLeft = false;
            tetromenoCanNotMoveDown = false;

            playerCanNotMoveRight = false;
            playerCanNotMoveLeft = false;
            playerCanNotMoveDown = false;
            playerCanNotMoveUp = false;

            //Resets absoluote tetromeno Positions
            absTetromenoBlockFarthestLeft = gameField.GetLength(0);
            absTetromenoBlockFarthestRight = 0;
            absTetromenoBlockFarthestDown = 0;

            #endregion

            #region Tetromeno Collision Box Logic

            //Temporary Holding Variables
            int tempX, tempY;

            for (int p = 0; p < tetromeno[activeTetromeno].getPositions().Length; p++)
            {
                //Clears the hit boxes from the previous Positions
                if (lastActiveTetromeno == activeTetromeno)
                {
                    gameField[(int)tetromenoBlocklastPositions[p].X, (int)(tetromenoBlocklastPositions[p].Y)] = false;
                }

                //Sets holding X value to the input position respective from the Tetromeno Object, and if less than 0 corrects the issue
                if ((int)(tetromeno[activeTetromeno].getPositions()[p].X) < 0)
                {
                    tempX = 0;
                }
                else
                {
                    tempX = (int)(tetromeno[activeTetromeno].getPositions()[p].X);
                }

                //Sets holding Y value to the input position respective from the Tetromeno Object, and if less than 0 corrects the issue
                if ((int)(tetromeno[activeTetromeno].getPositions()[p].Y) < 0)
                {
                    tempY = 0;
                }
                else
                {
                    tempY = (int)(tetromeno[activeTetromeno].getPositions()[p].Y);
                }

                //Sets the corrected local holding array for the postions of the blocks from the tetromeno
                tetromenoBlockPositions[p] = new Vector2(tempX, tempY);

                //Sets the hit boxes for the tetromeno at the current Positon
                gameField[(int)tetromenoBlockPositions[p].X, (int)tetromenoBlockPositions[p].Y] = true;

                //Resets the variable
                tetromenoBlocklastPositions[p] = new Vector2(tempX, tempY);
            }

            //Resets the variable
            lastActiveTetromeno = activeTetromeno;

            #endregion

            #region Teromeno Block Constraint Information


            //Local Varaibles used in calculations
            Boolean yPosAlreadyInList = false;
            List<int> yValuesUsed = new List<int>();
            List<List<int>> xValuesUsed = new List<List<int>>();

            //Finds all the unqiue Y Values in tetromeno's indivdual blocks
            for (int w = 0; w < tetromenoBlockPositions.Length; w++)
            {
                List<int> yPosSublist = new List<int>();

                if (yValuesUsed != null)
                {
                    for (int j = 0; j < yValuesUsed.Count; j++)
                    {
                        if (tetromenoBlockPositions[w].Y == yValuesUsed[j])
                        {
                            yPosAlreadyInList = true;
                        }
                    }
                    if (yPosAlreadyInList == false)
                    {
                        yValuesUsed.Add((int)tetromenoBlockPositions[w].Y);

                    }
                }
                else
                {
                    yValuesUsed.Add((int)tetromenoBlockPositions[w].Y);

                }
                yPosAlreadyInList = false;
            }

            yValuesUsed.Sort();

            //Finds the X values that exist relative to the Y values in the Tetrimino
            for (int k = 0; k < yValuesUsed.Count; k++)
            {
                List<int> xHoldingValues = new List<int>();

                for (int i = 0; i < tetromenoBlockPositions.Length; i++)
                {
                    if (tetromenoBlockPositions[i].Y == yValuesUsed[k])
                    {
                        xHoldingValues.Add((int)tetromenoBlockPositions[i].X);
                    }
                }
                xValuesUsed.Add(xHoldingValues);
            }

            //Debug Info Write
            debugStringData = "\n" + "yValuesUsed:";
            for (int m = 0; m < yValuesUsed.Count; m++)
            {
                xValuesUsed[m].Sort();

                debugStringData += "\nY" + (m + 1) + ": " + yValuesUsed[m];

                debugStringData += " X's:";
                for (int j = 0; j < xValuesUsed[m].Count; j++)
                {
                    debugStringData += " " + xValuesUsed[m][j] + ",";
                }

            }

            //Sets array index size for the farthest blocks
            farthestTetromenoBlockLeft = new int[xValuesUsed.Count];
            farthestTetromenoBlockRight = new int[xValuesUsed.Count];
            farthestTetromenoBlockDown = new int[xValuesUsed.Count];

            //Checks the farthest x Values for their repsective Y's in the List, aswell as the farthestdown Y Value
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

                farthestTetromenoBlockLeft[p] = mostLeftXOnY;
                farthestTetromenoBlockRight[p] = mostRightXOnY;

                //Checks for Overall Max X Values
                if (mostRightXOnY > absTetromenoBlockFarthestRight)
                {
                    absTetromenoBlockFarthestRight = mostRightXOnY;
                }

                if (mostLeftXOnY < absTetromenoBlockFarthestLeft)
                {
                    absTetromenoBlockFarthestLeft = mostLeftXOnY;
                }

                //Checks for Farthest Down Y Value
                if (yValuesUsed[p] > absTetromenoBlockFarthestDown)
                {
                    absTetromenoBlockFarthestDown = yValuesUsed[p];
                }

                farthestTetromenoBlockDown[p] = yValuesUsed[p];
            }

            #endregion

            #region Teromeno Collision Detection

            //(Left / Right)
            for (int i = 0; i < farthestTetromenoBlockLeft.Length; i++)
            {
                if (farthestTetromenoBlockLeft[i] > 0)
                {
                    if (gameField[farthestTetromenoBlockLeft[i] - 1, yValuesUsed[i]] == true)
                    {
                        tetromenoCanNotMoveLeft = true;
                    }
                }

                if (farthestTetromenoBlockRight[i] < gameField.GetLength(0))
                {
                    if (gameField[farthestTetromenoBlockRight[i] + 1, yValuesUsed[i]] == true)
                    {
                        tetromenoCanNotMoveRight = true;
                    }
                }
            }

            //(Down)
            for (int q = 0; q < absTetromenoBlockFarthestRight - absTetromenoBlockFarthestLeft; q++)
            {
                if (absTetromenoBlockFarthestDown < gameField.GetLength(1) - 1)
                {
                    if (gameField[absTetromenoBlockFarthestLeft + q, absTetromenoBlockFarthestDown + 1] == true)
                    {
                        tetromenoCanNotMoveDown = true;
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
            if (player1GridPos.X - 1 >= 0 && player1GridPos.X + 1 < gameField.GetLength(0)  && (player1GridPos.Y >= 0 && player1GridPos.Y < gameField.GetLength(1)))
            {
                //Right
                if(gameField[(int)player1GridPos.X + 1, (int)player1GridPos.Y] == true)
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

            #region Tetromeno Keyboard Input

            //Keyboard Input (Left Key)
            if (absTetromenoBlockFarthestLeft > 3 && tetromenoCanNotMoveLeft == false)
            {
                if (keyLeftDown == true)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    {
                        keyLeftDown = false;
                        tetromenoGridPos[activeTetromeno] = new Vector2(tetromenoGridPos[activeTetromeno].X - 1, tetromenoGridPos[activeTetromeno].Y);
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
            if (absTetromenoBlockFarthestRight < gameField.GetLength(0) - 4 && tetromenoCanNotMoveRight == false)
            {
                if (keyRightDown == true)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    {
                        keyRightDown = false;
                        tetromenoGridPos[activeTetromeno] = new Vector2(tetromenoGridPos[activeTetromeno].X + 1, tetromenoGridPos[activeTetromeno].Y);
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
            if (tetristype != 5)
            {
                if (keyUpDown == true)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    {
                        keyUpDown = false;

                        //Sets current roation value based off 0 - 3
                        if (tetromenoRotation < 3)
                            tetromenoRotation++;
                        else
                            tetromenoRotation = 0;

                        //Sends roation value to the currently active tetromeno
                        tetromeno[activeTetromeno].setRotation(tetromenoRotation);
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

            #endregion

            #region Tetromeno Movement Down

            //Checks to see if the block is able to move down 1 gridPos or not
            if (absTetromenoBlockFarthestDown < gameField.GetLength(1) - 1 && tetromenoCanNotMoveDown == false)
            {
                //Checks if the block can move down based off the elapsed time, and makes up for any lost movement since the last update
                while (milisecondsElapsedTetromenoTime - milisecondsTetromenoFallTime >= 1)
                {
                    //Move the tetromeno down one and takes away 1 movement worth of time from the elapsed time
                    tetromenoGridPos[activeTetromeno] = new Vector2(tetromenoGridPos[activeTetromeno].X, tetromenoGridPos[activeTetromeno].Y + 1);
                    milisecondsElapsedTetromenoTime -= milisecondsTetromenoFallTime;
                }
            }
            else
            {
                //Checks for block lock delay before locking in place and spawning new block
                if (milisecondsElapsedTetromenoTime - milisecondsTetromenoLockDelayTime >= 1)
                {
                    //Setting various variables required to spawn a new clean tetromeno
                    tetristype = random.Next(1, 7);
                    activeTetromeno += 1;
                    tetromeno.Add(new Tetromeno(tetristype, blockTexture));
                    tetromenoGridPos.Add(new Vector2(13, 0));
                    milisecondsElapsedTetromenoTime -= milisecondsTetromenoLockDelayTime;
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
            for (int k = 0; k < tetromeno.Count; k++)
            {
                tetromeno[k].Update(tetromenoGridPos[k], gridStartPos, GlobalVar.ScaleSize);
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
                        backdropColor = Color.LightSeaGreen;
                    else if (i < 2)
                        backdropColor = Color.LightCoral;
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
                        spriteBatch.DrawString(font, "AbsLeft: " + absTetromenoBlockFarthestLeft.ToString() + "\n" + "AbsRight: " + absTetromenoBlockFarthestRight.ToString() + "\n" + "AbsDown: " + absTetromenoBlockFarthestDown.ToString() + debugStringData + "\nMove Left: " + !tetromenoCanNotMoveLeft + "\nMove Right: " + !tetromenoCanNotMoveRight + "\nMove Down: " + !tetromenoCanNotMoveDown, new Vector2(10 * GlobalVar.ScaleSize.X, 10 * GlobalVar.ScaleSize.Y), Color.White, 0f, new Vector2(0, 0), GlobalVar.ScaleSize, SpriteEffects.None, 1f);
                        spriteBatch.DrawString(font, "Player Pos: " + "X: " + player1GridPos.X + " Y: " + player1GridPos.Y + "\nMove Left: " + !playerCanNotMoveLeft + "\nMove Right: " + !playerCanNotMoveRight + "\nMove Down: " + !playerCanNotMoveDown + "\nMove Up: " + !playerCanNotMoveUp, new Vector2(10 * GlobalVar.ScaleSize.X, 225 * GlobalVar.ScaleSize.Y), Color.White, 0f, new Vector2(0, 0), GlobalVar.ScaleSize, SpriteEffects.None, 1f);
                    }
                }
            }

            //Tetromeno Draw
            for (int k = 0; k < tetromeno.Count; k++)
            {
                tetromeno[k].Draw(spriteBatch);
            }

            player1.Draw(spriteBatch, player1SpriteEffects);

            //Store in Variable Last
            player1GridPosPrevious = player1GridPos;
        }

        public Boolean[,] getGameField()
        {
            return gameField;
        }

    }
}
