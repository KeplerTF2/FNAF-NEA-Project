using Microsoft.Xna.Framework;
using NEA_Project.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

enum CamState
{
    DOWN, GOING_UP, UP, GOING_DOWN
}

namespace FNAF_NEA_Project.Engine
{
    public class Cameras : IMonogame
    {
        private static bool Using = false;
        private static CamState State = CamState.DOWN;
        private float TriggerTimer = 0f;
        private bool TriggerAvailable = true;
        private MouseTrigger CamTrigger = new MouseTrigger(new Rectangle((384 * 2) - (576 / 2), (216 * 4) - 68, 576, 80));
        private AnimatedSprite CamBG;
        private AnimatedSprite LoadAnim;
        private SpriteItem CamIndicator;
        private SpriteItem CamMap;

        // TEST CAMERA
        private SingleCam Cam1 = new SingleCam(1, new Vector2(500, 500));
        public Cameras()
        {
            MonogameIManager.AddObject(this);
        }

        // Returns if cameras are in use
        public static bool IsUsing()
        {
            return Using;
        }

        public void Draw(GameTime gameTime)
        {
            DrawManager.EnqueueItem(CamBG);
            DrawManager.EnqueueItem(CamIndicator);
            DrawManager.EnqueueItem(LoadAnim);
            DrawManager.EnqueueItem(CamMap);
        }

        public void Initialize()
        {
            InputManager.AddKeyInput("FlipCam", Keys.S); // Keybind setup
            CamTrigger.MouseEntered += event_FlipCamera; // Trigger event setup
        }

        public void LoadContent()
        {
            // Creates the camera background sprite
            CamBG = new AnimatedSprite("flip", new AnimationData("CamFlip/", 5));
            CamBG.ZIndex = 2;
            CamBG.dp.Scale = new Vector2(4);
            CamBG.AnimationFinished += event_FlipAnimFinished;

            // Creates the camera map sprite
            CamMap = new SpriteItem("CamMap");
            CamMap.ZIndex = 2;
            CamMap.dp.Scale = new Vector2(4);
            CamMap.Visible = false;

            // Creates the load animation sprite
            LoadAnim = new AnimatedSprite("load", new AnimationData(new string[] { "CamLoad/0", "CamLoad/1", "CamLoad/2", "CamLoad/2", "CamLoad/3" }, 12));
            LoadAnim.ZIndex = 4;
            LoadAnim.dp.Scale = new Vector2(4);
            LoadAnim.Frame = 4;

            // Creates the trigger indicator sprite
            CamIndicator = new SpriteItem("CamIndicator");
            CamIndicator.ZIndex = 5;
            CamIndicator.dp.Pos = new Vector2((384 * 2) - (576 / 2), (216 * 4) - 68);
        }

        public void Update(GameTime gameTime)
        {
            // Updates animated sprites
            CamBG.Update(gameTime);
            LoadAnim.Update(gameTime);

            // Keybind Input Logic
            if (InputManager.GetKeyState("FlipCam").JustDown) { event_FlipCamera(); }
        }

        // Toggles whether cameras are up or not
        public void event_FlipCamera()
        {
            Using = !Using; // Sets Using variable

            // Camera background sprite logic
            CamBG.PlayBackwards = !Using;
            CamBG.SetPlaying(true);

            // Power usage logic
            Power.GlobalPower.SetToolStatus(Tools.CAMS, Using);
            Power.GlobalPower.CalculateUsage();

            // Sets the cameras' state to going up or going down
            if (Using) { State = CamState.GOING_UP; }
            else 
            {
                State = CamState.GOING_DOWN;
                LoadAnim.Reset(false);
                CamMap.Visible = false;
            }
        }

        // When the camera flip animation is finished, set state to either up or down
        public void event_FlipAnimFinished()
        {
            if (Using) { 
                State = CamState.UP;
                LoadAnim.Play();
                CamMap.Visible = true;
            }
            else { State = CamState.DOWN; }
        }
    }
}
