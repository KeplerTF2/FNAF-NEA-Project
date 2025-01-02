using FNAF_NEA_Project.Engine.Game;
using Microsoft.Xna.Framework;
using NEA_Project.Engine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace FNAF_NEA_Project.Engine
{
    public enum Animatronics 
    {
        Freddy, Bonnie, Chica, Foxy, GoldenFreddy, Helpy
    }
    public abstract class Animatronic: IMonogame
    {
        public event Notify Jumpscared;
        Timer SceneSwitchTimer = new Timer(1500);

        protected int Difficulty;
        protected Animatronics Name;
        protected int CurrentRoom;
        protected bool KillInOffice;
        protected bool HasJumpscare = true;
        protected bool IsJumpscaring = false;
        protected AnimatedSprite CamSprite;
        protected SpriteItem JumpscareSprite;
        protected int[] VisibleRooms = new int[] { 0, 2, 3, 5, 6, 7, 8, 12 }; // Default rooms that can be seen from the cameras
        private AudioEffect JumpSound = new AudioEffect("Jump", "Audio/jumpscare", 0.8f);

        public static Dictionary<Animatronics, Animatronic> AnimatronicDict = new Dictionary<Animatronics, Animatronic>();

        public int GetDifficulty()
        {
            return Difficulty;
        }

        public string GetName()
        {
            return Name.ToString();
        }

        public int GetCurrentRoom()
        {
            return CurrentRoom;
        }

        public void UpdateSprite()
        {
            if (VisibleRooms.Contains(Building.CamNumToID(Game1.GetOfficeScene().Cameras.CurrentCamNum)))
            {
                CamSprite.SetAnimation(CurrentRoom.ToString());
            }
        }

        protected bool ShouldDrawCamSprite()
        {
            if (VisibleRooms.Contains(CurrentRoom))
            {
                if (CurrentRoom != 2)
                    return (CurrentRoom == Building.CamNumToID(Game1.GetOfficeScene().Cameras.CurrentCamNum)) && (Game1.GetOfficeScene().Cameras.GetState() == CamState.UP);
                else // Special case for if an animatronic is on stage
                    return (Game1.GetOfficeScene().Cameras.CurrentCamNum == 3) && (Game1.GetOfficeScene().Cameras.GetState() == CamState.UP);
            }
            else
                return false;
        }

        // Creates camera and jumpscare sprites
        protected void CreateSprite()
        {
            if (HasJumpscare)
            {
                // Jumpscare sprite
                JumpscareSprite = new SpriteItem("Jumpscare/" + Name);
                JumpscareSprite.ZIndex = 10;
                JumpscareSprite.dp.Scale = new Vector2(4);
                JumpscareSprite.Visible = false;
            }

            // Creates the sprite for the animatronic when on cams
            CamSprite = new AnimatedSprite();
            CamSprite.ZIndex = 3;
            CamSprite.dp.Scale = new Vector2(4);

            foreach (int room in VisibleRooms)
            {
                CamSprite.SetAnimation(room.ToString(), new AnimationData(new string[] { "CamAnim/" + Name + "/" + room }));
            }
        }

        public abstract void Draw(GameTime gameTime);

        public abstract void Initialize();

        public abstract void LoadContent();

        public abstract void Update(GameTime gameTime);

        public abstract void DisposeTimers();

        public static void DisposeAllTimers()
        {
            foreach (Animatronic animatronic in AnimatronicDict.Values)
            {
                animatronic.DisposeTimers();
            }
        }

        public void Jumpscare()
        {
            if (!(IsJumpscaring || Game1.GetOfficeScene().IsJumpscared))
            {
                IsJumpscaring = true;
                JumpSound.Play();
                JumpscareSprite.Visible = true;
                Jumpscared?.Invoke();

                SceneSwitchTimer.Elapsed += SwitchScene;
                SceneSwitchTimer.AutoReset = false;
                SceneSwitchTimer.Start();
            }
        }

        private void SwitchScene()
        {
            Game1.CurrentGame.RequestChangeScene(Scenes.NIGHTLOSE);
        }

        private void SwitchScene(object sender, ElapsedEventArgs e)
        {
            SceneSwitchTimer.Dispose();
            SwitchScene();
        }

        // Mathematical formula that returns a reasonable time to wait based off difficulty. Difficulty can go below 1 or above 20 || NOTE: formula on desmos, put into document
        public static float GetTime(float TimeAt20, int Difficulty)
        {
            return 25 * TimeAt20 / (MathF.Exp(Difficulty * MathF.Log(20) / 20) + 5);
        }

        // Same formula, but with an offset value. Time returned can not go below this value regardless of difficulty
        public static float GetTime(float TimeAt20, int Difficulty, float Offset)
        {
            // Offset can't be larger than the time at 20 difficulty
            if (Offset > TimeAt20) Offset = TimeAt20;

            return 25 * TimeAt20 / (MathF.Exp(Difficulty * MathF.Log((20 * TimeAt20 + 5 * Offset) / (TimeAt20 - Offset)) / 20) + 5);
        }

        // Same formula, the offset is only applied once a cut-off difficulty is reached
        public static float GetTime(float TimeAt20, int Difficulty, float Offset, bool CutoffBefore20)
        {
            // Offset can't be larger than the time at 20 difficulty
            if (Offset > TimeAt20) Offset = TimeAt20;

            if (!CutoffBefore20)
            {
                if (Difficulty <= 20)
                    return 25 * TimeAt20 / (MathF.Exp(Difficulty * MathF.Log(20) / 20) + 5);
                else
                    return 25 * TimeAt20 / (MathF.Exp(Difficulty * MathF.Log((20 * TimeAt20 + 5 * Offset) / (TimeAt20 - Offset)) / 20) + 5);
            }
            else
            {
                float Value1 = 25 * TimeAt20 / (MathF.Exp(Difficulty * MathF.Log(20) / 20) + 5);
                float Value2 = 25 * TimeAt20 / (MathF.Exp(Difficulty * MathF.Log((20 * TimeAt20 + 5 * Offset) / (TimeAt20 - Offset)) / 20) + 5);

                if (Difficulty > 20)
                    return Value2;
                else
                    return MathF.Min(Value1, Value2);
            }
        }
    }
}
