using Microsoft.Xna.Framework.Graphics;
using NEA_Project.Engine;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable

namespace FNAF_NEA_Project.Engine
{
    public class AnimationData
    {
        protected string[] Frames;
        protected Texture2D?[] FrameTextures;
        public float FPS;
        public bool DoesLoop;

        public AnimationData(string[] frames)
        {
            Frames = frames;
            FPS = 24;
            DoesLoop = false;
            SetFrameTextures();
        }

        public AnimationData(string[] frames, bool doesLoop)
        {
            Frames = frames;
            FPS = 24;
            DoesLoop = doesLoop;
            SetFrameTextures();
        }

        public AnimationData(string[] frames, float fps)
        {
            Frames = frames;
            FPS = fps;
            DoesLoop = false;
            SetFrameTextures();
        }

        public AnimationData(string[] frames, float fPS, bool doesLoop)
        {
            Frames = frames;
            FPS = fPS;
            DoesLoop = doesLoop;
            SetFrameTextures();
        }

        public AnimationData(string frame, int count)
        {
            SetFrameArray(frame, count);
            FPS = 24;
            DoesLoop = false;
            SetFrameTextures();
        }

        public AnimationData(string frame, int count, bool doesLoop)
        {
            SetFrameArray(frame, count);
            FPS = 24;
            DoesLoop = doesLoop;
            SetFrameTextures();
        }

        public AnimationData(string frame, int count, float fps)
        {
            SetFrameArray(frame, count);
            FPS = fps;
            DoesLoop = false;
            SetFrameTextures();
        }

        public AnimationData(string frame, int count, float fPS, bool doesLoop)
        {
            SetFrameArray(frame, count);
            FPS = fPS;
            DoesLoop = doesLoop;
            SetFrameTextures();
        }

        // Automatically creates an array if the frame names follow the convention: X0, X1, X2 etc
        protected void SetFrameArray(string frame, int count)
        {
            Frames = new string[count];
            for (int i = 0; i < count; i++)
            {
                Frames[i] = frame + i.ToString();
            }
        }

        // Creates and loads all textures to and from the texture manager
        protected void SetFrameTextures()
        {
            if (Frames != null)
            {
                if (FrameTextures == null) FrameTextures = new Texture2D?[Frames.Length];
                for (int i = 0; i < Frames.Length; i++)
                {
                    TextureManager.AddTexture(Frames[i]);
                    FrameTextures[i] = TextureManager.GetTexture(Frames[i]);
                }
            }
        }

        // Returns length of animation in frames
        public int GetNumOfFrames()
        {
            return FrameTextures.Length;
        }

        // Gets a specific frame
        public Texture2D? GetFrameTexture(int frame)
        {
            return FrameTextures[frame];
        }
    }
}
