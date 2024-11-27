using NEA_Project.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAF_NEA_Project.Engine
{
    // A sprite that moves along with a scroll object
    public class ScrollSprite : SpriteItem
    {
        public string ScrollObjectID = "";

        public ScrollSprite(string ScrollObjectID)
        { 
            this.ScrollObjectID = ScrollObjectID;
        }

        public ScrollSprite(string textureName, string ScrollObjectID)
        {
            this.textureName = textureName;
            this.ScrollObjectID = ScrollObjectID;
            LoadContent();
        }

        // What to draw
        public override void Draw()
        {
            if (texture != null && Visible)
            {
                if (ScrollObject.GetList().ContainsKey(ScrollObjectID))
                {
                    ScrollObject so = ScrollObject.GetList()[ScrollObjectID];
                    Vector2 scale = GlobalCamera.ApplyCameraScale(dp.Scale);
                    Vector2 pos = GlobalCamera.ApplyCameraPosition(dp.Pos + new Vector2(so.GetScrollAmount(), 0));
                    float rot = GlobalCamera.ApplyCameraRotation(dp.Rot);
                    Color colour = GlobalCamera.ApplyCameraColour(dp.Colour);
                    MonogameGraphics._spriteBatch.Draw(texture, pos, dp.Size, colour, rot, dp.Origin, scale, dp.Effects, 1.0f);
                }
            }
        }
    }
}
