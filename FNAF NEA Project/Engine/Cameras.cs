using Microsoft.Xna.Framework;
using NEA_Project.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;

public enum CamState
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
        private AnimatedSprite StaticAnim;
        private SpriteItem CamIndicator;
        private SpriteItem CamMap;
        private TextItem CamLabel;
        private CamButton[] CamButtons;
        public static int CurrentCamNum = 1;
        private TemperatureSensor TempSensor = new TemperatureSensor();
        private PowerGenerator PowerGen = new PowerGenerator();

        private float StaticFade = 0.1f;

        // Vars for scrolling sprites
        private AnimatedSprite RoomSprite;
        private static float ScrollAmount = 0f;
        private float ScrollSpeed = 384f / 4f; // Pixels per second
        private float MaxScrollAmount = -384f;
        private float ScrollWait = 0f;
        private float MaxScrollWait = 4f;
        private bool IsScrolling = false;
        private bool ScrollRight = true; // If false, scroll left

        // Audio Effects
        private AudioEffect FlipSound = new AudioEffect("CamFlip", "Audio/camera_load_short", 0.5f);
        private AudioEffect TurnSound = new AudioEffect("CamTurn", "Audio/camera_turn", 0f);
        private AudioEffect BlipSound = new AudioEffect("CamBlip", "Audio/blip", 0.5f);
        private AudioEffect BuzzSound = new AudioEffect("Buzz", "Audio/Buzz", 0f);

        public Cameras()
        {
            InitCamButtons();
            MonogameIManager.AddObject(this);
        }

        // Returns if cameras are in use
        public static bool IsUsing()
        {
            return Using;
        }

        // Returns the state of the cams
        public static CamState GetState()
        {
            return State;
        }

        public void Draw(GameTime gameTime)
        {
            // Updates animated sprites
            CamBG.Update(gameTime);
            LoadAnim.Update(gameTime);
            StaticAnim.Update(gameTime);

            // Camera Scroll Logic
            ScrollSprite(gameTime);

            DrawManager.EnqueueItem(CamBG);
            DrawManager.EnqueueItem(RoomSprite);
            DrawManager.EnqueueItem(StaticAnim);
            DrawManager.EnqueueItem(CamIndicator);
            DrawManager.EnqueueItem(LoadAnim);
            DrawManager.EnqueueItem(CamMap);
            DrawManager.EnqueueItem(CamLabel);
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
            CamBG.ZIndex = 3;
            CamBG.dp.Scale = new Vector2(4);
            CamBG.AnimationFinished += event_FlipAnimFinished;

            // Creates the sprite for the room being viewed
            RoomSprite = new AnimatedSprite("RoomSprite", new AnimationData("CamBG/", 8));
            RoomSprite.ZIndex = 3;
            RoomSprite.dp.Scale = new Vector2(4);
            RoomSprite.Frame = 1;
            RoomSprite.Visible = false;

            // Creates the camera map sprite
            CamMap = new SpriteItem("CamMap");
            CamMap.ZIndex = 4;
            CamMap.dp.Scale = new Vector2(4);
            CamMap.Visible = false;

            // Creates the static sprite
            StaticAnim = new AnimatedSprite("StaticSprite", new AnimationData("Static/", 4, 10, true));
            StaticAnim.ZIndex = 4;
            StaticAnim.dp.Scale = new Vector2(4);
            StaticAnim.dp.Colour = new Color(0.1f, 0.1f, 0.1f, 0.1f);
            StaticAnim.Visible = false;
            StaticAnim.Play();

            // Creates the load animation sprite
            LoadAnim = new AnimatedSprite("load", new AnimationData(new string[] { "CamLoad/0", "CamLoad/1", "CamLoad/2", "CamLoad/2", "CamLoad/3" }, 12));
            LoadAnim.ZIndex = 4;
            LoadAnim.dp.Scale = new Vector2(4);
            LoadAnim.Frame = 4;

            // Creates the trigger indicator sprite
            CamIndicator = new SpriteItem("CamIndicator");
            CamIndicator.ZIndex = 6;
            CamIndicator.dp.Pos = new Vector2((384 * 2) - (576 / 2), (216 * 4) - 68);

            // Creates the text displaying which cam we are on
            CamLabel = new TextItem("DefaultFont", "Cam 01 - Parts & Service");
            CamLabel.ZIndex = 4;
            CamLabel.Visible = false;
            CamLabel.dp.Scale = new Vector2(0.5f);
            CamLabel.dp.Pos = new Vector2(512, 32);
        }

        public void Update(GameTime gameTime)
        {
            // Keybind Input Logic
            if (InputManager.GetKeyState("FlipCam").JustDown && !Power.PowerOut) { event_FlipCamera(); }

            // Updates opacity of static
            if (StaticFade > 0.1f)
            {
                // Change how long static fades depending on challenges enabled
                if (Challenges.HeavyStatic)
                    StaticFade -= (float)gameTime.ElapsedGameTime.TotalSeconds / 2f;
                else
                    StaticFade -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                float TempFade = MathF.Min(StaticFade, 1f);

                StaticAnim.dp.Colour = new Color(TempFade, TempFade, TempFade, TempFade);
                if (State == CamState.UP) BuzzSound.SetVolume(TempFade);
            }

            // Audio
            if (BuzzSound.GetState() != Microsoft.Xna.Framework.Audio.SoundState.Playing)
            {
                BuzzSound.Play(true);
            }

            if ((FlipSound.GetState() != Microsoft.Xna.Framework.Audio.SoundState.Playing) && CamBG.Frame == 2 && State == CamState.GOING_UP)
            {
                FlipSound.Play();
            }
        }

        public void SetStaticOpacity(float value)
        {
            // Limits value above 0.1 and to below or at the current value
            value = MathF.Max(value, 0.1f);
            value = MathF.Max(value, StaticFade);

            // Sets static opacity
            if (value > StaticFade)
            {
                StaticFade = value;
            }

            float TempFade = MathF.Min(StaticFade, 1f);

            StaticAnim.dp.Colour = new Color(TempFade, TempFade, TempFade, TempFade);
        }

        public void InitCamButtons()
        {
            // Sets up cam buttons
            CamButtons = new CamButton[7];
            CamButtons[0] = new CamButton(1, new Vector2(1004, 408)); // Cam 01
            CamButtons[1] = new CamButton(2, new Vector2(964, 592)); // Cam 02
            CamButtons[2] = new CamButton(3, new Vector2(1338, 618)); // Cam 03
            CamButtons[3] = new CamButton(4, new Vector2(1168, 678)); // Cam 04
            CamButtons[4] = new CamButton(5, new Vector2(984, 700)); // Cam 05
            CamButtons[5] = new CamButton(6, new Vector2(1070, 778)); // Cam 06
            CamButtons[6] = new CamButton(7, new Vector2(1318, 752)); // Cam 06

            foreach (CamButton cam in CamButtons)
            {
                cam.ButtonPressed += event_CamButtonPressed;
            }
        }

        // Toggles whether cameras are up or not
        public void event_FlipCamera()
        {
            Using = !Using; // Sets Using variable

            // Camera background sprite logic
            CamBG.PlayBackwards = !Using;
            CamBG.Play();

            // Power usage logic
            Power.GlobalPower.SetToolStatus(Tools.CAMS, Using);
            Power.GlobalPower.CalculateUsage();

            // Sets the cameras' state to going up or going down
            if (Using) { State = CamState.GOING_UP; FlipSound.Stop(); }
            else 
            {
                State = CamState.GOING_DOWN;
                LoadAnim.Reset(false);
                SetCamsVisible(false);
                FlipSound.Play();
            }
        }

        // When the camera flip animation is finished, set state to either up or down
        public void event_FlipAnimFinished()
        {
            if (Using) { 
                State = CamState.UP;
                LoadAnim.Play();
                if (Challenges.HeavyStatic) SetStaticOpacity(1f);
                else SetStaticOpacity(0.75f);
                SetCamsVisible(true);
            }
            else { State = CamState.DOWN; }
        }

        public void SetCamsVisible(bool value)
        {
            // Sprites
            CamMap.Visible = value;
            CamLabel.Visible = value;
            RoomSprite.Visible = value;
            StaticAnim.Visible = value;
            TempSensor.SetVisible(value);
            PowerGen.SetVisible(value);

            // Audio
            if (value)
            {
                TurnSound.SetVolume(0.5f);
            }
            else
            {
                TurnSound.SetVolume(0f);
                BuzzSound.SetVolume(0f);
            }

            // Cam Byttons
            foreach (CamButton cam in CamButtons)
            {
                cam.SetVisible(value);
            }
        }

        public void event_CamButtonPressed(int CamNum)
        {
            CurrentCamNum = CamNum;
            RoomSprite.Frame = CamNum;
            if (Challenges.HeavyStatic) SetStaticOpacity(1f);
            else SetStaticOpacity(0.5f);
            LoadAnim.Play();
            BlipSound.Play();
            TempSensor.SwitchCam(CamNum);
            PowerGen.SwitchCams();
            CamLabel.Text = "Cam " + string.Format("{0:00}", CamNum) + " - " + Building.GetRoom(CamNum).GetName();

            foreach (Animatronic Anim in Animatronic.AnimatronicDict.Values)
            {
                Anim.UpdateSprite();
            }
        }

        public void PowerOutage()
        {
            if (Using)
            {
                event_FlipCamera();
            }
            CamIndicator.Visible = false;
            CamTrigger.SetActive(false);
        }

        private void ScrollSprite(GameTime gameTime)
        {
            float Time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (IsScrolling)
            {
                // Calculates new scroll amount
                if (ScrollRight) ScrollAmount += Time * ScrollSpeed;
                else ScrollAmount -= Time * ScrollSpeed;

                // If scroll amount has reached a limit, stop scrolling and wait
                if (ScrollAmount > 0)
                {
                    ScrollAmount = 0;
                    IsScrolling = false;
                }
                else if (ScrollAmount < MaxScrollAmount)
                {
                    ScrollAmount = MaxScrollAmount;
                    IsScrolling = false;
                }

                // Scrolls the room sprite
                RoomSprite.dp.Pos.X = ScrollAmount;
            }
            else
            {
                // Increments wait time
                ScrollWait += Time;

                // When max wait time reached, start scrolling
                if (ScrollWait > MaxScrollWait)
                {
                    ScrollWait = 0;
                    IsScrolling = true;
                    ScrollRight = !ScrollRight;
                    TurnSound.Play();
                }
            }
        }

        public static float GetScrollAmount()
        {
            return ScrollAmount;
        }

        public void ShowAnimMovement(int CamFrom, int CamTo)
        {
            if ((!Challenges.SilentSteps) || (Challenges.SilentSteps && (CurrentCamNum == CamFrom || CurrentCamNum == CamTo)))
                SetStaticOpacity(1.5f);
        }
    }
}
