using FNAF_NEA_Project.Engine.Game;
using Microsoft.Xna.Framework;
using NEA_Project.Engine;
using SharpDX.MediaFoundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAF_NEA_Project.Engine
{
    public class TemperatureSensor : IMonogame
    {
        private SpriteItem TempBarBGSprite;
        private SpriteItem TempBarFGSprite;
        private SpriteItem CoolDisabled;
        private Button CoolButton = new Button(new Rectangle(1344, 260, 128, 64));
        private char CurrentTempGroup = 'B';
        private AudioEffect CoolSound = new AudioEffect("CoolSound", "Audio/IceBreak");
        private bool CanCool = true;
        private bool CamUp = false;
        private TemperatureGroups TempGroup;

        public TemperatureSensor() 
        {
            MonogameIManager.AddObject(this);
        }

        public void Draw(GameTime gameTime)
        {
            DrawManager.EnqueueItem(TempBarBGSprite);
            DrawManager.EnqueueItem(TempBarFGSprite);
            DrawManager.EnqueueItem(CoolDisabled);
        }

        public void Initialize()
        {
            CoolButton.MousePressed += CoolRoom;

            if (Game1.CurrentGame.CurrentScene.GetType() == typeof(OfficeScene))
            {
                TempGroup = ((OfficeScene)Game1.CurrentGame.CurrentScene).TempGroups;
            }
        }

        public void LoadContent()
        {
            // Load BG Sprite
            TempBarBGSprite = new SpriteItem("TempBar");
            TempBarBGSprite.ZIndex = 5;
            TempBarBGSprite.dp.Scale = new Vector2(4);
            TempBarBGSprite.dp.Pos = new Vector2(1344, 32);

            // Load FG Sprite
            TempBarFGSprite = new SpriteItem("TempBarFull");
            TempBarFGSprite.ZIndex = 5;
            TempBarFGSprite.dp.Scale = new Vector2(4);
            SetSensorReading(0);

            // Cool Disabled Button
            CoolDisabled = new SpriteItem("CoolDisabled");
            CoolDisabled.ZIndex = 5;
            CoolDisabled.dp.Scale = new Vector2(4);
            CoolDisabled.dp.Pos = new Vector2(1344, 32);
            CoolDisabled.Visible = false;

            SetVisible(false);

            // Audio
            CoolSound.SetVolume(0.4f);
        }

        public void Update(GameTime gameTime)
        {
            SetSensorReading(TempGroup.GetTemperature(CurrentTempGroup));
            UpdateButtonEnabled();
        }

        private void UpdateButtonEnabled()
        {
            if (TempGroup.IsOnCoolDown(CurrentTempGroup) && CanCool)
                CanCool = false;
            else if (!TempGroup.IsOnCoolDown(CurrentTempGroup) && !CanCool)
                CanCool = true;

            // Only make the button active IF the cameras are up
            if (CoolButton.GetActive() != CanCool && CamUp)
                CoolButton.SetActive(CanCool && CamUp);
            if (CoolDisabled.Visible != (!CanCool && CamUp))
                CoolDisabled.Visible = !CanCool && CamUp;
        }

        public void SetVisible(bool visible)
        {
            CamUp = visible;
            TempBarBGSprite.Visible = visible;
            TempBarFGSprite.Visible = visible;
            CoolButton.SetActive(visible);

            // Only make the button active IF the cameras are up
            if (CoolButton.GetActive() != CanCool && CamUp)
                CoolButton.SetActive(CanCool && CamUp);
            if (CoolDisabled.Visible != (!CanCool && CamUp))
                CoolDisabled.Visible = !CanCool && CamUp;
        }

        public void SetSensorReading(float value) // Value should range from 0 to 1
        {
            int Offset = (int)((1f - value) * 46);
            TempBarFGSprite.dp.Pos = new Vector2(1348, 72 + Offset * 4);
            TempBarFGSprite.dp.Size = new Rectangle(0, Offset, 30, 46 - Offset);
        }

        public void SwitchCam(int CamNum)
        {
            CurrentTempGroup = TemperatureGroups.CamToGroup(CamNum);
        }

        public void CoolRoom()
        {
            TempGroup.CoolGradual(CurrentTempGroup);
            Game1.GetOfficeScene().Power.RemovePower(0.75f);
            CoolSound.Play();
        }
    }
}
