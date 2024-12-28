using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        public event Notify Repaired;

        private SpriteItem BGSprite;
        private SpriteItem RepairSprite;
        private SpriteItem OnSprite;
        private Button GenButton = new Button(new Rectangle(1024, 64, 256, 128));
        private Button RepairButton = new Button(new Rectangle(1052, 212, 200, 44), false);
        private bool CamUp = false;
        private bool Generating = false;
        private bool Broken = false;

        private AudioEffect PowerGenPressSound = new AudioEffect("PowerGenPressSound", "Audio/press1");
        private AudioEffect PowerGenOnSound = new AudioEffect("PowerGenOnSound", "Audio/send");
        private AudioEffect RepairSound = new AudioEffect("Repair", "Audio/BuzzTurnOn", 0.8f);
        private SoundEffectInstance PowerGenOnInstance;

        public PowerGenerator()
        {
            MonogameIManager.AddObject(this);
        }

        public void Draw(GameTime gameTime)
        {
            DrawManager.EnqueueItem(BGSprite);
            DrawManager.EnqueueItem(OnSprite);
            DrawManager.EnqueueItem(RepairSprite);
        }

        public void Initialize()
        {
            GenButton.MousePressed += ToggleGenerator;
            RepairButton.MousePressed += Repair;
        }

        public void LoadContent()
        {
            // Load BG Sprite
            BGSprite = new SpriteItem("PowerGen");
            BGSprite.ZIndex = 5;
            BGSprite.dp.Scale = new Vector2(4);
            BGSprite.dp.Pos = new Vector2(1024, 64);

            // Load On Sprite
            OnSprite = new SpriteItem("PowerGenOn");
            OnSprite.ZIndex = 5;
            OnSprite.dp.Scale = new Vector2(4);
            OnSprite.dp.Pos = new Vector2(1024, 64);
            OnSprite.Visible = false;

            // Repair Sprite
            RepairSprite = new SpriteItem("Repair");
            RepairSprite.ZIndex = 5;
            RepairSprite.dp.Scale = new Vector2(4);
            RepairSprite.dp.Pos = new Vector2(1024, 64);
            RepairSprite.Visible = false;

            // Audio
            PowerGenPressSound.SetVolume(0.75f);
            PowerGenOnSound.SetVolume(0.15f);
            RepairSound.GetInstance().Pitch = 0.25f;
            PowerGenOnInstance = PowerGenOnSound.GetInstance();

            SetVisible(false);
        }

        public void Update(GameTime gameTime)
        {
            // Updates pitch of the sound based off power usage
            PowerGenOnInstance.Pitch = Power.GlobalPower.GetUsage() * -0.1f;
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
            if (RepairButton.GetActive() != (BGSprite.Visible && Broken))
            {
                RepairButton.SetActive(BGSprite.Visible && Broken);
                RepairSprite.Visible = BGSprite.Visible && Broken;
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

        public void Break()
        {
            Broken = true;
            UpdateButtonEnabled();
        }

        public void Repair()
        {
            Broken = false;
            UpdateButtonEnabled();
            Repaired?.Invoke();
            RepairSound.Play();
        }
    }
}
