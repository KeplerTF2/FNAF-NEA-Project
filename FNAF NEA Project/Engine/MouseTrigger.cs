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

        protected bool Active = true;
        protected Rectangle Rect = new Rectangle(0, 0, 64, 64);
        protected RectItem DebugRect = new RectItem();
        protected bool MouseInside = false;

        public event Notify MouseEntered;
        public event Notify MouseLeft;

        public MouseTrigger()
        {
            MonogameIManager.AddObject(this);
        }

        public MouseTrigger(bool Active)
        {
            this.Active = Active;
            MonogameIManager.AddObject(this);
        }

        public MouseTrigger(Rectangle Rect)
        {
            SetRect(Rect);
            MonogameIManager.AddObject(this);
        }

        public MouseTrigger(Rectangle Rect, bool Active)
        {
            SetRect(Rect);
            this.Active = Active;
            MonogameIManager.AddObject(this);
        }

        public MouseTrigger(bool Active, bool DrawDebug)
        {
            this.Active = Active;
            this.DrawDebug = DrawDebug;
            MonogameIManager.AddObject(this);
        }

        public MouseTrigger(Rectangle Rect, bool Active, bool DrawDebug)
        {
            SetRect(Rect);
            this.Active = Active;
            this.DrawDebug = DrawDebug;
            MonogameIManager.AddObject(this);
        }

        // Sets size and position of trigger
        public void SetRect(Rectangle Rect)
        {
            this.Rect = Rect;
            DebugRect.Size = new Vector2(Rect.Width, Rect.Height);
            DebugRect.dp.Pos = new Vector2(Rect.X, Rect.Y);
        }

        // Returns if mouse is inside
        public bool IsMouseInTrigger()
        {
            return MouseInside;
        }

        // Sets if active with the option to trigger events
        public void SetActive(bool Active, bool TriggerEvents)
        {
            this.Active = Active;
            if (MouseInside)
            {
                MouseInside = false;
                if (TriggerEvents) MouseLeft?.Invoke();
            }
        }

        // Sets if active without triggering events
        public void SetActive(bool Active)
        {
            this.Active = Active;
            if (MouseInside)
            {
                MouseInside = false;
            }
        }

        // Returns active
        public bool GetActive()
        {
            return Active;
        }

        // If DrawDebug is enabled, draw bounds of the trigger
        public void Draw(GameTime gameTime)
        {
            if (DrawDebug)
            {
                DrawManager.EnqueueItem(DebugRect);
            }
        }

        public virtual void Initialize()
        {
            DebugRect.ZIndex = 31;
        }

        public void LoadContent()
        {
            DebugRect.LoadContent();
        }

        // Mains logic for checking if the mouse is in bounds
        public virtual void Update(GameTime gameTime)
        {
            if (Active)
            {
                // Checks if mouse is in the window
                if (Mouse.GetState().X / (float)GlobalCamera.WindowSize.X * (float)GlobalCamera.Size.X >= 0
                && Mouse.GetState().X / (float)GlobalCamera.WindowSize.X * (float)GlobalCamera.Size.X <= 1280
                && Mouse.GetState().Y / (float)GlobalCamera.WindowSize.Y * (float)GlobalCamera.Size.Y >= 0
                && Mouse.GetState().Y / (float)GlobalCamera.WindowSize.Y * (float)GlobalCamera.Size.Y <= 720)
                {
                    // Checks if mouse is in the trigger
                    if (Rect.Left < (Mouse.GetState().X / (float)GlobalCamera.WindowSize.X * (float)GlobalCamera.Size.X)
                    && Rect.Right > (Mouse.GetState().X / (float)GlobalCamera.WindowSize.X * (float)GlobalCamera.Size.X)
                    && Rect.Top < (Mouse.GetState().Y / (float)GlobalCamera.WindowSize.Y * (float)GlobalCamera.Size.Y)
                    && Rect.Bottom > (Mouse.GetState().Y / (float)GlobalCamera.WindowSize.Y * (float)GlobalCamera.Size.Y))
                    {
                        // If so and previously wasn't, invoke MouseEntered event
                        if (!MouseInside) { MouseInside = true; MouseEntered?.Invoke(); }
                    }
                    // If not and previously was, invoke MouseLeft event
                    else if (MouseInside) { MouseInside = false; MouseLeft?.Invoke(); }
                }
            }
        }
    }
}
