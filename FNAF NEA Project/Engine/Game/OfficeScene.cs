using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NEA_Project.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAF_NEA_Project.Engine.Game
{
    public class OfficeScene : Scene
    {
        public ScrollObject Scroll;
        public ScrollSprite Office;
        public ScrollSprite Freddy;
        public Clock Time = new Clock();
        public Power Power = new Power();
        public DebugPosCollector DebugPosCollector = new DebugPosCollector(false);
        public Cameras Cameras = new Cameras();
        public Button Trigger = new Button(new Rectangle(32, 64, 128, 32), true, true);
        public Button Trigger2 = new Button(new Rectangle(40, 104, 120, 32), true, true);

        public OfficeScene()
        {
            textures = new string[] { "freddy", "Office", "Square64", "Camera", "CamIndicator" };
        }

        public override void Initialize()
        {
            DebugPosCollector.Initialize();

            Time.EndTimeReached += event_EndTimeReached;

            // Debug
            Trigger.MousePressed += event_TestTrigger;
            Trigger2.MousePressed += event_TestTrigger2;
        }

        public override void LoadContent()
        {
            // Should always be first!
            base.LoadContent();

            Scroll = new ScrollObject("Scroll", 0, 1280, -640, 0, true, true);
            Office = new ScrollSprite("Office", "Scroll");
            Freddy = new ScrollSprite("freddy", "Scroll");
            Freddy.dp.Pos.X = 200;
            Freddy.dp.Pos.Y = 200;
            Freddy.ZIndex = 1;
        }

        public override void Draw(GameTime gameTime)
        {
            DrawManager.EnqueueItem(Office);
            DrawManager.EnqueueItem(Freddy);
        }

        public override void Update(GameTime gameTime)
        {
        }

        private void event_EndTimeReached()
        {
            Game1.ChangeScene(new NightWonScene());
        }

        private void event_TestTrigger()
        {
            Time.Update(new GameTime(new TimeSpan(0, 0, 20), new TimeSpan(0, 0, 20)));
        }

        private void event_TestTrigger2()
        {
            Time.Update(new GameTime(new TimeSpan(0, 0, -20), new TimeSpan(0, 0, -20)));
        }
    }
}
