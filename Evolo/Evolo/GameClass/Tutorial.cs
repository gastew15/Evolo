using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StarByte.ui;

/**
* Evolo Tutorial System to be used to display tutorial information
* Author: G. Stewart
* Version: 2/13/15
*/
namespace Evolo.GameClass
{
    class Tutorial
    {
        private SpriteFont font;
        private PopUpHandler tutorialPopup;
        private Texture2D tutorialPopupTexture, tutorialPopupCloseButtonTexture;
        private Vector2 tutorialPopupPosition, tutorialPopupTextPosition;
        private Rectangle tutorialPopupCloseButtonRect;
        private Boolean tutorialPopupIsDragable = true;
        private const int tutorialPopupLinesOnPage = 9, tutorialPopupVerticalLineSpacing = 24;
        private int tutorialPopupCurrentTextSelection = 0;
        private String[] tutorialPopupText;
        private Color[] tutorialPopupColor;
        private Boolean isActive = true;
        private GameTime gameTime;
        private MouseState mouseStateCurrent, mouseStatePrevious;

        public void LoadContent(ContentManager Content, SpriteFont font)
        {
            this.font = font;

            //Array Intilization for text & colors to be used in popups
            tutorialPopupText = new String[] { "", "          Welcome to the tutorial Level!", "", "", "", "", "", "", "              Press Enter to Continue", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
            tutorialPopupColor = new Color[] { Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, };

            //Content Loading
            tutorialPopupTexture = Content.Load<Texture2D>("Sprites and Pictures/tutorialPopup");
            tutorialPopupCloseButtonTexture = Content.Load<Texture2D>("Sprites and Pictures/tutorialPopupCloseButton");

            //Set Up Tutorial PopUp
            tutorialPopupPosition = new Vector2((GlobalVar.ScreenSize.X / 2) - (tutorialPopupTexture.Width / 2), (GlobalVar.ScreenSize.Y / 2) - (tutorialPopupTexture.Height / 2));
            tutorialPopupTextPosition = new Vector2(tutorialPopupTextPosition.X + (12 * GlobalVar.ScaleSize.X), tutorialPopupTextPosition.Y + (20 * GlobalVar.ScaleSize.Y));
            tutorialPopupCloseButtonRect = new Rectangle((int)tutorialPopupPosition.X + (int)(380 * GlobalVar.ScaleSize.X), (int)tutorialPopupPosition.Y + (int)(2 * GlobalVar.ScaleSize.Y), (int)(tutorialPopupTexture.Width * GlobalVar.ScaleSize.X), (int)(tutorialPopupTexture.Height * GlobalVar.ScaleSize.Y));
            tutorialPopup = new PopUpHandler(tutorialPopupTexture, tutorialPopupCloseButtonTexture, tutorialPopupPosition, tutorialPopupTextPosition, tutorialPopupVerticalLineSpacing, tutorialPopupLinesOnPage, getCurrentTextData(), font, getCurrentColorData(), GlobalVar.ScreenSize, tutorialPopupCloseButtonRect, tutorialPopupIsDragable);
        }

        public void Update(GameTime gameTime, MouseState mouseStateCurrent, MouseState mouseStatePrevious)
        {
            this.gameTime = gameTime;
            this.mouseStateCurrent = mouseStateCurrent;
            this.mouseStatePrevious = mouseStatePrevious;
            //Checking, if ey pressed ooe to next, if close pressed voe to next set closoe to false

            tutorialPopupCloseButtonRect = new Rectangle((int)tutorialPopupPosition.X + (int)(380 * GlobalVar.ScaleSize.X), (int)tutorialPopupPosition.Y + (int)(2 * GlobalVar.ScaleSize.Y), (int)(tutorialPopupCloseButtonTexture.Width * GlobalVar.ScaleSize.X), (int)(tutorialPopupCloseButtonTexture.Height * GlobalVar.ScaleSize.Y));
            tutorialPopup.Update(gameTime, mouseStateCurrent, mouseStatePrevious, new Vector2(tutorialPopupPosition.X + (12 * GlobalVar.ScaleSize.X), tutorialPopupPosition.Y + 20), tutorialPopupCloseButtonRect, GlobalVar.ScreenSize, GlobalVar.ScaleSize);

            if (tutorialPopup.closeButtonPressed)
                isActive = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, "Objective:\nMove player to\nother platform and \nclear the number\nof lines indicated \non the right\n\nControls:\n -Player-\n Left: A\n Right: D\n Jump: W\n\n -Blocks-\n Left: LArrow\n Right: RArrow\n Rotate: UArrow\n SpeedUp: DArrow", new Vector2(32 * GlobalVar.ScaleSize.X + 5, 32 * GlobalVar.ScaleSize.Y + 5), Color.White, 0f, new Vector2(0, 0), GlobalVar.ScaleSize, SpriteEffects.None, 1f);
            if (isActive)
            {
                tutorialPopup.Draw(spriteBatch);
            }
        }

        private String[] getCurrentTextData()
        {
            String[] returnData = new String[tutorialPopupLinesOnPage];

            for (int i = 0; i < returnData.Length; i++)
            {
                returnData[i] = tutorialPopupText[tutorialPopupLinesOnPage * tutorialPopupCurrentTextSelection + i];
            }

            return returnData;
        }

        private Color[] getCurrentColorData()
        {
            Color[] returnData = new Color[tutorialPopupLinesOnPage];

            for (int i = 0; i < returnData.Length; i++)
            {
                returnData[i] = tutorialPopupColor[tutorialPopupLinesOnPage * tutorialPopupCurrentTextSelection + i];
            }

            return returnData;
        }

        public void resetTutorial()
        {
                isActive = true;
                tutorialPopup.closeButtonPressed = false;
                tutorialPopupCurrentTextSelection = 0;  
        }

        public Boolean getIsActive()
        {
            return isActive;
        }
    }
}
