﻿using Microsoft.Xna.Framework;
using NEA_Project.Engine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

public enum Tools
{
    CAMS, LEFT_DOOR, RIGHT_DOOR, GOLDEN_FREDDY, GENERATOR
}

namespace FNAF_NEA_Project.Engine
{
    public class Power : IMonogame
    {
        private TextItem DrawText;
        private TextItem LossText;
        private RectItem BlackOutSprite;
        private AnimatedSprite UsageBar;
        private float Amount = 101f;
        private int Usage = 1; // Num of bars
        private float PowerLoss = 8f; // %/min
        private Dictionary<Tools, bool> ActiveTools = new Dictionary<Tools, bool>();
        private float FadeAmount = 1f;
        private bool ShouldFade = false;
        private AudioEffect PowerOutSound = new AudioEffect("PowerOutSound", "Audio/powerdown", 0.75f);
        public bool PowerOut = false;

        public event Notify PowerOutReached;

        public Power() 
        {
            Debug.WriteLine("Creating POWER!!!");
            MonogameIManager.AddObject(this);
        }

        // Sets up dictionary that keeps track of whats being used
        private void SetupDictionary()
        {
            ActiveTools.Add(Tools.CAMS, false);
            ActiveTools.Add(Tools.LEFT_DOOR, false);
            ActiveTools.Add(Tools.RIGHT_DOOR, false);
            ActiveTools.Add(Tools.GOLDEN_FREDDY, false);
            ActiveTools.Add(Tools.GENERATOR, false);
        }

        public void Draw(GameTime gameTime)
        {
            if (!Game1.GetOfficeScene().InTutorial)
            {
                DrawText.QueueToDraw();
                LossText.QueueToDraw();
                UsageBar.QueueToDraw();
                BlackOutSprite.QueueToDraw();
            }
        }

        public void Initialize()
        {
            SetupDictionary();
        }

        public void LoadContent()
        {
            // Text setup logic
            DrawText = new TextItem("PixelFont", "100%");
            DrawText.dp.Scale = new Vector2(0.4f, 0.4f);
            DrawText.dp.Pos = new Vector2(16, (216 * 4) - 96);
            DrawText.ZIndex = 6;

            LossText = new TextItem("PixelFont", "-1%");
            LossText.dp.Scale = new Vector2(0.4f, 0.4f);
            LossText.dp.Pos = new Vector2(128, (216 * 4) - 96);
            LossText.ZIndex = 6;
            LossText.dp.Colour = new Color(0);

            // Usage bar sprite setup logic
            UsageBar = new AnimatedSprite("usage", new AnimationData("PowerUsage/", 6));
            UsageBar.dp.Scale = new Vector2(4);
            UsageBar.dp.Pos = new Vector2(16, (216 * 4) - 56);
            UsageBar.ZIndex = 6;
            UsageBar.Frame = 1;

            // Sprite that darkens the player view
            BlackOutSprite = new RectItem(1536 + 1, 864 + 1);
            BlackOutSprite.ZIndex = 10;
            BlackOutSprite.dp.Colour = new Color(0f, 0f, 0f, 0.5f);
            BlackOutSprite.Visible = false;
        }

        public void DoPowerOut(bool HideUIOnly = false)
        {
            // Only if we should do a complete powerout, and not just hide UI
            if (!HideUIOnly)
            {
                // Core power outage logic
                Amount = 0;
                PowerOut = true;

                // Invokes event
                PowerOutReached?.Invoke();

                // Plays power outage sound
                PowerOutSound.Play();

                // Darkens the screen
                BlackOutSprite.Visible = true;
            }

            // Sets UI to be invisible
            LossText.Visible = false;
            DrawText.Visible = false;
            UsageBar.Visible = false;
        }

        // Calculates new power
        public void Update(GameTime gameTime)
        {
            // Don't do power logic if power is out
            if ((!PowerOut) && (!Game1.GetOfficeScene().InTutorial))
            {
                float OldAmount = Amount;
                float AmountChange;

                // Remove Usage * PowerLoss per minute if Usage is above 0
                if (Usage != 0)
                    AmountChange = -(Usage * PowerLoss / 60f * (float)gameTime.ElapsedGameTime.TotalSeconds);
                // Otherwise add 15%/min to power
                else
                    AmountChange = 15f / 60f * (float)gameTime.ElapsedGameTime.TotalSeconds;

                // Double amount lost if golden freddy is active
                if (ActiveTools[Tools.GOLDEN_FREDDY])
                    AmountChange *= 2;

                Amount += AmountChange;

                // Invokes power out event if amount reaches 0
                if (Amount < 0)
                {
                    DoPowerOut();
                }

                // Caps power at 100% (effectively 101%, but just under so that the text says 100%)
                if (Amount > 100.99f) { Amount = 100.99f; }

                // Change label if needed
                else if ((int)OldAmount != (int)Amount)
                {
                    DrawText.Text = (int)Amount + "%";
                }
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
            // Base usage
            Usage = 1;

            // Add one usage per tool
            foreach (Tools tool in ActiveTools.Keys)
            {
                if ((tool != Tools.GENERATOR) && ActiveTools[tool]) Usage++;
            }

            // Handle power generator seperately
            if (ActiveTools[Tools.GENERATOR])
            {
                if (Usage > 2) Usage--;
                else Usage = 0;
            }

            // Show correct sprite
            UsageBar.Frame = Math.Max(Math.Min(Usage, 6), 0);
        }

        // Returns current power usage
        public int GetUsage()
        {
            return Usage;
        }

        // Updates if a tool is being used or not
        public void SetToolStatus(Tools tool, bool value)
        {
            ActiveTools[tool] = value;
        }

        public void RemovePower(float value)
        {
            if (!Game1.GetOfficeScene().InTutorial)
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
}
