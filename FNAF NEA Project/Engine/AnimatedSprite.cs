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

        // Either sets the current animation data,
        // Or if there is none of that name and data is provided, creates new animation data and sets current animation to that
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

        // Update logic, allows animations to play
        public void Update(GameTime gameTime)
        {
            // Logic for going through frames
            if (CurrentAnimation != null && Playing)
            {
                // Adds to time
                Time += gameTime.ElapsedGameTime.TotalSeconds;

                // If time is above 1 / FPS, progress to next frame
                if (Time >= 1d / CurrentAnimation.FPS)
                {
                    Time = 0;

                    // If playing backswards, subtracts a frame and checks if animation is done
                    if (PlayBackwards)
                    {
                        if (Frame > 0) Frame--;
                        else if (CurrentAnimation.DoesLoop) // Loops
                        {
                            Playing = true;
                            Frame = CurrentAnimation.GetNumOfFrames() - 1;
                            AnimationLooped?.Invoke();
                        }
                        else // Stops playing
                        {
                            Playing = false;
                            AnimationFinished?.Invoke();
                        }
                    }
                    // Otherwise, add a frame and checks if animation is done
                    else
                    {
                        if (Frame < CurrentAnimation.GetNumOfFrames() - 1) Frame++;
                        else if (CurrentAnimation.DoesLoop) // Loops
                        {
                            Playing = true;
                            Frame = 0;
                            AnimationLooped?.Invoke();
                        }
                        else // Stops playing
                        {
                            Playing = false;
                            AnimationFinished?.Invoke();
                        }
                    }
                }
            }
        }

        // Sets if playing without touching other variables
        public void SetPlaying(bool playing)
        {
            Playing = playing;
        }

        // Plays from the start (or end if playing backwards)
        public void Play()
        {
            Playing = true;
            Time = 0;
            if (PlayBackwards && CurrentAnimation != null) Frame = CurrentAnimation.GetNumOfFrames() - 1;
            else Frame = 0;
        }

        // Plays forward from the start
        public void PlayForwards()
        {
            Playing = true;
            Time = 0;
            Frame = 0;
            PlayBackwards = false;
        }

        // Plays backwards from the end
        public void PlayReversed()
        {
            Playing = true;
            Time = 0;
            if (CurrentAnimation != null) Frame = CurrentAnimation.GetNumOfFrames() - 1;
            PlayBackwards = true;
        }

        // Stops animation, but doesn't set the frame
        public void Stop()
        {
            Playing = false;
            Time = 0;
        }

        // Stops animation and sets frame to the start (or end if playing backwards)
        public void Reset(bool ResetToStart)
        {
            Playing = false;
            Time = 0;
            if (ResetToStart) Frame = 0;
            else if (CurrentAnimation != null) Frame = CurrentAnimation.GetNumOfFrames() - 1;
        }

        // Returns playing
        public bool IsPlaying()
        {
            return Playing;
        }
    }
}
