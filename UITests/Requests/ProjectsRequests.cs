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

        public static JObject GetProjectInfo(string projectName)
        {
            RestRequest request = new RestRequest("/project", Method.GET);
            AuthorizationRequests.LoginUser(Utilities.TestUserEmail, Utilities.TestUserPassword);
            Clients.AddAuthorizationToken(request);

            var response = Clients.BackendClient.Execute(request);

            JArray responseJArray = JArray.Parse(response.Content);
            return responseJArray.Children<JObject>()
                .FirstOrDefault(o => o["name"].ToString() == projectName);
        }

        public static int GetProjectId(string projectName)
        {
            var info = GetProjectInfo(projectName);
            return info["id"].Value<int>();
        }
    }
}
