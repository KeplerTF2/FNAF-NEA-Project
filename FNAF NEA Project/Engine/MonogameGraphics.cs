using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEA_Project.Engine
{
    public static class MonogameGraphics
    {
        public static GraphicsDeviceManager _graphics = null;
        public static SpriteBatch _spriteBatch;
        public static ContentManager? _content = null;

        public static void SetGraphicsDeviceManager(GraphicsDeviceManager graphics)
        {
            _graphics = graphics;
        }

        public static void SetSpriteBatch(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
        }

        public static void SetContent(ContentManager content)
        {
            _content = content;
        }
    }
}
