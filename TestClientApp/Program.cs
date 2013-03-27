using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Runtime.Serialization.Json;
using ResearchLinks.Data.Models;

namespace TestClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new HttpClient();
            var buffer = Encoding.ASCII.GetBytes("james:james2013");
            var authHeader = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(buffer));
            client.DefaultRequestHeaders.Authorization = authHeader;
            var task = client.GetAsync("http://localhost:55301/api/projects").ContinueWith(
                (requestTask) =>
                {
                    if (requestTask.IsFaulted)
                    {
                        var ex = requestTask.Exception;
                        Console.WriteLine("Request Exception: " + ex.Message);
                    }

                    else if (requestTask.IsCanceled)
                    {
                        Console.WriteLine("request cancelled");
                    }
                    else
                    {
                        HttpResponseMessage response = requestTask.Result;

                        if (response.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            Console.WriteLine("wrong credentials");
                        }
                        else
                        {
                            response.EnsureSuccessStatusCode();
                            response.Content.ReadAsAsync<List<Project>>().ContinueWith(
                                (read) =>
                                {
                                    var projects = read.Result;
                                    foreach (var project in projects)
                                    {
                                        Console.WriteLine(project.Name);
                                    }
                                }
                            );
                        }
                    }
                    client.Dispose();
                }
            );
                

            Console.WriteLine("Enter to exit...");
            Console.ReadLine();
        }
    }
}
