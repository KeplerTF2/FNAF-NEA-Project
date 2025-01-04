using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NEA_Project.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace FNAF_NEA_Project.Engine.Game
{
    public class MainMenu: Scene
    {
        public TextItem NameText = new TextItem("PixelFont", "PLACEHOLDER\nNAME");
        public TextItem NewGameText = new TextItem("PixelFont", "New Game");
        public TextItem CurrentGameText = new TextItem("PixelFont", "Current Game");
        public TextItem CurrentNightText = new TextItem("PixelFont", "Night 1");
        public TextItem CustomNightText = new TextItem("PixelFont", "Custom Night");
        public TextItem QuitText = new TextItem("PixelFont", "Quit");

        private Button NewGameButton = new Button(new Rectangle(64, 448, 336, 36));
        private Button CurrentGameButton = new Button(new Rectangle(64, 512, 506, 72));
        private Button CustomNightButton = new Button(new Rectangle(64, 608, 498, 36));
        private Button QuitButton = new Button(new Rectangle(64, 736, 160, 36));

        public MainMenu() { }

        public override void Initialize()
        {
            InputManager.AddKeyInput("QuitGame", Keys.Escape); // Keybind setup

            // Handles save data
            if (!SaveFileHandler.ReadSaveData()) SaveFileHandler.WriteSaveData();

            // All text item properties
            NameText.dp.Pos = new Vector2(64, 48);

            NewGameText.dp.Scale = new Vector2(0.5f);
            NewGameText.dp.Pos = new Vector2(64, 448);

            CurrentGameText.dp.Scale = new Vector2(0.5f);
            CurrentGameText.dp.Pos = new Vector2(64, 512);

            CurrentNightText.dp.Scale = new Vector2(0.5f, 0.33f);
            CurrentNightText.dp.Pos = new Vector2(64, 560);
            CurrentNightText.dp.Colour = Color.Gray;
            CurrentNightText.Text = "Night " + SaveData.NightNum;

            CustomNightText.dp.Scale = new Vector2(0.5f);
            CustomNightText.dp.Pos = new Vector2(64, 608);

            QuitText.dp.Scale = new Vector2(0.5f);
            QuitText.dp.Pos = new Vector2(64, 736);

            // All button properties
            NewGameButton.MouseReleased += NewGame;
            CurrentGameButton.MouseReleased += CurrentGame;
            CustomNightButton.MouseReleased += CustomNight;
            QuitButton.MouseReleased += Quit;

            // Deactivates custom night button if not unlocked
            if (!SaveData.CustomNight)
            {
                CustomNightText.dp.Colour = Color.Gray;
                CustomNightButton.SetActive(false);
            }
        }

        public override void LoadContent()
        {
            // Should always be first!
            base.LoadContent();

            NameText.LoadContent();
            NewGameText.LoadContent();
            CurrentGameText.LoadContent();
            CurrentNightText.LoadContent();
            CustomNightText.LoadContent();
            QuitText.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            DrawManager.EnqueueItem(NameText);
            DrawManager.EnqueueItem(NewGameText);
            DrawManager.EnqueueItem(CurrentGameText);
            DrawManager.EnqueueItem(CurrentNightText);
            DrawManager.EnqueueItem(CustomNightText);
            DrawManager.EnqueueItem(QuitText);
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.GetKeyState("QuitGame").JustDown) Quit();
        }

        private void NewGame()
        {
            Game1.CurrentGame.RequestOfficeScene(1);
            SaveData.NightNum = 1;
            SaveFileHandler.WriteSaveData();
        }

        private void CurrentGame()
        {
            Game1.CurrentGame.RequestChangeScene(Scenes.OFFICE);
        }

        private void CustomNight()
        {
            Game1.CurrentGame.RequestChangeScene(Scenes.CUSTOM_NIGHT);
        }

        private void Quit()
        {
            Game1.CurrentGame.Exit();
        }
    }
}
