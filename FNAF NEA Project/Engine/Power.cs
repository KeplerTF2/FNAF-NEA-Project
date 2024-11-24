using Microsoft.Xna.Framework;
using NEA_Project.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAF_NEA_Project.Engine
{
    public class Power : IMonogame
    {
        private TextItem DrawText;
        private float Amount = 101f;
        private float Usage = 0f; // %/min
        private float PassivePowerLoss = 5f; // %/min

        public event Notify PowerOutReached;

        public Power() 
        {
            MonogameIManager.AddObject(this);
        }

        public void Draw(GameTime gameTime)
        {
            DrawManager.EnqueueItem(DrawText);
        }

        public void Initialize()
        {
        }

        public void LoadContent()
        {
            DrawText = new TextItem("DefaultFont", "100%");
            DrawText.dp.Scale = new Vector2(0.5f, 0.5f);
            DrawText.dp.Pos = new Vector2(10, 720 - 48);
            DrawText.ZIndex = 5;
        }

        public void Update(GameTime gameTime)
        {
            CalculateUsage();
            float OldAmount = Amount;
            Amount -= Usage / 60f * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Amount <= 0) { PowerOutReached?.Invoke(); Amount = 0; }

            // Change label if needed
            else if ((int)OldAmount != (int)Amount)
            {
                DrawText.Text = (int)Amount + "%";
            }
        }

        private void CalculateUsage()
        {
            Usage = PassivePowerLoss;
        }
    }
}
