﻿using Microsoft.Xna.Framework;
using NEA_Project.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace FNAF_NEA_Project.Engine.Game
{
    public class NightWonScene : Scene
    {
        public TextItem text = new TextItem("DefaultFont", "You Win!");
        public Timer timer = new Timer(2000);

        public NightWonScene() { }

        public override void Initialize()
        {
            text.dp.Pos = new Vector2(640 - 160, 360 - 60);
            timer.Elapsed += NextNight;
            timer.Start();
        }

        public override void LoadContent()
        {
            // Should always be first!
            base.LoadContent();

            text.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            DrawManager.EnqueueItem(text);
        }

        public override void Update(GameTime gameTime)
        {
        }

        public void NextNight()
        {
            Game1.ChangeScene(new OfficeScene(Global.NightNum));
        }

        public void NextNight(object sender, EventArgs e)
        {
            NextNight();
        }
    }
}
