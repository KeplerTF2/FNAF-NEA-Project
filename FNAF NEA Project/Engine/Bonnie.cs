using FNAF_NEA_Project.Engine.Game;
using Microsoft.Xna.Framework;
using NEA_Project.Engine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace FNAF_NEA_Project.Engine
{
    public class Bonnie : Animatronic
    {
        Timer MoveTimer = new Timer(1000);
        Timer ReturnTimer = new Timer(3000);
        float BaseTime = 5f;
        float MaxTime = 5f;
        float CurrentTime = 0f;
        float DoorTime = 0f;
        float MaxDoorTime = 5f;
        int NextRoom;
        Random random = new Random();
        int[] ReturnRooms = new int[] { 5, 0, 1, 2 };
        bool Returning = false;

        // Audio
        private AudioEffect MoveSound = new AudioEffect("MoveBonnie", "Audio/metalwalk1", 0.25f);

        public Bonnie()
        {
            VisibleRooms = new int[] { 0, 2, 3, 5, 6, 7, 8 };
            BaseTime = GetTime(5.43f, Difficulty);
            MoveTimer.AutoReset = true;
            MoveTimer.Start();
            MoveTimer.Elapsed += UpdateNextMovement;

            ReturnTimer.AutoReset = false;
            ReturnTimer.Elapsed += Return;

            MonogameIManager.AddObject(this);
        }

        public Bonnie(int AI)
        {
            VisibleRooms = new int[] { 0, 2, 3, 5, 6, 7, 8 };
            Difficulty = AI;

            BaseTime = GetTime(6.43f, Difficulty);
            MoveTimer.AutoReset = true;
            MoveTimer.Start();
            MoveTimer.Elapsed += UpdateNextMovement;

            ReturnTimer.AutoReset = false;
            ReturnTimer.Elapsed += Return;

            MonogameIManager.AddObject(this);
        }

        public override void Draw(GameTime gameTime)
        {
            if (ShouldDrawCamSprite())
            {
                CamSprite.dp.Pos.X = Cameras.GetScrollAmount();
                DrawManager.EnqueueItem(CamSprite);
            }
        }

        public override void Initialize()
        {
            CurrentRoom = 2;
            Name = "Bonnie";
            AnimatronicDict.Add(Name, this);
            UpdateNextMovement();
        }

        public override void LoadContent()
        {
            CreateCamSprite();
            UpdateSprite();
            MoveSound.GetInstance().Pitch = -0.15f;
        }

        public override void Update(GameTime gameTime)
        {
            if (Difficulty != 0 && !Returning)
            {
                if ((CurrentRoom == 9 && Door.IsSideClosed(DoorSide.LEFT)) || (CurrentRoom == 11 && Door.IsSideClosed(DoorSide.RIGHT)))
                {
                    DoorTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (DoorTime > MaxDoorTime)
                        Return();
                    Debug.WriteLine("Door closed!");
                }
                else
                {
                    CurrentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (CurrentTime > MaxTime)
                        Move();
                }
            }
        }

        private void UpdateNextMovement()
        {
            if (CurrentRoom != 13)
            {
                // Finds target room for Bonnie
                int Target = 9;
                if (CurrentRoom == 9)
                    Target = 13;
                NextRoom = Building.GetNextRoom(CurrentRoom, Target);
                MaxTime = Building.GetTempRoomTime(CurrentRoom, NextRoom) * BaseTime;
            }
        }

        private void UpdateNextMovement(object sender, ElapsedEventArgs e)
        {
            UpdateNextMovement();
        }

        private void Move()
        {
            Cameras.ShowAnimMovement(Building.IDToCamNum(CurrentRoom), Building.IDToCamNum(NextRoom));
            CurrentRoom = NextRoom;
            CurrentTime = 0f;
            UpdateNextMovement();
            UpdateSprite();
            if (!Challenges.SilentSteps)
                MoveSound.Play();
            if (CurrentRoom == 13)
                Jumpscare();
        }

        public void HallwayFlashed()
        {
            ReturnTimer.Start();
            Returning = true;
        }

        private void Return()
        {
            if (CurrentRoom == 9 || CurrentRoom == 10 || CurrentRoom == 11)
            {
                // Footsteps on silent steps challenge ONLY if moving away from doors
                if (CurrentRoom != 10 && Challenges.SilentSteps)
                    MoveSound.Play();

                DoorTime = 0f;
                NextRoom = ReturnRooms[random.Next(ReturnRooms.Length)];
                Move();
            }
            Returning = false;
        }

        private void Return(object sender, ElapsedEventArgs e)
        {
            Return();
        }
    }
}
