using NetCoreAudio;
using System;
using System.Threading.Tasks;

namespace RecordAudio
{
    static class ProgramREC
    {
        /*[DllImport("winmm.dll", EntryPoint = "mciSendStringA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int mciSendString(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);*/
        static Player player = new Player();

        public static async Task Record()
        {
            Console.WriteLine("recording for only 10secs....");
            await player.Record();
        }

        public static async Task StopRecording()
        {
            await player.StopRecording();
        }
    }
}