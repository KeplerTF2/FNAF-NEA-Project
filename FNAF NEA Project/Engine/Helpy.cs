﻿using FNAF_NEA_Project.Engine.Game;
using Microsoft.Xna.Framework;
using NEA_Project.Engine;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace FNAF_NEA_Project.Engine
{
    public class Helpy : Animatronic
    {
        public event Notify Attacked;

        private float XPos = 2304f;
        private float YPos = 576f;
        private float BaseTime = 5f;
        private float CurrentTime = -5f;
        private bool Attacking = false;
        private bool Reteating = false;
        private AudioEffect LaughSound = new AudioEffect("LaughHelpy", "Audio/golden_laugh", 0.6f);
        private AudioEffect BoopSound = new AudioEffect("Boop", "Audio/nosepush", 0.75f);
        private AnimatedSprite HelpySprite;
        private Button NoseButton = new Button(new Rectangle(2304 + 72, 576 + 48, 48, 32));
        private ScrollObject Scroll;

        public Helpy(int AI)
        {
            Difficulty = AI;
            VisibleRooms = new int[] { };
            CurrentRoom = -1;
            Name = Animatronics.Helpy;

            BaseTime = GetTime(18.76f, Difficulty);

            MonogameIManager.AddObject(this);
        }

        public override void DisposeTimers() { }

        public override void Draw(GameTime gameTime)
        {
            // We want to calculate pos here for a smoother running anim
            if (Attacking)
            {
                XPos -= (float)gameTime.ElapsedGameTime.TotalSeconds * MathF.Sqrt(MathF.Sqrt(Difficulty)) * 220f;

                if (XPos < -192) Jumpscare();
            }

            if (Reteating)
            {
                YPos += (float)gameTime.ElapsedGameTime.TotalSeconds * 250f;

                if (YPos > 864)
                {
                    Reteating = false;
                    YPos = 576;
                    XPos = 2304f;
                }

                HelpySprite.dp.Pos.Y = YPos;
            }

            HelpySprite.dp.Pos.X = Scroll.GetScrollAmount() + XPos;
            HelpySprite.Update(gameTime);
            HelpySprite.QueueToDraw();
            JumpscareSprite.QueueToDraw();
        }

        public override void Initialize()
        {
            AnimatronicDict.Add(Name, this);
            NoseButton.MousePressed += OnNoseBooped;

            NoseButton.SetRectZIndex(28);
        }

        public override void LoadContent()
        {
            CreateSprite();

            // Creates the running sprite
            HelpySprite = new AnimatedSprite("Helpy", new AnimationData("HelpyRun/", 8, 10, true));
            HelpySprite.ZIndex = 2;
            HelpySprite.dp.Scale = new Vector2(4);
            HelpySprite.dp.Pos.Y = 576;
            HelpySprite.Visible = true;
            HelpySprite.Play();

            Scroll = ScrollObject.GetList()["Scroll"];

            LaughSound.GetInstance().Pitch = 0.5f;
        }

        public override void Update(GameTime gameTime)
        {
            if (!Game1.GetOfficeScene().InTutorial)
            {
                if (Difficulty != 0 && !Attacking)
                {
                    CurrentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (CurrentTime > BaseTime)
                    {
                        Attacking = true;
                        CurrentTime = 0f;
                        LaughSound.Play();
                        Attacked?.Invoke();

                        if (Challenges.OutputCheat)
                            Debug.WriteLine("Helpy attacked");
                    }
                }

                // Only update button pos on Update instead of Draw to reduce on processing
                if (Attacking)
                {
                    NoseButton.SetPos(new Vector2(XPos + 76 + Scroll.GetScrollAmount(), YPos + 52));
                }

                // Update if the nose should be active
                if (NoseButton.GetActive() != (!Game1.GetOfficeScene().Cameras.IsUsing() && Attacking))
                    NoseButton.SetActive(!Game1.GetOfficeScene().Cameras.IsUsing() && Attacking);
            }
        }

        public void OnNoseBooped()
        {
            Attacking = false;
            Reteating = true;
            BoopSound.Play();

            if (Challenges.OutputCheat)
                Debug.WriteLine("Helpy left");
        }
    }
}
