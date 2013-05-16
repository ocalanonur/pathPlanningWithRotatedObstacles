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
                    obs = new Obstacle(game, spriteBatch, texture, 200, 150, 30, 100, 0.5F);
                    obs.Enabled = true;
                    obstacles.Add(obs);
                    game.Components.Add(obs);
                    obs = new Obstacle(game, spriteBatch, texture, 200, 350, 30, 80, -0.40F);
                    obstacles.Add(obs);
                    game.Components.Add(obs);
                    obs = new Obstacle(game, spriteBatch, texture, 420, 10, 30, 250, 0.9F);
                    obstacles.Add(obs);
                    game.Components.Add(obs);
                    obs = new Obstacle(game, spriteBatch, texture, 480, 200, 30, 250, 0.2F);
                    obstacles.Add(obs);
                    game.Components.Add(obs);
                    obs = new Obstacle(game, spriteBatch, texture, 20, 50, 150, 30, 0.7F);
                    obstacles.Add(obs);
                    game.Components.Add(obs);
                    obs = new Obstacle(game, spriteBatch, texture, 600, 100, 120, 30, 0.7F);
                    obstacles.Add(obs);
                    game.Components.Add(obs);
                    obs = new Obstacle(game, spriteBatch, texture, 600, 300, 120, 30, 2.5F);
                    obstacles.Add(obs);
                    game.Components.Add(obs);
                    break;
                case "Scenario3":
                    obs = new Obstacle(game, spriteBatch, texture, 200, 150, 30, 100, 0.5F);
                    obs.Enabled = true;
                    obstacles.Add(obs);
                    game.Components.Add(obs);
                    obs = new Obstacle(game, spriteBatch, texture, 200, 400, 30, 80, -0.40F);
                    obstacles.Add(obs);
                    game.Components.Add(obs);
                    obs = new Obstacle(game, spriteBatch, texture, 400, 0, 30, 250, 0.9F);
                    obstacles.Add(obs);
                    game.Components.Add(obs);
                    obs = new Obstacle(game, spriteBatch, texture, 400, 200, 30, 250, 0.5F);
                    obstacles.Add(obs);
                    game.Components.Add(obs);
                    obs = new Obstacle(game, spriteBatch, texture, 20, 50, 150, 30, 0.7F);
                    obstacles.Add(obs);
                    game.Components.Add(obs);
                    obs = new Obstacle(game, spriteBatch, texture, 500, 175, 120, 30, 0.7F);
                    obstacles.Add(obs);
                    game.Components.Add(obs);
                    obs = new Obstacle(game, spriteBatch, texture, 450, 400, 120, 30, 2.5F);
                    obstacles.Add(obs);
                    game.Components.Add(obs);
                    obs = new Obstacle(game, spriteBatch, texture, 30, 300, 120, 30, 1.5F);
                    obstacles.Add(obs);
                    game.Components.Add(obs);
                    obs = new Obstacle(game, spriteBatch, texture, 200, 300, 120, 30, 1.5F);
                    obstacles.Add(obs);
                    game.Components.Add(obs);
                    obs = new Obstacle(game, spriteBatch, texture, 500, 280, 120, 30, -0.2F);
                    obstacles.Add(obs);
                    game.Components.Add(obs);
                    obs = new Obstacle(game, spriteBatch, texture, 200, 70, 120, 30, -0.2F);
                    obstacles.Add(obs);
                    game.Components.Add(obs);
                    break;
                case "DarBogaz":
                    obs = new Obstacle(game, spriteBatch, texture, 500, 0, 30, 230, 0.5F);
                    obs.Enabled = true;
                    obstacles.Add(obs);
                    game.Components.Add(obs);
                    obs = new Obstacle(game, spriteBatch, texture, 350, 235, 30, 250, 0.5F);
                    obstacles.Add(obs);
                    game.Components.Add(obs);
                    break;
                case "OrtaBuyukEngel":
                    obs = new Obstacle(game, spriteBatch, texture, 200, 150, 350, 250, 0.0F);
                    obs.Enabled = true;
                    obstacles.Add(obs);
                    game.Components.Add(obs);
                    break;
                case "DuzDarBogaz":
                    obs = new Obstacle(game, spriteBatch, texture, 350, 0, 30, 200, 0.0F);
                    obs.Enabled = true;
                    obstacles.Add(obs);
                    game.Components.Add(obs);
                    obs = new Obstacle(game, spriteBatch, texture, 350, 235, 30, 250, 0.0F);
                    obstacles.Add(obs);
                    game.Components.Add(obs);
                    break;
            }
        }
    }
}
