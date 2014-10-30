using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/*
 *  StarByte ParticleEffectHandler
 *  Author: G. Stewart
 *  Version: 9/24/14
 */

namespace StarByte.part
{
    class ParticleEffectsHandler
    {

        private Vector2 EmitterLocation, velocity;
        private List<Particle> particles;
        private List<Texture2D> text;
        private List<Texture2D> textures;
        private List<Color> drawPartiColor;
        private int particlesPerDraw, ttl;
        private Color drawParticleColor;
        private Random random;
        private float angle, angularVelocity, size;
        private Rectangle boundingBoxRect;
        private Boolean removeParticle = false;

        public ParticleEffectsHandler(List<Texture2D> textures, List<Color> drawParticleColor, Vector2 Emitterlocation, float angle, float angularVelocity, Vector2 velocity, float size, int ttl, int particlesPerDraw, Rectangle boundingBoxRect)
        {
            this.EmitterLocation = Emitterlocation;
            this.textures = textures;
            this.particles = new List<Particle>();
            this.drawPartiColor = drawParticleColor;
            this.particlesPerDraw = particlesPerDraw;
            this.angle = angle;
            this.angularVelocity = angularVelocity;
            this.velocity = velocity;
            this.size = size;
            this.ttl = ttl;
            this.boundingBoxRect = boundingBoxRect;
        }

        public void Update()
        {
            
            for (int i = 0; i < particlesPerDraw; i++)
            {
                particles.Add(GenerateNewParticle());
            }

            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update();
                if (particles[particle].TTL <= 0)
                {
                    removeParticle = true;
                }

                if (boundingBoxRect != null)
                {
                    //Handles all sides of it leaving the box and delete it
                    if (particles[particle].Position.X > (boundingBoxRect.X + boundingBoxRect.Width))
                    {
                        removeParticle = true;
                    }
                    else if (particles[particle].Position.X < boundingBoxRect.X)
                    {
                        removeParticle = true;
                    }
                    else if (particles[particle].Position.Y > (boundingBoxRect.Y + boundingBoxRect.Height))
                    {
                        removeParticle = true;
                    }
                    else if (particles[particle].Position.Y > boundingBoxRect.Y)
                    {
                        removeParticle = true;
                    }
                        
                }

                if (removeParticle == true)
                {
                    particles.RemoveAt(particle);
                    particle--;
                    removeParticle = false;
                }
            }
        }

        private Particle GenerateNewParticle()
        {
            Texture2D texture = textures[random.Next(textures.Count)];
            Vector2 position = EmitterLocation;
            Color color = drawPartiColor[random.Next(drawPartiColor.Count)];

            return new Particle(texture, position, velocity, angle, angularVelocity, color, size, ttl);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            for (int index = 0; index < particles.Count; index++)
            {
                particles[index].Draw(spriteBatch);
            }
            spriteBatch.End();
        }

    }
}
