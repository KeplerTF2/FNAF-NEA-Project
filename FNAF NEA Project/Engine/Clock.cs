﻿using Microsoft.Xna.Framework;
using NEA_Project.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace FNAF_NEA_Project.Engine
{
    public class Clock : IMonogame
    {
        private TextItem DrawText;
        private float Time = 0;
        private int Minute = -1;
        private int Hour = 12;
        private bool IsAM = true;
        private float TimeScale = 60f; // How much faster this clock should run compared to real time
        private float EndTime = 360;
        private int NightNum = 1;

        public event Notify EndTimeReached;

        public Clock()
        {
            MonogameIManager.AddObject(this);
        }

        public Clock(int NightNum)
        {
            this.NightNum = NightNum;
            MonogameIManager.AddObject(this);
        }

        public void Draw(GameTime gameTime)
        {
            DrawText.QueueToDraw();
        }

        public void Initialize()
        {
        }

        public void LoadContent()
        {
            // Text setup logic
            DrawText = new TextItem("PixelFont", "Night " + NightNum);
            DrawText.dp.Scale = new Vector2(0.4f, 0.4f);
            DrawText.dp.Pos = new Vector2(16, 16);
            DrawText.ZIndex = 6;
        }

        public void Update(GameTime gameTime)
        {
            if (!Game1.GetOfficeScene().InTutorial)
            {
                // Adds to time
                Time += (float)gameTime.ElapsedGameTime.TotalSeconds;
                float ActualTime = Time * TimeScale / 60f;

                if ((int)ActualTime % 60 != Minute) // Does a check to see if the label actually needs updating
                {
                    // Time calculations
                    Minute = (int)ActualTime % 60;
                    Hour = (int)(ActualTime / 60f) % 12;
                    if (Hour == 0) Hour = 12;
                    IsAM = (int)(ActualTime / 720f) % 2 == 0;

                    // Sets the label
                    DrawText.Text = String.Format("Night " + NightNum + "\n{0:00}:{1:00} " + (IsAM ? "AM" : "PM"), Hour, Minute);
                }

                // Checks if end time has been reached
                if (ActualTime >= EndTime) EndTimeReached?.Invoke();
            }
        }

        public void PowerOutage()
        {
            // Make the time invisible, but still have it count up
            DrawText.Visible = false;
        }
    }
}
