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
        private Boolean enterKeyTripped = false;
        private MouseState mouseStateCurrent, mouseStatePrevious;
        private KeyboardState keybState;

        public void LoadContent(ContentManager Content, SpriteFont font)
        {
            this.font = font;

            //Array Intilization for text & colors to be used in popups
            tutorialPopupText = new String[] { /*1*/  "", "          Welcome to the Tutorial Level!", "", "", "", "", "", "", "               Press Enter to Continue",      /*2*/      "", "                         Objective", "     You need to clear the number of lines", " shown on the right of the screen, and move", "    the player from the start platform to the", "        end platform before time expires.", "", "", "               Press Enter to Continue",    /*3*/     "", "                      What To Avoid", "     Blocks hitting the top of the player will", "       end the game, as will having a block", "          reach the top of the play field.", "    Additonally, be careful while moving the", "      player, as it's easy to become stuck.", "", "               Press Enter to Continue",  /*4*/   "", "                     Player Controls", "                      Move Left - " + GlobalVar.OptionsArray[2], "                     Move Right - " + GlobalVar.OptionsArray[3], "                       Jump Up - " + GlobalVar.OptionsArray[4], "", "", "", "               Press Enter to Continue",  /*5*/   "", "                     Block Controls", "                    Move Left - " + GlobalVar.OptionsArray[5], "                  Move Right - " + GlobalVar.OptionsArray[6], "                       Rotate - " + GlobalVar.OptionsArray[7], "              Speed Up Descent - " + GlobalVar.OptionsArray[8], "", "", "               Press Enter to Continue",  /*6*/   "", "                    That's About It!", " All that's left now is for you to start playing!", "", "", "", "", "", "                Press Enter to Begin!" };
            tutorialPopupColor = new Color[] { /*1*/ Color.White, Color.Yellow, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White,    /*2*/ Color.White, Color.Yellow, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White,      /*3*/ Color.White, Color.Yellow, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White,   /*4*/ Color.White, Color.Yellow, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, /*5*/ Color.White, Color.Yellow, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, /*6*/ Color.Yellow, Color.Yellow, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White };

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
            keybState = Keyboard.GetState();
            //Checking, if ey pressed ooe to next, if close pressed voe to next set closoe to false

            if (!enterKeyTripped && keybState.IsKeyDown(Keys.Enter))
            {
                enterKeyTripped = true;

                if (tutorialPopupCurrentTextSelection < (tutorialPopupText.Length / tutorialPopupLinesOnPage) -1)
                {
                    tutorialPopupCurrentTextSelection++;
                }
                else
                {
                    isActive = false;
                }
            }
            else if (keybState.IsKeyUp(Keys.Enter))
            {
                enterKeyTripped = false;
            }

            tutorialPopup.setText(getCurrentTextData(), getCurrentColorData());
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
