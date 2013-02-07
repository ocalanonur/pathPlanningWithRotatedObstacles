using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PathPlan.HelperClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PathPlan.ScenarioNS;
using PathPlan.ObstacleNS;
using Microsoft.Xna.Framework.Input;

namespace PathPlan.RoadmapNS
{
    class Roadmap : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public int numberOfSample;
        public int depth;
        public List<Configuration> samples = new List<Configuration>();

        private Game game;
        private GraphicsDeviceManager graphicDevice;
        private SpriteBatch spriteBatch;
        private Texture2D texture;
        private Scenario scenario;
        private Random rand = new Random();

        private float agentTextureWidth;
        private float agentTextureHeight;
        private bool drawRoadmap = false;

        public Roadmap(Game game,GraphicsDeviceManager graphicDevice,SpriteBatch spriteBatch,Texture2D texture,Scenario scenario, int numberOfSample, int depth, float agentTextureWidth, float agentTextureHeight)
            : base(game)
        {
            this.game = game;
            this.graphicDevice = graphicDevice;
            this.spriteBatch = spriteBatch;
            this.texture = texture;
            this.scenario = scenario;
            this.numberOfSample = numberOfSample;
            this.depth = depth;
            this.agentTextureWidth = agentTextureWidth;
            this.agentTextureHeight = agentTextureHeight;
            fillSampleList();
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.R) && !drawRoadmap)
                drawRoadmap = true;
            else if (Keyboard.GetState().IsKeyDown(Keys.R) && drawRoadmap)
                drawRoadmap = false;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Rectangle aPositionAdjusted;
            if (drawRoadmap)
            {
                spriteBatch.Begin();
                foreach (Configuration conf in samples)
                {
                    aPositionAdjusted = new Rectangle((int)conf.X + (int)(conf.Width / 2), (int)conf.Y + (int)(conf.Height / 2), (int)conf.Width, (int)conf.Height);
                    spriteBatch.Draw(texture, aPositionAdjusted, new Rectangle(0, 0, 2, 6), Color.Green, conf.Rotation, new Vector2(2 / 2, 6 / 2), SpriteEffects.None, 0);
                }
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }

        private void fillSampleList()
        {
            for (sbyte i = 0; i < numberOfSample; i++)
                samples.Add(getFreeConfiguration());
        }

        private Configuration getFreeConfiguration()
        {
            int displayWidth = graphicDevice.PreferredBackBufferWidth;
            int displayHeight = graphicDevice.PreferredBackBufferHeight;
            Configuration conf;
            bool continueSearch = true;
            do
            {
                conf = new Configuration(rand.Next(0, (int)(displayWidth - agentTextureWidth)), rand.Next(0, (int)(displayHeight - agentTextureHeight)), agentTextureWidth, agentTextureHeight, (float)rand.NextDouble()*6);
                continueSearch = false;
                foreach (Obstacle obs in scenario.obstacles)
                {
                    if (conf.Intersects(obs.config))
                    {
                        continueSearch = true;
                        break;
                    }
                }
            } while (continueSearch);
            return conf;
        }
    }
}
