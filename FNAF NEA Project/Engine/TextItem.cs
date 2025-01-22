using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NEA_Project.Engine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAF_NEA_Project.Engine
{
    // Drawable text
    public class TextItem : DrawItem
    {
        protected SpriteFont Font;
        protected string FontName;
        public string Text;
        public bool Centred = false;

        public TextItem(string FontName = "", string Text = "", bool Centred = false)
        {
            this.FontName = FontName;
            this.Text = Text;
            this.Centred = Centred;
            LoadContent();
        }

        public SpriteFont GetSpriteFont()
        {
            return Font;
        }

        // What to load
        public override void LoadContent()
        {
            Font = MonogameGraphics._content.Load<SpriteFont>(FontName);
            if (FontName == "PixelFont" && Font.LineSpacing == 85) Font.LineSpacing = 101;
        }

        // What to draw
        public override void Draw()
        {
            if (Font != null && Visible)
            {
                Vector2 scale = GlobalCamera.ApplyCameraScale(dp.Scale);
                Vector2 pos;
                if (Centred) pos = GlobalCamera.ApplyCameraPosition(dp.Pos - (Font.MeasureString(Text) * dp.Scale / 2f));
                else pos = GlobalCamera.ApplyCameraPosition(dp.Pos);
                float rot = GlobalCamera.ApplyCameraRotation(dp.Rot);
                Color colour = GlobalCamera.ApplyCameraColour(dp.Colour);
                MonogameGraphics._spriteBatch.DrawString(Font, Text, pos, colour, rot, dp.Origin, scale, SpriteEffects.None, 1);
            }
        }
    }
}
