using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA_Project.Engine
{
    public class DrawProperties
    {
        public Vector2 Pos = new Vector2(0,0);

        public Rectangle? Size = null;

        public Color Colour = Color.White;

        public float Rot = 0.0f;

        public Vector2 Origin = new Vector2(0, 0);

        public Vector2 Scale = new Vector2(1, 1);

        public SpriteEffects Effects = SpriteEffects.None;
    }
}
