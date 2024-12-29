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
    // Animatronics that roam the cameras and attack via the door and hallways
    public class MainAnimatronic : Animatronic
    {
        // Defaults
        public static Entrance[] AllEntrances = new Entrance[] { Entrance.LEFT_DOOR, Entrance.HALLWAY, Entrance.RIGHT_DOOR };
        public static int[] DefaultVisRooms = new int[] { 0, 2, 3, 5, 6, 7, 8, 12 };
        public static int[] DefaultReturnRooms = new int[] { 5, 0, 1, 2, 3, 4 };

        // Variables
        Timer MoveTimer = new Timer(1000);
        Timer ReturnTimer = new Timer(3000);
        float BaseTime = 5f;
        float MaxTime = 5f;
        float CurrentTime = 0f;
        float DoorTime = 0f;
        float MaxDoorTime = 5f;
        int NextRoom;
        Random random = new Random();
        int[] ReturnRooms = DefaultReturnRooms;
        bool Returning = false;
        Entrance NextEntrance = Entrance.LEFT_DOOR;
        Entrance[] AvailableEntrances = new Entrance[] { Entrance.LEFT_DOOR, Entrance.HALLWAY, Entrance.RIGHT_DOOR };

        // Audio
        private AudioEffect MoveSound;
        private AudioEffect BangSound;
        private float SoundPitch = 0f;

        public MainAnimatronic(int AI, string Name, float MovementTime, Entrance[] AvailableEntrances, int[] ReturnRooms, int[] VisibleRooms, string SoundName = "Audio/metalwalk1", float SoundPitch = 0f, float SoundVolume = 0.25f, int StartingRoom = 2)
        {
            // Assigns variables
            Difficulty = AI;
            this.Name = Name;
            this.AvailableEntrances = AvailableEntrances;
            this.ReturnRooms = ReturnRooms;
            this.VisibleRooms = VisibleRooms;
            this.SoundPitch = SoundPitch;
            CurrentRoom = StartingRoom;
            NextEntrance = AvailableEntrances[0];

            // Create movement sound
            MoveSound = new AudioEffect("Move" + Name, SoundName, SoundVolume);
            BangSound = new AudioEffect("Banging" + Name, "Audio/banging", 0.35f);

            // Create timers
            BaseTime = GetTime(MovementTime, Difficulty);
            MoveTimer.AutoReset = true;
            MoveTimer.Start();
            MoveTimer.Elapsed += UpdateNextMovement;

            ReturnTimer.AutoReset = false;
            ReturnTimer.Elapsed += Return;

            // Adds to monogame interface manager
            MonogameIManager.AddObject(this);
        }

        public MainAnimatronic(int AI, string Name, float MovementTime, Entrance AvailableEntrance, int[] ReturnRooms, int[] VisibleRooms, string SoundName = "Audio/metalwalk1", float SoundPitch = 0f, float SoundVolume = 0.25f, int StartingRoom = 2)
        {
            // Assigns variables
            Difficulty = AI;
            this.Name = Name;
            AvailableEntrances = new Entrance[] { AvailableEntrance };
            this.ReturnRooms = ReturnRooms;
            this.VisibleRooms = VisibleRooms;
            this.SoundPitch = SoundPitch;
            CurrentRoom = StartingRoom;
            NextEntrance = AvailableEntrance;

            // Create movement sound
            MoveSound = new AudioEffect("Move" + Name, SoundName, SoundVolume);
            BangSound = new AudioEffect("Banging" + Name, "Audio/banging", 0.35f);

            // Create timers
            BaseTime = GetTime(MovementTime, Difficulty);
            MoveTimer.AutoReset = true;
            MoveTimer.Start();
            MoveTimer.Elapsed += UpdateNextMovement;

            ReturnTimer.AutoReset = false;
            ReturnTimer.Elapsed += Return;

            // Adds to monogame interface manager
            MonogameIManager.AddObject(this);
        }

        public override void Draw(GameTime gameTime)
        {
            if (ShouldDrawCamSprite())
            {
                CamSprite.dp.Pos.X = Cameras.GetScrollAmount();
                DrawManager.EnqueueItem(CamSprite);
            }

            DrawManager.EnqueueItem(JumpscareSprite);
        }

        public override void Initialize()
        {
            AnimatronicDict.Add(Name, this);
            NextEntrance = AvailableEntrances[random.Next(AvailableEntrances.Length)];
            UpdateNextMovement();
        }

        public override void LoadContent()
        {
            CreateSprite();
            UpdateSprite();
            MoveSound.GetInstance().Pitch = SoundPitch;
            BangSound.GetInstance().Pitch = -0.25f;
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
            // Finds target room
            if (CurrentRoom != 13)
            {
                int Target = 13;
                if (CurrentRoom != 9 && CurrentRoom != 10 && CurrentRoom != 11)
                {
                    switch (NextEntrance)
                    {
                        case Entrance.LEFT_DOOR:
                            Target = 9; break;
                        case Entrance.HALLWAY:
                            Target = 10; break;
                        case Entrance.RIGHT_DOOR:
                            Target = 11; break;
                    }
                }
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
            if (Challenges.OutputCheat && (CurrentRoom == 9 || CurrentRoom == 10 || CurrentRoom == 11))
                Debug.WriteLine(Name + " has left");

            if ((!Challenges.SilentSteps) && (CurrentRoom != 9) && (CurrentRoom != 11))
                MoveSound.Play();

            Cameras.ShowAnimMovement(Building.IDToCamNum(CurrentRoom), Building.IDToCamNum(NextRoom));
            CurrentRoom = NextRoom;
            CurrentTime = 0f;
            UpdateNextMovement();
            UpdateSprite();
            if (CurrentRoom == 13)
                Jumpscare();

            if (Challenges.OutputCheat && (CurrentRoom == 9 || CurrentRoom == 10 || CurrentRoom == 11))
                Debug.WriteLine(Name + " is attacking in " + NextEntrance);
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
                // Door banging
                if (CurrentRoom != 10)
                    BangSound.Play();

                NextEntrance = AvailableEntrances[random.Next(AvailableEntrances.Length)];
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
