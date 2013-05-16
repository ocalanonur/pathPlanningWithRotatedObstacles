using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using PathPlan.HelperClasses;


namespace PathPlan.ObstacleNS
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Obstacle : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Texture2D texture;
        private SpriteBatch spriteBatch;
        public Configuration config;

        public Obstacle(Game game, SpriteBatch spriteBatch, Texture2D texture, float x, float y, float width, float height, float theInitialRotation)
            : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.texture = texture;
            config = new Configuration(new FloatRectangle(new Vector2(x,y),new Vector2(width,height)), theInitialRotation, "obstacle");
        }

        public Obstacle(Game game, SpriteBatch spriteBatch,Texture2D texture ,FloatRectangle theRectangle, float theInitialRotation)
            : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.texture = texture;
            config = new Configuration(theRectangle, theInitialRotation,"obstacle");
        }

        


        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {

            // TODO: Add your drawing code here
            // TODO: Add your drawing code here
            
            spriteBatch.Begin();
            Rectangle aPositionAdjusted = new Rectangle((int)this.config.X + (int)(this.config.Width / 2), (int)this.config.Y + (int)(this.config.Height / 2), (int)this.config.Width, (int)this.config.Height);
            spriteBatch.Draw(texture, aPositionAdjusted, new Rectangle(0, 0, 2, 6), Color.Red, this.config.Rotation, new Vector2(2 / 2, 6 / 2), SpriteEffects.None, 0);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
