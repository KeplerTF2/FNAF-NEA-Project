using FNAF_NEA_Project.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA_Project.Engine
{
    public static class DrawManager
    {
        private static Queue[] queueArray = new Queue[32];

        public static void Draw()
        {
            foreach (Queue queue in queueArray)
            {
                if (queue != null)
                {
                    while (queue.Length > 0)
                    {
                        dynamic drawItem = queue.Dequeue();
                        drawItem.Draw();
                    }
                }
            }
        }

        public static void EnqueueItem(DrawItem item)
        {
            if (item.Visible)
            {
                int pos = item.ZIndex;
                if (queueArray[pos] == null)
                {
                    queueArray[pos] = new Queue();
                }
                queueArray[pos].Enqueue(item);
            }
        }
    }
}
