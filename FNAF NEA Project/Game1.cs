using FNAF_NEA_Project.Engine;
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

    public enum Scenes
    {
        EMPTY, OFFICE, NIGHTWIN, NIGHTLOSE, MAIN_MENU
    }

    // BIG TODO: Make it so that changing scenes actually deletes everything prior! Probably involves making anything static, non-static

    public class Game1 : Game
    {
        public static Game1 CurrentGame;
        public Scene CurrentScene;

        private bool ShouldChangeScene = false;
        private Scenes SceneToChangeTo;

        private bool LoadingNextScene = false;
        private bool ShouldClearData = false;

        private int RequestedNightNum = 1;
        private bool UseGlobalNightNum = true;

        public Game1()
        {
            MonogameGraphics.SetGraphicsDeviceManager(new GraphicsDeviceManager(this));
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            CurrentGame = this;
        }

        public OfficeScene GetLocalOfficeScene()
        {
            if (CurrentScene.GetType() == typeof(OfficeScene))
            {
                return (OfficeScene)CurrentScene;
            }
            return null;
        }

        public static OfficeScene GetOfficeScene()
        {
            return CurrentGame.GetLocalOfficeScene();
        }

        public void RequestChangeScene(Scenes RequestedScene)
        {
            ShouldChangeScene = true;
            SceneToChangeTo = RequestedScene;
        }

        public void RequestOfficeScene(int NightNum)
        {
            RequestedNightNum = NightNum;
            UseGlobalNightNum = false;
            ShouldChangeScene = true;
            SceneToChangeTo = Scenes.OFFICE;
        }

        private void ChangeScene(Scenes RequestedScene)
        {
            ClearData();

            Scene scene;
            switch (RequestedScene)
            {
                case Scenes.OFFICE:
                    if (UseGlobalNightNum)
                        scene = new OfficeScene(SaveData.NightNum);
                    else
                    {
                        scene = new OfficeScene(RequestedNightNum);
                        UseGlobalNightNum = true;
                    }
                    break;
                case Scenes.NIGHTWIN:
                    scene = new NightWonScene();
                    break;
                case Scenes.NIGHTLOSE:
                    scene = new NightLostScene();
                    break;
                case Scenes.MAIN_MENU:
                    scene = new MainMenu();
                    break;
                default:
                    scene = new Scene();
                    break;
            }

            CurrentScene = scene;

            CurrentScene.Initialize();
            MonogameIManager.Initialize();

            CurrentScene.LoadContent();
            MonogameIManager.LoadContent();

            SceneToChangeTo = Scenes.EMPTY;
            LoadingNextScene = false;
            ShouldChangeScene = false;
        }

        private void ClearData()
        {
            Animatronic.DisposeAllTimers();
            Animatronic.AnimatronicDict.Clear();
            AudioManager.ClearSounds();
            MouseCursorManager.ResetData();
            ScrollObject.ClearList();
            InputManager.Clear();
            MonogameIManager.Clear();
            MonogameGraphics._content.Unload();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Window.AllowUserResizing = true;
            GlobalCamera.Size = new Vector2(1536, 864);

            //CurrentScene.Initialize();

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
            // CurrentScene.LoadContent();

            ChangeScene(Scenes.MAIN_MENU);

            MonogameIManager.LoadContent();

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (!ShouldChangeScene)
            {
                InputManager.Update(); // Should always come before any game logic!

                //if (ShouldChangeScene) ChangeScene();

                // TODO: Add your update logic here
                GlobalCamera.WindowSize = Window.ClientBounds.Size;

                CurrentScene.Update(gameTime);

                MonogameIManager.Update(gameTime);
                MouseCursorManager.Update(gameTime);
            }
            else ChangeScene(SceneToChangeTo);

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
