﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NEA_Project.Engine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAF_NEA_Project.Engine.Game
{
    public enum Entrance
    {
        LEFT_DOOR, HALLWAY, RIGHT_DOOR
    }

    public class OfficeScene : Scene
    {
        // Private Vars
        private ScrollObject Scroll;
        private ScrollSprite Office;
        private Clock Time;
        private DebugPosCollector DebugPosCollector = new DebugPosCollector(false);
        private HallwayLight HallwayLight = new HallwayLight();
        private AudioEffect Ambience1 = new AudioEffect("Ambience1", "Audio/nighttime_ambience");
        private AudioEffect Ambience1Amp = new AudioEffect("Ambience1Amp", "Audio/nighttime_ambience");
        private AudioEffect Ambience2 = new AudioEffect("Ambience2", "Audio/camera_light");
        private bool AmbiencePlaying = false;
        private Tutorial Tutorial = new Tutorial();

        // Animatronics
        private MainAnimatronic Freddy;
        private MainAnimatronic Bonnie;
        private MainAnimatronic Chica;
        private MainAnimatronic Foxy;
        private GoldenFreddy GoldenFreddy;
        private Helpy Helpy;

        // Public vars
        public int NightNum;
        public Power Power = new Power();
        public Cameras Cameras = new Cameras();
        public TemperatureGroups TempGroups = new TemperatureGroups();
        public Building Building = new Building();
        public Door LeftDoor = new Door(DoorSide.LEFT);
        public Door RightDoor = new Door(DoorSide.RIGHT);
        public bool IsJumpscared = false;
        public bool InTutorial = true;

        public OfficeScene()
        {
            Freddy = new MainAnimatronic(0, Animatronics.Freddy, 6.43f, MainAnimatronic.AllEntrances, MainAnimatronic.DefaultReturnRooms, MainAnimatronic.DefaultVisRooms, "Audio/metalwalk1");
            Bonnie = new MainAnimatronic(0, Animatronics.Bonnie, 6f, Entrance.LEFT_DOOR, new int[] { 5, 0, 1, 2 }, new int[] { 0, 2, 3, 5, 6, 7, 8 }, "Audio/metalwalk2");
            Chica = new MainAnimatronic(0, Animatronics.Chica, 5.47f, Entrance.RIGHT_DOOR, new int[] { 1, 2, 3, 4 }, new int[] { 0, 2, 3, 6, 12 }, "Audio/metalwalk3");
            Foxy = new MainAnimatronic(0, Animatronics.Foxy, 7.17f, Entrance.HALLWAY, new int[] { 0, 1, 2, 3 }, new int[] { 0, 2, 3, 6 }, "Audio/running", 0f, 0.25f, 1);
            GoldenFreddy = new GoldenFreddy(0);
            Helpy = new Helpy(0);

            Time = new Clock();
        }

        public OfficeScene(Dictionary<Animatronics, int> AIDict, int NightNum)
        {
            this.NightNum = NightNum;
            if (NightNum < 7) SaveData.NightNum = NightNum;
            Time = new Clock(NightNum);

            // Goes through the dictionary and assigns
            foreach (Animatronics Name in AIDict.Keys)
            {
                Debug.WriteLine(Name + ": " + AIDict[Name]);
                switch (Name)
                {
                    case Animatronics.Freddy:
                        Freddy = new MainAnimatronic(AIDict[Name], Name, 6.43f, MainAnimatronic.AllEntrances, MainAnimatronic.DefaultReturnRooms, MainAnimatronic.DefaultVisRooms, "Audio/metalwalk1"); break;
                    case Animatronics.Bonnie:
                        Bonnie = new MainAnimatronic(AIDict[Name], Name, 6f, Entrance.LEFT_DOOR, new int[] { 5, 0, 1, 2 }, new int[] { 0, 2, 3, 5, 6, 7, 8 }, "Audio/metalwalk2"); break;
                    case Animatronics.Chica:
                        Chica = new MainAnimatronic(AIDict[Name], Name, 5.47f, Entrance.RIGHT_DOOR, new int[] { 1, 2, 3, 4 }, new int[] { 0, 2, 3, 6, 12 }, "Audio/metalwalk3"); break;
                    case Animatronics.Foxy:
                        Foxy = new MainAnimatronic(AIDict[Name], Name, 7.17f, Entrance.HALLWAY, new int[] { 0, 1, 2, 3 }, new int[] { 0, 2, 3, 6 }, "Audio/running", 0f, 0.25f, 1); break;
                    case Animatronics.GoldenFreddy:
                        GoldenFreddy = new GoldenFreddy(AIDict[Name]); break;
                    case Animatronics.Helpy:
                        Helpy = new Helpy(AIDict[Name]); break;
                }
            }
        }

        public OfficeScene(int NightNum)
        {
            this.NightNum = NightNum;
            if (NightNum < 7) SaveData.NightNum = NightNum;
            Time = new Clock(NightNum);

            Dictionary<Animatronics, int> AIDict = NightSettings.GetAIs(NightNum);

            // Goes through the dictionary and assigns
            foreach (Animatronics Name in AIDict.Keys)
            {
                Debug.WriteLine(Name + ": " + AIDict[Name]);
                switch (Name)
                {
                    case Animatronics.Freddy:
                        Freddy = new MainAnimatronic(AIDict[Name], Name, 6.43f, MainAnimatronic.AllEntrances, MainAnimatronic.DefaultReturnRooms, MainAnimatronic.DefaultVisRooms, "Audio/metalwalk1"); break;
                    case Animatronics.Bonnie:
                        Bonnie = new MainAnimatronic(AIDict[Name], Name, 6f, Entrance.LEFT_DOOR, new int[] { 5, 0, 1, 2 }, new int[] { 0, 2, 3, 5, 6, 7, 8 }, "Audio/metalwalk2"); break;
                    case Animatronics.Chica:
                        Chica = new MainAnimatronic(AIDict[Name], Name, 5.47f, Entrance.RIGHT_DOOR, new int[] { 1, 2, 3, 4 }, new int[] { 0, 2, 3, 6, 12 }, "Audio/metalwalk3"); break;
                    case Animatronics.Foxy:
                        Foxy = new MainAnimatronic(AIDict[Name], Name, 7.17f, Entrance.HALLWAY, new int[] { 0, 1, 2, 3 }, new int[] { 0, 2, 3, 6 }, "Audio/running", 0f, 0.25f, 1); break;
                    case Animatronics.GoldenFreddy:
                        GoldenFreddy = new GoldenFreddy(AIDict[Name]); break;
                    case Animatronics.Helpy:
                        Helpy = new Helpy(AIDict[Name]); break;
                }
            }
        }

        public override void Initialize()
        {
            InputManager.AddKeyInput("MainMenu", Keys.Escape);

            DebugPosCollector.Initialize();

            Time.EndTimeReached += event_EndTimeReached;
            Power.PowerOutReached += event_PowerOutage;

            // Animatronics that can be flashed by the hallway light
            HallwayLight.Flashed += Freddy.HallwayFlashed;
            HallwayLight.Flashed += Foxy.HallwayFlashed;

            // Golden Freddy attacks
            GoldenFreddy.Attacked += event_GoldenFreddyAttacked;
            Cameras.PowerGen.Repaired += event_PowerGenFixed;
        }

        public override void LoadContent()
        {
            // Should always be first!
            base.LoadContent();

            Scroll = new ScrollObject("Scroll", 0, 1536, -768, 0, true, true);
            Office = new ScrollSprite("Office", "Scroll");
            Office.dp.Scale = new Vector2(4);

            foreach (Animatronic animatronic in Animatronic.AnimatronicDict.Values)
            {
                animatronic.Jumpscared += event_OnJumpscare;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Office.QueueToDraw();
        }

        public override void Update(GameTime gameTime)
        {
            // Audio
            if (!AmbiencePlaying)
            {
                AmbiencePlaying = true;
                Ambience1.Play(true);
                Ambience2.Play(true);
            }

            if (InputManager.GetKeyState("MainMenu").JustDown) MainMenu();
        }

        private void event_GoldenFreddyAttacked()
        {
            Power.SetToolStatus(Tools.GOLDEN_FREDDY, true);
            Power.CalculateUsage();
            Cameras.PowerGen.Break();
        }

        private void event_PowerGenFixed()
        {
            Power.SetToolStatus(Tools.GOLDEN_FREDDY, false);
            Power.CalculateUsage();
            GoldenFreddy.OnPowerGenRepair();
        }

        private void event_EndTimeReached()
        {
            SaveData.CurrentNightNum = NightNum;
            if (NightNum < 6) SaveData.NightNum++;
            else if (NightNum == 6) SaveData.CustomNight = true;

            SaveFileHandler.WriteSaveData();
            Challenges.SetAll(false);
            Game1.CurrentGame.RequestChangeScene(Scenes.NIGHTWIN);
        }

        private void event_PowerOutage()
        {
            // Calls power outage on relevant objects
            HallwayLight.PowerOutage();
            Cameras.PowerOutage();
            LeftDoor.PowerOutage();
            RightDoor.PowerOutage();
            Time.PowerOutage();

            // Stops ambient light sound
            Ambience2.Stop();

            // Amplifies ambient outside sound by creating another (you can't set volume above 1)
            Ambience1Amp.Play(true);
        }

        private void event_OnJumpscare()
        {
            IsJumpscared = true;

            // Reuse some power outages because they do what we want (deactivate input and hud)
            HallwayLight.PowerOutage();
            Cameras.PowerOutage();
            Time.PowerOutage();
            Power.DoPowerOut(true);
            Scroll.SetOut((int)Scroll.GetScrollAmount(), (int)Scroll.GetScrollAmount());

            LeftDoor.RemoveInput();
            RightDoor.RemoveInput();

            Challenges.SetAll(false);
        }

        private void MainMenu()
        {
            Challenges.SetAll(false);
            Game1.CurrentGame.RequestChangeScene(Scenes.MAIN_MENU);
        }
    }
}
