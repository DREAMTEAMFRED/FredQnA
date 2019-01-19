using System;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;


namespace SpeechToTextApp
{
    class ProgramSTT
    {
        public static string speech = "";
        public static bool proceed = false;

        public async Task RecognizeSpeechAsync()
        {
            // Creates an instance of a speech config with specified subscription key and service region.
            // Replace with your own subscription key // and service region (e.g., "westus").
            var config = SpeechConfig.FromSubscription(Environment.GetEnvironmentVariable("azure_STT_Key", EnvironmentVariableTarget.User), "westus");

            // Creates a speech recognizer.
            using (var recognizer = new SpeechRecognizer(config))
            {
                while(proceed == false)
                {
                    Console.WriteLine("Say something...");

                    // Performs recognition. RecognizeOnceAsync() returns when the first utterance has been recognized,
                    // so it is suitable only for single shot recognition like command or query. For long-running
                    // recognition, use StartContinuousRecognitionAsync() instead.
                    var result = await recognizer.RecognizeOnceAsync().ConfigureAwait(false);

                    // Checks result.
                    if (result.Reason == ResultReason.RecognizedSpeech)
                    {
                        speech = result.Text;
                        proceed = true;
                        //Console.WriteLine($"We recognized: {result.Text}");
                    }
                    else if (result.Reason == ResultReason.NoMatch)
                    {
                        Console.WriteLine($"NOMATCH: Speech could not be recognized.");
                        proceed = false;
                    }
                    else if (result.Reason == ResultReason.Canceled)
                    {
                        var cancellation = CancellationDetails.FromResult(result);
                        Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                        if (cancellation.Reason == CancellationReason.Error)
                        {
                            Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                            Console.WriteLine($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
                            Console.WriteLine($"CANCELED: Did you update the subscription info?");
                        }

                        proceed = false;
                    }
                }
                proceed = false;
            }
        }

        /*static void Main()
        {
            RecognizeSpeechAsync().Wait();
            Console.WriteLine("Please press a key to continue.");
            Console.ReadLine();
        }*/
    }
}
