using System;

using NAudio.Wave;

//install Naudio from Manage Nuget packages
namespace TextToSPeechApp
{
    class ProgramTTS
    {
        public static Test test = new Test();
        public static string path = "";
        public static void TTSEntry(string speechText)
        {
            //test.TextSpeech().Wait();

            //NetCoreSample netcore = new NetCoreSample();
            //Console.Write("What would you like to convert to Speech? ");
            //string speech = Console.ReadLine();
            string speech = speechText;
            test.TextSpeech(speech).Wait();
            NetCoreSample.Audio(path);
        }

        /*public static void PlaySound()
        {
            wave = new WaveFileReader("C:\\Users\\ogilo\\source\\repos\\TextToSpeech\\sample.wav");
            output = new DirectSoundOut();
            output.Init(new WaveChannel32(wave));
            output.Play();
            Console.WriteLine("\nJust played the sound!");
            if (output.PlaybackState == PlaybackState.Stopped)
            {
                output.Stop();
                output.Dispose();
                //wave.Dispose();
                wave.Close();
                wave = null;
                output = null;
            }
            
        }*/        
    }
}
