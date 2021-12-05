using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Speech.Synthesis;

namespace ITS
{
    public class ITSSpeech
    {
        private static SpeechSynthesizer _synth = new SpeechSynthesizer();
        static ITSSpeech()
        {
            _synth.SetOutputToDefaultAudioDevice();
        }

        public static void Speak(string str)
        {
            _synth.SpeakAsync(str);
        }

        public static void Pause()
        {
            _synth.Pause();
        }
    }
}
