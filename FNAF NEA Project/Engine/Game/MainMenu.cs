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
        private TextItem NameText = new TextItem("PixelFont", "Six\nStickmen\nAt\nFreddy's");
        private TextItem NewGameText = new TextItem("PixelFont", "New Game");
        private TextItem CurrentGameText = new TextItem("PixelFont", "Current Game");
        private TextItem CurrentNightText = new TextItem("PixelFont", "Night 1");
        private TextItem CustomNightText = new TextItem("PixelFont", "Custom Night");
        private TextItem QuitText = new TextItem("PixelFont", "Quit");
        private SpriteItem FreddySprite;
        private AnimatedSprite StaticAnim;

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
            NameText.dp.Scale = new Vector2(0.75f);
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

            // Creates the static sprite
            StaticAnim = new AnimatedSprite("StaticSprite", new AnimationData("Static/", 4, 10, true));
            StaticAnim.ZIndex = 4;
            StaticAnim.dp.Scale = new Vector2(4);
            StaticAnim.dp.Colour = new Color(0.2f, 0.2f, 0.2f, 0.2f);
            StaticAnim.Play();

            // Creates the trigger indicator sprite
            FreddySprite = new SpriteItem("freddy_mainmenu");
            FreddySprite.dp.Scale = new Vector2(4f);
            FreddySprite.ZIndex = 3;

            NameText.LoadContent();
            NewGameText.LoadContent();
            CurrentGameText.LoadContent();
            CurrentNightText.LoadContent();
            CustomNightText.LoadContent();
            QuitText.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            StaticAnim.Update(gameTime);

            NameText.QueueToDraw();
            NewGameText.QueueToDraw();
            CurrentGameText.QueueToDraw();
            CurrentNightText.QueueToDraw();
            CustomNightText.QueueToDraw();
            QuitText.QueueToDraw();
            StaticAnim.QueueToDraw();
            FreddySprite.QueueToDraw();
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
