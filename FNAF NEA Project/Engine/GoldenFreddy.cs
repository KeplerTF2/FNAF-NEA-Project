using FNAF_NEA_Project.Engine.Game;
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
    public class GoldenFreddy : Animatronic
    {
        public event Notify Attacked;

        private Timer MoveTimer = new Timer(1000);
        private float BaseTime = 5f;
        private float MaxTime = 5f;
        private float CurrentTime = 0f;
        private bool Attacking = false;
        private AudioEffect LaughSound = new AudioEffect("Laugh", "Audio/golden_laugh", 0.5f);

        public GoldenFreddy(int AI)
        {
            Difficulty = AI;
            VisibleRooms = new int[] { 7 };
            CurrentRoom = -1;
            Name = Animatronics.GoldenFreddy;
            HasJumpscare = false;

            BaseTime = GetTime(20f, Difficulty);
            MoveTimer.AutoReset = true;
            MoveTimer.Start();
            MoveTimer.Elapsed += UpdateNextMovement;

            MonogameIManager.AddObject(this);
        }

        public override void DisposeTimers()
        {
            MoveTimer.Stop();
            MoveTimer.Dispose();
        }

        public override void Draw(GameTime gameTime)
        {
            if (ShouldDrawCamSprite())
            {
                CamSprite.dp.Pos.X = Game1.GetOfficeScene().Cameras.GetScrollAmount();
                DrawManager.EnqueueItem(CamSprite);
            }
        }

        public override void Initialize()
        {
            AnimatronicDict.Add(Name, this);
            UpdateNextMovement();
        }

        public override void LoadContent()
        {
            CreateSprite();
            UpdateSprite();
        }

        public override void Update(GameTime gameTime)
        {
            if (Difficulty != 0 && (!Attacking) && (!Game1.GetOfficeScene().InTutorial))
            {
                CurrentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (CurrentTime > MaxTime)
                {
                    Attacking = true;
                    CurrentTime = 0f;
                    CurrentRoom = 7;
                    LaughSound.Play();
                    Attacked?.Invoke();
                    UpdateSprite();
                    Game1.GetOfficeScene().Cameras.ShowAnimMovement(Building.IDToCamNum(7), Building.IDToCamNum(7));

                    if (Challenges.OutputCheat)
                        Debug.WriteLine("Golden Freddy attacked");
                }
            }
        }

        private void UpdateNextMovement()
        {
            MaxTime = Game1.GetOfficeScene().Building.GetTempRoomTime(7, 8) * BaseTime;
        }

        private void UpdateNextMovement(object sender, ElapsedEventArgs e)
        {
            UpdateNextMovement();
        }

        public void OnPowerGenRepair()
        {
            Attacking = false;
            CurrentRoom = -1;
            UpdateSprite();
            Game1.GetOfficeScene().Cameras.ShowAnimMovement(Building.IDToCamNum(7), Building.IDToCamNum(7));

            if (Challenges.OutputCheat)
                Debug.WriteLine("Golden left");
        }
    }
}
