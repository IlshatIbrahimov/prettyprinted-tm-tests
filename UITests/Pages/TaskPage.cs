using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UITests.Pages
{
    public class TaskPage : WebPage
    {
        public readonly By editIcon = By.XPath("//button[@class='task__header-btn task__header-btn--edit']");
        public readonly By editTaskNameField = By.XPath("//input[@class='form__input-field form-control']");
        public readonly By editTaskContentField = By.XPath("//textarea[@class='form__textarea-field form-control']");
        public readonly By editTaskSaveButton = By.XPath("//button[text()='Save ']");
        public readonly By taskTitle = By.XPath("//div[@class='task__content-title']//span");
        public readonly By taskContent = By.XPath("//div[@class='task__text']//p");

        public TaskPage(BaseDriver baseDriver, int projectId, int taskId) : base(baseDriver, $"/project/{projectId}/task-{taskId}")
        {

        }

        public void EditTask(string taskName, string content)
        {
            Driver.Click(editIcon);
            Driver.FillField(editTaskNameField, taskName);
            Driver.FillField(editTaskContentField, content);
            Driver.Click(editTaskSaveButton);
        }

        public void AssertTaskFieldAreEdited(string taskName, string content)
        {
            string actualTaskName = Driver.GetElement(taskTitle).Text;
            string actualTaskContent = Driver.GetElement(taskContent).Text;

            Assert.Multiple(() =>
            {
                StringAssert.AreEqualIgnoringCase(taskName, actualTaskName);
                StringAssert.AreEqualIgnoringCase(content, actualTaskContent);
            });
        }
    }
}
