using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable

namespace NEA_Project.Engine
{
    public static class TextureManager
    {
        private static ContentManager? content = null;
        private static Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();

        public static void SetContentManager(ContentManager _content)
        {
            content = _content;
        }

        public static bool AddTexture(string name)
        {
            if (content == null) { return false; }
            if (Textures.ContainsKey(name)) { return false; }

            Texture2D texture = content.Load<Texture2D>(name);
            Textures.Add(name, texture);
            return true;
        }

        public static bool HasTexture(string name)
        {
            return Textures.ContainsKey(name);
        }

        public static Texture2D? GetTexture(string name)
        {
            if (!HasTexture(name)) { return null; }

            return Textures[name];
        }

        public static void ResetTextures()
        {
            Textures = new Dictionary<string, Texture2D>();
        }
    }
}
