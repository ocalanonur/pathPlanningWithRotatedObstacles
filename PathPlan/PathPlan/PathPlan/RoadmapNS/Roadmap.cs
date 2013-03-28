﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PathPlan.HelperClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PathPlan.ScenarioNS;
using PathPlan.ObstacleNS;
using Microsoft.Xna.Framework.Input;
using PathPlan.AgentNS;

namespace PathPlan.RoadmapNS
{
    public class Roadmap : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public int numberOfSample;
        public int depth;
        public List<Configuration> samples = new List<Configuration>();

        private string samplingMethod = "AroundObstacles";

        private Game1 game;
        private GraphicsDeviceManager graphicDevice;
        private SpriteBatch spriteBatch;
        private Texture2D texture;
        private Scenario scenario;
        private Random rand = new Random();

        private int displayWidth;
        private int displayHeight;

        private float agentTextureWidth;
        private float agentTextureHeight;
        private bool drawRoadmap = false;

        public Roadmap(Game1 game,GraphicsDeviceManager graphicDevice,SpriteBatch spriteBatch,Texture2D texture,Scenario scenario, int numberOfSample, int depth, float agentTextureWidth, float agentTextureHeight)
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
            displayWidth = graphicDevice.PreferredBackBufferWidth;
            displayHeight = graphicDevice.PreferredBackBufferHeight;
            fillSampleListRandom(samplingMethod);     // "Random" , "AroundObstacles" , "OnlyAroundObstacles"
            connectConfiguration();
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
                Texture2D blank = new Texture2D(game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                blank.SetData(new[] { Color.White });
                spriteBatch.Begin();
                foreach (Configuration conf in samples)
                {
                    aPositionAdjusted = new Rectangle((int)conf.X + (int)(conf.Width / 2), (int)conf.Y + (int)(conf.Height / 2), (int)conf.Width, (int)conf.Height);
                    spriteBatch.Draw(texture, aPositionAdjusted, new Rectangle(0, 0, 2, 6), Color.Green, conf.Rotation, new Vector2(2 / 2, 6 / 2), SpriteEffects.None, 0);
                    foreach (Configuration nbConf in conf.neighbors)
                    {
                        float angle = (float)Math.Atan2(nbConf.CollisionRectangle.position.Y - conf.CollisionRectangle.position.Y, nbConf.CollisionRectangle.position.X - conf.CollisionRectangle.position.X);
                        float length = Vector2.Distance(conf.CollisionRectangle.position, nbConf.CollisionRectangle.position);
                        spriteBatch.Draw(blank, new Vector2(conf.CollisionRectangle.position.X + conf.Width/2, conf.CollisionRectangle.position.Y+ conf.Height/2), null, Color.White, angle, Vector2.Zero, new Vector2(length, (float)1), SpriteEffects.None, 0);
                    }

                }
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }

        private void fillSampleListRandom( string method )
        {
            Configuration conf;
            switch (method)
            {
                case "Random":
                    for (int i = 0; i < numberOfSample; i++)
                        samples.Add(getFreeConfiguration());
                    break;
                case "AroundObstacles":
                    for (int i = 0; i < numberOfSample; i++)
                    {
                        conf = getRandomConfig();
                        Agent a = new Agent(game, spriteBatch, texture, conf.X, conf.Y, conf.Width, conf.Height, conf.Rotation);
                        if (a.config.isOnObstacle(scenario.obstacles))
                        {
                            Vector2 randomDirection;
                            do
                            {
                                randomDirection = new Vector2((float)rand.NextDouble() * 2 - 1, (float)rand.NextDouble() * 2 - 1);
                            } while (randomDirection.X == 0 && randomDirection.Y == 0);

                            do
                            {
                                a.config.ChangePosition(randomDirection.X, randomDirection.Y);
                            } while (a.config.isOnObstacle(scenario.obstacles));

                            samples.Add(a.config);
                        }
                        else
                            samples.Add(getFreeConfiguration());    // Eğer sample boşlukta çıktıysa direk alınır.
                    }
                    break;
                case "OnlyAroundObstacles":
                    for (int i = 0; i < numberOfSample; i++)
                    {
                        conf = getRandomConfig();
                        Agent a = new Agent(game, spriteBatch, texture, conf.X, conf.Y, conf.Width, conf.Height, conf.Rotation);
                        if (a.config.isOnObstacle(scenario.obstacles))
                        {
                            Vector2 randomDirection;
                            do
                            {

                                randomDirection = new Vector2((float)rand.NextDouble() * 2 - 1, (float)rand.NextDouble() * 2 - 1);
                            } while (randomDirection.X == 0 && randomDirection.Y == 0);

                            do
                            {
                                a.config.ChangePosition(randomDirection.X, randomDirection.Y);
                            } while (a.config.isOnObstacle(scenario.obstacles));

                            samples.Add(a.config);
                        }
                    }
                    break;
            }
        }

