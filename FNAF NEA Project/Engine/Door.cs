using Microsoft.Xna.Framework;
using NEA_Project.Engine;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace FNAF_NEA_Project.Engine
{
    public enum DoorState
    {
        OPEN, CLOSING, CLOSED, OPENING
    }

    public enum DoorSide
    {
        LEFT, RIGHT
    }

    public class Door : IMonogame
    {
        private DoorState State = DoorState.OPEN;
        private DoorSide Side;
        private AnimatedSprite DoorSprite;
        private Button DoorButton = new Button(new Rectangle(384, 192, 64, 64));
        private ScrollSprite ButtonClosedSprite;
        private ScrollSprite ButtonOpenSprite;
        private ScrollSprite ButtonInactiveSprite;
        private static Dictionary<DoorSide, bool> SideClosed = new Dictionary<DoorSide, bool>() { { DoorSide.LEFT, false }, { DoorSide.RIGHT, false } };
        private ScrollObject Scroll;
        private AudioEffect DoorSound;

        public Door()
        {
            DoorSound = new AudioEffect("Door" + Side.ToString(), "Audio/doorFX");
            MonogameIManager.AddObject(this);
        }

        public Door(DoorSide Side)
        {
            this.Side = Side;
            DoorSound = new AudioEffect("Door" + Side.ToString(), "Audio/doorFX");
            MonogameIManager.AddObject(this);
        }

        public void Draw(GameTime gameTime)
        {
            DoorSprite.Update(gameTime);
            DoorSprite.dp.Pos = GetDoorPos() + new Vector2(Scroll.GetScrollAmount(), 0);
            DrawManager.EnqueueItem(DoorSprite);
            DrawManager.EnqueueItem(ButtonClosedSprite);
            DrawManager.EnqueueItem(ButtonOpenSprite);
            DrawManager.EnqueueItem(ButtonInactiveSprite);
        }

        public void Initialize()
        {
            DoorButton.MousePressed += event_DoorButtonPressed;
        }

        public void LoadContent()
        {
            // Creates the animated door sprite
            DoorSprite = new AnimatedSprite("door" + Side, new AnimationData("Door/Close", 6));
            DoorSprite.ZIndex = 2;
            DoorSprite.dp.Scale = new Vector2(4);
            DoorSprite.AnimationFinished += event_DoorAnimFinished;

            Scroll = ScrollObject.GetList()["Scroll"];

            // Button Sprites
            ButtonClosedSprite = new ScrollSprite("DoorButtonClosed", "Scroll");
            ButtonClosedSprite.dp.Pos = GetButtonPos();
            ButtonClosedSprite.dp.Scale = new Vector2(4);
            ButtonClosedSprite.ZIndex = 2;
            ButtonClosedSprite.Visible = false;

            ButtonOpenSprite = new ScrollSprite("DoorButtonOpen", "Scroll");
            ButtonOpenSprite.dp.Pos = GetButtonPos();
            ButtonOpenSprite.dp.Scale = new Vector2(4);
            ButtonOpenSprite.ZIndex = 2;

            ButtonInactiveSprite = new ScrollSprite("DoorButtonInactive", "Scroll");
            ButtonInactiveSprite.dp.Pos = GetButtonPos();
            ButtonInactiveSprite.dp.Scale = new Vector2(4);
            ButtonInactiveSprite.ZIndex = 2;
            ButtonInactiveSprite.Visible = false;

            // Audio
            DoorSound.SetVolume(0.35f);
        }

        public void Update(GameTime gameTime)
        {
            DoorButton.SetPos(GetButtonPos() + new Vector2(Scroll.GetScrollAmount(), 0));
            if (!Power.PowerOut)
            {
                if (DoorButton.GetActive() == Cameras.IsUsing())
                    DoorButton.SetActive(!Cameras.IsUsing());
            }
        }

        public static bool IsSideClosed(DoorSide side)
        {
            return SideClosed[side];
        }

        private void event_DoorAnimFinished()
        {
            if (State == DoorState.OPENING)
            {
                State = DoorState.OPEN;
            }
            else if (State == DoorState.CLOSING)
            {
                State = DoorState.CLOSED;
            }
        }

        private void event_DoorButtonPressed()
        {
            // Door State
            if (State == DoorState.OPEN || State == DoorState.OPENING)
            {
                State = DoorState.CLOSING;
                DoorSprite.PlayForwards();
                SideClosed[Side] = true;
            }
            else
            {
                State = DoorState.OPENING;
                DoorSprite.PlayReversed();
                SideClosed[Side] = false;
            }

            // Button Sprite
            ButtonClosedSprite.Visible = State == DoorState.CLOSED || State == DoorState.CLOSING;
            ButtonOpenSprite.Visible = State == DoorState.OPEN || State == DoorState.OPENING;

            // Power
            switch (Side)
            {
                case DoorSide.LEFT:
                    Power.GlobalPower.SetToolStatus(Tools.LEFT_DOOR, State == DoorState.CLOSED || State == DoorState.CLOSING);
                    break;
                case DoorSide.RIGHT:
                    Power.GlobalPower.SetToolStatus(Tools.RIGHT_DOOR, State == DoorState.CLOSED || State == DoorState.CLOSING);
                    break;
            }

            Power.GlobalPower.CalculateUsage();

            // Audio
            DoorSound.Play();
        }

        private Vector2 GetDoorPos()
        {
            if (Side == DoorSide.LEFT)
                return new Vector2(384, 192);
            else
                return new Vector2(1632, 192);
        }

        private Vector2 GetButtonPos()
        {
            if (Side == DoorSide.LEFT)
                return new Vector2(704, 320);
            else
                return new Vector2(1536, 320);
        }

        public void PowerOutage()
        {
            if (State == DoorState.CLOSED || State == DoorState.CLOSING)
            {
                event_DoorButtonPressed();
            }
            DoorButton.SetActive(false);
            ButtonClosedSprite.Visible = false;
            ButtonOpenSprite.Visible = false;
            ButtonInactiveSprite.Visible = true;
        }
    }
}
