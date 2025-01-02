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
        public bool LMBPressed = false;
        public bool ClickedOutside = false;

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
            MouseCursorManager.AddButton(this); // Adds the button to MouseCursorManager for the proper cursor image
        }

        // Main logic
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime); // Checks if the mouse is within the button bounds

            // If valid, invoke relevant events if the left mouse button is clicked or not
            if (Active)
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed && !LMBPressed)
                {
                    LMBPressed = true;
                    ClickedOutside = !MouseInside;
                }
                else if (Mouse.GetState().LeftButton == ButtonState.Released && LMBPressed)
                {
                    LMBPressed = false;
                    ClickedOutside = false;
                }

                if (LMBPressed && ButtonPressed && (!MouseInside) && (!ClickedOutside))
                {
                    ButtonPressed = false;
                    LMBPressed = false;
                }

                if (MouseInside && !ClickedOutside)
                {
                    if (LMBPressed && !ButtonPressed) { ButtonPressed = true; MousePressed?.Invoke(); }
                    else if (!LMBPressed && ButtonPressed) { ButtonPressed = false; MouseReleased?.Invoke(); }
                }
            }
        }

        public void SetPos(Vector2 pos)
        {
            Rectangle NewRect = Rect;
            NewRect.X = (int)pos.X;
            NewRect.Y = (int)pos.Y;
            SetRect(NewRect);
        }
    }
}
