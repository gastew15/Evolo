using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/*
 *  StarByte SpriteSheetReader
 *  Author: G. Stewart
 *  Version: 9/24/14
 */

namespace StarByte.io
{
    class SpriteSheetReader
    {
        private Texture2D spriteSheet;
        private Vector2 drawLocation, spriteSize, spriteScale;
        private enum SheetType { Vertical, Horizontal, MultiRow };
        private SheetType sheetType;
        private int numberOfImages, currentImageNumber;
        private Rectangle sourceRect;

        public SpriteSheetReader(Texture2D spriteSheet, Vector2 drawLocation, Vector2 spriteSize)
        {
            this.spriteSheet = spriteSheet;
            this.drawLocation = drawLocation;
            this.spriteSize = spriteSize;
            currentImageNumber = 0;

            if (spriteSheet.Width > spriteSize.X && !(spriteSheet.Height > spriteSize.Y))
            {
                sheetType = SheetType.Horizontal;
                numberOfImages = (int)(spriteSheet.Width % spriteSize.X);
            }
            else if (spriteSheet.Height > spriteSize.Y && !(spriteSheet.Width > spriteSize.X))
            {
                sheetType = SheetType.Vertical;
                numberOfImages = (int)(spriteSheet.Height % spriteSize.Y);
            }
        }

        public SpriteSheetReader(Texture2D spriteSheet, Vector2 drawLocation, Vector2 imageSize, int numberOfImages)
        {
            this.spriteSheet = spriteSheet;
            this.drawLocation = drawLocation;
            this.spriteSize = imageSize;
            this.numberOfImages = numberOfImages;
            currentImageNumber = 0;

            sheetType = SheetType.MultiRow;
        }

        public void Update(GameTime gameTime, Vector2 drawLocation, Boolean changeImage, Vector2 spriteScale)
        {
            this.drawLocation = drawLocation;
            this.spriteScale = spriteScale;

            if (changeImage == true)
            {
                if (currentImageNumber < numberOfImages)
                    currentImageNumber++;
                else
                    currentImageNumber = 0;

                changeImage = false;
            }

            if (sheetType == SheetType.Horizontal)
            {
                sourceRect = new Rectangle(((int)spriteSize.X * currentImageNumber), 0, (int)spriteSize.X, (int)spriteSize.Y);
            }
            else if (sheetType == SheetType.Vertical)
            {
                sourceRect = new Rectangle(0, ((int)spriteSize.Y * currentImageNumber), (int)spriteSize.X, (int)spriteSize.Y);
            }
            else if (sheetType == SheetType.MultiRow)
            {
                //Soon....
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteSheet, drawLocation, sourceRect, Color.White, 0, new Vector2(0), spriteScale, SpriteEffects.None, 0);
        }

        public void setImageNumber(int setImageNumber)
        {
            currentImageNumber = setImageNumber;
        }

    }
}
