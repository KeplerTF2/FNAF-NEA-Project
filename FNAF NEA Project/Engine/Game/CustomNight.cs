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
        private TextItem text = new TextItem("DefaultFont", "Custom Night");
        private Color DarkGray = new Color(0.25f, 0.25f, 0.25f);

        // Animatronics
        private CNCharacter Freddy = new CNCharacter(Animatronics.Freddy, new Vector2(128, 144));
        private CNCharacter Bonnie = new CNCharacter(Animatronics.Bonnie, new Vector2(384, 144));
        private CNCharacter Chica = new CNCharacter(Animatronics.Chica, new Vector2(640, 144));
        private CNCharacter Foxy = new CNCharacter(Animatronics.Foxy, new Vector2(128, 464));
        private CNCharacter GoldenFreddy = new CNCharacter(Animatronics.GoldenFreddy, new Vector2(384, 464));
        private CNCharacter Helpy = new CNCharacter(Animatronics.Helpy, new Vector2(640, 464));

        private Button StartNightButton = new Button(new Rectangle(1000, 532, 300, 200), true, true);
        private TextItem StartNightText = new TextItem("PixelFont", "Start", true);

        private Button HeavyStaticButton = new Button(new Rectangle(1000, 128, 300, 100), true, true);
        private Button FaultyTempButton = new Button(new Rectangle(1000, 256, 300, 100), true, true);
        private Button SilentStepsButton = new Button(new Rectangle(1000, 384, 300, 100), true, true);
        private TextItem HeavyStaticText = new TextItem("PixelFont", "Heavy\nStatic", true);
        private TextItem FaultyTempText = new TextItem("PixelFont", "Faulty\nTemp.", true);
        private TextItem SilentStepsText = new TextItem("PixelFont", "Silent\nSteps", true);

        public CustomNight() { }

        public override void Initialize()
        {
            InputManager.AddKeyInput("MainMenu", Keys.Escape);

            // Resets challenges
            Challenges.SetAll(false);

            text.dp.Pos = new Vector2(640 - 160, 360 - 60);

            StartNightButton.MousePressed += StartNight;
            StartNightButton.SetRectColour(Color.Gray);
            StartNightButton.SetRectZIndex(0);

            StartNightText.dp.Pos = new Vector2(1150, 632);
            StartNightText.dp.Scale = new Vector2(0.6f);
            StartNightText.ZIndex = 5;

            // Challenge Modifiers
            HeavyStaticButton.MousePressed += ToggleHeavyStatic;
            HeavyStaticButton.SetRectColour(DarkGray);
            HeavyStaticButton.SetRectZIndex(0);

            HeavyStaticText.dp.Pos = new Vector2(1150, 178);
            HeavyStaticText.dp.Scale = new Vector2(1f / 3f);
            HeavyStaticText.ZIndex = 5;

            FaultyTempButton.MousePressed += ToggleFaultyTemp;
            FaultyTempButton.SetRectColour(DarkGray);
            FaultyTempButton.SetRectZIndex(0);

            FaultyTempText.dp.Pos = new Vector2(1150, 306);
            FaultyTempText.dp.Scale = new Vector2(1f / 3f);
            FaultyTempText.ZIndex = 5;

            SilentStepsButton.MousePressed += ToggleSilentSteps;
            SilentStepsButton.SetRectColour(DarkGray);
            SilentStepsButton.SetRectZIndex(0);

            SilentStepsText.dp.Pos = new Vector2(1150, 434);
            SilentStepsText.dp.Scale = new Vector2(1f / 3f);
            SilentStepsText.ZIndex = 5;
        }

        public override void LoadContent()
        {
            // Should always be first!
            base.LoadContent();

            text.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            StartNightText.QueueToDraw();
            HeavyStaticText.QueueToDraw();
            FaultyTempText.QueueToDraw();
            SilentStepsText.QueueToDraw();
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.GetKeyState("MainMenu").JustDown)
            {
                Challenges.SetAll(false);
                CNCharacter.ClearAIDict();
                Game1.CurrentGame.RequestChangeScene(Scenes.MAIN_MENU);
            }
        }

        private void StartNight()
        {
            NightSettings.CustomAI = CNCharacter.GetAIDict();
            CNCharacter.ClearAIDict();
            Game1.CurrentGame.RequestOfficeScene(7);
        }

        private void ToggleHeavyStatic()
        {
            Challenges.HeavyStatic = HeavyStaticButton.Toggled;
            if (HeavyStaticButton.Toggled) HeavyStaticButton.SetRectColour(Color.LightGray);
            else HeavyStaticButton.SetRectColour(DarkGray);
        }
        private void ToggleFaultyTemp()
        {
            Challenges.FaultyTemp = FaultyTempButton.Toggled;
            if (FaultyTempButton.Toggled) FaultyTempButton.SetRectColour(Color.LightGray);
            else FaultyTempButton.SetRectColour(DarkGray);
        }
        private void ToggleSilentSteps()
        {
            Challenges.SilentSteps = SilentStepsButton.Toggled;
            if (SilentStepsButton.Toggled) SilentStepsButton.SetRectColour(Color.LightGray);
            else SilentStepsButton.SetRectColour(DarkGray);
        }
    }
}
