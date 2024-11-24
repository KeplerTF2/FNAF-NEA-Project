using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NEA_Project.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAF_NEA_Project.Engine
{
    public class RectItem : DrawItem
    {
        private Vector2 vec64 = new Vector2(64, 64);

        protected Texture2D texture = null;

        public Vector2 Size = new Vector2(64, 64);

        public RectItem() { }

        public RectItem(Vector2 Size)
        {
            this.Size = Size;
            LoadContent();
        }

        public RectItem(int X, int Y)
        {
            Size = new Vector2(X, Y);
            LoadContent();
        }

        // What to load
        public override void LoadContent()
        {
            TextureManager.AddTexture("Square64");
            texture = TextureManager.GetTexture("Square64");
        }

        // What to draw
        public override void Draw()
        {
            if (texture != null && Visible)
            {
                Vector2 scale = GlobalCamera.ApplyCameraScale(dp.Scale) * Size / vec64;
                Vector2 pos = GlobalCamera.ApplyCameraPosition(dp.Pos);
                float rot = GlobalCamera.ApplyCameraRotation(dp.Rot);
                Color colour = GlobalCamera.ApplyCameraColour(dp.Colour);
                MonogameGraphics._spriteBatch.Draw(texture, pos, dp.Size, colour, rot, dp.Origin, scale, dp.Effects, 1.0f);
            }
        }
    }
}
