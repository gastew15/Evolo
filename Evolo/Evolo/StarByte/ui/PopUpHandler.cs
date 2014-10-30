#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

/*
 *  StarByte PopupHandler
 *  Author: G. Stewart
 *  Version: 9/24/14
 */

namespace StarByte.ui
{
    class PopUpHandler
    {
        SpriteFont font;
        private Texture2D popupBackground, closeButtonTexture;
        private Vector2 screenSize, popupWindowPos, popupWindowStartingPos, drawScale, popupTextDrawPos, popupTextDrawPosOrginal;
        private String[] popupText;
        private int linesOnAPage, verticalLineSpacing;
        private Boolean popupWindowMouseOver, isDragable;
        public  Boolean closeButtonPressed { get; set; }
        private Color popupWindowDrawColor, closeButtonColor;
        private Color[] textColor;
        private Rectangle closeButtonRect;

        public PopUpHandler(Texture2D popupBackground, Texture2D closeButtonTexture, Vector2 popupWindowPos, Vector2 popupTextDrawPos, int verticalLineSpacing, int linesOnAPage, String[] popupText, SpriteFont font, Color[] textColor, Vector2 screenSize, Rectangle closeButtonRect, Boolean isDragable)
        {
            this.popupBackground = popupBackground;
            this.closeButtonTexture = closeButtonTexture;
            this.popupTextDrawPos = popupTextDrawPos;
            this.popupTextDrawPosOrginal = popupTextDrawPos;
            this.verticalLineSpacing = verticalLineSpacing;
            this.linesOnAPage = linesOnAPage;
            this.popupText = popupText;
            this.font = font;
            this.screenSize = screenSize;
            this.popupWindowPos = popupWindowPos;
            this.popupWindowStartingPos = popupWindowPos;
            if (textColor == null)
                this.textColor = new Color[popupText.Length];
            else
                this.textColor = textColor;
            this.closeButtonRect = closeButtonRect;
            this.isDragable = isDragable;
        }

        public void Update(GameTime gameTime, MouseState mouseStateCurrent, MouseState mouseStatePrevious, Vector2 textDrawPosition, Rectangle closeButtonRect,Vector2 screenSize, Vector2 drawScale)
        {
            this.closeButtonRect = closeButtonRect;
            this.screenSize = screenSize;
            this.drawScale = drawScale;
            this.popupTextDrawPosOrginal = textDrawPosition;

            if (isDragable == true)
            {
                //Checks to see if the mouse is inside the bounds
                if ((mouseStateCurrent.Y > popupWindowPos.Y && mouseStateCurrent.Y < (popupWindowPos.Y + (popupBackground.Height * drawScale.Y))) && (mouseStateCurrent.X > popupWindowPos.X && mouseStateCurrent.X < (popupWindowPos.X + (popupBackground.Width * drawScale.X))))
                    popupWindowMouseOver = true;
                else
                    popupWindowMouseOver = false;

                //Checks to see if the box is outside of bounds
                if (popupWindowPos.X < 0)
                    popupWindowPos.X = 0;
                else if ((popupWindowPos.X + (popupBackground.Width * drawScale.X)) > screenSize.X)
                    popupWindowPos.X = (screenSize.X - (popupBackground.Width * drawScale.X));
                else if (popupWindowPos.Y < 0)
                    popupWindowPos.Y = 0;
                else if ((popupWindowPos.Y + (popupBackground.Height * drawScale.Y)) > screenSize.Y)
                    popupWindowPos.Y = (screenSize.Y - (popupBackground.Height * drawScale.Y));

                //Processes the information and decides to move the box or not
                if (popupWindowMouseOver == true && (mouseStateCurrent.LeftButton == ButtonState.Pressed))
                {
                    popupWindowPos.X -= (mouseStatePrevious.X - mouseStateCurrent.X);
                    popupWindowPos.Y -= (mouseStatePrevious.Y - mouseStateCurrent.Y);

                    popupWindowDrawColor = Color.LightGray;
                }
                else
                {
                    popupWindowDrawColor = Color.White;
                }
            }
            else
            {
                popupWindowPos = popupWindowStartingPos;
                popupWindowDrawColor = Color.White;
            }

            //Checks for the close button being hovered over
            if (mouseStateCurrent.X > (closeButtonRect.X + (popupWindowPos.X - popupWindowStartingPos.X)) && mouseStateCurrent.X < ((closeButtonRect.X + (closeButtonRect.Width * drawScale.X)) + (popupWindowPos.X - popupWindowStartingPos.X)) && mouseStateCurrent.Y > (closeButtonRect.Y + (popupWindowPos.Y - popupWindowStartingPos.Y)) && mouseStateCurrent.Y < ((closeButtonRect.Y + (closeButtonRect.Height * drawScale.Y)) + (popupWindowPos.Y - popupWindowStartingPos.Y)))
            {
                closeButtonColor = Color.LightCoral;

                //Checks if the button was pressed
                if (mouseStateCurrent.LeftButton == ButtonState.Pressed)
                {
                    closeButtonPressed = true;
                }
            }
            else
            {
                closeButtonColor = Color.White;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(popupBackground, popupWindowPos, null, Color.White, 0f, new Vector2(0, 0), drawScale, SpriteEffects.None, 1f);
            spriteBatch.Draw(closeButtonTexture, new Vector2((closeButtonRect.X + (popupWindowPos.X - popupWindowStartingPos.X)), (closeButtonRect.Y + (popupWindowPos.Y - popupWindowStartingPos.Y))), null, closeButtonColor, 0f, new Vector2(0, 0), drawScale, SpriteEffects.None, 1f);
            Vector2 textDrawPos;

            popupTextDrawPos = new Vector2(popupTextDrawPosOrginal.X + (popupWindowPos.X - popupWindowStartingPos.X), popupTextDrawPosOrginal.Y + (popupWindowPos.Y - popupWindowStartingPos.Y));

            if (popupText != null)
            {
                for(int j = 0; j < popupText.Length; j++)
                {
                    if (textColor[j] == null)
                        textColor[j] = Color.White;
                    textDrawPos = new Vector2(popupTextDrawPos.X, popupTextDrawPos.Y + ((verticalLineSpacing * drawScale.Y) * j));
                    spriteBatch.DrawString(font, popupText[j], textDrawPos, textColor[j], 0f, new Vector2(0,0), drawScale, SpriteEffects.None, 1f);
                }
            }
        }

        public void setText(String[] popupText, Color[] textColor)
        {
            this.popupText = popupText;
            this.textColor = textColor;
        }
    }
}
