using NetCoreAudio;
using System.Threading.Tasks;
using System;
using System.Threading;

namespace TextToSPeechApp
{
    public class NetCoreSample
    {
        public static void Audio(string path)
        {
            Player player = new Player();
            //player.PlaybackFinished += OnPlaybackFinished;

             //Console.WriteLine("Welcome to the demo of NetCoreAudio package");
            //ShowFileEntryPrompt();
            //var fileName = "C:\\Users\\ogilo\\source\\repos\\TextToSpeech\\sample.wav";
            //ShowInstruction();
            //string command = "play";

                try
                {
                    //Console.WriteLine($"Playing {"C:\\Users\\ogilo\\source\\repos\\TextToSpeech\\sample.wav"}");
                           
                    player.Play(path).Wait();

                    while(player.Playing)
                    {
                        Thread.Sleep(3000);
                    }
                   // if (command == "exit") break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Thread.Sleep(3000);
                }
        }

        /*private static void ShowFileEntryPrompt()
        {
            Console.WriteLine("Please enter the full path to the file you would like to play:");
        }*/

        /*private static void ShowInstruction()
        {
            Console.WriteLine("You can manipulate the player with the following commands:");
            Console.WriteLine("play - Play the specified file from the start");
            Console.WriteLine("pause - Pause the playback");
            Console.WriteLine("resume - Resume the playback");
            Console.WriteLine("stop - Stop the playback");
            Console.WriteLine("change - Change the file name");
            Console.WriteLine("exit - Exit the app");
        }*/

        /*private static void OnPlaybackFinished(object sender, EventArgs e)
        {
            Console.WriteLine("Playback finished");
        }*/
    }
}
