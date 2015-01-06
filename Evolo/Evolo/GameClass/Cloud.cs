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
    class Cloud
    {
        static int blockAmount = 3600;
        Texture2D bBlockTexture;
        int colorDistance = 500;
        double colorStartHeight = 500;
        //Test Block Variables
        ColorGradient[] colorBlock = new ColorGradient[blockAmount];
        ColorGradient[] colorBlock2 = new ColorGradient[blockAmount];
        Rectangle[] bBlockRectangle = new Rectangle[blockAmount];
        Color[] finalColor = new Color[blockAmount];
        //stupid things remove later
        Random rand = new Random();
        Boolean colorStartUp = false;
        //(s)tarting color, (m)iddle color, (e)nd color, (r)andom color variance for mid (mid only becuase it makes the colors change when they cross mid and it looks bad)
        int sRed, sBlue, sGreen, mRed, mBlue, mGreen, eRed, eBlue, eGreen;
        int[] rRed = new int[blockAmount];
        int[] rGreen = new int[blockAmount];
        int[] rBlue = new int[blockAmount];
        /*
         private Texture2D cloudTexture;
         public Vector2[] cloudPosition;
         public float[] cloudSpeed;
         private Vector2[] cloudScale;
         private Random cloudRandom;
         private Rectangle[] cloudRectangle;
         private int cloudnum = 100;
         */

        public void Initialize()
        {
            Random rand = new Random();
            //Obviously just the rectangle that the block uses
            int x = 0;
            int p = 0;
            for (int i = 0; i < bBlockRectangle.Length; i++)
            {
                bBlockRectangle[i] = new Rectangle(
                    (int)((8 + (16 * p)) * GlobalVar.ScaleSize.X),
                    (int)((0 + (16 * x)) * GlobalVar.ScaleSize.Y),
                    (int)(rand.Next(0,0) + 16 * GlobalVar.ScaleSize.X),
                    (int)(rand.Next(0,0) + 16 * GlobalVar.ScaleSize.Y));
                p++;
                if (p > 79)
                {
                    x++;
                    p = 0;
                }
                
            }
            //Colors for (s)tart, (m)iddle and (e)nd
            sRed =0;
            sGreen = 0;
            sBlue = 0;
            mRed = 20;
            mGreen = 10;
            mBlue = 20;
            eRed = 255;
            eGreen = 30;
            eBlue = 30;
            
            //The set up for the color - First is the RGB Value orginally, and Second is the RGB that it will end at
            for (int j = 0; j < colorBlock.Length; j++)
            {
                rRed[j] = rand.Next(0, 6) - 3;
                rGreen[j] = rand.Next(0, 6) - 3;
                rBlue[j] = rand.Next(0, 6) - 3;
                colorBlock[j] = new ColorGradient(new Vector3(mRed + rRed[j], mGreen + rGreen[j], mBlue + rBlue[j]), new Vector3(eRed + (rand.Next(0, 12) - 6), eGreen + (rand.Next(0, 12) - 6), eBlue + (rand.Next(0, 12) - 6)));
                colorBlock2[j] = new ColorGradient(new Vector3(sRed + (rand.Next(0, 6) - 3), sGreen + (rand.Next(0, 6) - 3), sBlue + (rand.Next(0, 6) - 3)), new Vector3(mRed + rRed[j], mGreen + rGreen[j], mBlue + rBlue[j]));
            }
            
            /*
            cloudRandom = new Random();
            cloudPosition = new Vector2[cloudnum];
            cloudScale = new Vector2[cloudnum];
            cloudRectangle = new Rectangle[cloudnum];
            cloudSpeed = new float[cloudnum];

            for (int x = 0; x < cloudnum; x++)
            {

                int cloudType = cloudRandom.Next(1, 100);
                if (cloudType < 26)
                {
                    cloudType = cloudRandom.Next(1, 4);
                    switch (cloudType)
                    {
                        case 1: cloudRectangle[x] = new Rectangle(0, 0, 96, 64); break;
                        case 2: cloudRectangle[x] = new Rectangle(96, 0, 64, 64); break;
                        case 3: cloudRectangle[x] = new Rectangle(160, 0, 96, 64); break;
                        case 4: cloudRectangle[x] = new Rectangle(256, 0, 32, 128); break;
                    }
                }
                else
                    cloudRectangle[x] = new Rectangle(288, 0, 160, 64);

                double randomPasser = cloudRandom.NextDouble() * (3.0f - 0.5f) + 0.5f;
                cloudPosition[x] = new Vector2(cloudRandom.Next(-32, 1312), cloudRandom.Next(-64, 784));
                cloudScale[x] = new Vector2((float)randomPasser, (float)randomPasser);
                
            }
             */
        }


        public void LoadContent(ContentManager Content)
        {
            //cloudTexture = Content.Load<Texture2D>("Sprites and Pictures/CloudSheet1");
            bBlockTexture = Content.Load<Texture2D>("Sprites and Pictures/SkyBlock3");
        }

        public void Update(GameTime gameTime, float millisecondsElapsedGameTime)
        {
            
            if (Keyboard.GetState().IsKeyDown(Keys.Y))
            {
                colorDistance = colorDistance + 2;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.H))
            {
                colorDistance = colorDistance - 2;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.U))
            {
                colorStartHeight = colorStartHeight + 2;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.J))
            {
                colorStartHeight = colorStartHeight - 2;
            }

            for (int j = 0; j < bBlockRectangle.Length; j++)
            {
                //Just the movement for the test Block
                if ((millisecondsElapsedGameTime % 2) == 0)
                {
                    
                    //Color change for day/night cycle
                    if (colorStartHeight <= 775)
                    {
                        if (colorStartUp == true)
                            colorStartHeight = colorStartHeight + (0.0000001f * (double)Math.Abs(colorStartHeight - 345));
                        else
                            colorStartHeight = colorStartHeight - (0.0000001f * (double)Math.Abs(colorStartHeight - 345));
                    }
                    else
                    {
                        if (colorStartUp == true)
                            colorStartHeight = colorStartHeight + (0.0000001f * (double)Math.Abs(colorStartHeight - 1250));
                        else
                            colorStartHeight = colorStartHeight - (0.0000001f * (double)Math.Abs(colorStartHeight - 1250));
                    }


                    if (colorStartHeight <= 350)
                        colorStartUp = true;
                    if (colorStartHeight >= 1200)
                        colorStartUp = false;


                    if (bBlockRectangle[j].Y < GlobalVar.ScreenSize.Y)
                        bBlockRectangle[j].Y = bBlockRectangle[j].Y + 1;
                    else
                    {
                        bBlockRectangle[j].Y = 1;
                    }
                }
            }
            //Test Block Update
            for (int i = 0; i < colorBlock.Length; i++)
            {
                //Does a size update to the rectangle
                //bBlockRectangle[i] = new Rectangle(bBlockRectangle[i].X, bBlockRectangle[i].Y, (int)(rand.Next(0,22) + 16 * GlobalVar.ScaleSize.X), (int)(rand.Next(0,22) + 16 * GlobalVar.ScaleSize.Y));
                colorBlock[i].Update(new Vector2(GlobalVar.ScreenSize.X, (float)colorStartHeight), new Vector2(bBlockRectangle[i].X, bBlockRectangle[i].Y), (int)((GlobalVar.ScreenSize.Y) * GlobalVar.ScaleSize.Y));
                colorBlock2[i].Update(new Vector2(GlobalVar.ScreenSize.X, 0), new Vector2(bBlockRectangle[i].X, bBlockRectangle[i].Y), (int)(((colorStartHeight)) * GlobalVar.ScaleSize.Y));
                if (bBlockRectangle[i].Y < colorStartHeight)
                {
                    finalColor[i] = colorBlock2[i].GetColor();
                }
                else
                    finalColor[i] = colorBlock[i].GetColor();
            }
          
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            //Just the test Block Drawing
            for (int j = 0; j < bBlockRectangle.Length; j++)
            {
                spriteBatch.Draw(bBlockTexture, new Rectangle((int)(bBlockRectangle[j].X * GlobalVar.ScaleSize.X), (int)(bBlockRectangle[j].Y * GlobalVar.ScaleSize.Y), bBlockRectangle[j].Width, bBlockRectangle[j].Height), null, finalColor[j], 0f, new Vector2(bBlockRectangle[j].Height / 2, bBlockRectangle[j].Width / 2), SpriteEffects.None, 0f);
            }

            /*
            for (int x = 0; x < cloudnum; x++)
            {
                //spriteBatch.Draw(cloudTexture, new Vector2(100,100), cloudRectangle[x], Color.White, 0f, new Vector2(80, 32), new Vector2(1,1), SpriteEffects.None, 1f);
                spriteBatch.Draw(cloudTexture, cloudPosition[x] * GlobalVar.ScaleSize, cloudRectangle[x], Color.White, 0f, new Vector2(0, 0), cloudScale[x] * GlobalVar.ScaleSize, SpriteEffects.None, 1f);
                //spriteBatch.Draw(cloudTexture, new Rectangle(256, 0, 32, 128), null, Color.White, 0f, new Vector2(80, 32), SpriteEffects.None, 1f);
            }
            */
        }
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
            return bBlockRectangle[10];
        }
    }
}
