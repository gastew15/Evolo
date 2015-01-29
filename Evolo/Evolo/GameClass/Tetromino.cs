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
 * Version: 1/29/15
 */

namespace Evolo.GameClass
{
    class Tetromino
    {

        //Small Block Texture
        private Texture2D blockTexture;

        //Small Block Postions
        private Vector2[] blockPos = new Vector2[4];

        private Vector2[] currentBlockPos = new Vector2[4];

        //Block Position Modifers
        private Vector2[] blockPosModifers = new Vector2[] { new Vector2(), new Vector2(), new Vector2(), new Vector2() };

        //ScaleSize
        private Vector2 scaleSize;

        //Start Position
        private Vector2 gridStartPosition;

        //tetromino Color
        private Color drawColor;

        //Spawn point for dominant block
        private Vector2 drawPoint; // X, Y

        //tetromino Type
        private int tetrominoType;

        //Used to tell what block is the center of rotation
        private int rotationBlockPosition = 0;

        private int rotation = 0;

        private Boolean[] blockPosActive = new Boolean[4] { true, true, true, true };

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
                    blockPos[0] = new Vector2(drawPoint.X + blockPosModifers[0].X, drawPoint.Y + blockPosModifers[0].Y);
                    blockPos[1] = new Vector2(drawPoint.X + 1 + blockPosModifers[1].X, drawPoint.Y + blockPosModifers[1].Y);
                    blockPos[2] = new Vector2(drawPoint.X + 2+ blockPosModifers[2].X, drawPoint.Y + blockPosModifers[2].Y);
                    blockPos[3] = new Vector2(drawPoint.X  + 3 + blockPosModifers[3].X, drawPoint.Y + blockPosModifers[3].Y);
                    drawColor = Color.Cyan;
                    rotationBlockPosition = 1;
                    break;
                //T Block
                case 2:
                    blockPos[0] = new Vector2(drawPoint.X + blockPosModifers[0].X, drawPoint.Y + blockPosModifers[0].Y);
                    blockPos[1] = new Vector2(drawPoint.X + 1 + blockPosModifers[1].X, drawPoint.Y - 1 + blockPosModifers[1].Y);
                    blockPos[2] = new Vector2(drawPoint.X + 1 + blockPosModifers[2].X, drawPoint.Y + blockPosModifers[2].Y);
                    blockPos[3] = new Vector2(drawPoint.X + 2 + blockPosModifers[3].X, drawPoint.Y + blockPosModifers[3].Y);
                    rotationBlockPosition = 2;
                    drawColor = Color.MediumPurple;

                    break;
                //J block
                case 3:
                    blockPos[0] = new Vector2(drawPoint.X + blockPosModifers[0].X, drawPoint.Y + blockPosModifers[0].Y);
                    blockPos[1] = new Vector2(drawPoint.X + blockPosModifers[1].X, drawPoint.Y - 1 + blockPosModifers[1].Y);
                    blockPos[2] = new Vector2(drawPoint.X + 1 + blockPosModifers[2].X, drawPoint.Y + blockPosModifers[2].Y);
                    blockPos[3] = new Vector2(drawPoint.X + 2 + blockPosModifers[3].X, drawPoint.Y + blockPosModifers[3].Y);
                    rotationBlockPosition = 0;
                    drawColor = Color.Blue;

                    break;
                //L Block
                case 4:
                    blockPos[0] = new Vector2(drawPoint.X + blockPosModifers[0].X, drawPoint.Y + blockPosModifers[0].Y);
                    blockPos[1] = new Vector2(drawPoint.X + 1 + blockPosModifers[1].X, drawPoint.Y + blockPosModifers[1].Y);
                    blockPos[2] = new Vector2(drawPoint.X + 2 + blockPosModifers[2].X, drawPoint.Y + blockPosModifers[2].Y);
                    blockPos[3] = new Vector2(drawPoint.X + 2 + blockPosModifers[3].X, drawPoint.Y - 1 + blockPosModifers[3].Y);
                    rotationBlockPosition = 2;
                    drawColor = Color.Orange;
                    break;
                //O Block
                case 5:
                    blockPos[0] = new Vector2(drawPoint.X + blockPosModifers[0].X, drawPoint.Y + blockPosModifers[0].Y);
                    blockPos[1] = new Vector2(drawPoint.X + 1 + blockPosModifers[1].X, drawPoint.Y + blockPosModifers[1].Y);
                    blockPos[2] = new Vector2(drawPoint.X + 1 + blockPosModifers[2].X, drawPoint.Y - 1 + blockPosModifers[2].Y);
                    blockPos[3] = new Vector2(drawPoint.X + blockPosModifers[3].X, drawPoint.Y - 1 + blockPosModifers[3].Y);
                    rotationBlockPosition = 0;
                    drawColor = Color.Yellow;

                    break;
                //S Block
                case 6:
                    blockPos[0] = new Vector2(drawPoint.X + blockPosModifers[0].X, drawPoint.Y + blockPosModifers[0].Y);
                    blockPos[1] = new Vector2(drawPoint.X + 1 + blockPosModifers[1].X, drawPoint.Y + blockPosModifers[1].Y);
                    blockPos[2] = new Vector2(drawPoint.X + 1 + blockPosModifers[2].X, drawPoint.Y - 1 + blockPosModifers[2].Y);
                    blockPos[3] = new Vector2(drawPoint.X + 2 + blockPosModifers[3].X, drawPoint.Y - 1 + blockPosModifers[3].Y);
                    rotationBlockPosition = 2;
                    drawColor = Color.LawnGreen;

