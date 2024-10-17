using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.ComponentModel.Design;

namespace SegundoJuego
{
    public class Game1 : Game
    {
        private Texture2D menuTexture;
        private Vector2 menuPosition;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D naveTexture;
        private Texture2D bombaTexture;
        private Vector2 navePosition;
        private Vector2 bombaPosition;
        private Random random;
        private long puntuacion;
        private Rectangle rectangulo1;
        private Rectangle rectangulo2;
        float bombaSpeed;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            random = new Random();
            puntuacion = 0;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            menuPosition = new Vector2(0, 0);
            navePosition = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            bombaPosition = new Vector2(random.Next(_graphics.PreferredBackBufferWidth), -15);
            bombaSpeed = 1.0f;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
