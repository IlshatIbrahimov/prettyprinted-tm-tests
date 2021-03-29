using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace FuncTests.Requests
{
    public class BackendClient
    {
        public static string backendUrl = "https://prettyprinted-tm-backend.herokuapp.com/";
        public string UserToken { get; set; } = "";
        public RestClient client;

        public BackendClient()
        {
            client = new RestClient()
            {
                FollowRedirects = true,
                CookieContainer = new System.Net.CookieContainer(),
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Safari/537.36"
            };
            client.AddDefaultHeader("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
            client.AddDefaultHeader("Accept-Encoding", "gzip, deflate");
            client.AddDefaultHeader("Accept", "*/*");
        }

        public void AddAuthorizationToken(RestRequest request)
        {
            request.AddHeader("Authorization", $"Bearer {UserToken}");
        }

        public void Authorize(string email, string password)
        {
            RestRequest request = new RestRequest("/auth", Method.POST);

            JObject requestJObject = new JObject
            {
                { "email", email },
                { "password", password }
            };

            request.RequestFormat = DataFormat.Json;
            request.AddParameter("application/json", requestJObject, ParameterType.RequestBody);

            var response = client.Execute(request);

            JObject responseJObject = JObject.Parse(response.Content);
            UserToken = responseJObject["jwt"].Value<String>();
        }

        public void RegisterUser(string name, string surname, string email, string password)
        {
            RestRequest request = new RestRequest("/register", Method.POST);

            JObject requestJObject = new JObject
            {
                { "name", name },
                { "surname", surname },
                { "email", email },
                { "password", password }
            };

            request.RequestFormat = DataFormat.Json;
            request.AddParameter("application/json", requestJObject, ParameterType.RequestBody);

            var response = client.Execute(request);

            JObject responseJObject = JObject.Parse(response.Content);
            UserToken = responseJObject["jwt"].Value<String>();
        }
    }
}
