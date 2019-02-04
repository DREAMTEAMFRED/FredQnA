using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using MovieMarvel;
using RecordAudio;
using System.Threading;
using System.Threading.Tasks;

namespace RestSTT
{
    class ProgramRestSTT
    {
        public static string text = "";
        public static int noSpeech = 0;
        public async Task SpeechToText()
        {
            /*if ((args.Length < 2) || (string.IsNullOrWhiteSpace(args[0])))
            {
                Console.WriteLine("Arg[0]: Specify the endpoint to hit https://speech.platform.bing.com/recognize");
                Console.WriteLine("Arg[1]: Specify a valid input wav file.");

                return;
            }*/

            await ProgramREC.Record();
            Thread.Sleep(5000);
            await ProgramREC.StopRecording();


            // Note: Sign up at https://azure.microsoft.com/en-us/try/cognitive-services/ to get a subscription key.  
            // Navigate to the Speech tab and select Bing Speech API. Use the subscription key as Client secret below.
            AuthenticationSTT auth = new AuthenticationSTT(Environment.GetEnvironmentVariable("azure_STT_Key", EnvironmentVariableTarget.User));

            string requestUri = "https://westus.stt.speech.microsoft.com/speech/recognition/conversation/cognitiveservices/v1?language=en-US"; //args[0];/*.Trim(new char[] { '/', '?' });*/

            string host = @"westus.stt.speech.microsoft.com";
            string contentType = @"audio/wav; codec=""audio/pcm""; samplerate=16000";
            List<string> texts;

            /*
             * Input your own audio file or use read from a microphone stream directly.
             */
            string curDir = Directory.GetCurrentDirectory();
            //string audioFile = curDir + "\\test.wav"; //args[1];
            string responseString;
            FileStream fs = null;

            try
            {
                var token = auth.GetAccessToken();
                //Console.WriteLine("Token: {0}\n", token);
                //Console.WriteLine("Request Uri: " + requestUri + Environment.NewLine);

                HttpWebRequest request = null;
                request = (HttpWebRequest)HttpWebRequest.Create(requestUri);
                request.SendChunked = true;
                request.Accept = @"application/json;text/xml";
                request.Method = "POST";
                request.ProtocolVersion = HttpVersion.Version11;
                request.Host = host;
                request.ContentType = contentType;
                request.Headers["Authorization"] = "Bearer " + token;

                int trial = 0;
                while(trial < 2)
                {
                    using (fs = new FileStream(@"record.wav", FileMode.Open, FileAccess.Read))
                    {

                        /*
                         * Open a request stream and write 1024 byte chunks in the stream one at a time.
                         */
                        byte[] buffer = null;
                        int bytesRead = 0;
                        using (Stream requestStream = request.GetRequestStream())
                        {
                            /*
                             * Read 1024 raw bytes from the input audio file.
                             */
                            buffer = new Byte[checked((uint)Math.Min(1024, (int)fs.Length))];
                            while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                requestStream.Write(buffer, 0, bytesRead);
                            }

                            // Flush
                            requestStream.Flush();
                        }
                    }
                    trial++;
                }
                
                /*
                     * Get the response from the service.
                     */
                //Console.WriteLine("Response:");
                using (WebResponse response = request.GetResponse())
                {
                    //Console.WriteLine(((HttpWebResponse)response).StatusCode);

                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        responseString = sr.ReadToEnd();

                        JsonNinja jninja = new JsonNinja(responseString);

                        texts = jninja.GetDetails("\"DisplayText\"");
                    }

                    Console.WriteLine(texts[0]);
                    text = texts[0];
                    //Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.ToString());
                //Console.WriteLine(ex.Message);
                Console.WriteLine("Nothing recorded...");
                text = "Nothing recorded";
                //Console.ReadLine();
            }
        }
    }
}
