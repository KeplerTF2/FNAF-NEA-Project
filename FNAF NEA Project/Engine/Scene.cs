using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using FNAF_NEA_Project.Engine;

namespace NEA_Project.Engine
{
    public class Scene : IMonogame
    {
        public virtual void Initialize() { }

        public virtual void LoadContent() 
        {
            TextureManager.ResetTextures();
        }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Draw(GameTime gameTime) { }
    }
}
