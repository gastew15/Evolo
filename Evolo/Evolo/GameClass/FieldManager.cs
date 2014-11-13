#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
#endregion

namespace Evolo.GameClass
{
    class FieldManager
    {

        //Gamefield Boolean array
        private Boolean[,] gameField = new Boolean[26, 22];
        private Texture2D blockTexture, playerTexture;
        private Vector2 gridStartPos, player1GridPos, levelStartPoint, levelEndPoint;
        private SpriteEffects player1SpriteEffects;
        private Random random = new Random();
        private int tetristype = 5;
        private bool keyLeftDown, keyRightDown, keyUpDown;
        private bool keyADown, keyDDown, keyWDown;
        private bool player1Jump = false;

        private int milisecondsElapsedTetromenoTime = 0;
        private int milisecondsTetromenoFallTime = 300;
        private int milisecondsTetromenoLockDelayTime = 400;

        private int farthestLeft, farthestRight, farthestUp, farthestDown;
        private Vector2[] farthestBlocks;
        private int activeTetromeno = 0;
        private int lastActiveTetromeno = 0;
        int rotation = 0;

        private List <Tetromeno> tetromeno = new List<Tetromeno>();
        private List <Vector2> tetromenoGridPos = new List<Vector2>();
        private Vector2[] lastPosition, position;
        private int sameYPosNum;
        private Boolean canNotMoveRight, canNotMoveLeft;

        private String testingStringData = "";

        //Temp
        private Player player1;

        public FieldManager()
        {

        }

        public void Intilize()
        {
            player1SpriteEffects = SpriteEffects.None;
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

            lastPosition = new Vector2[tetromeno[activeTetromeno].getPositions().Length];
            position = new Vector2[tetromeno[activeTetromeno].getPositions().Length];
            sameYPosNum = 0;

            lastActiveTetromeno = activeTetromeno;

            for (int i = 0; i < 22; i++)
            {
                gameField[17, i] = true;
            }

            //Player Set Up
            //player1 = new Player(playerTexture);
            //player1GridPos = levelStartPoint;
 
        }

