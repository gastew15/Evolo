using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using StarByte.ui;

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

        public void LoadContent(ContentManager Content, SpriteFont font)
        {
            this.font = font;

            //Array Intilization for text & colors to be used in popups
            tutorialPopupText = new String[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
            tutorialPopupColor = new Color[] { Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, };

            //Content Loading
            tutorialPopupTexture = Content.Load<Texture2D>("Sprites and Pictures/tutorialPopup");
            tutorialPopupCloseButtonTexture = Content.Load<Texture2D>("Sprites and Pictures/tutorialPopupCloseButton");
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, "Objective:\nMove player to\nother platform and \nclear the number\nof lines indicated \non the right\n\nControls:\n -Player-\n Left: A\n Right: D\n Jump: W\n\n -Blocks-\n Left: LArrow\n Right: RArrow\n Rotate: UArrow\n SpeedUp: DArrow", new Vector2(32 * GlobalVar.ScaleSize.X + 5, 32 * GlobalVar.ScaleSize.Y + 5), Color.White, 0f, new Vector2(0, 0), GlobalVar.ScaleSize, SpriteEffects.None, 1f);
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
    }
}
