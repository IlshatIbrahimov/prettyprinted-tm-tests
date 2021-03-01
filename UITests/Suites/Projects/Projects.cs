using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using NUnit.Framework;
using System;
using System.Threading;
using UITests.Pages;
using UITests.Requests;

namespace UITests.Suites.Projects
{
    [Parallelizable]
    [TestFixture]
    [AllureParentSuite("Модуль \"Проекты\"")]
    [AllureSuite("Проекты")]
    [Category("All")]
    [Category("Projects")]
    [AllureNUnit]
    class Projects : SuiteTemplate
    {
        private MainPage page;
        private string projectName;
        private string requestProjectName;

        [OneTimeSetUp]
        public override void SetUp()
        {
            base.SetUp();
            page = new MainPage(fixture.Driver);
            Utilities.Login(fixture.Driver);

            // подготовка тестовых данных
            requestProjectName = "Autotest project request " + DateTime.Now;
            ProjectsRequests.CreateProject(requestProjectName);
        }

        [TestCase(Description = "Создание проекта (Успех)")]
        public void CreateProjectSuccess()
        {
            projectName = "Autotest project " + DateTime.Now;
            
            page.OpenPage();
            page.CreateProject(projectName);

            Thread.Sleep(2000);

            page.AssertProjectExists(projectName);
        }

        [TestCase(Description = "Переход к созданному проекту")]
        public void NavigateToProject()
        {
            page.OpenPage();
            page.GoToProject(requestProjectName);

            page.AssertTitleContains(requestProjectName);
        }
    }
}
