using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAF_NEA_Project.Engine
{
    // Sets the correct mouse cursor image
    public static class MouseCursorManager
    {
        private static List<Button> Buttons = new List<Button>(); // Stores all current button objects
        private static MouseCursor CurrentCursor = MouseCursor.Arrow; // The current mouse cursor image

        // Clears all data
        public static void ResetData()
        {
            Buttons.Clear(); 
        }

        // Adds a button to the list
        public static void AddButton(Button button)
        {
            Buttons.Add(button);
        }

        // Removes a button from the list
        public static void RemoveButton(Button button)
        {
            Buttons.Remove(button);
        }

        // Main logic for setting the correct image
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
