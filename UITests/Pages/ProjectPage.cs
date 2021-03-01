using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UITests.Pages
{
    class ProjectPage : WebPage
    {
        public readonly By tasksListItem = By.XPath("//ul[@class='tasks__list']//li//div//a[@class='tasks__list-title-name']");
        public readonly By addTaskButton = By.XPath("//button[text()='Add task']");
        public readonly By taskNameField = By.XPath("//input[@id='nameTask']");
        public readonly By taskContentField = By.XPath("//textarea[@id='content']");
        public readonly By taskPriorityField = By.XPath("//select[@id='priority']");
        public readonly By taskPriorityOptions = By.XPath("//select[@id='priority']//option");
        public readonly By taskTypeField = By.XPath("//select[@id='type']");
        public readonly By taskTypeOptions = By.XPath("//select[@id='type']//option");
        public readonly By taskStatusField = By.XPath("//select[@id='status']");
        public readonly By taskStatusOptions = By.XPath("//select[@id='status']//option");
        public readonly By taskAssigneeField = By.XPath("//select[@id='assignee']");
        public readonly By taskAssigneeOptions = By.XPath("//select[@id='assignee']//option");
        public readonly By createTaskButton = By.XPath("//button[text()='Create ']");

        public ProjectPage(BaseDriver baseDriver, int projectId) : base(baseDriver, $"/project/{projectId}")
        {

        }

        public void CreateTask(string taskName, string content, string assignee = null, string priority = null, string type = null, string status = null)
        {
            OpenPage();
            Driver.Click(addTaskButton);

            Driver.FillField(taskNameField, taskName);
            Driver.FillField(taskContentField, content);

            if (!String.IsNullOrEmpty(assignee))
            {
                Driver.Click(taskAssigneeField);
                Driver.Click(Utilities.ConcatXPath(taskAssigneeOptions, $"[text()='{assignee} ']"));
            }
            if (!String.IsNullOrEmpty(priority))
            {
                Driver.Click(taskPriorityField);
                Driver.Click(Utilities.ConcatXPath(taskPriorityOptions, $"[text()='{priority} ']"));
            }
            if (!String.IsNullOrEmpty(type))
            {
                Driver.Click(taskTypeField);
                Driver.Click(Utilities.ConcatXPath(taskTypeOptions, $"[text()='{type} ']"));
            }
            if (!String.IsNullOrEmpty(status))
            {
                Driver.Click(taskStatusField);
                Driver.Click(Utilities.ConcatXPath(taskStatusOptions, $"[text()='{status} ']"));
            }

            Driver.Click(createTaskButton);

            Thread.Sleep(1000);
        }

        public void AssertTaskInProjectExists(string taskName)
        {
            OpenPage();

            var tasks = Driver.GetElements(tasksListItem);

            bool isContains = tasks.Any(t => t.Text.Equals(taskName));
            Assert.True(isContains);
        }
    }
}
