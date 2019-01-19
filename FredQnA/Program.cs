using MovieMarvel;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace FredQnA
{
    class Program
    {
        public static HttpClient client = new HttpClient();
        static string appKey = Environment.GetEnvironmentVariable("Wolfram_App_Key", EnvironmentVariableTarget.User);
        static string wolframText = "";
      
        static void Main(string[] args)
        {
            Console.WriteLine("Enter a search:");
            string search = Console.ReadLine();
            GetAnswer(search).Wait();
        }

        public static async Task GetAnswer(string search)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue
                ("applicationException/json"));

            // grab 20 vids
            HttpResponseMessage response = await client.GetAsync("https://api.wolframalpha.com/v1/result?i=" + search + "&appid=" + appKey);

            if (response.IsSuccessStatusCode)
            {
                string Data = await response.Content.ReadAsStringAsync();
                wolframText = Data.Split('.')[0];
                //Console.WriteLine(wolframText);
                if (wolframText.Length < 15)
                {
                    AskQuestion(search).Wait();
                }
                else
                {
                    Console.WriteLine(wolframText);
                }
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("please rephrase your question");
                string newSearch = Console.ReadLine();
                await GetAnswer(newSearch);
            }
        }

        private static async Task AskQuestion(string question)
        {
            string newText = "";

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue
                ("applicationException/json"));

            HttpResponseMessage response = await client.GetAsync("http://api.duckduckgo.com/?q=" + question + "&format=json");

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
                    Console.WriteLine(addStr);
                }
                else
                {
                    if (texts.Count == 0)
                    {
                        string data = ""; //this is so that if the search field is empty it does not show what was last searched
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
