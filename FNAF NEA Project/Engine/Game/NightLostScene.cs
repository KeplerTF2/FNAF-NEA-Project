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
    public class NightLostScene : Scene
    {
        public TextItem text = new TextItem("DefaultFont", "You Lose :(");
        public Timer timer = new Timer(2000);

        public NightLostScene() { }

        public override void Initialize()
        {
            text.dp.Pos = new Vector2(640 - 160, 360 - 60);
            timer.Elapsed += MainMenu;
            timer.AutoReset = false;
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

        public void MainMenu()
        {
            Game1.CurrentGame.RequestChangeScene(Scenes.MAIN_MENU);
        }

        public void MainMenu(object sender, EventArgs e)
        {
            timer.Dispose();
            MainMenu();
        }
    }
}
