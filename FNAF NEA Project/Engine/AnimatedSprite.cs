using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NEA_Project.Engine;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

# nullable enable

namespace FNAF_NEA_Project.Engine
{
    public class AnimatedSprite : DrawItem
    {
        protected Dictionary<string, AnimationData> Animations = new Dictionary<string, AnimationData>();
        protected string CurrentAnimationName = "";
        protected AnimationData? CurrentAnimation;
        protected int Frame;
        protected double Time;
        protected bool Playing = false;

        public event Notify AnimationFinished;
        public event Notify AnimationLooped;

        public AnimatedSprite() { }

        public AnimatedSprite(Dictionary<string, AnimationData> Animations)
        { 
            this.Animations = Animations;
        }

        public AnimatedSprite(string animationName, AnimationData animation)
        {
            SetAnimation(animationName, animation);
        }

        public void SetAnimation(string animationName)
        {
            if (Animations.ContainsKey(animationName))
            {
                CurrentAnimationName = animationName;
                CurrentAnimation = Animations[animationName];
                Frame = 0;
                Time = 0;
            }
        }

        public void SetAnimation(string animationName, bool play)
        {
            if (Animations.ContainsKey(animationName))
            {
                CurrentAnimationName = animationName;
                CurrentAnimation = Animations[animationName];
                Frame = 0;
                Time = 0;
                Playing = play;
            }
        }

        public void SetAnimation(string animationName, AnimationData animation)
        {
            if (Animations.ContainsKey(animationName))
                Animations[animationName] = animation;
            else
                Animations.Add(animationName, animation);

            CurrentAnimationName = animationName;
            CurrentAnimation = animation;
            Frame = 0;
            Time = 0;
        }

        public void SetAnimation(string animationName, AnimationData animation, bool play)
        {
            if (Animations.ContainsKey(animationName))
                Animations[animationName] = animation;
            else
                Animations.Add(animationName, animation);

            CurrentAnimationName = animationName;
            CurrentAnimation = animation;
            Frame = 0;
            Time = 0;
            Playing = play;
        }


        // What to load
        public override void LoadContent()
        {
        }

        // What to draw
        public override void Draw()
        {
            if (CurrentAnimation != null && Visible)
            {
                if (CurrentAnimation.GetFrameTexture(Frame) != null)
                {
                    Vector2 scale = GlobalCamera.ApplyCameraScale(dp.Scale);
                    Vector2 pos = GlobalCamera.ApplyCameraPosition(dp.Pos);
                    float rot = GlobalCamera.ApplyCameraRotation(dp.Rot);
                    Color colour = GlobalCamera.ApplyCameraColour(dp.Colour);
                    MonogameGraphics._spriteBatch.Draw(CurrentAnimation.GetFrameTexture(Frame), pos, dp.Size, colour, rot, dp.Origin, scale, dp.Effects, 1.0f);
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            // Logic for going through frames
            if (CurrentAnimation != null && Playing)
            {
                Time += gameTime.ElapsedGameTime.TotalSeconds;
                if (Time >= 1f / CurrentAnimation.FPS)
                {
                    Time = 0;
                    if (Frame < CurrentAnimation.GetNumOfFrames() - 1) Frame++;
                    else Playing = CurrentAnimation.DoesLoop;
                }
            }
        }

        public void SetPlaying(bool playing)
        {
            Playing = playing;
        }

        public void Play()
        {
            Playing = true;
            Time = 0;
            Frame = 0;
        }

        public void Stop()
        {
            Playing = false;
            Time = 0;
        }

        public bool IsPlaying()
        {
            return Playing;
        }
    }
}
