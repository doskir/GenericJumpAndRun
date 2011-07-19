using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Keys = Microsoft.Xna.Framework.Input.Keys;

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
        private Texture2D _heroTexture;
        private Texture2D _enemyTexture;
        private Texture2D _startZoneTexture;
        private Texture2D _finishZoneTexture;
        private SpriteFont _spriteFont;
        private Level _currentLevel;
        private Player _player;
        private Camera _camera;
        private readonly List<Texture2D> _blocks = new List<Texture2D>();
        private readonly Form _form;
        public Game1()
        {
           
            graphics = new GraphicsDeviceManager(this);
            _form = (Form) Control.FromHandle(Window.Handle);
            graphics.PreferredBackBufferWidth = 640;
            graphics.PreferredBackBufferHeight = 480;
            Content.RootDirectory = "Content";
#if DEBUG
            IsMouseVisible = true;
            _logWindow = new LogWindow();
            _logWindow.Show();
            _logWindow.AddMessage("Press P to print the current level");
            _logWindow.AddMessage("Press N to enable noclip,flying and pause");
            _logWindow.AddMessage("Press R to reload the level");
            _logWindow.AddMessage("LeftClick to cycle through the available blocks");
            _logWindow.AddMessage(
                "RightClick to spawn or remove an enemy, their spawnpoint will be added to the level file");
            _logWindow.AddMessage("the start and finish points have to be moved manually by editing the level file");
            _logWindow.AddMessage(
                "Press L to save the current level to the file \"customlevel.txt\" and start a new round with it");
            RandomDebugFunctionToBeRemoved();

        }
        public void RandomDebugFunctionToBeRemoved()
        {
            var sb = new StringBuilder();
            for (int y = 0; y <= 512; y+=32)
            {
                sb.AppendLine("-32," + y + ",borderblock");
            }

#endif
        }

        internal Level LoadLevelFromFile(string filename)
        {
            var level = new Level();
            using(var sr = new StreamReader(filename))
            {
                while (!sr.EndOfStream)
                {
                    string s = sr.ReadLine();
                    if (s != null && (s.StartsWith("//") || s == ""))
                        continue;
                    if (s != null)
                    {
                        string[] split = s.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries);
                        if (split[2] == "startzone")
                        {
                            Texture2D sprite = _startZoneTexture;
                            var startZone = new GameObject(
                                new Vector2(float.Parse(split[0]), float.Parse(split[1])), Vector2.Zero, sprite,
                                GameObject.ObjectType.StartZone);
                            level.StartZone = startZone;
                        }
                        else if (split[2] == "finishzone")
                        {
                            Texture2D sprite = _finishZoneTexture;
                            var finishZone = new GameObject(
                                new Vector2(float.Parse(split[0]), float.Parse(split[1])), Vector2.Zero, sprite,
                                GameObject.ObjectType.FinishZone);
                            level.FinishZone = finishZone;
                        }
                        else if(split[2] == "enemy")
                        {
                            Texture2D sprite = _enemyTexture;
                            var enemy = new Enemy(new Vector2(float.Parse(split[0]), float.Parse(split[1])), Vector2.Zero,
                                                    sprite);
                            level.GameObjects.Add(enemy);
                        }
                        else
                        {
                            var sprite = Content.Load<Texture2D>(split[2]);
                            sprite.Name = split[2];
                            var gameObject = new GameObject(
                                new Vector2(float.Parse(split[0]), float.Parse(split[1])),
                                new Vector2(0, 0), sprite, GameObject.ObjectType.Block);
                            level.GameObjects.Add(gameObject);
                        }
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
            _camera = new Camera(0, 0, 640, 480);

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

            _spriteFont = Content.Load<SpriteFont>("SpriteFont1");

            _blocks.Add(Content.Load<Texture2D>("borderblock"));
            _blocks.Last().Name = "borderblock";
            _blocks.Add(Content.Load<Texture2D>("dirt"));
            _blocks.Last().Name = "dirt";
            _blocks.Add(Content.Load<Texture2D>("stoneblock"));
            _blocks.Last().Name = "stoneblock";

            _startZoneTexture = Content.Load<Texture2D>("startzone");
            _startZoneTexture.Name = "startzone";
            _finishZoneTexture = Content.Load<Texture2D>("finishzone");
            _finishZoneTexture.Name = "finishzone";

            _enemyTexture = Content.Load<Texture2D>("enemy");
            _enemyTexture.Name = "enemy";

            _heroTexture = Content.Load<Texture2D>("hero");
            _heroTexture.Name = "hero";

            LoadLevel("level.txt");

        }
        private void LoadLevel(string levelname)
        {
            _currentLevel = LoadLevelFromFile(levelname);
            _currentLevel.Name = levelname;

            _player = new Player(_currentLevel.StartZone.Position, new Vector2(0, 5), _heroTexture);

            _currentLevel.GameObjects.Add(_player);
            _camera.LockToObject(_player);
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
        private readonly LogWindow _logWindow;
        private bool _noclip;
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
                Exit();

            KeyboardState newKeyboardState = Keyboard.GetState();
            MouseState newMouseState = Mouse.GetState();

#if DEBUG
            if (_oldKeyboardState.IsKeyUp(Keys.P) && newKeyboardState.IsKeyDown(Keys.P))
            {
                _logWindow.AddMessage(_currentLevel.ToLevelString());
            }
            if (_oldKeyboardState.IsKeyUp(Keys.N) && newKeyboardState.IsKeyDown(Keys.N))
            {
                _noclip = !_noclip;
                _camera.LockToPlayingArea = !_noclip;
                _currentLevel.Playing = !_noclip;
            }
            if (_oldKeyboardState.IsKeyUp(Keys.R) && newKeyboardState.IsKeyDown(Keys.R))
            {
                LoadLevel(_currentLevel.Name);
                return;
            }
            if(_oldKeyboardState.IsKeyUp(Keys.L) && newKeyboardState.IsKeyDown(Keys.L))
            {
                _currentLevel.SaveLevelToFile("customlevel.txt");
                LoadLevel("customlevel.txt");
            }
            if (_form.Focused && newMouseState.LeftButton == ButtonState.Pressed || newMouseState.RightButton == ButtonState.Pressed)
            {
                int mousePositionX = (int) (newMouseState.X + _camera.Position.X);
                int mousePositionY = (int) (newMouseState.Y + _camera.Position.Y);
                int x = (mousePositionX)/32*32;
                int y = (mousePositionY)/32*32;
                if (mousePositionX <= 0)
                    x -= 32;
                GameObject block = null;
                var boundingRectangle = new BoundingRectangle(mousePositionX - 1, mousePositionY - 1,
                                                                            2, 2);
                var temp =
                    _currentLevel.GameObjects.Where(gobj => gobj.BoundingRectangle.IntersectsWith(boundingRectangle));

                if (temp.Count() == 1)
                    block = temp.First();

                if (newMouseState.LeftButton == ButtonState.Pressed && _oldMouseState.LeftButton == ButtonState.Released)
                {
                    if (block != null)
                    {
                        int nextBlockIndex = _blocks.IndexOf(block.Sprite) + 1;
                        if (nextBlockIndex >= _blocks.Count())
                        {
                            _currentLevel.GameObjects.Remove(block);
                        }
                        else
                        {
                            block.Sprite = _blocks[nextBlockIndex];
                        }
                    }
                    else
                    {
                        block = new GameObject(new Vector2(x, y), Vector2.Zero, _blocks[0],
                                               GameObject.ObjectType.Block);
                        _currentLevel.GameObjects.Add(block);
                    }
                }
                if (newMouseState.RightButton == ButtonState.Pressed
                    && _oldMouseState.RightButton == ButtonState.Released)
                {
                    if (block == null)
                    {
                        var newEnemy = new Enemy(new Vector2(x, y), Vector2.Zero, _enemyTexture);
                        bool emptyPosition = true;
                        foreach (GameObject gobj in _currentLevel.GameObjects)
                        {
                            if (gobj.Type == GameObject.ObjectType.Enemy)
                            {
                                var existingEnemy = (Enemy) gobj;
                                if (existingEnemy.SpawnLocation == newEnemy.SpawnLocation)
                                {
                                    emptyPosition = false;
                                }
                            }
                        }
                        if (emptyPosition)
                            _currentLevel.GameObjects.Add(newEnemy);
                    }
                    else
                    {
                        if (block.Type == GameObject.ObjectType.Enemy)
                            _currentLevel.GameObjects.Remove(block);


                    }
                }
            }
            _oldMouseState = newMouseState;

            if (_noclip)
            {
                if (newKeyboardState.IsKeyDown(Keys.Left))
                {
                    Vector2 newPos = _player.Position;
                    newPos.X -= 2;
                    _player.Position = newPos;
                }
                else if (newKeyboardState.IsKeyDown(Keys.Right))
                {
                    Vector2 newPos = _player.Position;
                    newPos.X += 2;
                    _player.Position = newPos;
                }
                if (newKeyboardState.IsKeyDown(Keys.Up))
                {
                    Vector2 newPos = _player.Position;
                    newPos.Y -= 2;
                    _player.Position = newPos;
                }
                if (newKeyboardState.IsKeyDown(Keys.Down))
                {
                    Vector2 newPos = _player.Position;
                    newPos.Y += 2;
                    _player.Position = newPos;
                }
            }
            else
            {
#endif
                if (_currentLevel.Playing)
                {
                    if (newKeyboardState.IsKeyDown(Keys.Left))
                    {
                        _player.Move(MovingObject.Direction.Left);
                    }
                    else if (newKeyboardState.IsKeyDown(Keys.Right))
                    {
                        _player.Move(MovingObject.Direction.Right);
                    }
                    if (newKeyboardState.IsKeyDown(Keys.Up))
                    {
                        _player.Jump();
                    }
                    foreach (GameObject gobj in _currentLevel.GameObjects)
                    {
                        gobj.Update(_currentLevel);
                    }
                    if (_player.HasFinished(_currentLevel))
                    {
                        _currentLevel.Finished = true;
                    }
                }


#if DEBUG
            }
#endif
            _camera.Update();

            _oldKeyboardState = newKeyboardState;

            base.Update(gameTime);
        }



        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            foreach (GameObject gobj in _currentLevel.GameObjects)
            {
                if (_camera.Visible(gobj))
                {
                    Vector2 actualPosition = gobj.Position - _camera.Position;
                    spriteBatch.Draw(gobj.Sprite, actualPosition, Color.White);
                }
            }
            if(_currentLevel.Finished)
            {
                const string text = "YOU WIN!";
                Vector2 textSize = _spriteFont.MeasureString(text);
                var screenCenter = new Vector2(GraphicsDevice.Viewport.Width/2, 100);
                spriteBatch.DrawString(_spriteFont, text, screenCenter - textSize/2, Color.Black);
            }
            if(!_player.Alive)
            {
                string text = "You died" + Environment.NewLine + "Press R to restart.";
                Vector2 textSize = _spriteFont.MeasureString(text);
                var screenCenter = new Vector2(GraphicsDevice.Viewport.Width/2, 100);
                spriteBatch.DrawString(_spriteFont, text, screenCenter - textSize/2, Color.Black);
            }
#if DEBUG
            Vector2 startPosition = _currentLevel.StartZone.Position - _camera.Position;
            Vector2 finishPosition = _currentLevel.FinishZone.Position - _camera.Position;
            spriteBatch.Draw(_currentLevel.StartZone.Sprite, startPosition, Color.White);
            spriteBatch.Draw(_currentLevel.FinishZone.Sprite, finishPosition, Color.White);
#endif
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
