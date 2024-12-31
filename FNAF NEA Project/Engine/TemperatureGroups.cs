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

        private float CoolDownTimer = 0f;
        private bool OnCoolDown = false;

        public Temperature()
        {
            Value = 0f;
            Rate = 1f;
        }

        public Temperature(float Rate)
        {
            Value = 0f;
            this.Rate = Rate;
        }

        public float GetValue() { return Value; }

        public void UpdateValue(GameTime gameTime)
        {
            if (!OnCoolDown)
            {
                Value += Rate / 60f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (Value > 1.0f) Value = 1.0f;
            }
            else
            {
                // Cools temperature
                Value -= 0.2f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (Value < 0.0f)
                { 
                    Value = 0.0f;

                    // End cooldown timer prematurely if on faulty temp
                    if (Challenges.FaultyTemp)
                    {
                        CoolDownTimer = 0f;
                        OnCoolDown = false;
                    }
                }

                // Handles cool down code
                CoolDownTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (CoolDownTimer > 20f)
                {
                    CoolDownTimer = 0f;
                    OnCoolDown = false;
                }
            }
        }

        public void ResetValue()
        {
            Value = 0;
        }

        public void PutOnCoolDown()
        {
            CoolDownTimer = 0f;
            OnCoolDown = true;
        }

        public bool IsOnCoolDown()
        {
            return OnCoolDown;
        }
    }

    public class TemperatureGroups: IMonogame
    {
        private Dictionary<char, Temperature> Groups = new Dictionary<char, Temperature>();

        public TemperatureGroups()
        {
            // Inits relevant temperature groups
            Groups.Add('A', new Temperature(60f / 150f)); // Cam 03
            Groups.Add('B', new Temperature(60f / 105f)); // Cam 01
            Groups.Add('C', new Temperature(60f / 120f)); // Cam 04
            Groups.Add('D', new Temperature(60f / 135f)); // Cam 07
            Groups.Add('E', new Temperature(60f / 75f)); // Cam 06
            Groups.Add('F', new Temperature(60f / 90f)); // Cam 02
            Groups.Add('G', new Temperature(60f / 60f)); // Cam 05
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

        public float GetTemperature(char c)
        {
            return Groups[c].GetValue();
        }

        public void CoolInstant(char c)
        {
            Groups[c].ResetValue();
        }

        public void CoolGradual(char c)
        {
            Groups[c].PutOnCoolDown();
        }

        public bool IsOnCoolDown(char c)
        {
            return Groups[c].IsOnCoolDown();
        }

        // Gets the temperature group character associated with the cam num
        public static char CamToGroup(int CamNum)
        {
            switch (CamNum)
            {
                case 1:
                    return 'B';
                case 2:
                    return 'F';
                case 3:
                    return 'A';
                case 4:
                    return 'C';
                case 5:
                    return 'G';
                case 6:
                    return 'E';
                case 7:
                    return 'D';
                default:
                    return '_';
            }
        }
    }
}
