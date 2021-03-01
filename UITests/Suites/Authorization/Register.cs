using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UITests.Pages.Authorization;
using UITests.Requests;

namespace UITests.Suites.Authorization
{
    [Parallelizable]
    [TestFixture]
    [AllureParentSuite("Модуль \"Авторизация\"")]
    [AllureSuite("Регистрация")]
    [Category("All")]
    [Category("Authorization")]
    [Category("Register")]
    [AllureNUnit]
    class Register : SuiteTemplate
    {
        private AuthorizationPage page;
        private string name;
        private string surname;
        private string email;
        private string password;
        private string emailDuplicate;

        [OneTimeSetUp]
        public override void SetUp()
        {
            base.SetUp();
            page = new AuthorizationPage(fixture.Driver);

            // подготовка тестовых данных
            name = "Autotest user " + DateTime.Now.ToString("ddMMyyyyhhmmss");
            surname = "Autotest user " + DateTime.Now.ToString("ddMMyyyyhhmmss");
            email = "autotestmailRegister" + DateTime.Now.ToString("ddMMyyyyhhmmss") + "@prettyprintedautotest.com";
            emailDuplicate = "autotestmailDuplicate" + DateTime.Now.ToString("ddMMyyyyhhmmss") + "@prettyprintedautotest.com";
            password = Utilities.GenerateNumbers(8);
            AuthorizationRequests.RegisterUser(name, surname, emailDuplicate, password);
        }

        [TestCase(Description = "Регистрация пользователя (Успех)")]
        public void SignUpSuccess()
        {
            page.OpenRegistrationForm();
            page.Register(name, surname, email, password);

            page.AssertHasLocator(By.XPath($"//div[@class='sidebar__footer-title'][text()='{name} {surname}']"));
        }

        [TestCase(Description = "Регистрация уже существующего пользователя (Провал)")]
        public void RegisterDuplicateFail()
        {
            page.OpenRegistrationForm();
            page.Register(name, surname, emailDuplicate, password);

            page.AssertErrorContains("This email address already registered!");
        }
    }
}