        public void Update(GameTime gameTime)
        {
            milisecondsElapsedTetromenoTime += gameTime.ElapsedGameTime.Milliseconds;
            gridStartPos = new Vector2((GlobalVar.ScreenSize.X / 2) - (((blockTexture.Width * GlobalVar.ScaleSize.X) * gameField.GetLength(0)) / 2), (GlobalVar.ScreenSize.Y / 2) - (((blockTexture.Height * GlobalVar.ScaleSize.Y) * (gameField.GetLength(1) + 2)) / 2));
            canNotMoveRight = false;
            canNotMoveLeft = false;

            /*
            * Tetromeno Put into Grid
            */
            {
                //gameField = new Boolean[26, 22];
                int xstuff, ystuff;

                for (int p = 0; p < tetromeno[activeTetromeno].getPositions().Length; p++)
                {
                    if(lastActiveTetromeno == activeTetromeno)
                        gameField[(int)lastPosition[p].X, (int)(lastPosition[p].Y)] = false;

                    //gameField[13, 0] = true;
                    if ((int)(tetromeno[activeTetromeno].getPositions()[p].X) < 0)
                        xstuff = 0;
                    else
                        xstuff = (int)(tetromeno[activeTetromeno].getPositions()[p].X);
                    if ((int)(tetromeno[activeTetromeno].getPositions()[p].Y) < 0)
                        ystuff = 0;
                    else
                        ystuff = (int)(tetromeno[activeTetromeno].getPositions()[p].Y);

                    position[p] = new Vector2(xstuff, ystuff);

                    gameField[(int)position[p].X, (int)position[p].Y] = true;

                    /*
                     * Testing Area
                     */


                    /*
                     * 
                     */

                    lastPosition[p] = new Vector2(xstuff, ystuff);
                }
            }

            lastActiveTetromeno = activeTetromeno;

            /*
             * Tetromeno Movement Keys
             */
            {
                //Finds bounds of blocks to use for collisions
                farthestBlocks = tetromeno[activeTetromeno].getPositions();
                farthestLeft = gameField.GetLength(0) - 1;
                farthestRight = 0;
                farthestUp = gameField.GetLength(1) - 1;
                farthestDown = 0;

                for (int i = 0; i < farthestBlocks.Length; i++)
                {
                    //Left
                    if (farthestBlocks[i].X < farthestLeft)
                    {
                        farthestLeft = (int)farthestBlocks[i].X;
                    }
                    //Right
                    if (farthestBlocks[i].X > farthestRight)
                    {
                        farthestRight = (int)farthestBlocks[i].X;
                    }
                    //Up (Not being Used ATM)
                    if (farthestBlocks[i].Y < farthestUp)
                    {
                        farthestUp = (int)farthestBlocks[i].Y;
                    }
                    //Down
                    if (farthestBlocks[i].Y > farthestDown)
                    {
                        farthestDown = (int)farthestBlocks[i].Y;
                    }
                }

                /*
                 * Testing
                 */
                
                //Find Y Values
                Boolean yPosAlreadyInList = false;
                List<int> yValuesUsed = new List<int> { };

                for (int w = 0; w < position.Length; w++)
                {
                    if (yValuesUsed != null)
                    {
                        for (int j = 0; j < yValuesUsed.Count; j++)
                        {
                            if (position[w].Y == yValuesUsed[j])
                            {
                                yPosAlreadyInList = true;
                            }
                        }
                        if (yPosAlreadyInList == false)
                        {
                            yValuesUsed.Add((int)position[w].Y);
                        }
                    }
                    else
                    {
                        yValuesUsed.Add((int)position[w].Y);
                    }
                    yPosAlreadyInList = false;
                }



                testingStringData = "\n" + "yValuesUsed:";
                for (int m = 0; m < yValuesUsed.Count; m++)
                {
                    testingStringData += "\n" + (m + 1) + ": " + yValuesUsed[m];
                }

                /*
                 * 
                 */

                //Left
                if (farthestLeft > 3)
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

                //Right
                if (farthestRight < gameField.GetLength(0) - 4 && canNotMoveRight == false)
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

                //Down
                if (farthestDown < gameField.GetLength(1) - 1)
                {
                    while (milisecondsElapsedTetromenoTime - milisecondsTetromenoFallTime >= 1)
                    {
                        tetromenoGridPos[activeTetromeno] = new Vector2(tetromenoGridPos[activeTetromeno].X, tetromenoGridPos[activeTetromeno].Y + 1);
                        milisecondsElapsedTetromenoTime -= milisecondsTetromenoFallTime;
                    }
                }
                else
                {
                    if (milisecondsElapsedTetromenoTime - milisecondsTetromenoLockDelayTime >= 1)
                    {
                        tetristype = random.Next(1, 7);
                        activeTetromeno += 1;
                        tetromeno.Add(new Tetromeno(tetristype, blockTexture));
                        tetromenoGridPos.Add(new Vector2(13, 0));
                        milisecondsElapsedTetromenoTime -= milisecondsTetromenoLockDelayTime;
                    }
                }

                //Up
                if (farthestUp > 0 && tetristype != 5)
                {
                    if (keyUpDown == true)
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.Up))
                        {
                            keyUpDown = false;
                            if (rotation < 3)
                                rotation++;
                            else
                                rotation = 0;

                            tetromeno[activeTetromeno].setRotation(rotation);
                            //tetromenoGridPos[activeTetromeno] = new Vector2(tetromenoGridPos[activeTetromeno].X + 1, tetromenoGridPos[activeTetromeno].Y);
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

            } //End Tetromeno Movement

            /*
             * Player Movement Keys
             */

            /*
            {
                //Left
                if (keyADown == true)
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
                if (keyDDown == true)
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
                if (keyWDown == true)
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
                    if (milliScecondsElapsedGameTime % 220 == 0)
                    {

                        player1GridPos.Y += 1;
                        player1Jump = false;

                    }
                }
             
            }
             */
            //End Player Movement

            for (int k = 0; k < tetromeno.Count; k++)
            {
                tetromeno[k].Update(tetromenoGridPos[k], gridStartPos, GlobalVar.ScaleSize);
            }
            
            //player1.Update(new Vector2(gridStartPos.X + (player1GridPos.X * (blockTexture.Width * GlobalVar.ScaleSize.X)), gridStartPos.Y + (player1GridPos.Y * (blockTexture.Height * GlobalVar.ScaleSize.Y))), GlobalVar.ScaleSize, Color.White);
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

                    if (gameField[j, i] == true)
                    {
                        //backdropColor = Color.Blue;
                    }
                  
                    //Draws the block to the screen at the specified point based on the for loop
                    spriteBatch.Draw(blockTexture, new Vector2(gridStartPos.X + ((blockTexture.Width * GlobalVar.ScaleSize.X) * j), gridStartPos.Y + ((blockTexture.Height * GlobalVar.ScaleSize.Y) * i)), null, backdropColor, 0, new Vector2(0), GlobalVar.ScaleSize, SpriteEffects.None, 0);

                    spriteBatch.DrawString(font, "Left: " + farthestLeft.ToString() + " " + "Right: " + farthestRight.ToString() + "\n" + "Up: " + farthestUp.ToString() + " " + "Down: " + farthestDown.ToString() + "\nSame YPos: " + sameYPosNum + testingStringData, new Vector2(10 * GlobalVar.ScaleSize.X, 10 * GlobalVar.ScaleSize.Y), Color.Wheat);
                }
            }

            //Temp Teromeno Set Up
            for (int k = 0; k < tetromeno.Count; k++)
            {
                tetromeno[k].Draw(spriteBatch);
            }

            //player1.Draw(spriteBatch, player1SpriteEffects);
        }

        public Boolean[,] getGameField()
        {
            return gameField;
        }

    }
}