                    break;
                //Z Block
                case 7:
                    blockPos[0] = new Vector2(drawPoint.X + blockPosModifers[0].X, drawPoint.Y - 1 + blockPosModifers[0].Y);
                    blockPos[1] = new Vector2(drawPoint.X + 1 + blockPosModifers[1].X, drawPoint.Y - 1 + blockPosModifers[1].Y);
                    blockPos[2] = new Vector2(drawPoint.X + 1 + blockPosModifers[2].X, drawPoint.Y + blockPosModifers[2].Y);
                    blockPos[3] = new Vector2(drawPoint.X + 2 + blockPosModifers[3].X, drawPoint.Y + blockPosModifers[3].Y);
                    rotationBlockPosition = 1;
                    drawColor = Color.Red;

                    break;
            }

            #region Rotation Math

            switch (rotation)
            {
                case 0:
                    for (int i = 0; i < 4; i++)
                    {
                        currentBlockPos[i] = blockPos[i];
                    }
                    break;
                case 1:
                    for (int i = 0; i < 4; i++)
                    {
                        if (i != rotationBlockPosition)
                        {
                            currentBlockPos[i].X = (float)(Math.Cos((Math.PI / 180) * 90) * (blockPos[i].X - blockPos[rotationBlockPosition].X) - Math.Sin((Math.PI / 180) * 90) * (blockPos[i].Y - blockPos[rotationBlockPosition].Y) + blockPos[rotationBlockPosition].X);
                            currentBlockPos[i].Y = (float)(Math.Sin((Math.PI / 180) * 90) * (blockPos[i].X - blockPos[rotationBlockPosition].X) + Math.Cos((Math.PI / 180) * 90) * (blockPos[i].Y - blockPos[rotationBlockPosition].Y) + blockPos[rotationBlockPosition].Y);
                        }
                        else
                        {
                            currentBlockPos[i] = blockPos[i];
                        }
                    }
                    break;
                case 2:
                    for (int i = 0; i < 4; i++)
                    {
                        if (i != rotationBlockPosition)
                        {
                            currentBlockPos[i].X = (float)(Math.Cos((Math.PI / 180) * 180) * (blockPos[i].X - blockPos[rotationBlockPosition].X) - Math.Sin((Math.PI / 180) * 180) * (blockPos[i].Y - blockPos[rotationBlockPosition].Y) + blockPos[rotationBlockPosition].X);
                            currentBlockPos[i].Y = (float)(Math.Sin((Math.PI / 180) * 180) * (blockPos[i].X - blockPos[rotationBlockPosition].X) + Math.Cos((Math.PI / 180) * 180) * (blockPos[i].Y - blockPos[rotationBlockPosition].Y) + blockPos[rotationBlockPosition].Y);
                        }
                        else
                        {
                            currentBlockPos[i] = blockPos[i];
                        }
                    }
                    break;
                case 3:
                    for (int i = 0; i < 4; i++)
                    {
                        if (i != rotationBlockPosition)
                        {
                            currentBlockPos[i].X = (float)(Math.Cos((Math.PI / 180) * 270) * (blockPos[i].X - blockPos[rotationBlockPosition].X) - Math.Sin((Math.PI / 180) * 270) * (blockPos[i].Y - blockPos[rotationBlockPosition].Y) + blockPos[rotationBlockPosition].X);
                            currentBlockPos[i].Y = (float)(Math.Sin((Math.PI / 180) * 270) * (blockPos[i].X - blockPos[rotationBlockPosition].X) + Math.Cos((Math.PI / 180) * 270) * (blockPos[i].Y - blockPos[rotationBlockPosition].Y) + blockPos[rotationBlockPosition].Y);
                        }
                        else
                        {
                            currentBlockPos[i] = blockPos[i];
                        }
                    }
                    break;
            }

            #endregion

        }



        public void Draw(SpriteBatch spriteBatch)
        {
            for (int j = 0; j < blockPos.Length; j++)
            {
                if (blockPosActive[j] == true)
                {
                    spriteBatch.Draw(blockTexture, new Vector2(gridStartPosition.X + (currentBlockPos[j].X * (blockTexture.Width * scaleSize.X)), gridStartPosition.Y + (currentBlockPos[j].Y * (blockTexture.Height * scaleSize.Y))), null, drawColor, 0f, new Vector2(0, 0), scaleSize, SpriteEffects.None, 1f);
                }
            }
        }

        public Vector2[] getPositions()
        {
            Vector2[] tempReturn = new Vector2[currentBlockPos.Length];
            for (int i = 0; i < tempReturn.Length; i++)
            {
                if (blockPosActive[i] == true)
                {
                    tempReturn[i] = currentBlockPos[i];
                }
            }

            return tempReturn;
        }

        public void setRotation(int rotation)
        {
            this.rotation = rotation;
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
            return rotation;
        }

        public void setColorTemp(Color color)
        {
            this.drawColor = color;
        }

        public Boolean[] getBlockPosActive()
        {
            return blockPosActive;
        }

        public void setBlockPosActive(Boolean[] blockPosActive)
        {
            this.blockPosActive = blockPosActive;
        }

        public void setBlockPositions(Vector2[] blockPositions)
        {
            for (int i = 0; i < blockPosActive.Length; i++)
            {
                blockPosModifers[i] = new Vector2(blockPositions[i].X - currentBlockPos[i].X, blockPositions[i].Y - currentBlockPos[i].Y);
            }

            //blockPosModifers = blockPosModifers;
        }

        public Vector2[] getRawBlockPositions()
        {
            return currentBlockPos;
        }
    }
}
