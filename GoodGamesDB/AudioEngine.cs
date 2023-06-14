using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace GoodGamesDB
{
    internal static class AudioEngine
    {
        public static void PlaySound(string sound)
        {
            if (Index.MuteSound) return;

            string soundPath = string.Empty;
            switch (sound)
            {
                case "MAIN":
                    soundPath = @"audio\main_click.wav";
                    break;
            }

            if (soundPath != string.Empty)
            {
                SoundPlayer sp = new SoundPlayer(soundPath);
                sp.Play();
            }

        }
    }
}
