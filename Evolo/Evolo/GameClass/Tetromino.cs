using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


/**
 * Evolo tetromino handler to handle the artibutes and movement of indivudal blocks with-in the tetromino
 * Author: Dalton, Josh, Gavin, Kurtis
 * Version: 11/24/14
 */

namespace Evolo.GameClass
{
    class Tetromino
    {

        //Small Block Texture
        Texture2D blockTexture;

        //Small Block Postions
        Vector2[] blockPos = new Vector2[4];

        Vector2[] currentBlockPos = new Vector2[4];

        //ScaleSize
        Vector2 scaleSize;

        //Start Position
        Vector2 gridStartPosition;

        //tetromino Color
        Color drawColor;

        //Spawn point for dominant block
        Vector2 drawPoint; // X, Y

        //tetromino Type
        int tetrominoType;

        int roation = 0;

        public Tetromino(int tetrominoType, Texture2D blockTexture)
        {
            this.tetrominoType = tetrominoType;
            this.blockTexture = blockTexture;
        }

        public void Update(Vector2 drawPoint, Vector2 gridStartPosition, Vector2 scaleSize)
        {
            this.drawPoint = drawPoint;
            this.scaleSize = scaleSize;
            this.gridStartPosition = gridStartPosition;


                switch (tetrominoType)
                {
                    //I  Block
                    case 1:

                        blockPos[0] = drawPoint;
                        blockPos[1] = new Vector2(drawPoint.X + 1, drawPoint.Y);
                        blockPos[2] = new Vector2(drawPoint.X + 2, drawPoint.Y);
                        blockPos[3] = new Vector2(drawPoint.X - 1, drawPoint.Y);
                        drawColor = Color.Cyan;
                        
                        break;
                    //T Block
                    case 2:

                        blockPos[0] = drawPoint;
                        blockPos[1] = new Vector2(drawPoint.X, drawPoint.Y - 1);
                        blockPos[2] = new Vector2(drawPoint.X - 1, drawPoint.Y);
                        blockPos[3] = new Vector2(drawPoint.X + 1, drawPoint.Y);            

                        drawColor = Color.MediumPurple;

                        break;
                    //J block
                    case 3:

                        blockPos[0] = drawPoint;
                        blockPos[1] = new Vector2(drawPoint.X, drawPoint.Y - 1);
                        blockPos[2] = new Vector2(drawPoint.X + 1, drawPoint.Y);
                        blockPos[3] = new Vector2(drawPoint.X + 2, drawPoint.Y);
                        
                        drawColor = Color.Blue;

                        break;
                    //L Block
                    case 4:

                        blockPos[0] = drawPoint;
                        blockPos[1] = new Vector2(drawPoint.X, drawPoint.Y - 1);
                        blockPos[2] = new Vector2(drawPoint.X - 1, drawPoint.Y);
                        blockPos[3] = new Vector2(drawPoint.X - 2, drawPoint.Y);

                        drawColor = Color.Orange;

                        break;
                    //O Block
                    case 5:
                        
                        blockPos[0] = drawPoint;
                        blockPos[1] = new Vector2(drawPoint.X + 1, drawPoint.Y);
                        blockPos[2] = new Vector2(drawPoint.X + 1, drawPoint.Y - 1);
                        blockPos[3] = new Vector2(drawPoint.X, drawPoint.Y - 1);
                        
                        drawColor = Color.Yellow;

                        break;
                    //S Block
                    case 6:

                        blockPos[0] = drawPoint;
                        blockPos[1] = new Vector2(drawPoint.X + 1, drawPoint.Y - 1);
                        blockPos[2] = new Vector2(drawPoint.X, drawPoint.Y - 1); 
                        blockPos[3] = new Vector2(drawPoint.X - 1, drawPoint.Y);

                        drawColor = Color.LawnGreen;

                        break;
                    //Z Block
                    case 7:

                        blockPos[0] = drawPoint; 
                        blockPos[1] = new Vector2(drawPoint.X - 1, drawPoint.Y - 1);
                        blockPos[2] = new Vector2(drawPoint.X, drawPoint.Y - 1);
                        blockPos[3] = new Vector2(drawPoint.X + 1, drawPoint.Y);
                        
                        drawColor = Color.Red;

                        break;
                }

                #region Rotation Math
                currentBlockPos[0] = drawPoint;
                switch (roation)
                {
                    case 0:
                        for (int i = 1; i < 4; i++)
                        {
                            currentBlockPos[i] = blockPos[i];
                        }
                        break;
                    case 1:
                        for (int i = 1; i < 4; i++)
                        {
                                currentBlockPos[i].X = (float)(Math.Cos((Math.PI / 180) * 90) * (blockPos[i].X - drawPoint.X) - Math.Sin((Math.PI / 180) * 90) * (blockPos[i].Y - drawPoint.Y) + drawPoint.X);
                                currentBlockPos[i].Y = (float)(Math.Sin((Math.PI / 180) * 90) * (blockPos[i].X - drawPoint.X) + Math.Cos((Math.PI / 180) * 90) * (blockPos[i].Y - drawPoint.Y) + drawPoint.Y);
                        }
                        break;
                    case 2:
                        for (int i = 1; i < 4; i++)
                        {
                                currentBlockPos[i].X = (float)(Math.Cos((Math.PI / 180) * 180) * (blockPos[i].X - drawPoint.X) - Math.Sin((Math.PI / 180) * 180) * (blockPos[i].Y - drawPoint.Y) + drawPoint.X);
                                currentBlockPos[i].Y = (float)(Math.Sin((Math.PI / 180) * 180) * (blockPos[i].X - drawPoint.X) + Math.Cos((Math.PI / 180) * 180) * (blockPos[i].Y - drawPoint.Y) + drawPoint.Y);
                        }
                        break;
                    case 3:
                        for (int i = 1; i < 4; i++)
                        {
                                currentBlockPos[i].X = (float)(Math.Cos((Math.PI / 180) * 270) * (blockPos[i].X - drawPoint.X) - Math.Sin((Math.PI / 180) * 270) * (blockPos[i].Y - drawPoint.Y) + drawPoint.X);
                                currentBlockPos[i].Y = (float)(Math.Sin((Math.PI / 180) * 270) * (blockPos[i].X - drawPoint.X) + Math.Cos((Math.PI / 180) * 270) * (blockPos[i].Y - drawPoint.Y) + drawPoint.Y);
                        }
                        break;
                }
                #endregion

        }

       

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int j = 0; j < blockPos.Length; j++)
            {
                spriteBatch.Draw(blockTexture, new Vector2(gridStartPosition.X + (currentBlockPos[j].X * (blockTexture.Width * scaleSize.X)), gridStartPosition.Y + (currentBlockPos[j].Y * (blockTexture.Height * scaleSize.Y))), null, drawColor, 0f, new Vector2(0, 0), scaleSize, SpriteEffects.None, 1f);
            }
        }

        public Vector2[] getPositions()
        {
            return currentBlockPos;
        }

        public void setRotation(int rotation)
        {
            this.roation = rotation;
        }

        public int getTetrisType()
        {
            return tetrominoType;
        }

        public void setTetrisType(int tetrominoType)
        {
            this.tetrominoType = tetrominoType;
        }

        public int getRotation()
        {
            return roation;
        }

        public void setColorTemp(Color color)
        {
            this.drawColor = color;
        }
    }
}
