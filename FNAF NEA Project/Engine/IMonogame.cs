using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAF_NEA_Project.Engine
{
    // Interface that interacts with Monogame's core procedures
    // Implement on objects that utilise these so that they only have to be called once in the class itself,
    // rather then for every instance of a class
    public interface IMonogame
    {
        void Initialize();

        void LoadContent();

        void Update(GameTime gameTime);

        void Draw(GameTime gameTime);
    }
}
