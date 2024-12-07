using Microsoft.Xna.Framework;
using NEA_Project.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAF_NEA_Project.Engine
{
    public class SingleCam : IMonogame
    {
        private int Num;
        private Vector2 Pos;
        private Button Button;
        private TextItem Text;

        public SingleCam()
        {
            MonogameIManager.AddObject(this);
        }

        public SingleCam(int Num)
        {
            this.Num = Num;
            MonogameIManager.AddObject(this);
        }

        public SingleCam(int Num, Vector2 Pos)
        {
            this.Num = Num;
            this.Pos = Pos;
            MonogameIManager.AddObject(this);
        }

        public void Draw(GameTime gameTime)
        {
            DrawManager.EnqueueItem(Text);
        }

        public void Initialize()
        {
        }

        public void LoadContent()
        {
            Text = new TextItem("DefaultFont", string.Format("{0:00}", Num));
            Text.dp.Pos = Pos;
            Text.ZIndex = 4;
            Text.dp.Scale = new Vector2(1f / 3f);
        }

        // Calculates new power
        public void Update(GameTime gameTime)
        {

        }
    }
}
