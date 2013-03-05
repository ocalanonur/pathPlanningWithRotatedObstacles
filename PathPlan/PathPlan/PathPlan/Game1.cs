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
using PathPlan.ScenarioNS;
using PathPlan.ObstacleNS;
using PathPlan.HelperClasses;
using PathPlan.AgentNS;
using PathPlan.RoadmapNS;
using PathPlan.HelperClasses.ShortestPathAlgorithm;


namespace PathPlan
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;

        Texture2D t;
        Scenario scenario;
        Roadmap roadmap;
        Agent agent;
        Dijkstra dijsktra;

        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("myFont");
            t = Content.Load<Texture2D>("Square");
            scenario = new Scenario(this, spriteBatch, t, "Scenario2");
            roadmap = new Roadmap(this, graphics, spriteBatch, t, scenario, 100, 20, 20, 60);
            dijsktra = new Dijkstra(roadmap);
            agent = new Agent(this, spriteBatch, t, 0, 0, 20, 60, 0.0F, Color.Yellow,dijsktra);
            
            
            roadmap.Enabled = true;
            agent.Enabled = true;
            this.Components.Add(agent);
            this.Components.Add(roadmap);
            
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            // TODO: Add your update logic here
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            //spriteBatch.Begin();
            //spriteBatch.DrawString(font, agent.config.Rotation.ToString(), new Vector2(90, 90), Color.Gold);
            //spriteBatch.End();          
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
