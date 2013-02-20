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
using PathPlan.ObstacleNS;


namespace PathPlan.ScenarioNS
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Scenario
    {
        private Game game;
        private SpriteBatch spriteBatch;
        private Texture2D texture;
        public string name;
        public List<Obstacle> obstacles = new List<Obstacle>();

        public Scenario(Game game,SpriteBatch spriteBatch,Texture2D texture,string name)
        {
            // TODO: Construct any child components here
            this.game = game;
            this.spriteBatch = spriteBatch;
            this.texture = texture;
            this.name = name;
            CreateObstacles();
        }

        public void CreateObstacles()
        {
            Obstacle obs;
            switch (name)
            {
                case "Scenario1":
                    obs = new Obstacle(game, spriteBatch, texture, 200, 0, 50, 500, 0.0F);
                    obs.Enabled = true;
                    obstacles.Add(obs);
                    game.Components.Add(obs);
                    obs = new Obstacle(game, spriteBatch, texture, 0, 200, 800, 30, 0.0F);
                    obstacles.Add(obs);
                    game.Components.Add(obs);
                    break;
                case "Scenario2":
                    obs = new Obstacle(game, spriteBatch, texture, 200, 150, 50, 140, 0.5F);
                    obs.Enabled = true;
                    obstacles.Add(obs);
                    game.Components.Add(obs);
                    obs = new Obstacle(game, spriteBatch, texture, 200, 350, 70, 90, -0.40F);
                    obstacles.Add(obs);
                    game.Components.Add(obs);
                    obs = new Obstacle(game, spriteBatch, texture, 400, 50, 50, 250, 0.9F);
                    obstacles.Add(obs);
                    game.Components.Add(obs);
                    obs = new Obstacle(game, spriteBatch, texture, 530, 200, 50, 250, 0.2F);
                    obstacles.Add(obs);
                    game.Components.Add(obs);
                    break;
            }
        }
    }
}
