#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
#endregion

/**
 * Evolo tetromeno handler to handle the artibutes and movement of indivudal blocks with-in the tetromeno
 * Author: Dalton, Josh, Gavin
 * Version: 10/10/14
 */

namespace Evolo.GameClass
{
    class Tetromeno
    {


        //Small Block Texture
        Texture2D blockTexture;

        //Small Block Postions
        Vector2[] blockPos = new Vector2[4];

        //ScaleSize
        Vector2 scaleSize;

        //Tetromeno Color
        Color drawColor;

        //Spawn point for dominant block
        Vector2 drawPoint; // X, Y

        //Tetromeno Type
        int tetromenoType;

        



        public Tetromeno(int tetromenoType, Texture2D blockTexture)
        {
            this.tetromenoType = tetromenoType;
            this.blockTexture = blockTexture;
        }

        public void Update(Vector2 drawPoint, Vector2 scaleSize)
        {
            this.drawPoint = drawPoint;
            this.scaleSize = scaleSize;


                switch (tetromenoType)
                {
                        /*
                         * NOTE REPLACE ALL 1's with Width or height of the block...... Also change the draw so it works right too.
                         */
                    //I  Block
                    case 1:
                        blockPos[0] = drawPoint;
                        blockPos[1] = new Vector2(drawPoint.X + (blockTexture.Width * scaleSize.X), drawPoint.Y );
                        blockPos[2] = new Vector2(drawPoint.X + 2 * (blockTexture.Width * scaleSize.X), drawPoint.Y );
                        blockPos[3] = new Vector2(drawPoint.X - (blockTexture.Width * scaleSize.X), drawPoint.Y );
                        drawColor = Color.Cyan;
                        
                        break;
                    //T Block
                    case 2:
                        blockPos[0] = new Vector2(drawPoint.X, drawPoint.Y - (blockTexture.Height * scaleSize.Y));
                        blockPos[1] = drawPoint;
                        blockPos[2] = new Vector2(drawPoint.X - (blockTexture.Width * scaleSize.X), drawPoint.Y);
                        blockPos[3] = new Vector2(drawPoint.X + (blockTexture.Width * scaleSize.X), drawPoint.Y);
                        drawColor = Color.MediumPurple;
                        
                        break;
                    //J block
                    case 3:
                        blockPos[0] = drawPoint;
                        blockPos[1] = new Vector2(drawPoint.X, drawPoint.Y - (blockTexture.Height * scaleSize.Y));
                        blockPos[2] = new Vector2(drawPoint.X + (blockTexture.Width * scaleSize.X), drawPoint.Y);
                        blockPos[3] = new Vector2(drawPoint.X + 2 * (blockTexture.Width * scaleSize.X), drawPoint.Y);                        
                        drawColor = Color.Blue;
                        break;
                    //L Block
                    case 4:
                        blockPos[0] = drawPoint;
                        blockPos[1] = new Vector2(drawPoint.X, drawPoint.Y - (blockTexture.Height * scaleSize.Y));
                        blockPos[2] = new Vector2(drawPoint.X - (blockTexture.Width * scaleSize.X), drawPoint.Y);
                        blockPos[3] = new Vector2(drawPoint.X - 2 * (blockTexture.Width * scaleSize.X), drawPoint.Y);
                        drawColor = Color.Orange;
                        break;
                    //O Block
                    case 5:
                        blockPos[0] = new Vector2(drawPoint.X, drawPoint.Y - (blockTexture.Height * scaleSize.Y));
                        blockPos[1] = new Vector2(drawPoint.X + (blockTexture.Width * scaleSize.X), drawPoint.Y);
                        blockPos[2] = new Vector2(drawPoint.X + (blockTexture.Width * scaleSize.X), drawPoint.Y - (blockTexture.Height * scaleSize.Y));
                        blockPos[3] = drawPoint;
                        drawColor = Color.Yellow;
                        break;
                    //S Block
                    case 6:
                        blockPos[0] = new Vector2(drawPoint.X, drawPoint.Y - (blockTexture.Height * scaleSize.Y));
                        blockPos[1] = new Vector2(drawPoint.X + (blockTexture.Width * scaleSize.X), drawPoint.Y - (blockTexture.Height * scaleSize.Y));
                        blockPos[2] = drawPoint;
                        blockPos[3] = new Vector2(drawPoint.X - (blockTexture.Width * scaleSize.X), drawPoint.Y);
                        drawColor = Color.LawnGreen;
                        break;
                    //Z Block
                    case 7:
                        blockPos[0] = new Vector2(drawPoint.X, drawPoint.Y - (blockTexture.Height * scaleSize.Y));
                        blockPos[1] = new Vector2(drawPoint.X - (blockTexture.Width * scaleSize.X), drawPoint.Y - (blockTexture.Height * scaleSize.Y));
                        blockPos[2] = drawPoint;
                        blockPos[3] = new Vector2(drawPoint.X + (blockTexture.Width * scaleSize.X), drawPoint.Y);
                        drawColor = Color.Red;
                        break;
                }

        }

       

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int j = 0; j < blockPos.Length; j++)
            {
                spriteBatch.Draw(blockTexture, blockPos[j], null, drawColor, 0f, new Vector2(0, 0), scaleSize, SpriteEffects.None, 1f);
            }
        }

        public Vector2[] getPositions()
        {
            return blockPos;
        }
    }
}
