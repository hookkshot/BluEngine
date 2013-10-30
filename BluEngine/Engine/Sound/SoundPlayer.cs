using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Audio;

using BluEngine.ScreenManager;

namespace BluEngine.Engine.Sound
{
    public class SoundPlayer
    {
        #region Fields

        private static List<SoundEffectInstance> sounds = new List<SoundEffectInstance>();

        #endregion

        private SoundPlayer() { }

        public static SoundEffectInstance Play(SoundEffect sound, bool loop)
        {
            SoundEffectInstance newSound = sound.CreateInstance();
            newSound.Volume = ScreenManager.ScreenManager.Instance.SoundLevel;
            newSound.IsLooped = loop;
            newSound.Play();

            sounds.Add(newSound);

            return newSound;
        }

        public static SoundEffectInstance Play(SoundEffect sound)
        {
            return Play(sound, false);
        }

        public static void StopAll()
        {
            foreach (SoundEffectInstance item in sounds)
            {
                item.Stop();
            }

            sounds = new List<SoundEffectInstance>();
        }

        public static void Update()
        {
            for (int i = sounds.Count-1; i > -1; i--)
            {
                if (!sounds[i].IsLooped && sounds[i].State == SoundState.Stopped)
                    sounds.RemoveAt(i);
            }
        }
    }
}
