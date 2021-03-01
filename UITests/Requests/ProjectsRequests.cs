using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UITests.Requests
{
    public static class ProjectsRequests
    {
        public static void CreateProject(string projectName)
        {
            RestRequest request = new RestRequest("/project", Method.POST);
            AuthorizationRequests.LoginUser(Utilities.TestUserEmail, Utilities.TestUserPassword);
            Clients.AddAuthorizationToken(request);

            JObject requestJObject = new JObject();
            requestJObject.Add("name", projectName);

            request.RequestFormat = DataFormat.Json;
            request.AddParameter("application/json", requestJObject, ParameterType.RequestBody);

            Clients.BackendClient.Execute(request);
        }
    }
}
