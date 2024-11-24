using Microsoft.Xna.Framework;
using NEA_Project.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAF_NEA_Project.Engine.Game
{
    public class TestScene : Scene
    {
        public ScrollObject Scroll;
        public ScrollSprite Office;
        public ScrollSprite Freddy;
        public Clock Time = new Clock();

        public TestScene()
        {
            textures = new string[] { "freddy", "Office" };
        }

        public override void Initialize()
        {
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
    }
}
