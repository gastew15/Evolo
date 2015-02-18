#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
#endregion

/*
 *  StarByte MenuHandler
 *  Author: G. Stewart
 *  Version: 11/6/14
 */

namespace StarByte.ui
{
    class MenuHandler
    {
        SpriteFont font;
        private Vector2 startingPos, drawPosition, textPosition, screenSize, drawScale, buttonBackdropPos, menuTitlePos;
        private Texture2D buttonBackground, buttonBackdrop, indicatorLight, menuTitle;
        private String[] buttonText;
        private int verticalSpacing, menuNumberSelected, menuNumberHover = 1, menuNumberHoverPrevious = 1;
        private Color[] textColor, orginalTextColor;
        private Boolean testButtonMouseOver, isPressedUp, isPressedDown, isPressedEnter = true;
        private KeyboardState keybState;
        private SoundEffect menuHoverChangeSoundEffect, menuClickedSoundEffect;
        private Boolean overRideButtonPress = false;

        //Basic
        public MenuHandler(Texture2D menuTitle, Vector2 startingPos, Vector2 menuTitlePos, int verticalSpacing, Texture2D buttonBackground, String[] buttonText, SpriteFont font, SoundEffect menuHoverChangeSoundEffect, SoundEffect menuClickedSoundEffect, Vector2 screenSize)
        {
            this.menuTitle = menuTitle;
            this.startingPos = startingPos;
            this.menuTitlePos = menuTitlePos;
            this.buttonBackground = buttonBackground;
            this.buttonText = buttonText;
            this.verticalSpacing = verticalSpacing;
            this.font = font;
            this.menuHoverChangeSoundEffect = menuHoverChangeSoundEffect;
            this.menuClickedSoundEffect = menuClickedSoundEffect;
            this.screenSize = screenSize;
            textColor = new Color[buttonText.Length];
        }

        //Complicated (SERA)
        public MenuHandler(Texture2D menuTitle, Texture2D buttonBackdrop, Texture2D indicatorLight, Vector2 startingPos, Vector2 buttonBackdropPos, Vector2 menuTitlePos, int verticalSpacing, Texture2D buttonBackground, String[] buttonText, SpriteFont font, SoundEffect menuHoverChangeSoundEffect, SoundEffect menuClickedSoundEffect, Vector2 screenSize)
        {
            this.menuTitle = menuTitle;
            this.buttonBackdrop = buttonBackdrop;
            this.indicatorLight = indicatorLight;
            this.startingPos = startingPos;
            this.buttonBackdropPos = buttonBackdropPos;
            this.menuTitlePos = menuTitlePos;
            this.buttonBackground = buttonBackground;
            this.buttonText = buttonText;
            this.verticalSpacing = verticalSpacing;
            this.menuHoverChangeSoundEffect = menuHoverChangeSoundEffect;
            this.menuClickedSoundEffect = menuClickedSoundEffect;
            this.font = font;
            this.screenSize = screenSize;
            textColor = new Color[buttonText.Length];
        }

        //Complicated (More generalized)
        public MenuHandler(Texture2D menuTitle, Texture2D buttonBackdrop, Vector2 startingPos, Vector2 buttonBackdropPos, Vector2 menuTitlePos, int verticalSpacing, Texture2D buttonBackground, String[] buttonText, SpriteFont font, SoundEffect menuHoverChangeSoundEffect, SoundEffect menuClickedSoundEffect, Vector2 screenSize)
        {
            this.menuTitle = menuTitle;
            this.buttonBackdrop = buttonBackdrop;
            this.startingPos = startingPos;
            this.buttonBackdropPos = buttonBackdropPos;
            this.menuTitlePos = menuTitlePos;
            this.buttonBackground = buttonBackground;
            this.buttonText = buttonText;
            this.verticalSpacing = verticalSpacing;
            this.menuHoverChangeSoundEffect = menuHoverChangeSoundEffect;
            this.menuClickedSoundEffect = menuClickedSoundEffect;
            this.font = font;
            this.screenSize = screenSize;
            textColor = new Color[buttonText.Length];
        }

