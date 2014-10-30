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
        Boolean[,] gameField = new Boolean[26, 22];
        Texture2D blockTexture, playerTexture;
        Vector2 gridStartPos, player1GridPos, levelStartPoint, levelEndPoint;
        SpriteEffects player1SpriteEffects;
        Random random = new Random();
        int tetristype;
        bool spawn = true;
        Vector2 tetromeno1GridPos = new Vector2(13, 0);
        float lastupdate = 0;
        bool leftDown = true;
        bool rightDown = true;
        bool wallCollisionLeft = false;
        bool wallCollisionRight = false;
        bool keyleftDown, keyrightDown;
        bool keyADown, keyDDown, keyWDown;
        bool player1Jump = false;

        //TEMP
        Tetromeno tetromeno1;
        Player player1;

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
            //tetromeno1 = new Tetromeno(1, blockTexture);

            //Temp levelSP
            levelStartPoint = new Vector2(1, 17);

            //Player Set Up
            player1 = new Player(playerTexture);
            player1GridPos = levelStartPoint;
 
        }

        public void Update(float milliScecondsElapsedGameTime)
        {
            gridStartPos = new Vector2((GlobalVar.ScreenSize.X / 2) - (((blockTexture.Width * GlobalVar.ScaleSize.X) * gameField.GetLength(0)) / 2), (GlobalVar.ScreenSize.Y / 2) - (((blockTexture.Height * GlobalVar.ScaleSize.Y) * (gameField.GetLength(1) + 2)) / 2));
            tetristype = random.Next(1, 8);

            if (spawn == true)
            {
                tetromeno1 = new Tetromeno(tetristype, blockTexture);
                spawn = false;
            }
            else
            {

            }
            //Code to check and move the Tetromeno

            if (tetromeno1GridPos.Y != 21)
            {

                if (milliScecondsElapsedGameTime % 82 == 0)
                {
                    //if (milliScecondsElapsedGameTime - lastupdate == 500)
                    //{
                    tetromeno1GridPos.Y += 1;
                    lastupdate = milliScecondsElapsedGameTime;
                    //}
                    //if (milliScecondsElapsedGameTime - lastupdate != 500)
                    //{
                    //tetromeno1GridPos.Y += (milliScecondsElapsedGameTime - lastupdate / 500);
                    //lastupdate = milliScecondsElapsedGameTime;
                    //}
                }


                if (leftDown == true)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            if (tetromeno1.getPositions()[i].X == (gridStartPos.X + ((blockTexture.Width * GlobalVar.ScaleSize.X) * 3)))
                            {
                                wallCollisionLeft = true;
                            }
                        }
                        if (wallCollisionLeft == false)
                        {
                            leftDown = false;
                            tetromeno1GridPos.X -= 1;
                        }
                        if (wallCollisionLeft == true)
                        {
                            wallCollisionLeft = false;
                        }
                    }
                }
                if (Keyboard.GetState().IsKeyUp(Keys.Left))
                    if (leftDown == false)
                    {
                        leftDown = true;
                    }


                if (rightDown == true)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if (tetromeno1.getPositions()[j].X == (gridStartPos.X + ((blockTexture.Width * GlobalVar.ScaleSize.X) * 22)))
                            {
                                wallCollisionRight = true;
                            }
                        }
                        if (wallCollisionRight == false)
                        {
                            rightDown = false;
                            tetromeno1GridPos.X += 1;
                        }
                        if (wallCollisionRight == true)
                        {
                            wallCollisionRight = false;
                        }
                    }
                }
                if (Keyboard.GetState().IsKeyUp(Keys.Right))
                    if (rightDown == false)
                    {
                        rightDown = true;
                    }
            }
            else
            {

                if ((milliScecondsElapsedGameTime - lastupdate) % 41 != 0)
                {
                    #region Lock Delay Movement
                    if (leftDown == true)
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.Left))
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                if (tetromeno1.getPositions()[i].X == (gridStartPos.X + ((blockTexture.Width * GlobalVar.ScaleSize.X) * 3)))
                                {
                                    wallCollisionLeft = true;
                                }
                            }
                            if (wallCollisionLeft == false)
                            {
                                leftDown = false;
                                tetromeno1GridPos.X -= 1;
                            }
                            if (wallCollisionLeft == true)
                            {
                                wallCollisionLeft = false;
                            }
                        }
                    }
                    if (Keyboard.GetState().IsKeyUp(Keys.Left))
                        if (leftDown == false)
                        {
                            leftDown = true;
                        }


                    if (rightDown == true)
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.Right))
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                if (tetromeno1.getPositions()[j].X == (gridStartPos.X + ((blockTexture.Width * GlobalVar.ScaleSize.X) * 22)))
                                {
                                    wallCollisionRight = true;
                                }
                            }
                            if (wallCollisionRight == false)
                            {
                                rightDown = false;
                                tetromeno1GridPos.X += 1;
                            }
                            if (wallCollisionRight == true)
                            {
                                wallCollisionRight = false;
                            }
                        }
                    }
                    if (Keyboard.GetState().IsKeyUp(Keys.Right))
                        if (rightDown == false)
                        {
                            rightDown = true;
                        }
                    #endregion
                }

                else
                {
                    spawn = true;
                    tetromeno1GridPos.Y = 1;
                    tetromeno1GridPos.X = 13;
                }
            }

            /*
             * Player Movement Keys
             */
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
            }//End Player Movement

            tetromeno1.Update(new Vector2(gridStartPos.X + (tetromeno1GridPos.X * (blockTexture.Width * GlobalVar.ScaleSize.X)), gridStartPos.Y + (tetromeno1GridPos.Y * (blockTexture.Height * GlobalVar.ScaleSize.Y))), GlobalVar.ScaleSize);
            player1.Update(new Vector2(gridStartPos.X + (player1GridPos.X * (blockTexture.Width * GlobalVar.ScaleSize.X)), gridStartPos.Y + (player1GridPos.Y * (blockTexture.Height * GlobalVar.ScaleSize.Y))), GlobalVar.ScaleSize, Color.White);
        }

        public void Draw(SpriteBatch spriteBatch)
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
                  
                    //Draws the block to the screen at the specified point based on the for loop
                    spriteBatch.Draw(blockTexture, new Vector2(gridStartPos.X + ((blockTexture.Width * GlobalVar.ScaleSize.X) * j), gridStartPos.Y + ((blockTexture.Height * GlobalVar.ScaleSize.Y) * i)), null, backdropColor, 0, new Vector2(0), GlobalVar.ScaleSize, SpriteEffects.None, 0);
                }
            }

            //Temp Teromeno Set Up
            tetromeno1.Draw(spriteBatch);

            player1.Draw(spriteBatch, player1SpriteEffects);
        }

        public Boolean[,] getGameField()
        {
            return gameField;
        }

    }
}
