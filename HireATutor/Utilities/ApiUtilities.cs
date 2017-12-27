using HireATutor.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace HireATutor.Utilities
{
    public static class ApiUtilities
    {
        public static HttpResponseMessage PerformApiCall<T>(int id = -1)
        {
            System.Diagnostics.Debug.WriteLine("Called...");

            using (System.Threading.ExecutionContext.SuppressFlow())
            {
                using (var client = new HttpClient(new HttpServer(GlobalConfiguration.DefaultHandler)))
                {
                    client.BaseAddress = new Uri("http://localhost:1533/");

                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    Task<HttpResponseMessage> requestTask = null;
                    Task<HttpResponseMessage> responseTask = null;
                    HttpResponseMessage response = null;
                    string apiEndpoint = "api/";

                    System.Diagnostics.Debug.WriteLine("Type is " + typeof(T).Name.ToLower() + "... ID" + id);
                    apiEndpoint += pluralizeObj(typeof(T).Name.ToLower()) + "/";

                    if (id > -1 && typeof(T) != typeof(Instructor))
                    {
                        apiEndpoint += id.ToString();
                    }

                    requestTask = client.GetAsync(apiEndpoint);
                    System.Diagnostics.Debug.WriteLine("Request sent to " + apiEndpoint);

                    responseTask = Task.Run(() => requestTask);
                    response = responseTask.Result;

                    if (response.IsSuccessStatusCode)
                    {
                        System.Diagnostics.Debug.WriteLine("Retrieved object...");
                        var jsonResponse = response.Content.ReadAsStringAsync().Result;

                        if (id > -1)
                        {
                            // T retObj = JsonConvert.DeserializeObject<T>(jsonResponse);
                            response.Content = new ObjectContent(typeof(T), JsonConvert.DeserializeObject(jsonResponse, typeof(T)), new JsonMediaTypeFormatter());
                        }
                        else
                        {
                            // List<T> retObj = JsonConvert.DeserializeObject<List<T>>(jsonResponse);
                            response.Content = new ObjectContent(typeof(List<T>), JsonConvert.DeserializeObject(jsonResponse, typeof(List<T>)), new JsonMediaTypeFormatter());
                        }

                    }

                    System.Diagnostics.Debug.WriteLine("Returning response...");
                    return response;
                }
            }
        }

        public static string pluralizeObj(string objName)
        {
            switch (objName)
            {
                case "category":
                    return "categories";
                default:
                    return objName + "s";
            }
        }
    }
}