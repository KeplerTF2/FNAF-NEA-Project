using Microsoft.Xna.Framework;
using NEA_Project.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAF_NEA_Project.Engine
{
    public class Cameras : IMonogame
    {
        private static bool Using = false;
        private float TriggerTimer = 0f;
        private bool TriggerAvailable = true;
        private MouseTrigger CamTrigger = new MouseTrigger(new Rectangle(352, 655, 576, 66));
        private AnimatedSprite CamBG;
        private SpriteItem CamIndicator;

        public Cameras()
        {
            MonogameIManager.AddObject(this);
        }

        public static bool IsUsing()
        {
            return Using;
        }

        public void Draw(GameTime gameTime)
        {
            DrawManager.EnqueueItem(CamBG);
            DrawManager.EnqueueItem(CamIndicator);
        }

        public void Initialize()
        {
            CamTrigger.MouseEntered += event_FlipCamera;
        }

        public void LoadContent()
        {
            CamBG = new AnimatedSprite("flip", new AnimationData("CamLoad/", 5));
            CamBG.ZIndex = 2;
            CamBG.dp.Scale = new Vector2(5f / 6f);

            CamIndicator = new SpriteItem("CamIndicator");
            CamIndicator.ZIndex = 5;
            CamIndicator.dp.Pos = new Vector2(352, 655);
        }

        public void Update(GameTime gameTime)
        {
            CamBG.Update(gameTime);
            /*
            if (!TriggerAvailable)
            {
                TriggerTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (TriggerTimer >= 0.4f)
                {
                    TriggerAvailable = true;
                    TriggerTimer = 0f;
                    CamIndicator.Visible = true;
                }
            }
            */
        }

        public void event_FlipCamera()
        {
            Using = !Using;
            CamBG.PlayBackwards = !Using;
            CamBG.SetPlaying(true);
            //CamIndicator.Visible = false;
            //TriggerAvailable = false;
        }
    }
}
