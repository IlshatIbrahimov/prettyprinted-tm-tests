using FuncTests.Requests;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace FuncTests.Suites
{
    [Parallelizable]
    [AllureSuite("Tasks")]
    [Category("Tasks")]
    [AllureNUnit]
    class Tasks
    {
        private TasksRequests requests;
        private int projectId;

        [OneTimeSetUp]
        public void SetUp()
        {
            requests = new TasksRequests();

            string projectName = "Autotest project for tasks " + DateTime.Now;
            ProjectsRequests projectsRequests = new ProjectsRequests();
            projectsRequests.CreateProject(projectName);
            projectId = projectsRequests.GetProjectId(projectName);
        }

        [Order(0)]
        [TestCase(true, "Test task", "Test task", 1, 0, 1, 1,  Description = "Создание таска со всеми заполненными полями (успех)")]
        [TestCase(false, null, null, null, null, null, null, Description = "Создание таска с незаполненными полями (провал)")]
        public void CreateTask(bool withProject, string taskName, string content, int? assigneeId, int? priority, int? status, int? type)
        {
            int? localProjectId = null;
            if (withProject)
                localProjectId = projectId;

            var response = requests.CreateTask(localProjectId, taskName, content, assigneeId, priority, status, type);

            Assert.Multiple(() =>
            {
                if (withProject && taskName != null && content != null && assigneeId != null && priority != null && status != null && type != null)
                {
                    Assert.NotNull(requests.GetTaskInfo(projectId, "Test task"));

                    Assert.True(response.ContainsKey("assignee"), "В ответе отсутствует ключ assignee!");
                    Assert.True(response["assignee"].Value<int>("id") == assigneeId, "assigneeId в ответе не совпадает с заданным!");
                    Assert.True(response.ContainsKey("author"), "В ответе отсутствует ключ author!");
                    Assert.True(response.ContainsKey("comments"), "В ответе отсутствует ключ comments!");
                    Assert.True(response.ContainsKey("id"), "В ответе отсутствует ключ id!");
                    Assert.True(response.ContainsKey("name"), "В ответе отсутствует ключ name!");
                    Assert.True(response.Value<String>("name") == taskName, "name в ответе не совпадает с заданным!");
                    Assert.True(response.ContainsKey("priority"), "В ответе отсутствуте ключ priority!");
                    Assert.True(response["priority"].Value<int>("id") == priority, "priorityId в ответе не совпадает с заданным!");
                    Assert.True(response.ContainsKey("project"), "В ответе отсутствует ключ project!");
                    Assert.True(response["project"].Value<int>("id") == localProjectId, "projectId в ответе не совпадает с заданным!");
                    Assert.True(response.ContainsKey("status"), "В ответе отсутствует ключ status!");
                    Assert.True(response["status"].Value<int>("id") == status, "statusId в ответе не совпадает с заданным!");
                    Assert.True(response.ContainsKey("type"), "В ответе отсутствует ключ type!");
                    Assert.True(response["type"].Value<int>("id") == type, "typeId в ответе не совпадает с заданным!");
                }

                if (!withProject)
                    StringAssert.AreEqualIgnoringCase(response.Value<String>("projectId"), "не должно равняться null");
                if (taskName == null)
                    StringAssert.AreEqualIgnoringCase(response.Value<String>("name"), "не должно быть пустым");
                if (content == null)
                    StringAssert.AreEqualIgnoringCase(response.Value<String>("content"), "не должно равняться null");
                if (assigneeId == null)
                    StringAssert.AreEqualIgnoringCase(response.Value<String>("assigneeId"), "не должно равняться null");
                if (priority == null)
                    StringAssert.AreEqualIgnoringCase(response.Value<String>("priorityId"), "не должно равняться null");
                if (status == null)
                    StringAssert.AreEqualIgnoringCase(response.Value<String>("statusId"), "не должно равняться null");
                if (type == null)
                    StringAssert.AreEqualIgnoringCase(response.Value<String>("typeId"), "не должно равняться null");


            });
        }

        [Order(1)]
        [TestCase(Description = "Редактирование таска")]
        public void EditTask()
        {

        }
    }
}