        //Basic Menu Update
        public void Update(GameTime gameTime, MouseState mouseStateCurrent, MouseState mouseStatePrevious, Vector2 screenSize, Vector2 startingPos, Vector2 menuTitlePos, Vector2 drawScale, Color[] orginalTextColor, Boolean soundNotMuted)
        {
            this.screenSize = screenSize;
            this.startingPos = startingPos;
            this.menuTitlePos = menuTitlePos;
            this.drawScale = drawScale;
            this.orginalTextColor = orginalTextColor;
            menuNumberSelected = 0;
            keybState = Keyboard.GetState();

            //Up Key Down what to do (Sets Boolean)  Menu Up
            if (keybState.IsKeyDown(Keys.Up) && !isPressedUp)
            {
                isPressedUp= true;
                {
                    if (menuNumberHover > 1)
                    {
                        menuNumberHover--;
                    }
                    else
                    {
                        menuNumberHover = buttonText.Length;
                    }
                }
            }
            //Up Key Up Logic (Resets Boolean)
            if (keybState.IsKeyUp(Keys.Up))
            {
                if (isPressedUp)
                    isPressedUp = false;
            }

            //Down Key Down what to do (Sets Boolean)  Menu Up
            if (keybState.IsKeyDown(Keys.Down) && !isPressedDown)
            {
                isPressedDown = true;
                {
                    if (menuNumberHover < buttonText.Length)
                    {
                        menuNumberHover++;
                    }
                    else
                    {
                        menuNumberHover = 1;
                    }

                    //Note this is where all the things you want to do when it's pressed go. EX: If menuNumberSelect == 1 do this Ect.. 
                }
            }
            //Down Key Up Logic (Resets Boolean)
            if (keybState.IsKeyUp(Keys.Down))
            {
                if (isPressedDown)
                    isPressedDown = false;
            }

            //Enter Key Down what to do (Sets Boolean)  Menu Up
            if (keybState.IsKeyDown(Keys.Enter) && !isPressedEnter)
            {
                isPressedEnter = true;
                {
                    menuNumberSelected = menuNumberHover;
                    menuNumberHoverPrevious = menuNumberHover;
                    if (soundNotMuted == true)
                    {
                        menuClickedSoundEffect.Play();
                    }
                }
            }
            //Enter Key Up Logic (Resets Boolean)
            if (keybState.IsKeyUp(Keys.Enter) && menuNumberHoverPrevious!= menuNumberHover)
            {
                if (isPressedEnter)
                    isPressedEnter = false;
            }

            for (int j = 0; j < (buttonText.Length); j++)
            {
                //Draw Location Calculations
                if (buttonBackdrop != null)
                    drawPosition.Y = ((screenSize.Y - (buttonBackdrop.Height * drawScale.Y)) + (35 * drawScale.Y)) + (((buttonBackground.Height * drawScale.Y) * j) + ((verticalSpacing * drawScale.Y) * j));
                else
                    drawPosition.Y = startingPos.Y + (((buttonBackground.Height * drawScale.Y) * j) + ((verticalSpacing * drawScale.Y) * j)); //(screenSize.Y - (buttonBackdrop.Height * drawScale.Y)) + (35*drawScale.Y)
                if (buttonBackdrop != null)
                    drawPosition.X = screenSize.X - ((buttonBackdrop.Width * drawScale.X) - (39 * drawScale.X));
                else
                    drawPosition.X = startingPos.X;

                if ((mouseStateCurrent.Y > drawPosition.Y && mouseStateCurrent.Y < (drawPosition.Y + (buttonBackground.Height * drawScale.Y))) && (mouseStateCurrent.X > drawPosition.X && mouseStateCurrent.X < (drawPosition.X + (buttonBackground.Width * drawScale.X))))
                    testButtonMouseOver = true;
                else
                    testButtonMouseOver = false;

                if ((testButtonMouseOver == true && (mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released)) || overRideButtonPress == true)
                {
                    //testButtonDrawColor = Color.Red;
                    if (soundNotMuted == true)
                    {
                        menuClickedSoundEffect.Play();
                    }

                    menuNumberSelected = j + 1;
                    menuNumberHoverPrevious = j + 1;
                    textColor[j] = Color.Green;
                    overRideButtonPress = false;
                }
                else if (testButtonMouseOver == true)
                {
                    //testButtonDrawColor = Color.LightGray;
                    menuNumberHover = j +1;
                }
                else
                {
                    textColor[j] = orginalTextColor[j];
                    //testButtonDrawColor = Color.White;
                }

                if (menuNumberHover == j + 1)
                {
                    textColor[j] = Color.LimeGreen;
                }
            }

            //Sound Effect Logic
            if (menuNumberHover != menuNumberHoverPrevious)
            {
                if (soundNotMuted == true)
                {
                    menuHoverChangeSoundEffect.Play();
                }
            }

            menuNumberHoverPrevious = menuNumberHover;
        }

