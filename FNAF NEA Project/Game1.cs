﻿using FNAF_NEA_Project.Engine;
using FNAF_NEA_Project.Engine.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NEA_Project.Engine;
using System.Collections.Generic;

namespace FNAF_NEA_Project
{
    // Delegates
    public delegate void Notify();
    public delegate void NotifyInt(int value);

    // BIG TODO: Make it so that changing scenes actually deletes everything prior! Probably involves making anything static, non-static

    public class Game1 : Game
    {
        public static Game1 CurrentGame;
        private static Scene CurrentScene;

        private static bool ShouldChangeScene = false;
        private static Scene SceneToChangeTo;

        public Game1()
        {
            MonogameGraphics.SetGraphicsDeviceManager(new GraphicsDeviceManager(this));
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            CurrentGame = this;

            CurrentScene = new OfficeScene(1);
        }

        public static void ChangeScene(Scene scene)
        {
            SceneToChangeTo = scene;
            ShouldChangeScene = true;
        }

        private static void ChangeScene()
        {
            AudioManager.ClearSounds();
            MouseCursorManager.ResetData();
            MonogameIManager.Clear();
            CurrentScene = SceneToChangeTo;
            CurrentScene.Initialize();
            CurrentScene.LoadContent();
            ShouldChangeScene = false;
            SceneToChangeTo = null;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Window.AllowUserResizing = true;
            GlobalCamera.Size = new Vector2(1536, 864);

            CurrentScene.Initialize();

            MonogameIManager.Initialize();

            //InputManager.AddKeyInput("fullscreen", Keys.F);

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
            CurrentScene.LoadContent();

            MonogameIManager.LoadContent();

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.Update(); // Should always come before any game logic!

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            GlobalCamera.WindowSize = Window.ClientBounds.Size;

            CurrentScene.Update(gameTime);

            MonogameIManager.Update(gameTime);
            MouseCursorManager.Update(gameTime);

            if (ShouldChangeScene) ChangeScene();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            CurrentScene.Draw(gameTime);

            MonogameIManager.Draw(gameTime);

            // This is the final step after enqueuing all draw requests
            MonogameGraphics._spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            DrawManager.Draw();
            MonogameGraphics._spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
