using Microsoft.Xna.Framework;
using SharpDX.Direct2D1.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA_Project.Engine
{
    static public class GlobalCamera
    {
        public static DrawProperties dp = new DrawProperties();
        public static Vector2 Size = new Vector2(0, 0);
        public static Point WindowSize = new Point(0, 0);

        // Calculates the new position when the camera position, rotation and scale are applied to it
        static public Vector2 ApplyCameraPosition(Vector2 OldPos)
        {
            Vector2 Scale = dp.Scale * new Vector2(WindowSize.X / Size.X, WindowSize.Y / Size.Y);
            // Applies camera position and scale
            Vector2 NewPos = new Vector2((OldPos.X - dp.Pos.X) * Scale.X + dp.Origin.X, (OldPos.Y - dp.Pos.Y) * Scale.Y + dp.Origin.Y);

            // Creates temporary position
            float tempX = NewPos.X;
            float tempY = NewPos.Y;

            // Applies camera rotation
            NewPos.X = (tempX - dp.Origin.X) * MathF.Cos(dp.Rot) + (tempY - dp.Origin.Y) * -MathF.Sin(dp.Rot);
            NewPos.X += dp.Origin.X;
            NewPos.Y = (tempX - dp.Origin.X) * MathF.Sin(dp.Rot) + (tempY - dp.Origin.Y) * MathF.Cos(dp.Rot);
            NewPos.Y += dp.Origin.Y;

            return NewPos;
        }

        // Calculates a new colour when the camera colour is multiplied on top
        static public Color ApplyCameraColour(Color OldColour)
        {
            return new Color(OldColour.R * dp.Colour.R / 65025f, OldColour.G * dp.Colour.G / 65025f, OldColour.B * dp.Colour.B / 65025f, OldColour.A * dp.Colour.A / 65025f);
        }

        // Calculates a new scale when the camera scale is multiplied with it
        static public Vector2 ApplyCameraScale(Vector2 OldScale)
        {
            if (Size != new Vector2(0,0) && WindowSize != new Point(0, 0)) { return new Vector2(OldScale.X / dp.Scale.X * (WindowSize.X / Size.X), OldScale.Y / dp.Scale.Y * (WindowSize.Y / Size.Y)); }
            else { return new Vector2(OldScale.X / dp.Scale.X, OldScale.Y / dp.Scale.Y); }
        }

        // Calculates a new rotation when the camera rotation is added to it
        static public float ApplyCameraRotation(float OldRotation)
        {
            return OldRotation + dp.Rot;
        }
    }
}
