using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuncTests.Requests
{
    class TasksRequests : Requests
    {
        public JObject CreateTask(int? projectId = null, string taskName = null, string content = null, int? assigneeId = null,
            int? priority = null, int? status = null, int? type = null )
        {
            RestRequest request = new RestRequest("/task", Method.POST);
            Client.Authorize(Utilities.TestUserEmail, Utilities.TestUserPassword);
            Client.AddAuthorizationToken(request);

            JObject requestJObject = new JObject();
            if (projectId != null)
                requestJObject.Add("projectId", projectId);
            if (taskName != null)
                requestJObject.Add("name", taskName);
            if (content != null)
                requestJObject.Add("content", content);
            if (assigneeId != null)
                requestJObject.Add("assigneeId", assigneeId);
            if (priority != null)
                requestJObject.Add("priorityId", priority);
            if (status != null)
                requestJObject.Add("statusId", status);
            if (type != null)
                requestJObject.Add("typeId", type);

            request.RequestFormat = DataFormat.Json;
            request.AddParameter("application/json", requestJObject, ParameterType.RequestBody);

            return JObject.Parse(Client.client.Execute(request).Content);
        }

        public JObject EditTask(int projectId, string taskName,
            string newTaskName = null, string content = null, int? priority = null,
            int? status = null, int? type = null)
        {
            var taskInfo = GetTaskInfo(projectId, taskName);

            RestRequest request = new RestRequest("/task", Method.PUT);
            Client.Authorize(Utilities.TestUserEmail, Utilities.TestUserPassword);
            Client.AddAuthorizationToken(request);

            JObject requestJObject = new JObject();
            requestJObject.Add("id", taskInfo.Value<int>("id"));

            if (newTaskName != null)
                requestJObject.Add("name", newTaskName);
            else
                requestJObject.Add("name", taskInfo.Value<String>("name"));

            if (content != null)
                requestJObject.Add("content", content);
            else
                requestJObject.Add("content", taskInfo.Value<String>("content"));

            requestJObject.Add("assigneeId", taskInfo["assignee"].Value<int>("id"));

            if (priority != null)
                requestJObject.Add("priorityId", priority);
            else
                requestJObject.Add("priorityId", taskInfo["priority"].Value<int>("id"));

            if (status != null)
                requestJObject.Add("statusId", status);
            else
                requestJObject.Add("statusId", taskInfo["status"].Value<int>("id"));

            if (type != null)
                requestJObject.Add("typeId", type);
            else
                requestJObject.Add("typeId", taskInfo["type"].Value<int>("id"));

            request.RequestFormat = DataFormat.Json;
            request.AddParameter("application/json", requestJObject, ParameterType.RequestBody);

            return JObject.Parse(Client.client.Execute(request).Content);

        }

        public JObject DeleteTask(int projectId, string taskName)
        {
            int taskId = GetTaskId(projectId, taskName);

            RestRequest request = new RestRequest($"/task/{taskId}", Method.DELETE);
            Client.Authorize(Utilities.TestUserEmail, Utilities.TestUserPassword);
            Client.AddAuthorizationToken(request);

            return JObject.Parse(Client.client.Execute(request).Content);
        }

        public JObject GetTaskInfo(int projectId, string taskName)
        {
            RestRequest request = new RestRequest($"/project/{projectId}", Method.GET);
            Client.Authorize(Utilities.TestUserEmail, Utilities.TestUserPassword);
            Client.AddAuthorizationToken(request);

            var response = Client.client.Execute(request);
            JObject responseJObject = JObject.Parse(response.Content);
            JArray tasksList = JArray.Parse(responseJObject["tasks"].ToString());
            return tasksList.Children<JObject>()
                .FirstOrDefault(o => o["name"].ToString() == taskName);
        }

        public int GetTaskId(int projectId, string taskName)
        {
            var info = GetTaskInfo(projectId, taskName);
            return info["id"].Value<int>();
        }
    }
}
