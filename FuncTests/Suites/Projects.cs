using FuncTests.Requests;
using Newtonsoft.Json.Linq;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuncTests.Suites
{
    [Parallelizable]
    [AllureSuite("Projects")]
    [Category("Projects")]
    [AllureNUnit]
    class Projects
    {
        private ProjectsRequests requests;

        [OneTimeSetUp]
        public void SetUp()
        {
            requests = new ProjectsRequests();
        }

        [TestCase(Description = "Создание проекта")]
        public void CreateProject()
        {
            string projectName = "Autotest project " + DateTime.Now;
            var response = requests.CreateProject(projectName);

            Assert.Multiple(() =>
            {
                Assert.NotNull(requests.GetProjectInfo(projectName), "Созданный проект не найден!");

                Assert.True(response.ContainsKey("comments"), "В ответе не содержится поле comments!");
                Assert.True(response.ContainsKey("id"), "В ответе не содержится поле id!");
                Assert.True(response.ContainsKey("name"), "В ответе не содержится поле name!");
                Assert.True(response.Value<String>("name") == projectName, "name в ответе не совпадает с созданным!");
                Assert.True(response.ContainsKey("tasks"), "В ответе не содержится поле tasks!");
            });
        }
    }
}
