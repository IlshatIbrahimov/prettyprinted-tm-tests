using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UITests.Requests
{
    public static class AuthorizationRequests
    {
        public static void RegisterUser(string name, string surname, string email, string password)
        {
            RestRequest request = new RestRequest("/register", Method.POST);

            JObject requestJObject = new JObject();
            requestJObject.Add("name", name);
            requestJObject.Add("surname", surname);
            requestJObject.Add("email", email);
            requestJObject.Add("password", password);

            request.RequestFormat = DataFormat.Json;
            request.AddParameter("application/json", requestJObject, ParameterType.RequestBody);

            var response = Clients.BackendClient.Execute(request);

            JObject responseJObject = JObject.Parse(response.Content);
            Clients.UserToken = responseJObject["jwt"].Value<String>();
        }
    }
}
