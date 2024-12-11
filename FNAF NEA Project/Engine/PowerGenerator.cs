using Microsoft.Xna.Framework;
using NEA_Project.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAF_NEA_Project.Engine
{
    public class PowerGenerator : IMonogame
    {
        private SpriteItem BGSprite;
        private SpriteItem OnSprite;
        private Button GenButton = new Button(new Rectangle(1024, 64, 256, 128));
        private AudioEffect PowerGenPressSound = new AudioEffect("PowerGenPressSound", "Audio/press1");
        private AudioEffect PowerGenOnSound = new AudioEffect("PowerGenOnSound", "Audio/send");
        private bool CamUp = false;
        private bool Generating = false;

        public PowerGenerator()
        {
            MonogameIManager.AddObject(this);
        }

        public void Draw(GameTime gameTime)
        {
            DrawManager.EnqueueItem(BGSprite);
            DrawManager.EnqueueItem(OnSprite);
        }

        public void Initialize()
        {
            GenButton.MousePressed += ToggleGenerator;
        }

        public void LoadContent()
        {
            // Load BG Sprite
            BGSprite = new SpriteItem("PowerGen");
            BGSprite.ZIndex = 3;
            BGSprite.dp.Scale = new Vector2(4);
            BGSprite.dp.Pos = new Vector2(1024, 64);

            // Load On Sprite
            OnSprite = new SpriteItem("PowerGenOn");
            OnSprite.ZIndex = 3;
            OnSprite.dp.Scale = new Vector2(4);
            OnSprite.dp.Pos = new Vector2(1024, 64);

            OnSprite.Visible = false; // TEMP

            // Audio
            PowerGenPressSound.SetVolume(0.75f);
            PowerGenOnSound.SetVolume(0.15f);

            SetVisible(false);
        }

        public void Update(GameTime gameTime)
        {
            //UpdateButtonEnabled();
        }

        private void UpdateButtonEnabled()
        {
            // Only make the button active IF the cameras are up
            if (GenButton.GetActive() != ((Cameras.CurrentCamNum == 5) && CamUp))
            {
                GenButton.SetActive((Cameras.CurrentCamNum == 5) && CamUp);
                BGSprite.Visible = (Cameras.CurrentCamNum == 5) && CamUp;
            }
            if (OnSprite.Visible != ((Cameras.CurrentCamNum == 5) && CamUp && Generating))
            {
                OnSprite.Visible = (Cameras.CurrentCamNum == 5) && CamUp && Generating;
            }
        }

        public void SwitchCams()
        {
            if (Cameras.CurrentCamNum != 5 && Generating)
                SetGenerator(false);
            UpdateButtonEnabled();
        }

        public void SetVisible(bool visible)
        {
            CamUp = visible;
            BGSprite.Visible = visible;
            //OnSprite.Visible = visible;
            GenButton.SetActive(visible);

            if (Generating) SetGenerator(false);

            // Only make the button active IF the cameras are up
            UpdateButtonEnabled();
        }

        public void ToggleGenerator()
        {
            SetGenerator(!Generating);
        }

        public void SetGenerator(bool value)
        {
            Generating = value;
            Power.GlobalPower.SetToolStatus(Tools.GENERATOR, value);
            Power.GlobalPower.CalculateUsage();
            PowerGenPressSound.Play();
            if (value) PowerGenOnSound.Play(true);
            else PowerGenOnSound.Stop();
            UpdateButtonEnabled();
        }
    }
}
