using FredKB;
using MovieMarvel;
using NAudio.Wave;
using SpeechToTextApp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using TextToSPeechApp;

namespace FredQnA
{
    class Program
    {
        public static HttpClient client = new HttpClient();
        static string appKey = Environment.GetEnvironmentVariable("Wolfram_App_Key", EnvironmentVariableTarget.User);
        static string wolframText = "";
        public static ProgramSTT speech = new ProgramSTT();
        public static string word = "";
        public static string access = "*";
        public static bool proceed = true;
        public static bool test = false;
        public static string hiFred = "hello fred, hi fred, hey fred,";

        static void Main(string[] args)
        {
            while (true)
            {
                while(test == false)
                {
                    access = ProgramSTT.speech.ToLower();
                    if (!hiFred.Contains(access))
                    {
                        ProgramSTT.welcome = "hello fred";
                        speech.RecognizeSpeechAsync().Wait();
                        access = ProgramSTT.speech.ToLower();
                        proceed = true;
                    }
                    while (hiFred.Contains(access))
                    {
                        if(proceed)
                        {
                            ProgramTTS.TTSEntry("Hello!");
                            /*using (var fileStream = new FileStream(@"beep2.wav", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                            {
                                string path = fileStream.Name;
                                NetCoreSample.Audio(path);
                            }*/
                            proceed = false;
                        }
                        speech.RecognizeSpeechAsync().Wait();
                        word = ProgramSTT.speech;
                        access = word;
                        if(word.Equals("*"))
                        {
                            // do  nothing!
                        }
                        else
                        {
                            wolframText = ProgramKB.FredKB(word);

                            if(wolframText == "")
                            {
                                GetAnswer(word).Wait();
                            }
                            else
                            {
                                test = true;
                            }
                        }
                        //test = true;
                    }
                }
                

                //Console.WriteLine("Enter a search:");
                //string search = Console.ReadLine();

                ProgramTTS.TTSEntry(wolframText);
                test = false;
                ProgramSTT.speech = "hello fred"; // use a getter and setter!
            }
        }

        public static async Task GetAnswer(string search)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue
                ("applicationException/json"));

            // grab 20 vids
            HttpResponseMessage response = await client.GetAsync($"https://api.wolframalpha.com/v1/result?i= {search}&appid={appKey}");

            if (response.IsSuccessStatusCode)
            {
                test = true;
                string Data = await response.Content.ReadAsStringAsync();
                wolframText = Data.Split(". ")[0];
                //Console.WriteLine(wolframText);
                if (wolframText.Length < 10)
                {
                    AskQuestion(search).Wait();
                }
                else
                {
                    Console.WriteLine(wolframText);
                }
                //Console.ReadLine();
            }
            else
            {
                //Console.WriteLine("please rephrase your question");
                ProgramTTS.TTSEntry("please rephrase your question");
                test = false;
                ProgramSTT.speech = "hello fred";
                //string newSearch = Console.ReadLine();
                //speech.RecognizeSpeechAsync().Wait();
                //await GetAnswer(newSearch);
            }
        }

        private static async Task AskQuestion(string question)
        {
            string newText = "";

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue
                ("applicationException/json"));

            HttpResponseMessage response = await client.GetAsync($"http://api.duckduckgo.com/?q= {question} &format=json");

            if (response.IsSuccessStatusCode)
            {
                string Data = await response.Content.ReadAsStringAsync();
                JsonNinja ninja = new JsonNinja(Data);
                List<string> answer = ninja.GetDetails("\"Abstract\"");
                List<string> rTopics = ninja.GetDetails("\"RelatedTopics\"");
                JsonNinja ninji = new JsonNinja(rTopics[0]);
                List<string> texts = ninji.GetDetails("\"Text\"");
                //Console.WriteLine("Answer: \n");
                if (answer[0] != "")
                {
                    string addStr = answer[0].Split('.')[0];
                    wolframText += "\n" + addStr;
                    Console.WriteLine(wolframText);
                }
                else
                {
                    if (texts.Count == 0)
                    {
                        //string data = ""; //this is so that if the search field is empty it does not show what was last searched
                        Console.WriteLine(wolframText);
                    }
                    else
                    {
                        int count = 1;
                        wolframText += "\nFound " + texts.Count + " other result(s)";
                        //Console.WriteLine("Found " + texts.Count + " results");
                        foreach (string text in texts)
                        {
                            newText += count + ": " + text.Split('.')[0].Replace("\\", "") + "\n";
                            //Console.WriteLine(count + ": " + newText + "\n");
                            count++;
                        }
                        wolframText += "\n" + newText;
                        Console.WriteLine(wolframText);
                    }
                }
            }
            else
            {
                string Data = ""; //this is so that if the search field is empty it does not show what was last searched
                Console.WriteLine(Data);
            }
        }
    }
}