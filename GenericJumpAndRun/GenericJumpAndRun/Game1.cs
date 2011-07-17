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
        private Texture2D enemyTexture;
        private Texture2D startZoneTexture;
        private Texture2D finishZoneTexture;
        private SpriteFont spriteFont;
        private Level currentLevel;
        private Player player;
        private Camera camera;
        private List<Texture2D> blocks = new List<Texture2D>();
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = 640;
            graphics.PreferredBackBufferHeight = 480;
            Content.RootDirectory = "Content";
#if DEBUG
            this.IsMouseVisible = true;
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
                    if (split[2] == "startzone")
                    {
                        Texture2D sprite = startZoneTexture;
                        GameObject startZone = new GameObject(
                            new Vector2(float.Parse(split[0]), float.Parse(split[1])), Vector2.Zero, sprite,
                            GameObject.ObjectType.StartZone);
                        level.StartZone = startZone;
                    }
                    else if (split[2] == "finishzone")
                    {
                        Texture2D sprite = finishZoneTexture;
                        GameObject finishZone = new GameObject(
                            new Vector2(float.Parse(split[0]), float.Parse(split[1])), Vector2.Zero, sprite,
                            GameObject.ObjectType.FinishZone);
                        level.FinishZone = finishZone;
                    }
                    else
                    {
                        Texture2D sprite = Content.Load<Texture2D>(split[2]);
                        sprite.Name = split[2];
                        GameObject gameObject = new GameObject(
                            new Vector2(float.Parse(split[0]), float.Parse(split[1])),
                            new Vector2(0, 0), sprite, GameObject.ObjectType.Block);
                        level.GameObjects.Add(gameObject);
                    }
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
            camera = new Camera(0, 0, 640, 480);

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

            spriteFont = Content.Load<SpriteFont>("SpriteFont1");

            blocks.Add(Content.Load<Texture2D>("borderblock"));
            blocks.Last().Name = "borderblock";
            blocks.Add(Content.Load<Texture2D>("dirt"));
            blocks.Last().Name = "dirt";
            blocks.Add(Content.Load<Texture2D>("stoneblock"));
            blocks.Last().Name = "stoneblock";

            startZoneTexture = Content.Load<Texture2D>("startzone");
            startZoneTexture.Name = "startzone";
            finishZoneTexture = Content.Load<Texture2D>("finishzone");
            finishZoneTexture.Name = "finishzone";

            enemyTexture = Content.Load<Texture2D>("enemy");
            enemyTexture.Name = "enemy";

            heroTexture = Content.Load<Texture2D>("hero");
            heroTexture.Name = "hero";

            LoadLevel("level.txt");

        }
        private void LoadLevel(string levelname)
        {
            currentLevel = LoadLevelFromFile(levelname);
            currentLevel.Name = levelname;

            player = new Player(currentLevel.StartZone.Position, new Vector2(0, 5), heroTexture);

            currentLevel.GameObjects.Add(player);
            camera.LockToObject(player);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        private KeyboardState _oldKeyboardState = Keyboard.GetState();
        private MouseState _oldMouseState = Mouse.GetState();
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

            KeyboardState newKeyboardState = Keyboard.GetState();
            MouseState newMouseState = Mouse.GetState();

#if DEBUG
            if(_oldKeyboardState.IsKeyUp(Keys.P) && newKeyboardState.IsKeyDown(Keys.P))
            {
                logWindow.AddMessage(currentLevel.ToLevelString());
            }
            if(_oldKeyboardState.IsKeyUp(Keys.N) && newKeyboardState.IsKeyDown(Keys.N))
            {
                noclip = !noclip;
                camera.LockToPlayingArea = !noclip;
                currentLevel.Playing = !noclip;
            }
            if(_oldKeyboardState.IsKeyUp(Keys.R) && newKeyboardState.IsKeyDown(Keys.R))
            {
                LoadLevel(currentLevel.Name);
                return;
            }
            int mousePositionX = (int)(newMouseState.X + camera.Position.X);
            int mousePositionY = (int)(newMouseState.Y + camera.Position.Y);
            int x = (mousePositionX) / 32 * 32;
            int y = (mousePositionY) / 32 * 32;
            if (mousePositionX <= 0)
                x -= 32;
            GameObject block = null;
            try
            {
                var temp = currentLevel.GameObjects.Where(
                    gobj => gobj.BoundingRectangle.X == x && gobj.BoundingRectangle.Y == y);
                if (temp.Count() == 1)
                    block = temp.First();
            }
            catch (Exception)
            { }
            if(newMouseState.LeftButton == ButtonState.Pressed && _oldMouseState.LeftButton == ButtonState.Released)
            {
                if (block != null)
                {
                    int nextBlockIndex = blocks.IndexOf(block.Sprite) + 1;
                    if (nextBlockIndex >= blocks.Count())
                    {
                        currentLevel.GameObjects.Remove(block);
                    }
                    else
                    {
                        block.Sprite = blocks[nextBlockIndex];
                    }
                }
                else
                {
                    block = new GameObject(new Vector2(x,y),Vector2.Zero, blocks[0],GameObject.ObjectType.Block);
                    currentLevel.GameObjects.Add(block);
                }
            }
            if (newMouseState.RightButton == ButtonState.Pressed && _oldMouseState.RightButton == ButtonState.Released)
            {
                if (block == null)
                {
                    currentLevel.GameObjects.Add(new Enemy(new Vector2(x, y), Vector2.Zero, enemyTexture));
                }
                else
                {
                    if (block.Type == GameObject.ObjectType.Enemy)
                        currentLevel.GameObjects.Remove(block);
                        
                    
                }
            }
            _oldMouseState = newMouseState;

            if (noclip)
            {
                if (newKeyboardState.IsKeyDown(Keys.Left))
                {
                    Vector2 newPos = player.Position;
                    newPos.X -= 2;
                    player.Position = newPos;
                }
                else if (newKeyboardState.IsKeyDown(Keys.Right))
                {
                    Vector2 newPos = player.Position;
                    newPos.X += 2;
                    player.Position = newPos;
                }
                if (newKeyboardState.IsKeyDown(Keys.Up))
                {
                    Vector2 newPos = player.Position;
                    newPos.Y -= 2;
                    player.Position = newPos;
                }
                if (newKeyboardState.IsKeyDown(Keys.Down))
                {
                    Vector2 newPos = player.Position;
                    newPos.Y += 2;
                    player.Position = newPos;
                }
            }
            else
            {
#endif
                if (currentLevel.Playing)
                {
                    if (newKeyboardState.IsKeyDown(Keys.Left))
                    {
                        player.Move(Player.Direction.Left);
                    }
                    else if (newKeyboardState.IsKeyDown(Keys.Right))
                    {
                        player.Move(Player.Direction.Right);
                    }
                    if (newKeyboardState.IsKeyDown(Keys.Up))
                    {
                        player.Jump();
                    }
                    foreach (GameObject gobj in currentLevel.GameObjects)
                    {
                        gobj.Update(currentLevel);
                    }
                    if (player.HasFinished(currentLevel))
                    {
                        currentLevel.Finished = true;
                    }
                }


#if DEBUG
            }
#endif
            camera.Update();

            _oldKeyboardState = newKeyboardState;

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
            if(currentLevel.Finished)
            {
                string text = "YOU WIN!";
                Vector2 textSize = spriteFont.MeasureString(text);
                Vector2 screenCenter = new Vector2(GraphicsDevice.Viewport.Width/2, 100);
                spriteBatch.DrawString(spriteFont, text, screenCenter - textSize/2, Color.Black);
            }
            if(!player.Alive)
            {
                string text = "You died" + Environment.NewLine + "Press R to restart.";
                Vector2 textSize = spriteFont.MeasureString(text);
                Vector2 screenCenter = new Vector2(GraphicsDevice.Viewport.Width/2, 100);
                spriteBatch.DrawString(spriteFont, text, screenCenter - textSize/2, Color.Black);
            }
#if DEBUG
            Vector2 startPosition = currentLevel.StartZone.Position - camera.Position;
            Vector2 finishPosition = currentLevel.FinishZone.Position - camera.Position;
            spriteBatch.Draw(currentLevel.StartZone.Sprite, startPosition, Color.White);
            spriteBatch.Draw(currentLevel.FinishZone.Sprite, finishPosition, Color.White);
#endif
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
