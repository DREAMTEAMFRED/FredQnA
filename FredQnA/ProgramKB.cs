using System;
using System.Collections.Generic;
using System.Json;
using System.Net.Http;
using System.Text;

namespace FredKB
{
    class ProgramKB
    {
        public static string FredKB(string quest)
        {
            // Represents the various elements used to create HTTP request URIs
            // for QnA Maker operations.
            // From Publish Page: HOST
            // Example: https://YOUR-RESOURCE-NAME.azurewebsites.net/qnamaker
            string host = "https://qnafred.azurewebsites.net/qnamaker";

            // Authorization endpoint key
            // From Publish Page
            string endpoint_key = Environment.GetEnvironmentVariable("azure_KB_key", EnvironmentVariableTarget.User);

            // Management APIs postpend the version to the route
            // From Publish Page, value after POST
            // Example: /knowledgebases/ZZZ15f8c-d01b-4698-a2de-85b0dbf3358c/generateAnswer
            string route = "/knowledgebases/85e578a7-bf44-4f12-b46e-26efa40b1653/generateAnswer";

            //string quest = Console.ReadLine();
            quest = quest.Replace("'", "");

            // JSON format for passing question to service
            string question = @"{'question': '" + quest + "','top': 1}";

            string answer = "";
            double rating;
            //int scores;

            // Create http client
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                // POST method
                request.Method = HttpMethod.Post;

                // Add host + service to get full URI
                request.RequestUri = new Uri(host + route);

                // set question
                request.Content = new StringContent(question, Encoding.UTF8, "application/json");

                // set authorization
                request.Headers.Add("Authorization", "EndpointKey " + endpoint_key);

                // Send request to Azure service, get response
                var response = client.SendAsync(request).Result;
                var jsonResponse = response.Content.ReadAsStringAsync().Result;

                // Output JSON response
                // Parse json response
                JsonObject jsonDoc = (JsonObject)JsonValue.Parse(jsonResponse);
                try
                {
                    JsonArray jsonArray = (JsonArray)jsonDoc["answers"];

                    foreach (JsonObject obj in jsonArray)
                    {
                        obj.TryGetValue("answer", out JsonValue result);
                        obj.TryGetValue("score", out JsonValue score);
                        rating = Convert.ToDouble(score.ToString());
                        if(rating > 90.0)
                        {
                            answer = result.ToString();
                        }
                        else
                        {
                            answer = "";
                        }
                    }
                    //Console.WriteLine(answer.Replace("\"", ""));
                    return answer;
                }
                catch
                {
                    answer = "";
                    return answer;
                }

                //Console.ReadLine();
            }
        }
    }
}