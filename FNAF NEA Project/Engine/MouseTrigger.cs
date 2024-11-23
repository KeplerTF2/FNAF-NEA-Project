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
    public class MouseTrigger : IMonogame
    {
        public bool DrawDebug = false;

        private bool Active = true;
        private Rectangle Rect = new Rectangle(0, 0, 64, 64);
        private RectItem DebugRect = new RectItem();
        private bool MouseInside = false;

        public event Notify MouseEntered;
        public event Notify MouseLeft;

        public MouseTrigger() { }

        public MouseTrigger(bool Active)
        {
            this.Active = Active;
        }

        public MouseTrigger(Rectangle Rect)
        {
            SetRect(Rect);
        }

        public MouseTrigger(Rectangle Rect, bool Active)
        {
            SetRect(Rect);
            this.Active = Active;
        }

        public MouseTrigger(bool Active, bool DrawDebug)
        {
            this.Active = Active;
        }

        public MouseTrigger(Rectangle Rect, bool Active, bool DrawDebug)
        {
            SetRect(Rect);
            this.Active = Active;
            this.DrawDebug = DrawDebug;
        }

        public void SetRect(Rectangle Rect)
        {
            this.Rect = Rect;
            DebugRect.Size = new Vector2(Rect.Width, Rect.Height);
            DebugRect.dp.Pos = new Vector2(Rect.X, Rect.Y);
        }

        public bool IsMouseInTrigger()
        {
            return MouseInside;
        }

        public void SetActive(bool Active, bool TriggerEvents)
        {
            this.Active = Active;
            if (MouseInside)
            {
                MouseInside = false;
                if (TriggerEvents) MouseLeft?.Invoke();
            }
        }

        public void SetActive(bool Active)
        {
            this.Active = Active;
            if (MouseInside)
            {
                MouseInside = false;
            }
        }

        public bool GetActive()
        {
            return Active;
        }

        public void Draw(GameTime gameTime)
        {
            if (DrawDebug)
            {
                DrawManager.EnqueueItem(DebugRect);
            }
        }

        public void Initialize()
        {
            DebugRect.ZIndex = 31;
        }

        public void LoadContent()
        {
            DebugRect.LoadContent();
        }

        public void Update(GameTime gameTime)
        {
            if (Active)
            {
                if (Rect.Left < (Mouse.GetState().X / (float)GlobalCamera.WindowSize.X * (float)GlobalCamera.Size.X)
                && Rect.Right > (Mouse.GetState().X / (float)GlobalCamera.WindowSize.X * (float)GlobalCamera.Size.X)
                && Rect.Top   < (Mouse.GetState().Y / (float)GlobalCamera.WindowSize.Y * (float)GlobalCamera.Size.Y)
                && Rect.Bottom > (Mouse.GetState().Y / (float)GlobalCamera.WindowSize.Y * (float)GlobalCamera.Size.Y))
                {
                    if (!MouseInside) { MouseInside = true; MouseEntered?.Invoke(); Debug.WriteLine("Mouse entered"); }
                }
                else if (MouseInside) { MouseInside = false; MouseLeft?.Invoke(); Debug.WriteLine("Mouse left"); }
            }
        }
    }
}
