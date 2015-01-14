using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System.Runtime.InteropServices;
using StarByte.io;
using StarByte.misc;
using StarByte.graphics;
using Evolo.GameClass;

namespace Evolo.GameClass
{
    class Background
    {
        private string resolutionState;
        private static int blockAmount = 3680;
        private Texture2D bBlockTexture;
        private int colorDistance = 500;
        private double colorStartHeight = 500;
        //Test Block Variables
        private ColorGradient[] colorBlock2 = new ColorGradient[blockAmount];
        private ColorGradient[] colorBlock1 = new ColorGradient[blockAmount];
        private Rectangle[] bBlockRectangle = new Rectangle[blockAmount];
        private Color[] finalColor = new Color[blockAmount];
        private Random rand = new Random();
        private Boolean colorStartUp = false;
        //(s)tarting color, (m)iddle color, (e)nd color, (r)andom color variance for mid (mid only becuase it makes the colors change when they cross mid and it looks bad)
        private int sRed, sBlue, sGreen, mRed, mBlue, mGreen, eRed, eBlue, eGreen;
        private int[] rRed = new int[blockAmount];
        private int[] rGreen = new int[blockAmount];
        private int[] rBlue = new int[blockAmount];
        private Vector2[] initBlockLocation = new Vector2[blockAmount];

        public void Initialize()
        {
            Random rand = new Random();
            //Obviously just the rectangle that the block uses
            int x = 0;
            int p = 0;
            for (int i = 0; i < bBlockRectangle.Length; i++)
            {
                bBlockRectangle[i] = new Rectangle(
                    (int)(16 * p),
                    (int)(16 * x),
                    (int)(16),
                    (int)(16));
                p++;
                if (p > 79)
                {
                    x++;
                    p = 0;
                }
                initBlockLocation[i] = new Vector2(bBlockRectangle[i].X, bBlockRectangle[i].Y);
            }
            //Colors for (s)tart, (m)iddle and (e)nd
            sRed =0;
            sGreen = 0;
            sBlue = 0;
            mRed = 20;
            mGreen = 10;
            mBlue = 30;
            eRed = 200;
            eGreen = 30;
            eBlue = 50;

            //The set up for the color - First is the RGB Value orginally, and Second is the RGB that it will end at
            for (int j = 0; j < blockAmount; j++)
            {
                rRed[j] = rand.Next(0, 6) - 3;
                rGreen[j] = rand.Next(0, 6) - 3;
                rBlue[j] = rand.Next(0, 6) - 3;
                colorBlock1[j] = new ColorGradient(new Vector3(sRed + (rand.Next(0, 6) - 3), sGreen + (rand.Next(0, 6) - 3), sBlue + (rand.Next(0, 6) - 3)), new Vector3(mRed + rRed[j], mGreen + rGreen[j], mBlue + rBlue[j]));
                colorBlock2[j] = new ColorGradient(new Vector3(mRed + rRed[j], mGreen + rGreen[j], mBlue + rBlue[j]), new Vector3(eRed + (rand.Next(0, 12) - 6), eGreen + (rand.Next(0, 12) - 6), eBlue + (rand.Next(0, 12) - 6)));
            }
        }

        public void LoadContent(ContentManager Content)
        {
            bBlockTexture = Content.Load<Texture2D>("Sprites and Pictures/SkyBlock4");
        }

        public void Update(GameTime gameTime, float millisecondsElapsedGameTime)
        {
            
            if (Keyboard.GetState().IsKeyDown(Keys.J))
            {
                for (int j = 0; j < bBlockRectangle.Length; j++)
                    initBlockLocation[j].Y--;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.U))
            {
                for (int j = 0; j < bBlockRectangle.Length; j++)
                    initBlockLocation[j].Y++;
            }


