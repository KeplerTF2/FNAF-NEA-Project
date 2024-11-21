using FNAF_NEA_Project.Engine.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NEA_Project.Engine;

namespace FNAF_NEA_Project
{
    public class Game1 : Game
    {
        TestScene testScene = new TestScene();

        public Game1()
        {
            MonogameGraphics.SetGraphicsDeviceManager(new GraphicsDeviceManager(this));
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Window.AllowUserResizing = true;
            GlobalCamera.Size = new Vector2(1280, 720);

            testScene.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // This should always be first
            MonogameGraphics.SetSpriteBatch(new SpriteBatch(GraphicsDevice));
            MonogameGraphics.SetContent(Content);

            // This should always be second
            TextureManager.SetContentManager(Content);

            // Scenes
            testScene.LoadContent();

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            GlobalCamera.WindowSize = Window.ClientBounds.Size;

            testScene.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            testScene.Draw(gameTime);

            // This is the final step after enqueuing all draw requests
            MonogameGraphics._spriteBatch.Begin();
            DrawManager.Draw();
            MonogameGraphics._spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
