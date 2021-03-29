using Allure.Commons;
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
        [TestCase(true, "Test task", "Test task", 1, 1, 1, 1,  Description = "Создание таска со всеми заполненными полями (успех)")]
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
                    StringAssert.AreEqualIgnoringCase("не должно равняться null", response.Value<String>("projectId"));
                if (taskName == null)
                    StringAssert.AreEqualIgnoringCase("не должно равняться null", response.Value<String>("name"));
                if (content == null)
                    StringAssert.AreEqualIgnoringCase("не должно равняться null", response.Value<String>("content"));
                if (assigneeId == null)
                    StringAssert.AreEqualIgnoringCase("не должно равняться null", response.Value<String>("assigneeId"));
                if (priority == null)
                    StringAssert.AreEqualIgnoringCase("не должно равняться null", response.Value<String>("priorityId"));
                if (status == null)
                    StringAssert.AreEqualIgnoringCase("не должно равняться null", response.Value<String>("statusId"));
                if (type == null)
                    StringAssert.AreEqualIgnoringCase("не должно равняться null", response.Value<String>("typeId"));


            });
        }

        [Order(1)]
        [TestCase("Test task edited", "Test task edited", 2, 2, 2, Description = "Редактирование таска")]
        public void EditTask(string newTaskName = null, string newContent = null,
            int? priority = null, int? status = null, int? type = null)
        {
            var response = requests.EditTask(projectId, "Test task",
                newTaskName, newContent, priority, status, type);

            Assert.Multiple(() =>
            {
                Assert.True(response.ContainsKey("id"), "В ответе не содержится ключ id!");
                Assert.True(response.ContainsKey("name"), "В ответе не содержится ключ name!");
                if (newTaskName != null)
                    StringAssert.AreEqualIgnoringCase(newTaskName, response.Value<String>("name"), "name в ответе не совпадает с заданным!");
                Assert.True(response.ContainsKey("author"), "В ответе не содержится ключ author!");
                Assert.True(response.ContainsKey("assignee"), "В ответе не содержится ключ assignee!");
                Assert.True(response.ContainsKey("content"), "В ответе не содержится ключ content!");
                if (newContent != null)
                    StringAssert.AreEqualIgnoringCase(newContent, response.Value<String>("content"), "content в ответе не совпадает с заданным!");
                Assert.True(response.ContainsKey("project"), "В ответе не содержится ключ project!");
                Assert.True(response.ContainsKey("status"), "В ответе не содержится ключ status!");
                if (status != null)
                    Assert.True(response["status"].Value<int>("id") == status, "status в ответе не совпадает с заданным!");
                Assert.True(response.ContainsKey("type"), "В ответе не содержится ключ type!");
                if (type != null)
                    Assert.True(response["type"].Value<int>("id") == type, "type в ответе не совпадает с заданным!");
                Assert.True(response.ContainsKey("priority"), "В ответе не содержится ключ priority!");
                if (priority != null)
                    Assert.True(response["priority"].Value<int>("id") == priority, "priority в ответе не совпадает с заданным!");
            });
        }

        [TestCase(Description = "Удаление таска")]
        public void DeleteTask()
        {
            AllureLifecycle.Instance.WrapInStep(() =>
            {
                requests.CreateTask(projectId, "Test task to delete", "Test task", 1, 1, 1, 1);
            }, "Создание дополнительного таска для удаления");

            AllureLifecycle.Instance.WrapInStep(() =>
            {
                var response = requests.DeleteTask(projectId, "Test task to delete");
                var deletedTaskInfo = requests.GetTaskInfo(projectId, "Test task to delete");

                Assert.Multiple(() =>
                {
                    StringAssert.AreEqualIgnoringCase("The task have been deleted!", response.Value<String>("message"));
                    Assert.IsNull(deletedTaskInfo);
                });
            }, "Удаление таска");
        }
    }
}
