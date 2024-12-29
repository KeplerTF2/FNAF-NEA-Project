﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NEA_Project.Engine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FNAF_NEA_Project.Engine.Game
{
    public enum Entrance
    {
        LEFT_DOOR, HALLWAY, RIGHT_DOOR
    }

    public class OfficeScene : Scene
    {
        public ScrollObject Scroll;
        public ScrollSprite Office;
        public Clock Time = new Clock();
        public Power Power = new Power();
        public DebugPosCollector DebugPosCollector = new DebugPosCollector(false);
        public Cameras Cameras = new Cameras();
        public TemperatureGroups TempGroups = new TemperatureGroups();
        public Building Building = new Building();
        public Door LeftDoor = new Door(DoorSide.LEFT);
        public Door RightDoor = new Door(DoorSide.RIGHT);
        public HallwayLight HallwayLight = new HallwayLight();
        private AudioEffect Ambience1 = new AudioEffect("Ambience1", "Audio/nighttime_ambience");
        private AudioEffect Ambience1Amp = new AudioEffect("Ambience1Amp", "Audio/nighttime_ambience");
        private AudioEffect Ambience2 = new AudioEffect("Ambience2", "Audio/camera_light");
        private bool AmbiencePlaying = false;

        public AnimatedSprite sprite;

        // Animatronics
        public MainAnimatronic Freddy = new MainAnimatronic(20, "Freddy", 6.43f, MainAnimatronic.AllEntrances, MainAnimatronic.DefaultReturnRooms, MainAnimatronic.DefaultVisRooms, "Audio/metalwalk1");
        public MainAnimatronic Bonnie = new MainAnimatronic(20, "Bonnie", 6f, Entrance.LEFT_DOOR, new int[] { 5, 0, 1, 2 }, new int[] { 0, 2, 3, 5, 6, 7, 8 }, "Audio/metalwalk2");
        public MainAnimatronic Chica = new MainAnimatronic(20, "Chica", 5.47f, Entrance.RIGHT_DOOR, new int[] { 1, 2, 3, 4 }, new int[] { 0, 2, 3, 6, 12 }, "Audio/metalwalk3");
        public MainAnimatronic Foxy = new MainAnimatronic(20, "Foxy", 7.17f, Entrance.HALLWAY, new int[] { 0, 1, 2, 3 }, new int[] { 0, 2, 3, 6 }, "Audio/running", 0f, 0.25f, 1);
        public GoldenFreddy GoldenFreddy = new GoldenFreddy(20);
        public Helpy Helpy = new Helpy(20);
        public static bool IsJumpscared = false;

        public OfficeScene() { }

        public override void Initialize()
        {
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
                Debug.WriteLine("Added event to " + animatronic.GetName());
            }
        }

        public override void Draw(GameTime gameTime)
        {
            DrawManager.EnqueueItem(Office);
            //DrawManager.EnqueueItem(sprite);
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
            Game1.ChangeScene(new NightWonScene());
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
        }
    }
}
