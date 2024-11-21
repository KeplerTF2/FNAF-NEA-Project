using FNAF_NEA_Project.Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA_Project.Engine
{
    public abstract class DrawItem
    {
        // Order of which it should be drawn when passed to the DrawManager
        private int zIndex = 0;

        public int ZIndex
        {
            get { return zIndex; }
            set 
            {
                if (value < 0) { value = 0; }
                if (value > 31) { value = 31; }
                zIndex = value; 
            }
        }

        public DrawProperties dp = new DrawProperties();

        public bool Visible = true;

        // What to load
        public abstract void LoadContent();

        // What to draw
        public abstract void Draw();

        public void QueueToDraw()
        {
            DrawManager.EnqueueItem(this);
        }
    }
}
