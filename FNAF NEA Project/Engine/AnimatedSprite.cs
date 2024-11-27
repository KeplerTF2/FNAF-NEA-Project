using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NEA_Project.Engine;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        protected double Time;
        protected bool Playing = false;
        public bool PlayBackwards = false;
        public int Frame;

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
                    if (PlayBackwards)
                    {
                        if (Frame > 0) Frame--;
                        else if (CurrentAnimation.DoesLoop)
                        {
                            Playing = true;
                            Frame = CurrentAnimation.GetNumOfFrames() - 1;
                            AnimationLooped?.Invoke();
                        }
                        else
                        {
                            Playing = false;
                            AnimationFinished?.Invoke();
                        }
                    }
                    else
                    {
                        if (Frame < CurrentAnimation.GetNumOfFrames() - 1) Frame++;
                        else if (CurrentAnimation.DoesLoop)
                        {
                            Playing = true;
                            Frame = 0;
                            AnimationLooped?.Invoke();
                        }
                        else
                        {
                            Playing = false;
                            AnimationFinished?.Invoke();
                        }
                    }
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
            if (PlayBackwards && CurrentAnimation != null) Frame = CurrentAnimation.GetNumOfFrames() - 1;
            else Frame = 0;
        }

        public void PlayForwards()
        {
            Playing = true;
            Time = 0;
            Frame = 0;
            PlayBackwards = false;
        }

        public void PlayReversed()
        {
            Playing = true;
            Time = 0;
            if (CurrentAnimation != null) Frame = CurrentAnimation.GetNumOfFrames() - 1;
            PlayBackwards = true;
        }

        public void Stop()
        {
            Playing = false;
            Time = 0;
        }

        public void Reset(bool ResetToStart)
        {
            Playing = false;
            Time = 0;
            if (ResetToStart) Frame = 0;
            else if (CurrentAnimation != null) Frame = CurrentAnimation.GetNumOfFrames() - 1;
        }

        public bool IsPlaying()
        {
            return Playing;
        }
    }
}