        private void fillSampleListAroundObstacles()
        {
            Configuration conf;
            for (int i = 0; i < numberOfSample; i++)
            {
                conf = getRandomConfig();
                Agent a = new Agent(game, spriteBatch, texture, conf.X, conf.Y, conf.Width, conf.Height, conf.Rotation);
                if (a.config.isOnObstacle(scenario.obstacles))
                {
                    Vector2 randomDirection;
                    do
                    {

                        randomDirection = new Vector2((float)rand.NextDouble() * 2 - 1, (float)rand.NextDouble() * 2 - 1);
                    } while (randomDirection.X == 0 && randomDirection.Y == 0);

                    do
                    {
                        a.config.ChangePosition(randomDirection.X, randomDirection.Y);
                    } while (a.config.isOnObstacle(scenario.obstacles));

                    samples.Add(a.config);

                }
                else
                {
                    // Eğer sample boşlukta çıktıysa direk alınır.
                    //samples.Add(getFreeConfiguration());
                }
            }
        }

        private Configuration getFreeConfiguration()
        {
            Configuration conf;
            bool continueSearch = true;
            do
            {
                conf = getRandomConfig();
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

        private void connectConfiguration()
        {
            foreach (Configuration c1 in samples)
            {
                foreach (Configuration c2 in samples)
                {
                    if (c1 == c2)   // Eğer yapılandırmalar aynı ise birbirine bağlanmaz.
                        continue;
                    if (c1.neighbors.Count < depth) // Eğer eklenmiş komşu yapılandırma sayısı henüz depth değerine ulaşmamışsa...
                    {
                        if (c1.isConnectable(c2, scenario.obstacles))
                        {
                            c1.neighbors.Add(c2);
                            c2.neighbors.Add(c1);   // SOnradan
                        }
                    }
                    else    // Komşu sayısı sınırına gelinmiş. Ama daha yakın komşu eklenmesi gerekiyor. En uzak komşu çıkarılacak.
                    {
                        int farthestIndex = c1.getFarthestNeighborConfIndex();
                        //float neighborDistance = Vector2.Distance(c1.CollisionRectangle.position, c1.neighbors[farthestIndex].CollisionRectangle.position);
                        //float newConfDistance = Vector2.Distance(c1.CollisionRectangle.position, c2.CollisionRectangle.position);
                        float neighborDistance = c1.distance(c1.neighbors[farthestIndex]);
                        float newConfDistance = c1.distance(c2);
                        if (newConfDistance < neighborDistance)
                        {
                            if (c1.isConnectable(c2, scenario.obstacles))
                            {
                                c1.neighbors[farthestIndex].neighbors.Remove(c1);   /// Sonradan
                                c1.neighbors.RemoveAt(farthestIndex);
                                c1.neighbors.Add(c2);
                                c2.neighbors.Add(c1);   /// Sonradan
                            }
                        }
                    }
                }
            }
        }

        private Configuration getRandomConfig()
        {
            return new Configuration(rand.Next(0, (int)(displayWidth - agentTextureWidth)), rand.Next(0, (int)(displayHeight - agentTextureHeight)), agentTextureWidth, agentTextureHeight, (float)rand.NextDouble() * MathHelper.TwoPi);
        }

    }
}
