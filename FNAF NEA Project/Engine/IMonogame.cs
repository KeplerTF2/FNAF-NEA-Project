using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAF_NEA_Project.Engine
{
    public interface IMonogame
    {
        // Interface that interacts with Monogame's core procedures

        void Initialize();

        void LoadContent();

        void Update(GameTime gameTime);

        void Draw(GameTime gameTime);
    }
}
