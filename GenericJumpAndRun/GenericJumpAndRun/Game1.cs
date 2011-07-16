using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GenericJumpAndRun
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;


        //the sprites will be 32x32
        //the screen will be 20 tiles wide and 15 tiles high
        private Texture2D heroTexture;
        private Level currentLevel;
        private Player player;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = 640;
            graphics.PreferredBackBufferHeight = 480;
            Content.RootDirectory = "Content";
        }
        internal Level LoadLevelFromFile(string filename)
        {
            Level level = new Level();
            using(StreamReader sr = new StreamReader(filename))
            {
                while (!sr.EndOfStream)
                {
                    string[] split = sr.ReadLine().Split(new string[] {","}, StringSplitOptions.RemoveEmptyEntries);
                    Texture2D sprite = Content.Load<Texture2D>(split[2]);
                    GameObject gameObject = new GameObject(new Vector2(float.Parse(split[0]), float.Parse(split[1])),
                                                           new Vector2(0, 0), sprite);
                    level.GameObjects.Add(gameObject);
                }
            }
            return level;
            //X,Y,spritename
            //first line = X
            //second line = Y
            //third line = spritename
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

            heroTexture = Content.Load<Texture2D>("hero");
            currentLevel = LoadLevelFromFile("leveldata.txt");
            player = new Player(new Vector2(32, 64), new Vector2(0,5), heroTexture);
            currentLevel.GameObjects.Add(player);
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

        private KeyboardState _oldState = Keyboard.GetState();
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
            KeyboardState newState = Keyboard.GetState();
            if (newState.IsKeyDown(Keys.Left))
            {
                player.Move(Player.Direction.Left);
            }
            else if (newState.IsKeyDown(Keys.Right))
            {
                player.Move(Player.Direction.Right);
            }
            if (newState.IsKeyDown(Keys.Up))
            {
                player.Jump();
            }
            _oldState = newState;

            player.Update(currentLevel);
            player.Fall(currentLevel);


            base.Update(gameTime);
        }



        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        

        private Vector2 cameraPosition = new Vector2(0, 0);
        protected override void Draw(GameTime gameTime)
        {
            Vector2 minPosition = cameraPosition + new  Vector2(-50, -50);
            Vector2 maxPosition = cameraPosition + new Vector2(690, 530);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            foreach (GameObject gobj in currentLevel.GameObjects)
            {
                Vector2 actualPosition = gobj.Position + cameraPosition;
                if (actualPosition.X < minPosition.X || actualPosition.Y < minPosition.X
                    || actualPosition.X > maxPosition.X || actualPosition.Y > maxPosition.Y)
                {
                    continue;
                }
                spriteBatch.Draw(gobj.Sprite, actualPosition, Color.White);

            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
