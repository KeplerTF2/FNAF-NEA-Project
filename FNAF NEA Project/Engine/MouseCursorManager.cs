using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAF_NEA_Project.Engine
{
    public static class MouseCursorManager
    {
        private static List<Button> Buttons = new List<Button>();
        private static MouseCursor CurrentCursor = MouseCursor.Arrow;

        public static void ResetData()
        {
            Buttons.Clear(); 
        }

        public static void AddButton(Button button)
        {
            Buttons.Add(button);
        }

        public static void RemoveButton(Button button)
        {
            Buttons.Remove(button);
        }

        public static void Update(GameTime gameTime)
        {
            MouseCursor NewCursor = MouseCursor.Arrow;
            foreach (Button button in Buttons)
            {
                if (button.IsMouseInTrigger()) { NewCursor = MouseCursor.Hand; break; }
            }
            if (NewCursor != CurrentCursor)
            {
                CurrentCursor = NewCursor;
                Mouse.SetCursor(NewCursor);
            }
        }
    }
}
