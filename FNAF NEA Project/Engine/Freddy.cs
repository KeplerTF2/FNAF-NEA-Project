using Microsoft.Xna.Framework;
using NEA_Project.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAF_NEA_Project.Engine
{
    public class Freddy : Animatronic
    {
        public Freddy()
        {
            MonogameIManager.AddObject(this);
        }

        public override void Draw(GameTime gameTime)
        {
            if (ShouldDrawCamSprite())
            {
                CamSprite.dp.Pos.X = Cameras.GetScrollAmount();
                DrawManager.EnqueueItem(CamSprite);
            }
        }

        public override void Initialize()
        {
            CurrentRoom = 2;
            Name = "Freddy";
            AnimatronicDict.Add(Name, this);
        }

        public override void LoadContent()
        {
            CreateCamSprite();
            UpdateSprite();
        }

        public override void Update(GameTime gameTime) { }
    }
}
