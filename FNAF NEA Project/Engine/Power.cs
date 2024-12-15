using Microsoft.Xna.Framework;
using NEA_Project.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

public enum Tools
{
    CAMS, LEFT_DOOR, RIGHT_DOOR, FLASHLIGHT, GENERATOR
}

namespace FNAF_NEA_Project.Engine
{
    public class Power : IMonogame
    {
        private TextItem DrawText;
        private TextItem LossText;
        private AnimatedSprite UsageBar;
        private float Amount = 101f;
        private int Usage = 1; // Num of bars
        private float PowerLoss = 8f; // %/min
        private Dictionary<Tools, bool> ActiveTools = new Dictionary<Tools, bool>();
        private float FadeAmount = 1f;
        private bool ShouldFade = false;

        public static Power GlobalPower;

        public event Notify PowerOutReached;

        public Power() 
        {
            MonogameIManager.AddObject(this);
            GlobalPower = this;
        }

        // Sets up dictionary that keeps track of whats being used
        private void SetupDictionary()
        {
            ActiveTools.Add(Tools.CAMS, false);
            ActiveTools.Add(Tools.LEFT_DOOR, false);
            ActiveTools.Add(Tools.RIGHT_DOOR, false);
            ActiveTools.Add(Tools.FLASHLIGHT, false);
            ActiveTools.Add(Tools.GENERATOR, false);
        }

        public void Draw(GameTime gameTime)
        {
            DrawManager.EnqueueItem(DrawText);
            DrawManager.EnqueueItem(LossText);
            DrawManager.EnqueueItem(UsageBar);
        }

        public void Initialize()
        {
        }

        public void LoadContent()
        {
            // Text setup logic
            DrawText = new TextItem("DefaultFont", "100%");
            DrawText.dp.Scale = new Vector2(0.5f, 0.5f);
            DrawText.dp.Pos = new Vector2(16, (216 * 4) - 102);
            DrawText.ZIndex = 6;

            LossText = new TextItem("DefaultFont", "-1%");
            LossText.dp.Scale = new Vector2(0.5f, 0.5f);
            LossText.dp.Pos = new Vector2(112, (216 * 4) - 102);
            LossText.ZIndex = 6;
            LossText.dp.Colour = new Color(0);

            // Usage bar sprite setup logic
            UsageBar = new AnimatedSprite("usage", new AnimationData("PowerUsage/", 6));
            UsageBar.dp.Scale = new Vector2(4);
            UsageBar.dp.Pos = new Vector2(16, (216 * 4) - 56);
            UsageBar.ZIndex = 6;
            UsageBar.Frame = 1;
        }

        // Calculates new power
        public void Update(GameTime gameTime)
        {
            float OldAmount = Amount;

            // Remove Usage * PowerLoss per minute if Usage is above 0
            if (Usage != 0)
                Amount -= Usage * PowerLoss / 60f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            // Otherwise add 20%/min to power
            else
                Amount += 20f / 60f * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Invokes power out event if amount reaches 0
            if (Amount < 0) { PowerOutReached?.Invoke(); Amount = 0; }

            // Caps power at 100% (effectively 101%, but just under so that the text says 100%)
            if (Amount > 100.99f) { Amount = 100.99f; }

            // Change label if needed
            else if ((int)OldAmount != (int)Amount)
            {
                DrawText.Text = (int)Amount + "%";
            }

            // Updates fade amount
            if (ShouldFade)
            {
                FadeAmount -= (float)gameTime.ElapsedGameTime.TotalSeconds / 2.5f;
                LossText.dp.Colour = new Color(FadeAmount, FadeAmount / 4f, 0f, FadeAmount);
                if (FadeAmount <= 0f)
                {
                    FadeAmount = 1f;
                    LossText.dp.Colour = new Color(0);
                    ShouldFade = false;
                }
            }
        }

        // Calculates how much power should be used
        public void CalculateUsage()
        {
            Usage = 1;
            foreach (Tools tool in ActiveTools.Keys)
            {
                if (tool == Tools.GENERATOR)
                {
                    if (ActiveTools[tool]) Usage -= 2;
                }
                else if (ActiveTools[tool]) Usage++;
            }
            UsageBar.Frame = Math.Max(Math.Min(Usage, 6), 0);
        }

        // Updates if a tool is being used or not
        public void SetToolStatus(Tools tool, bool value)
        {
            ActiveTools[tool] = value;
        }

        public void RemovePower(float value)
        {
            float OldAmount = Amount;

            Amount -= value;

            // Invokes power out event if amount reaches 0
            if (Amount < 0) { PowerOutReached?.Invoke(); Amount = 0; }

            // Change label if needed
            else if ((int)OldAmount != (int)Amount)
            {
                DrawText.Text = (int)Amount + "%";
            }

            // Minus text
            LossText.Text = "-" + (int)MathF.Max(value, 1f) + "%";
            LossText.dp.Colour = new Color(1f, 0.25f, 0f, 1f);
            ShouldFade = true;
            FadeAmount = 1f;
        }
    }
}
