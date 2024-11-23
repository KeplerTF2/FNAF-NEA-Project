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

        public OfficeScene()
        {
            textures = new string[] { "freddy", "Office" };
        }

        public override void Initialize()
        {
            // Time Init Logic
            Time.Initialize();
            Time.EndTimeReached += event_EndTimeReached;

            // Power Init Logic
            Power.Initialize();
        }

        public override void LoadContent()
        {
            // Should always be first!
            base.LoadContent();

            Time.LoadContent();

            Power.LoadContent();

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
            Time.Draw(gameTime);
            Power.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            Time.Update(gameTime);
            Power.Update(gameTime);
        }

        private void event_EndTimeReached()
        {
            Game1.ChangeScene(new NightWonScene());
        }
    }
}