            //Get data from the file to choose right options for blocks on startup
            resolutionState = GlobalVar.OptionsArray[0] + "x" + GlobalVar.OptionsArray[1];
            for (int i = 0; i < colorBlock2.Length; i++)
            {
                //Just the movement for the test Block
                if ((millisecondsElapsedGameTime % 2) == 0)
                {

                    //boolean for making the horizon go up/down
                    if (colorStartHeight <= (GlobalVar.ScreenSize.Y / 3) + 50)
                        colorStartUp = true;
                    if (colorStartHeight >= (GlobalVar.ScreenSize.Y * 1.5) - 50)
                        colorStartUp = false;

                    //Color change for day/night cycle
                    if (colorStartHeight <= (GlobalVar.ScreenSize.Y / 3) * 2)
                    {
                        if (colorStartUp == true)
                            colorStartHeight = colorStartHeight + (0.0000001f * (double)Math.Abs(colorStartHeight - (GlobalVar.ScreenSize.Y / 3)));
                        else
                            colorStartHeight = colorStartHeight - (0.0000001f * (double)Math.Abs(colorStartHeight - (GlobalVar.ScreenSize.Y / 3)));
                    }
                    else
                    {
                        if (colorStartUp == true)
                            colorStartHeight = colorStartHeight + (0.0000001f * (double)Math.Abs(colorStartHeight - (GlobalVar.ScreenSize.Y * 1.5)));
                        else
                            colorStartHeight = colorStartHeight - (0.0000001f * (double)Math.Abs(colorStartHeight - (GlobalVar.ScreenSize.Y * 1.5)));
                    }
                }
                #region Block Scaling Switch
                //Case for manually scaling the background when resolution is changed in game, keeps the same amount of blocks on screen but changes size/spacing
                switch (resolutionState)
                {
                    case "800x600":
                        bBlockRectangle[i] = new Rectangle((int)((initBlockLocation[i].X / 8f) * 5f), (int)((initBlockLocation[i].Y / 8f) * 7f), (int)(0 + 10), (int)(0 + 14));
                        if (bBlockRectangle[i].Y < 630)
                            initBlockLocation[i].Y = initBlockLocation[i].Y + 1;
                        else
                        {
                            initBlockLocation[i].Y = -(bBlockRectangle[i].Height) - 1;
                        }
                        break;
                    case "1280x720":
                        bBlockRectangle[i] = new Rectangle((int)initBlockLocation[i].X, (int)initBlockLocation[i].Y, (int)(0 + 16), (int)(0 + 16));
                        if (bBlockRectangle[i].Y < GlobalVar.ScreenSize.Y + 1)
                            initBlockLocation[i].Y = initBlockLocation[i].Y + 1;
                        else
                        {
                            initBlockLocation[i].Y = -(bBlockRectangle[i].Height) + 2;
                        }
                        break;
                    case "1366x768":
                        bBlockRectangle[i] = new Rectangle((int)((initBlockLocation[i].X / 8f) * 10f), (int)((initBlockLocation[i].Y / 8f) * 9f), (int)(0 + 20), (int)(0 + 18));
                        if (bBlockRectangle[i].Y < 808)
                            initBlockLocation[i].Y = initBlockLocation[i].Y + 1;
                        else
                        {
                            initBlockLocation[i].Y = -(bBlockRectangle[i].Height) + 3;
                        }
                        break;
                    case "1600x900":
                        bBlockRectangle[i] = new Rectangle((int)((initBlockLocation[i].X / 8f) * 10f), (int)((initBlockLocation[i].Y / 8f) * 10f), (int)(0 + 20), (int)(0 + 20));
                        if (bBlockRectangle[i].Y < GlobalVar.ScreenSize.Y)
                            initBlockLocation[i].Y = initBlockLocation[i].Y + 1;
                        else
                        {
                            initBlockLocation[i].Y = -(bBlockRectangle[i].Height) + 5;
                        }
                        break;
                    case "1920x1080":
                        bBlockRectangle[i] = new Rectangle((int)((initBlockLocation[i].X / 8f) * 12f), (int)((initBlockLocation[i].Y / 8f) * 12f), (int)(0 + 24), (int)(0 + 24));
                        if (bBlockRectangle[i].Y < GlobalVar.ScreenSize.Y)
                            initBlockLocation[i].Y = initBlockLocation[i].Y + 1;
                        else
                        {
                            initBlockLocation[i].Y = -(bBlockRectangle[i].Height) + 9;
                        }
                        break;
                }
                #endregion
                colorBlock1[i].Update(new Vector2(GlobalVar.ScreenSize.X, 0), new Vector2(bBlockRectangle[i].X, bBlockRectangle[i].Y), (int)((colorStartHeight)));
                colorBlock2[i].Update(new Vector2(GlobalVar.ScreenSize.X, (float)colorStartHeight), new Vector2(bBlockRectangle[i].X, bBlockRectangle[i].Y), (int)(GlobalVar.ScreenSize.Y));
                if (bBlockRectangle[i].Y < colorStartHeight)
                {
                    finalColor[i] = colorBlock1[i].GetColor();
                }
                else
                    finalColor[i] = colorBlock2[i].GetColor();
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            //Block Drawing, not * by scale because it breaks the background when resolution is changed and makes everything look ugly
            for (int j = 0; j < bBlockRectangle.Length; j++)
            {
                spriteBatch.Draw(bBlockTexture, new Rectangle((int)(bBlockRectangle[j].X), (int)(bBlockRectangle[j].Y), bBlockRectangle[j].Width, bBlockRectangle[j].Height), null, finalColor[j], 0f, new Vector2(0, 0), SpriteEffects.None, 0f);
            }
        }
        //mostly for testing the values of colors/blocks and writing them to the debug screen
        public Color GetColor()
        {
            return finalColor[10];
        }
        public double getHeight()
        {
            return colorStartHeight;
        }
        public int getDistance()
        {
            return colorDistance;
        }
        public Rectangle getRekt()
        {
            return bBlockRectangle[0];
        }
        public Rectangle getRekt2()
        {
            return bBlockRectangle[3679];
        }
        public void SetResolutionState(String resolutionState)
        {
            this.resolutionState = resolutionState;
        }
    }
}
