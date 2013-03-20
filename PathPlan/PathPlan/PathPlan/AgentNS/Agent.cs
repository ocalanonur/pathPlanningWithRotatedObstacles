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
using PathPlan.HelperClasses.ShortestPathAlgorithm;


namespace PathPlan.AgentNS
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Agent : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Game game;
        private SpriteBatch spriteBatch;
        private Texture2D texture;
        public Configuration config;
        public Color color;

        public Configuration startConfig;
        public Configuration goalConfig;

        private bool startConfigLocated = false;
        private bool goalConfigLocated = false;
        private MouseState mauseState;

        private Dijkstra dijkstra;


        public Agent(Game game, SpriteBatch spriteBatch, Texture2D texture, float x, float y, float width, float height, float theInitialRotation, Color color,Dijkstra dijkstra)
            : base(game)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
            this.texture = texture;
            config = new Configuration(new FloatRectangle(new Vector2(x, y), new Vector2(width, height)), theInitialRotation);
            this.color = color;
            this.dijkstra = dijkstra;
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
            

            mauseState = Mouse.GetState();
            if (!startConfigLocated && !goalConfigLocated)
            {
                config = new Configuration(mauseState.X, mauseState.Y, config.Width, config.Height, config.Rotation);
                if (Keyboard.GetState().IsKeyDown(Keys.T))
                    config.Rotation += 0.1F;
                if (mauseState.LeftButton == ButtonState.Pressed)
                {
                    startConfigLocated = true;
                    startConfig = new Configuration(mauseState.X, mauseState.Y, config.Width, config.Height, config.Rotation);
                    dijkstra.startConfig = startConfig;
                }
            }
            else if (startConfigLocated && !goalConfigLocated)
            {
                config = new Configuration(mauseState.X, mauseState.Y, config.Width, config.Height, config.Rotation);
                if (Keyboard.GetState().IsKeyDown(Keys.T))
                    config.Rotation += 0.1F;
                if (mauseState.RightButton == ButtonState.Pressed)
                {
                    goalConfigLocated = true;
                    goalConfig = new Configuration(mauseState.X, mauseState.Y, config.Width, config.Height, config.Rotation);
                    dijkstra.goalConfig = goalConfig;
                    dijkstra.addStartAndGoalConfigtoConnectedRoadmapList();
                    dijkstra.getShortestPath();
                    config = new Configuration(startConfig.X, startConfig.Y, startConfig.Width, startConfig.Height, startConfig.Rotation);
                }
            }
            else
            {
                if (dijkstra.stations.Count != 0)
                {
                    Vector2 yon = dijkstra.stations.ElementAt(0).CollisionRectangle.position - config.CollisionRectangle.position;
                    float mesafe = Vector2.Distance(dijkstra.stations.ElementAt(0).CollisionRectangle.position, config.CollisionRectangle.position);
                    float aci = dijkstra.stations.ElementAt(0).Rotation - config.Rotation;
                    float donusNormalize = aci / mesafe;
                    yon.Normalize();
                    this.config.ChangePosition(yon.X, yon.Y);
                    this.config.Rotation += donusNormalize;
                    if (Vector2.Distance(dijkstra.stations.ElementAt(0).CollisionRectangle.position, config.CollisionRectangle.position) < 1)
                    {
                        //config = dijkstra.stations.ElementAt(0);
                        dijkstra.stations.Dequeue();
                    }
                }
                
                //float mesafe = Vector2.Distance(goalConfig.CollisionRectangle.position, config.CollisionRectangle.position);
                //float aci = goalConfig.Rotation - config.Rotation;
                //float donusNormalize = aci / mesafe;
                //yon.Normalize();
                //this.config.ChangePosition(yon.X, yon.Y);
                //this.config.Rotation += donusNormalize;
            }
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
            Rectangle aPositionAdjusted;
            spriteBatch.Begin();
            
            if (!startConfigLocated && !goalConfigLocated)
            {
                /*
                 * Her iki deðiþkenin false olduðu durumda buraya girilir. Burada baþlangýç ve bitiþ yapýlandýrmalarý girilmemiþtir.
                 */
            }
            else if (startConfigLocated && !goalConfigLocated)
            {
                /*
                 * Burada sadece Agentýn baþlangýç yapýlandýrmasý seçilmiþtir.
                 */
                aPositionAdjusted = new Rectangle((int)startConfig.X + (int)(startConfig.Width / 2), (int)startConfig.Y + (int)(startConfig.Height / 2), (int)startConfig.Width, (int)startConfig.Height);
                spriteBatch.Draw(texture, aPositionAdjusted, new Rectangle(0, 0, 2, 6), Color.Black, startConfig.Rotation, new Vector2(2 / 2, 6 / 2), SpriteEffects.None, 0);
            }
            else
            {
                aPositionAdjusted = new Rectangle((int)startConfig.X + (int)(startConfig.Width / 2), (int)startConfig.Y + (int)(startConfig.Height / 2), (int)startConfig.Width, (int)startConfig.Height);
                spriteBatch.Draw(texture, aPositionAdjusted, new Rectangle(0, 0, 2, 6), Color.Black, startConfig.Rotation, new Vector2(2 / 2, 6 / 2), SpriteEffects.None, 0);
                aPositionAdjusted = new Rectangle((int)goalConfig.X + (int)(goalConfig.Width / 2), (int)goalConfig.Y + (int)(goalConfig.Height / 2), (int)goalConfig.Width, (int)goalConfig.Height);
                spriteBatch.Draw(texture, aPositionAdjusted, new Rectangle(0, 0, 2, 6), Color.Brown, goalConfig.Rotation, new Vector2(2 / 2, 6 / 2), SpriteEffects.None, 0);
            }
            aPositionAdjusted = new Rectangle((int)this.config.X + (int)(this.config.Width / 2), (int)this.config.Y + (int)(this.config.Height / 2), (int)this.config.Width, (int)this.config.Height);
            spriteBatch.Draw(texture, aPositionAdjusted, new Rectangle(0, 0, 2, 6), color, this.config.Rotation, new Vector2(2 / 2, 6 / 2), SpriteEffects.None, 0);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
