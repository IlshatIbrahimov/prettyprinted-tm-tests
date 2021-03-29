using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuncTests.Requests
{
    public class ProjectsRequests : Requests
    {
        public JObject CreateProject(string projectName)
        {
            RestRequest request = new RestRequest("/project", Method.POST);
            Client.Authorize(Utilities.TestUserEmail, Utilities.TestUserPassword);
            Client.AddAuthorizationToken(request);

            JObject requestJObject = new JObject();
            requestJObject.Add("name", projectName);

            request.RequestFormat = DataFormat.Json;
            request.AddParameter("application/json", requestJObject, ParameterType.RequestBody);

            return JObject.Parse(Client.client.Execute(request).Content);
        }

        public JObject GetProjectInfo(string projectName)
        {
            RestRequest request = new RestRequest("/project", Method.GET);
            Client.Authorize(Utilities.TestUserEmail, Utilities.TestUserPassword);
            Client.AddAuthorizationToken(request);

            var response = Client.client.Execute(request);

            JArray responseJArray = JArray.Parse(response.Content);
            return responseJArray.Children<JObject>()
                .FirstOrDefault(o => o["name"].ToString() == projectName);
        }

        public int GetProjectId(string projectName)
        {
            var info = GetProjectInfo(projectName);
            return info["id"].Value<int>();
        }
    }
}
