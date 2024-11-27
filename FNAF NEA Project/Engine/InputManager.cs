using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Contains the key, if it's up or down, and if it's just been pressed or released
public class KeyState
{
    private Keys key;
    public bool IsUp;
    public bool IsDown;
    public bool JustUp;
    public bool JustDown;

    public KeyState(Keys key)
    {
        this.key = key;
        IsUp = false;
        IsDown = true;
        JustUp = false;
        JustDown = false;
    }

    public void SetIsUp(bool value)
    {
        IsUp = value;
        IsDown = !value;
    }

    public Keys GetKey() { return key; }
}

namespace FNAF_NEA_Project.Engine
{
    public static class InputManager
    {
        private static Dictionary<string, KeyState> Keys = new Dictionary<string, KeyState>();

        // Adds a keybind (a key along with a name) to the dictionary
        public static void AddKeyInput(string name, Keys key)
        {
            Keys.Add(name, new KeyState(key));
        }

        // Removes a keybind from the dictionary
        public static void RemoveKeyInput(string name)
        {
            Keys.Remove(name);
        }

        // For every keybind, perform logic to check if it's pressed,
        // and if it's been recent pressed or released
        public static void Update()
        {
            foreach (string keyName in Keys.Keys)
            {
                if (Keyboard.GetState().IsKeyDown(Keys[keyName].GetKey()))
                {
                    if (!Keys[keyName].IsDown)
                    {
                        Keys[keyName].SetIsUp(false);
                        Keys[keyName].JustDown = true;
                    }
                    else if (Keys[keyName].JustDown)
                        Keys[keyName].JustDown = false;
                }
                else
                {
                    if (!Keys[keyName].IsUp)
                    {
                        Keys[keyName].SetIsUp(true);
                        Keys[keyName].JustUp = true;
                    }
                    else if (Keys[keyName].JustUp)
                        Keys[keyName].JustUp = false;
                }
            }
        }

        // Returns the KeyState of a key bind
        public static KeyState GetKeyState(string name)
        {
            return Keys[name];
        }
    }
}
