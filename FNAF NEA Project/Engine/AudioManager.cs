﻿using Microsoft.Xna.Framework.Audio;
using NEA_Project.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// TODO: Add input validation for some procedures

namespace FNAF_NEA_Project.Engine
{
    public static class AudioManager
    {
        private static Dictionary<string, SoundEffect> SoundEffects = new Dictionary<string, SoundEffect>();
        private static Dictionary<string, SoundEffectInstance> SoundEffectInstances = new Dictionary<string, SoundEffectInstance>();

        public static void AddSound(string Name, string Path)
        {
            SoundEffects.TryAdd(Name, MonogameGraphics._content.Load<SoundEffect>(Path));
            SoundEffectInstances.TryAdd(Name, SoundEffects[Name].CreateInstance());
        }

        public static SoundEffectInstance? GetSound(string Name)
        {
            if (SoundEffectInstances.ContainsKey(Name))
                return SoundEffectInstances[Name];
            else
                return null;
        }

        public static void ClearSounds()
        {
            foreach (SoundEffect SoundEffect in SoundEffects.Values)
            {
                SoundEffect.Dispose();
                //SoundEffect.Dispose();
            }

            SoundEffects = new Dictionary<string, SoundEffect>();
            SoundEffectInstances = new Dictionary<string, SoundEffectInstance>();
        }
    }
}
