using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable

namespace NEA_Project.Engine
{
    public class SpriteItem : DrawItem
    {
        protected string textureName = "";
        protected Texture2D? texture = null;
        public Texture2D? Texture { get { return texture; } }

        public SpriteItem() { }
        public SpriteItem(string textureName)
        {
            this.textureName = textureName;
            LoadContent();
        }


        // What to load
        public override void LoadContent()
        {
            TextureManager.AddTexture(textureName);
            texture = TextureManager.GetTexture(textureName);
        }

        // What to draw
        public override void Draw()
        {
            if (texture != null && Visible)
            {
                Vector2 scale = GlobalCamera.ApplyCameraScale(dp.Scale);
                Vector2 pos = GlobalCamera.ApplyCameraPosition(dp.Pos);
                float rot = GlobalCamera.ApplyCameraRotation(dp.Rot);
                Color colour = GlobalCamera.ApplyCameraColour(dp.Colour);
                MonogameGraphics._spriteBatch.Draw(texture, pos, dp.Size, colour, rot, dp.Origin, scale, dp.Effects, 1.0f);
            }
        }
    }
}
