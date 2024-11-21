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
        public SpriteItem Freddy;

        public TestScene()
        {
            textures = new string[] { "freddy" };
        }

        public override void Initialize()
        {
        }

        public override void LoadContent()
        {
            // Should always be first!
            base.LoadContent();

            Freddy = new SpriteItem("freddy");
        }

        public override void Draw(GameTime gameTime)
        {
            DrawManager.EnqueueItem(Freddy);
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
