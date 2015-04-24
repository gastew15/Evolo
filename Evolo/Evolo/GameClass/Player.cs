using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

/** Class Discription
 * Evolo class to create and handle player objects in the game
 * Author: Gavin
 * Version: 4/24/14
 */

namespace Evolo.GameClass
{
    class Player
    {
        Vector2 position, scaleSize;
        Texture2D playerTexture;
        Color drawColor;
        double xVelocity = 0.0;
        double yVelocity = 0.0;
        double gravity;
        double friction;


        public Player(Texture2D playerTexture, Vector2 position, double gravity, double friction)
        {
            this.playerTexture = playerTexture;
            this.position = position;
            this.gravity = gravity;
            this.friction = friction;
        }

        public void Update(Vector2 scaleSize)
        {
            this.scaleSize = scaleSize;

            position.X += (float)xVelocity * scaleSize.X;
            position.Y += (float)yVelocity * scaleSize.Y;

            if (xVelocity > 0 && xVelocity - friction >= 0)
                xVelocity -= friction * scaleSize.X;
            else if(xVelocity < 0 && xVelocity + friction >= 0)
                xVelocity += friction * scaleSize.X;
            else
                xVelocity = 0;

            if (yVelocity - gravity >= 0)
                yVelocity -= gravity * scaleSize.Y;
            else
                yVelocity = 0;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            spriteBatch.Draw(playerTexture, position, null, drawColor, 0f, new Vector2(0, 0), scaleSize, spriteEffects, 1f); 
        }

        public void Jump()
        {
            yVelocity = 2.0;
        }

        public Color getColor()
        {
            return drawColor;
        }

        public void setColor(Color color)
        {
            drawColor = color;
        }

        public void setPosition(Vector2 position)
        {
            this.position = position;
        }

        public Vector2 getPosition()
        {
            return position;
        }

        public double getGravity()
        {
            return gravity;
        }

        public void setGravity(double gravity)
        {
            this.gravity = gravity;
        }

        public double getFriction()
        {
            return friction;
        }

        public void setFriction(double friction)
        {
            this.friction = friction;
        }

        public double getXVelocity()
        {
            return xVelocity;
        }

        public void setXVelocity(double xVelocity)
        {

        }

        public double getYVelocity()
        {
            return yVelocity;
        }

        public void setYVelocity(double yVelocity)
        {

        }
    }
}