        //Complex Menu Update

        //Update(GameTime gameTime, MouseState mouseStateCurrent, MouseState mouseStatePrevious, Vector2 screenSize, Vector2 startingPos, Vector2 menuTitlePos, Vector2 drawScale, Color[] orginalTextColor, Boolean soundNotMuted)

        public void Update(GameTime gameTime, MouseState mouseStateCurrent, MouseState mouseStatePrevious, Vector2 screenSize, Vector2 startingPos, Vector2 buttonBackdropPos, Vector2 menuTitlePos, Vector2 drawScale, Color[] orginalTextColor, Boolean soundNotMuted)
        {
            this.screenSize = screenSize;
            this.startingPos = startingPos;
            this.buttonBackdropPos = buttonBackdropPos;
            this.menuTitlePos = menuTitlePos;
            this.drawScale = drawScale;
            this.orginalTextColor = orginalTextColor;
            menuNumberSelected = 0;
            keybState = Keyboard.GetState();

            //Up Key Down what to do (Sets Boolean)  Menu Up
            if (keybState.IsKeyDown(Keys.Up) && !isPressedUp)
            {
                isPressedUp = true;
                {
                    if (menuNumberHover > 1)
                    {
                        menuNumberHover--;
                    }
                    else
                    {
                        menuNumberHover = buttonText.Length;
                    }
                }
            }
            //Up Key Up Logic (Resets Boolean)
            if (keybState.IsKeyUp(Keys.Up))
            {
                if (isPressedUp)
                    isPressedUp = false;
            }

            //Down Key Down what to do (Sets Boolean)  Menu Up
            if (keybState.IsKeyDown(Keys.Down) && !isPressedDown)
            {
                isPressedDown = true;
                {
                    if (menuNumberHover < buttonText.Length)
                    {
                        menuNumberHover++;
                    }
                    else
                    {
                        menuNumberHover = 1;
                    }

                    //Note this is where all the things you want to do when it's pressed go. EX: If menuNumberSelect == 1 do this Ect.. 
                }
            }
            //Down Key Up Logic (Resets Boolean)
            if (keybState.IsKeyUp(Keys.Down))
            {
                if (isPressedDown)
                    isPressedDown = false;
            }

            //Enter Key Down what to do (Sets Boolean)  Menu Up
            if (keybState.IsKeyDown(Keys.Enter) && !isPressedEnter)
            {
                isPressedEnter = true;
                {
                    menuNumberSelected = menuNumberHover;
                    menuNumberHoverPrevious = menuNumberHover;
                    if (soundNotMuted == true)
                    {
                        menuClickedSoundEffect.Play();
                    }
                }
            }
            //Enter Key Up Logic (Resets Boolean)
            if (keybState.IsKeyUp(Keys.Enter))
            {
                if (isPressedEnter)
                    isPressedEnter = false;
            }

            for (int j = 0; j < (buttonText.Length); j++)
            {
                //Draw Location Calculations
                //if (buttonBackdrop != null)
                    //drawPosition.Y = ((screenSize.Y - (buttonBackdrop.Height * drawScale.Y)) + (35 * drawScale.Y)) + (((buttonBackground.Height * drawScale.Y) * j) + ((verticalSpacing * drawScale.Y) * j));
                //else
                drawPosition.Y = startingPos.Y + (((buttonBackground.Height * drawScale.Y) * j) + ((verticalSpacing * drawScale.Y) * j)); //(screenSize.Y - (buttonBackdrop.Height * drawScale.Y)) + (35*drawScale.Y)
                //if (buttonBackdrop != null)
                    //drawPosition.X = screenSize.X - ((buttonBackdrop.Width * drawScale.X) - (39 * drawScale.X));
                //else
                 drawPosition.X = startingPos.X;

                if ((mouseStateCurrent.Y > drawPosition.Y && mouseStateCurrent.Y < (drawPosition.Y + (buttonBackground.Height * drawScale.Y))) && (mouseStateCurrent.X > drawPosition.X && mouseStateCurrent.X < (drawPosition.X + (buttonBackground.Width * drawScale.X))))
                    testButtonMouseOver = true;
                else
                    testButtonMouseOver = false;

                if ((testButtonMouseOver == true && (mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released)) || overRideButtonPress == true)
                {
                    //testButtonDrawColor = Color.Red;
                    if (soundNotMuted == true)
                    {
                        menuClickedSoundEffect.Play();
                    }

                    menuNumberSelected = j + 1;
                    menuNumberHoverPrevious = j + 1;
                    textColor[j] = Color.Green;
                    overRideButtonPress = false;
                }
                else if (testButtonMouseOver == true)
                {
                    //testButtonDrawColor = Color.LightGray;
                    menuNumberHover = j + 1;
                }
                else
                {
                    textColor[j] = orginalTextColor[j];
                    //testButtonDrawColor = Color.White;
                }

                if (menuNumberHover == j + 1)
                {
                    textColor[j] = Color.White;
                }
            }

            //Sound Effect Logic
            if (menuNumberHover != menuNumberHoverPrevious)
            {
                if (soundNotMuted == true)
                {
                    menuHoverChangeSoundEffect.Play();
                }
            }

            menuNumberHoverPrevious = menuNumberHover;
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            if(menuTitle != null)
                spriteBatch.Draw(menuTitle, menuTitlePos, null, Color.White, 0, new Vector2(0), drawScale, SpriteEffects.None, 0);
            if(buttonBackdrop != null)
                spriteBatch.Draw(buttonBackdrop, buttonBackdropPos, null, Color.White, 0, new Vector2(0), drawScale, SpriteEffects.None, 0);

            for (int j = 0; j < (buttonText.Length); j++)
            {
                //Draw Location Calculations
                //if(buttonBackdrop != null)
                    //drawPosition.Y = ((screenSize.Y - (buttonBackdrop.Height * drawScale.Y)) + (35 * drawScale.Y)) + (((buttonBackground.Height * drawScale.Y) * j) + ((verticalSpacing * drawScale.Y) * j));
                //else
                drawPosition.Y = startingPos.Y + (((buttonBackground.Height * drawScale.Y) * j) + ((verticalSpacing * drawScale.Y) * j)); //(screenSize.Y - (buttonBackdrop.Height * drawScale.Y)) + (35*drawScale.Y)
                //if (buttonBackdrop != null)
                    //drawPosition.X = screenSize.X - ((buttonBackdrop.Width * drawScale.X) - (39 * drawScale.X));
               // else
                   drawPosition.X = startingPos.X;

                    textPosition.Y = drawPosition.Y + (((buttonBackground.Height * drawScale.Y) / 2) - ((font.MeasureString(buttonText[j]).Y * drawScale.Y) / 2));
                    textPosition.X = drawPosition.X + (((buttonBackground.Width * drawScale.X) / 2) - ((font.MeasureString(buttonText[j]).X * drawScale.X) / 2));

                //Actual Drawing
                    spriteBatch.Draw(buttonBackground, drawPosition, null, Color.White, 0f, new Vector2(0, 0), drawScale, SpriteEffects.None, 1f);

                    spriteBatch.DrawString(font, buttonText[j], textPosition, textColor[j], 0f, new Vector2(0, 0), drawScale, SpriteEffects.None, 1f);
            }

            if(indicatorLight !=null && menuNumberHover != 0)
                spriteBatch.Draw(indicatorLight, new Vector2(screenSize.X - ((buttonBackdrop.Width * drawScale.X) - (8 * drawScale.X)), ((screenSize.Y - (buttonBackdrop.Height * drawScale.Y)) + (40 * drawScale.Y)) + (((buttonBackground.Height * drawScale.Y) * (menuNumberHover - 1)) + ((verticalSpacing * drawScale.Y) * (menuNumberHover - 1)))), null, Color.White, 0f, new Vector2(0, 0), drawScale, SpriteEffects.None, 1f);
        }

        public int menuNumberSelection()
        {
            return menuNumberSelected;
        }

        public void setMenuHoverNumber(int menuNumberHover)
        {
            this.menuNumberHover = menuNumberHover;
        }

        public void setMenuHoberNumberPrevious(int menuNumberHoverPrevious)
        {
            this.menuNumberHoverPrevious = menuNumberHoverPrevious;
        }

        public void setSimulatedButtonPress(int buttonToPress)
        {
            menuNumberHover = buttonToPress;
            menuNumberHoverPrevious = buttonToPress;
            overRideButtonPress = true;
        }

        public void resetEnterTripped()
        {
            isPressedEnter = true;
        }
    }
}
