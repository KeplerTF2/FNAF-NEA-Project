using Microsoft.Xna.Framework;
using NEA_Project.Engine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAF_NEA_Project.Engine
{
    public class Temperature
    {
        private float Value;
        public float Rate; // Decrease per min

        public Temperature()
        {
            Value = 1f;
            Rate = 1f;
        }

        public Temperature(float Rate)
        {
            Value = 1f;
            this.Rate = Rate;
        }

        public float GetValue() { return Value; }

        public void UpdateValue(GameTime gameTime)
        {
            Value -= Rate / 60f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Value < 0.5f) Value = 0.5f;
        }
    }

    public class TemperatureGroups: IMonogame
    {
        private static Dictionary<char, Temperature> Groups = new Dictionary<char, Temperature>();

        public TemperatureGroups()
        {
            // Inits relevant temperature groups
            Groups.Add('A', new Temperature(30f / 50f)); // Cam 03
            Groups.Add('B', new Temperature(30f / 30f)); // Cam 01
            Groups.Add('C', new Temperature(30f / 40f)); // Cam 04
            Groups.Add('D', new Temperature(30f / 40f)); // Cam 07
            Groups.Add('E', new Temperature(30f / 30f)); // Cam 06
            Groups.Add('F', new Temperature(30f / 30f)); // Cam 02
            Groups.Add('G', new Temperature(30f / 20f)); // Cam 05
            Groups.Add('_', new Temperature()); // Office

            MonogameIManager.AddObject(this);
        }

        public void Draw(GameTime gameTime)
        {
        }

        public void Initialize()
        {
        }

        public void LoadContent()
        {
        }

        public void Update(GameTime gameTime)
        {
            foreach (char c in Groups.Keys)
            {
                if (c != '_')
                    Groups[c].UpdateValue(gameTime);
            }
        }

        public static float GetTemperature(char c)
        {
            return Groups[c].GetValue();
        }
    }
}
