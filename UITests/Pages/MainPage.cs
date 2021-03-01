using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UITests.Pages
{
    public class MainPage : WebPage
    {
        public readonly By addProjectButton = By.XPath("//button[text()='Add project ']");
        public readonly By projectsListItem = By.XPath("//ul[@class='sidebar__list sidebar__list--project scroll']//li//a");

        public MainPage(BaseDriver baseDriver) : base(baseDriver, "")
        {

        }

        public void CreateProject(string projectName)
        {
            Driver.Click(addProjectButton);
            Driver.FillField(By.XPath("//input[@id='projectName']"), projectName);
            Driver.Click(By.XPath("//button[text()='Create ']"));
        }

        public void AssertProjectExists(string projectName)
        {
            var projects = Driver.GetElements(projectsListItem);

            bool isContains = projects.Any(p => p.Text.Equals(projectName));
            Assert.True(isContains);
        }
    }
}
