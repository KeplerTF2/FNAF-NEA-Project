using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using NEA_Project.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

enum QueueState
{
    START, MID, END
}

namespace FNAF_NEA_Project.Engine
{
    public class AudioEffect : IMonogame
    {
        private string Name;
        private string Path;
        private float Volume = 1f;
        private SoundEffectInstance SoundEffectInstance;
        private QueueState QueuedToPlay = QueueState.START; // Used for trying to play a sound whilst it's still playing

        public AudioEffect(string Name)
        {
            this.Name = Name;
            Path = Name;
            MonogameIManager.AddObject(this);
        }

        public AudioEffect(string Name, string Path)
        {
            this.Name = Name;
            this.Path = Path;
            MonogameIManager.AddObject(this);
        }

        public AudioEffect(string Name, string Path, float Volume)
        {
            this.Name = Name;
            this.Path = Path;
            this.Volume = Volume;
            MonogameIManager.AddObject(this);
        }

        public SoundEffectInstance GetInstance()
        {
            return AudioManager.GetSound(Name);
        }

        public void Draw(GameTime gameTime) { }

        public void Initialize() { }

        public void LoadContent()
        {
            AudioManager.AddSound(Name, Path);
            SoundEffectInstance = GetInstance();
            SoundEffectInstance.Volume = Volume;
        }

        public void Update(GameTime gameTime)
        {
            switch (QueuedToPlay)
            {
                case QueueState.MID:
                    QueuedToPlay = QueueState.END;
                    break;
                case QueueState.END:
                    QueuedToPlay = QueueState.START;
                    SoundEffectInstance.Play();
                    break;
            }
        }

        public void Play()
        {
            switch (SoundEffectInstance.State)
            {
                case SoundState.Playing:
                    SoundEffectInstance.Stop();
                    QueuedToPlay = QueueState.MID;
                    break;
                case SoundState.Paused:
                    SoundEffectInstance.Resume();
                    break;
                case SoundState.Stopped:
                    SoundEffectInstance.Play();
                    break;
            }
        }

        public void Play(bool Looped)
        {
            SoundEffectInstance.IsLooped = Looped;
            Play();
        }

        public void Pause()
        {
            SoundEffectInstance.Pause();
        }

        public void Stop()
        {
            SoundEffectInstance.Stop();
        }
        public void Stop(bool Immediate)
        {
            SoundEffectInstance.Stop(Immediate);
        }

        public void SetVolume(float volume)
        {
            SoundEffectInstance.Volume = volume;
        }
    }
}
