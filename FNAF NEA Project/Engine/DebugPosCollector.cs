using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NEA_Project.Engine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAF_NEA_Project.Engine
{
    public class DebugPosCollector : IMonogame
    {
        public bool Active = true;

        public RectItem rect = new RectItem(64, 32);

        public DebugPosCollector() { }
        public DebugPosCollector(bool Active)
        {
            this.Active = Active;
        }

        public void Draw(GameTime gameTime)
        {
            if (Active)
            {
                rect.dp.Pos = new Vector2(Mouse.GetState().X / (float)GlobalCamera.WindowSize.X * (float)GlobalCamera.Size.X,
                                          Mouse.GetState().Y / (float)GlobalCamera.WindowSize.Y * (float)GlobalCamera.Size.Y);
                DrawManager.EnqueueItem(rect);
            }
        }

        public void Initialize()
        {
            rect.dp.Colour = new Color(Color.White, 0.5f);
            rect.ZIndex = 31;
        }

        public void LoadContent()
        {
            rect.LoadContent();
        }

        public void Update(GameTime gameTime)
        {
            if (Active)
            {
                Debug.WriteLine("X: " + (int)(Mouse.GetState().X / (float)GlobalCamera.WindowSize.X * (float)GlobalCamera.Size.X)
                             + " Y: " + (int)(Mouse.GetState().Y / (float)GlobalCamera.WindowSize.Y * (float)GlobalCamera.Size.Y));
            }
        }
    }
}
