using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StarByte.graphics;

namespace Evolo.GameClass
{
    class Cloud
    {
        Texture2D bBlockTexture;
        int colorDistance = 500;
        int colorStartHeight = 500;
        //Test Block Variables
        ColorGradient[] colorBlock = new ColorGradient[41];
        ColorGradient[] colorBlock2 = new ColorGradient[41];
        Rectangle[] bBlockRectangle = new Rectangle[41];
        Color[] finalColor = new Color[41];
        int sRed, sBlue, sGreen, eRed, eBlue, eGreen;
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
            //Obviously just the rectangle that the block uses
            for (int i = 0; i < bBlockRectangle.Length; i++)
            {
                bBlockRectangle[i] = new Rectangle(
                    (int)((0 + (0 * i)) * GlobalVar.ScaleSize.X),
                    (int)((0 + (18 * i)) * GlobalVar.ScaleSize.Y),
                    (int)(GlobalVar.ScreenSize.X * GlobalVar.ScaleSize.X),
                    (int)(18 * GlobalVar.ScaleSize.Y));
            }

            sRed = 0;
            sGreen = 0;
            sBlue =0;
            eRed = 100;
            eGreen = 100;
            eBlue = 100;
            //The set up for the color - First is the RGB Value orginally, and Second is the RGB that it will end at
            for (int j = 0; j < colorBlock.Length; j++)
            {
                colorBlock[j] = new ColorGradient(new Vector3(sRed, sGreen, sBlue), new Vector3(eRed, eGreen, eBlue));
                colorBlock2[j] = new ColorGradient(new Vector3(0,0,0), new Vector3(0,0,0));
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
            bBlockTexture = Content.Load<Texture2D>("Sprites and Pictures/SkyBlock1");
        }

        public void Update(GameTime gameTime, float millisecondsElapsedGameTime)
        {

            if (Keyboard.GetState().IsKeyDown(Keys.H))
            {
                colorDistance++;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Y))
            {
                colorDistance--;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.U))
            {
                colorStartHeight++;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.J))
            {
                colorStartHeight--;
            }
            for (int j = 0; j < bBlockRectangle.Length; j++)
            {
                //Just the movement for the test Block
                if ((millisecondsElapsedGameTime % 2) == 0)
                {
                    if (bBlockRectangle[j].Y < GlobalVar.ScreenSize.Y)
                        bBlockRectangle[j].Y = bBlockRectangle[j].Y + 1;
                    else
                    {
                        bBlockRectangle[j].Y = -17;
                    }
                }

            }
            //Test Block Update
            for (int i = 0; i < colorBlock.Length; i++)
            {
                //Does a size update to the rectangle
                bBlockRectangle[i] = new Rectangle(bBlockRectangle[i].X, bBlockRectangle[i].Y, (int)(GlobalVar.ScreenSize.X * GlobalVar.ScaleSize.X), (int)(18 * GlobalVar.ScaleSize.Y));
                colorBlock[i].Update(new Vector2(GlobalVar.ScreenSize.X, colorStartHeight), new Vector2(bBlockRectangle[i].X, bBlockRectangle[i].Y), (int)((colorDistance) * GlobalVar.ScaleSize.Y));
                colorBlock2[i].Update(new Vector2(GlobalVar.ScreenSize.X, colorStartHeight), new Vector2(bBlockRectangle[i].X, bBlockRectangle[i].Y), (int)((colorDistance) * GlobalVar.ScaleSize.Y));
                if (bBlockRectangle[i].Y < colorStartHeight)
                {
                    finalColor[i] = colorBlock2[i].GetColor();
                }
                else
                    finalColor[i] = colorBlock[i].GetColor();
            }
            /*
            //Testing Clouds
            for (int x = 0; x < cloudnum; x++)
            {
                cloudPosition[x] += new Vector2(0.5f / cloudScale[x].X,0);//new Vector2(0.1f * (millisecondsElapsedGameTime/1000),0);
                if (cloudPosition[x].X >= GlobalVar.ScreenSize.X)
                {
                    cloudPosition[x].X = -288 * cloudScale[x].X;
                    cloudPosition[x].Y = cloudRandom.Next(-64, 784);
                }
            }
             */
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            //Just the test Block Drawing
            for (int j = 0; j < bBlockRectangle.Length; j++)
            {
                spriteBatch.Draw(bBlockTexture, new Rectangle((int)(bBlockRectangle[j].X * GlobalVar.ScaleSize.X), (int)(bBlockRectangle[j].Y * GlobalVar.ScaleSize.Y), bBlockRectangle[j].Width, bBlockRectangle[j].Height), null, finalColor[j], 0f, new Vector2(), SpriteEffects.None, 1f);
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
        public int Getdistance()
        {
            return colorBlock[1].getDist();
        }
        public int getHeight()
        {
            return colorStartHeight;
        }
    }
}
