using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UITests.Requests
{
    public static class TasksRequests
    {
        public static void CreateTask(int projectId, string taskName, string content)
        {
            RestRequest request = new RestRequest("/task", Method.POST);
            AuthorizationRequests.LoginUser(Utilities.TestUserEmail, Utilities.TestUserPassword);
            Clients.AddAuthorizationToken(request);

            JObject requestJObject = new JObject();
            requestJObject.Add("assigneeId", 1);
            requestJObject.Add("content", content);
            requestJObject.Add("priority", 0);
            requestJObject.Add("projectId", projectId);
            requestJObject.Add("status", 0);
            requestJObject.Add("taskName", taskName);
            requestJObject.Add("type", 0);

            request.RequestFormat = DataFormat.Json;
            request.AddParameter("application/json", requestJObject, ParameterType.RequestBody);

            Clients.BackendClient.Execute(request);
        }

        public static JObject GetTaskInfo(int projectId, string taskName)
        {
            RestRequest request = new RestRequest($"/project/{projectId}", Method.GET);
            Clients.AddAuthorizationToken(request);

            var response = Clients.BackendClient.Execute(request);
            JObject responseJObject = JObject.Parse(response.Content);
            JArray tasksList = JArray.Parse(responseJObject["taskList"].ToString());
            return tasksList.Children<JObject>()
                .FirstOrDefault(o => o["name"].ToString() == taskName);
        }

        public static int GetTaskId(int projectId, string taskName)
        {
            var info = GetTaskInfo(projectId, taskName);
            return info["id"].Value<int>();
        }
    }
}
