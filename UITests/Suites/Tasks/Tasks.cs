using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UITests.Pages;
using UITests.Requests;

namespace UITests.Suites.Tasks
{
    [Parallelizable]
    [TestFixture]
    [AllureParentSuite("Модуль \"Задачи\"")]
    [AllureSuite("Задачи")]
    [Category("All")]
    [Category("Tasks")]
    [AllureNUnit]
    class Tasks : SuiteTemplate
    {
        private ProjectPage page;
        private TaskPage taskPage;
        private string taskName;
        private string taskContent;
        private string taskAssignee = "Admin Admin";

        [OneTimeSetUp]
        public override void SetUp()
        {
            base.SetUp();
            Utilities.Login(fixture.Driver);

            // подготовка тестовых данных
            string projectName = "Autotest for tasks project " + DateTime.Now;
            ProjectsRequests.CreateProject(projectName);
            int projectId = ProjectsRequests.GetProjectId(projectName);
            string requestTaskName = "Autotest task from request" + DateTime.Now;
            TasksRequests.CreateTask(projectId, requestTaskName, "content");
            int taskId = TasksRequests.GetTaskId(projectId, requestTaskName);

            page = new ProjectPage(fixture.Driver, projectId);
            taskPage = new TaskPage(fixture.Driver, projectId, taskId);
        }

        [TestCase(Description = "Создание задачи (Успех)")]
        public void CreateTaskSuccess()
        {
            taskName = "Autotest task " + DateTime.Now;
            taskContent = "Autotest task content";

            page.CreateTask(taskName, taskContent, taskAssignee);

            page.AssertTaskInProjectExists(taskName);
        }

        [TestCase(Description = "Создание задачи с пустыми полями (Провал)")]
        public void CreateTaskWithEmptyFieldsFail()
        {
            page.CreateTask("", "");

            Assert.Multiple(() =>
            {
                page.AssertHasLocator(By.XPath("//div[@id='input'][@class='invalid-feedback'][text()='This field is required.']"));
                page.AssertHasLocator(By.XPath("//div[@id='textarea'][@class='invalid-feedback'][text()='This field is required.']"));
            });
        }

        [TestCase(Description = "Создание задачи без исполнителя (Провал)")]
        public void CreateTaskWithoutAssigneeFail()
        {
            page.CreateTask("Task", "Task");

            page.AssertHasLocator(By.XPath("//div[@class='auth__invalid']//p[text()='Please, pick assignee']"));
        }

        [TestCase(Description = "Редактирование задачи")]
        public void EditTaskSuccess()
        {
            string editName = "Autotest task edited";
            string editContent = "Autotest task content edited";

            taskPage.OpenPage();
            taskPage.EditTask(editName, editContent);

            Thread.Sleep(1000);

            taskPage.AssertTaskFieldAreEdited(editName, editContent);
        }
    }
}
