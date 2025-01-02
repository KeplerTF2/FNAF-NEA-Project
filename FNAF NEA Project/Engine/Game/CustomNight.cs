using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NEA_Project.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAF_NEA_Project.Engine.Game
{
    public class CustomNight : Scene
    {
        public TextItem text = new TextItem("DefaultFont", "Custom Night");

        // Animatronics
        public CNCharacter Freddy = new CNCharacter(Animatronics.Freddy, new Vector2(128, 144));
        public CNCharacter Bonnie = new CNCharacter(Animatronics.Bonnie, new Vector2(384, 144));
        public CNCharacter Chica = new CNCharacter(Animatronics.Chica, new Vector2(640, 144));
        public CNCharacter Foxy = new CNCharacter(Animatronics.Foxy, new Vector2(128, 464));
        public CNCharacter GoldenFreddy = new CNCharacter(Animatronics.GoldenFreddy, new Vector2(384, 464));
        public CNCharacter Helpy = new CNCharacter(Animatronics.Helpy, new Vector2(640, 464));

        public Button StartNightButton = new Button(new Rectangle(1000, 332, 300, 200), true, true);
        public TextItem StartNightText = new TextItem("DefaultFont", "Start");

        public CustomNight() { }

        public override void Initialize()
        {
            InputManager.AddKeyInput("MainMenu", Keys.Escape);

            text.dp.Pos = new Vector2(640 - 160, 360 - 60);
            StartNightButton.MousePressed += StartNight;
            StartNightButton.SetRectColour(Color.Gray);
            StartNightButton.SetRectZIndex(0);
            StartNightText.dp.Pos = new Vector2(1000, 332);
            StartNightText.ZIndex = 5;
        }

        public override void LoadContent()
        {
            // Should always be first!
            base.LoadContent();

            text.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            DrawManager.EnqueueItem(StartNightText);
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.GetKeyState("MainMenu").JustDown)
            {
                CNCharacter.ClearAIDict();
                Game1.CurrentGame.RequestChangeScene(Scenes.MAIN_MENU);
            }
        }

        public void StartNight()
        {
            NightSettings.CustomAI = CNCharacter.GetAIDict();
            CNCharacter.ClearAIDict();
            Game1.CurrentGame.RequestOfficeScene(7);
        }
    }
}
