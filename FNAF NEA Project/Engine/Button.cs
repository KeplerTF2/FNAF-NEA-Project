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
    public class Button : MouseTrigger
    {
        public bool ButtonPressed = false;

        public event Notify MousePressed;
        public event Notify MouseReleased;

        public Button() { }

        public Button(bool Active)
        {
            this.Active = Active;
            MonogameIManager.AddObject(this);
        }

        public Button(Rectangle Rect)
        {
            SetRect(Rect);
            MonogameIManager.AddObject(this);
        }

        public Button(Rectangle Rect, bool Active)
        {
            SetRect(Rect);
            this.Active = Active;
            MonogameIManager.AddObject(this);
        }

        public Button(bool Active, bool DrawDebug)
        {
            this.Active = Active;
            this.DrawDebug = DrawDebug;
            MonogameIManager.AddObject(this);
        }

        public Button(Rectangle Rect, bool Active, bool DrawDebug)
        {
            SetRect(Rect);
            this.Active = Active;
            this.DrawDebug = DrawDebug;
            MonogameIManager.AddObject(this);
        }

        public override void Initialize()
        {
            base.Initialize();

            MouseCursorManager.AddButton(this);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Active && MouseInside)
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed && !ButtonPressed) { ButtonPressed = true; MousePressed?.Invoke(); }
                else if (Mouse.GetState().LeftButton == ButtonState.Released && ButtonPressed) { ButtonPressed = false; MouseReleased?.Invoke(); }
            }
        }
    }
}
