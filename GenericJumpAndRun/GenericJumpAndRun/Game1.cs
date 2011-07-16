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
        private Camera camera;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = 640;
            graphics.PreferredBackBufferHeight = 480;
            Content.RootDirectory = "Content";
#if DEBUG
            logWindow = new LogWindow();
            logWindow.Show();
            RandomDebugFunctionToBeRemoved();
        }
        public void RandomDebugFunctionToBeRemoved()
        {
            StringBuilder sb = new StringBuilder();
            for (int y = 0; y <= 512; y+=32)
            {
                sb.AppendLine("-32," + y + ",borderblock");
            }

#endif
        }

        internal Level LoadLevelFromFile(string filename)
        {
            Level level = new Level();
            using(StreamReader sr = new StreamReader(filename))
            {
                while (!sr.EndOfStream)
                {
                    string s = sr.ReadLine();
                    if (s.StartsWith("//") || s == "")
                        continue;
                    string[] split = s.Split(new string[] {","}, StringSplitOptions.RemoveEmptyEntries);
                    Texture2D sprite = Content.Load<Texture2D>(split[2]);
                    sprite.Name = split[2];
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
            heroTexture.Name = "hero";
            currentLevel = LoadLevelFromFile("leveldata.txt");
            player = new Player(new Vector2(32, 64), new Vector2(0,5), heroTexture);
            currentLevel.GameObjects.Add(player);
            camera = new Camera(0, 0, 640, 480);
            camera.LockToObject(player);
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
#if DEBUG
        private LogWindow logWindow;
        private bool noclip;
#endif
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

#if DEBUG
            if (_oldState.IsKeyUp(Keys.Space) && newState.IsKeyDown(Keys.Space))
            {
                int x = (int) player.Position.X/32*32;
                int y = (int) player.Position.Y/32*32;
                string assetName = "air";

                try
                {
                    assetName =
                        currentLevel.GameObjects.Where(gobj => gobj.BoundingRectangle.X == x && gobj.BoundingRectangle.Y == y).Single().Sprite
                        .Name;
                }
                catch (InvalidOperationException)
                {
                }
                if (assetName == "air")
                {
                    currentLevel.GameObjects.Add(new GameObject(new Vector2(x, y), Vector2.Zero,
                                                                Content.Load<Texture2D>("dirt")));
                }
                logWindow.AddMessage(x + "," + y + "," + assetName);
                Console.WriteLine("{0},{1},{2}", x, y, assetName);
            }
            if(_oldState.IsKeyUp(Keys.N) && newState.IsKeyDown(Keys.N))
            {
                noclip = !noclip;
            }

            if (noclip)
            {
                if (newState.IsKeyDown(Keys.Left))
                {
                    Vector2 newPos = player.Position;
                    newPos.X -= 2;
                    player.Position = newPos;
                }
                else if (newState.IsKeyDown(Keys.Right))
                {
                    Vector2 newPos = player.Position;
                    newPos.X += 2;
                    player.Position = newPos;
                }
                if (newState.IsKeyDown(Keys.Up))
                {
                    Vector2 newPos = player.Position;
                    newPos.Y -= 2;
                    player.Position = newPos;
                }
                if(newState.IsKeyDown(Keys.Down))
                {
                    Vector2 newPos = player.Position;
                    newPos.Y += 2;
                    player.Position = newPos;
                }
            }
            else
            {
#endif

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
            player.Update(currentLevel);
#if DEBUG
            }
#endif
            camera.Update();

            _oldState = newState;

            base.Update(gameTime);
        }



        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Vector2 minPosition = player.Position -camera.Position - new  Vector2(500, 500);
            Vector2 maxPosition = player.Position -camera.Position + new Vector2(500, 500);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            foreach (GameObject gobj in currentLevel.GameObjects)
            {
                if (camera.Visible(gobj))
                {
                    Vector2 actualPosition = gobj.Position - camera.Position;

                    spriteBatch.Draw(gobj.Sprite, actualPosition, Color.White);
                }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
