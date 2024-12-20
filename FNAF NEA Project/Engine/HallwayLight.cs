﻿using Microsoft.Xna.Framework;
using NEA_Project.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAF_NEA_Project.Engine
{
    public class HallwayLight : IMonogame
    {
        private Button FlashButton = new Button(new Rectangle(1088, 64, 128, 64));
        private ScrollSprite FlashButtonUsedSprite;
        private ScrollSprite FlashButtonSprite;
        private ScrollSprite HallwaySprite;
        private static Dictionary<DoorSide, bool> SideClosed = new Dictionary<DoorSide, bool>() { { DoorSide.LEFT, false }, { DoorSide.RIGHT, false } };
        private ScrollObject Scroll;
        private AudioEffect FlashSound = new AudioEffect("FlashSound", "Audio/HallwayFlash");
        private float CoolDown = 0f;
        private float MaxCoolDown = 4f;
        private bool OnCoolDown = false;

        public HallwayLight()
        {
            MonogameIManager.AddObject(this);
        }

        public HallwayLight(DoorSide Side)
        {
            MonogameIManager.AddObject(this);
        }

        public void Draw(GameTime gameTime)
        {
            DrawManager.EnqueueItem(FlashButtonUsedSprite);
            DrawManager.EnqueueItem(FlashButtonSprite);
            DrawManager.EnqueueItem(HallwaySprite);
        }

        public void Initialize()
        {
            FlashButton.MousePressed += event_FlashButtonPressed;
        }

        public void LoadContent()
        {
            Scroll = ScrollObject.GetList()["Scroll"];

            // Button Sprites
            FlashButtonUsedSprite = new ScrollSprite("HallwayLightCooldown", "Scroll");
            FlashButtonUsedSprite.dp.Pos = new Vector2(1088, 64);
            FlashButtonUsedSprite.dp.Scale = new Vector2(4);
            FlashButtonUsedSprite.ZIndex = 2;
            FlashButtonUsedSprite.Visible = false;

            FlashButtonSprite = new ScrollSprite("HallwayLight", "Scroll");
            FlashButtonSprite.dp.Pos = new Vector2(1088, 64);
            FlashButtonSprite.dp.Scale = new Vector2(4);
            FlashButtonSprite.ZIndex = 2;

            // Hallway Sprites
            HallwaySprite = new ScrollSprite("HallwayFlash", "Scroll");
            HallwaySprite.dp.Pos = new Vector2(864, 144);
            HallwaySprite.dp.Scale = new Vector2(4);
            HallwaySprite.ZIndex = 2;
            HallwaySprite.dp.Colour = new Color(0);

            // Audio
            FlashSound.SetVolume(0.35f);
        }

        public void Update(GameTime gameTime)
        {
            FlashButton.SetPos(new Vector2(1088 + Scroll.GetScrollAmount(), 64));
            if (!Power.PowerOut)
            {
                if (FlashButton.GetActive() == Cameras.IsUsing())
                    FlashButton.SetActive(!Cameras.IsUsing());
            }

            if (OnCoolDown)
            {
                float AlphaValue = MathF.Max(0, (2 - CoolDown) / 2);
                HallwaySprite.dp.Colour = new Color(AlphaValue, AlphaValue, AlphaValue, AlphaValue);
                CoolDown += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (CoolDown >= MaxCoolDown)
                {
                    OnCoolDown = false;
                    CoolDown = 0f;
                    if (!Power.PowerOut)
                    {
                        FlashButtonSprite.Visible = true;
                        FlashButtonUsedSprite.Visible = false;
                    }
                    HallwaySprite.dp.Colour = new Color(0);
                }
            }
        }

        public static bool IsSideClosed(DoorSide side)
        {
            return SideClosed[side];
        }

        private void event_FlashButtonPressed()
        {
            if (!OnCoolDown)
            {
                OnCoolDown = true;
                FlashButtonSprite.Visible = false;
                FlashButtonUsedSprite.Visible = true;

                // Power
                Power.GlobalPower.RemovePower(1f);

                // Audio
                FlashSound.Play();
            }
        }

        public void PowerOutage()
        {
            FlashButton.SetActive(false);
            FlashButtonSprite.Visible = false;
            FlashButtonUsedSprite.Visible = true;
        }
    }
}
