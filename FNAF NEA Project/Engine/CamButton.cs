using Microsoft.Xna.Framework;
using NEA_Project.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAF_NEA_Project.Engine
{
    public class CamButton : IMonogame
    {
        private int Num;
        private Vector2 Pos;
        private Button Button;
        private RectItem Rect;
        private TextItem Text;

        public event NotifyInt ButtonPressed;

        public CamButton()
        {
            Button = new Button(new Rectangle((int)Pos.X, (int)Pos.Y, 64, 48), false);
            Button.MousePressed += event_ButtonPressed;
            MonogameIManager.AddObject(this);
        }

        public CamButton(int Num)
        {
            this.Num = Num;
            Button = new Button(new Rectangle((int)Pos.X, (int)Pos.Y, 64, 48), false);
            Button.MousePressed += event_ButtonPressed;
            MonogameIManager.AddObject(this);
        }

        public CamButton(int Num, Vector2 Pos)
        {
            this.Num = Num;
            this.Pos = Pos;
            Button = new Button(new Rectangle((int)Pos.X, (int)Pos.Y, 64, 48), false);
            Button.MousePressed += event_ButtonPressed;
            MonogameIManager.AddObject(this);
        }

        public void Draw(GameTime gameTime)
        {
            DrawManager.EnqueueItem(Rect);
            DrawManager.EnqueueItem(Text);
        }

        public void Initialize()
        {
        }

        public void LoadContent()
        {
            Text = new TextItem("DefaultFont", string.Format("{0:00}", Num));
            Text.dp.Pos = Pos + new Vector2(16, 8);
            Text.ZIndex = 4;
            Text.dp.Scale = new Vector2(1f / 3f);
            Text.Visible = false;

            Rect = new RectItem(64, 48);
            Rect.dp.Pos = Pos;
            Rect.dp.Colour = Color.Gray;
            Rect.ZIndex = 4;
            Rect.Visible = false;
        }

        // Calculates new power
        public void Update(GameTime gameTime)
        {

        }

        public void SetVisible(bool value)
        {
            Text.Visible = value;
            Rect.Visible = value;
            Button.SetActive(value);
        }

        public void event_ButtonPressed()
        {
            ButtonPressed?.Invoke(Num);
        }
    }
}
