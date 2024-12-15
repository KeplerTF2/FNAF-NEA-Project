using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NEA_Project.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAF_NEA_Project.Engine.Game
{
    public class OfficeScene : Scene
    {
        public ScrollObject Scroll;
        public ScrollSprite Office;
        public Clock Time = new Clock();
        public Power Power = new Power();
        public DebugPosCollector DebugPosCollector = new DebugPosCollector(false);
        public Cameras Cameras = new Cameras();
        public TemperatureGroups TempGroups = new TemperatureGroups();
        public Building Building = new Building();
        public Door LeftDoor = new Door(DoorSide.LEFT);
        public Door RightDoor = new Door(DoorSide.RIGHT);
        private AudioEffect Ambience1 = new AudioEffect("Ambience1", "Audio/nighttime_ambience");
        private AudioEffect Ambience2 = new AudioEffect("Ambience2", "Audio/camera_light");
        private bool AmbiencePlaying = false;

        public AnimatedSprite sprite;

        public OfficeScene() { }

        public override void Initialize()
        {
            DebugPosCollector.Initialize();

            Time.EndTimeReached += event_EndTimeReached;
        }

        public override void LoadContent()
        {
            // Should always be first!
            base.LoadContent();

            Scroll = new ScrollObject("Scroll", 0, 1536, -768, 0, true, true);
            Office = new ScrollSprite("Office", "Scroll");
            Office.dp.Scale = new Vector2(4);
        }

        public override void Draw(GameTime gameTime)
        {
            DrawManager.EnqueueItem(Office);
            //DrawManager.EnqueueItem(sprite);
        }

        public override void Update(GameTime gameTime)
        {
            // Audio
            if (!AmbiencePlaying)
            {
                AmbiencePlaying = true;
                Ambience1.Play(true);
                Ambience2.Play(true);
            }
        }

        private void event_EndTimeReached()
        {
            Game1.ChangeScene(new NightWonScene());
        }

        private void event_TestTrigger()
        {
            Time.Update(new GameTime(new TimeSpan(0, 0, 20), new TimeSpan(0, 0, 20)));
        }

        private void event_TestTrigger2()
        {
            //sprite.Play();
        }
    }
}
