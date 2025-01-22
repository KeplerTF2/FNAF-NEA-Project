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

        private RectItem rect = new RectItem(64, 48);

        public DebugPosCollector() 
        {
            MonogameIManager.AddObject(this);
        }

        public DebugPosCollector(bool Active)
        {
            this.Active = Active;
            MonogameIManager.AddObject(this);
        }

        // If active, draw the rectangle at where the mouse is
        public void Draw(GameTime gameTime)
        {
            if (Active)
            {
                rect.dp.Pos = new Vector2(Mouse.GetState().X / (float)GlobalCamera.WindowSize.X * (float)GlobalCamera.Size.X,
                                          Mouse.GetState().Y / (float)GlobalCamera.WindowSize.Y * (float)GlobalCamera.Size.Y);
                DrawManager.EnqueueItem(rect);
            }
        }

        // Sets up rectangle
        public void Initialize()
        {
            rect.dp.Colour = new Color(Color.White, 0.5f);
            rect.ZIndex = 31;
        }

        // Sets up rectangle
        public void LoadContent()
        {
            rect.LoadContent();
        }

        // If active, print mouse co-ords (in terms of global camera size) to debug console
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
