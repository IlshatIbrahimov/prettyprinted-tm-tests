using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using UITests.Pages.Authorization;
using UITests.Requests;

namespace UITests.Suites.Authorization
{
    [Parallelizable]
    [TestFixture]
    [AllureParentSuite("Модуль \"Авторизация\"")]
    [AllureSuite("Логин")]
    [Category("All")]
    [Category("Authorization")]
    [Category("Login")]
    [AllureNUnit]
    class Login : SuiteTemplate
    {
        private AuthorizationPage page;
        private string name;
        private string surname;
        private string email;
        private string password;

        [OneTimeSetUp]
        public override void SetUp()
        {
            base.SetUp();
            page = new AuthorizationPage(fixture.Driver);

            // подготовка тестовых данных
            name = "Autotest user " + DateTime.Now.ToString("ddMMyyyyhhmmss");
            surname = "Autotest user " + DateTime.Now.ToString("ddMMyyyyhhmmss");
            email = "autotestmail" + DateTime.Now.ToString("ddMMyyyyhhmmss") + "@prettyprintedautotest.com";
            password = Utilities.GenerateNumbers(8);
            AuthorizationRequests.RegisterUser(name, surname, email, password);
        }

        [TestCase(Description = "Авторизация пользователя (Успех)")]
        public void LoginUserSuccess()
        {
            page.OpenPage();
            page.LogIn(email, password);

            page.AssertHasLocator(By.XPath($"//div[@class='sidebar__footer-title'][text()='{name} {surname}']"));
        }

        [TestCase(Description = "Авторизация пользователя с неверным паролем (Провал)")]
        public void LoginUserIncorrectPassword()
        {
            page.OpenPage();
            page.LogIn(email, "incorrectpassword");

            page.AssertHasLocator(page.authErrorMessage);
        }

        [TestCase(Description = "Авторизация незарегистрированного пользователя (Провал)")]
        public void LogInUserIncorrectEmail()
        {
            page.OpenPage();
            page.LogIn("incorrectemail" + DateTime.Now.ToString("ddMMyyyyhhmmss") + "@prettyprintedautotest.com", "incorrectpassword");

            page.AssertHasLocator(page.authErrorMessage);
        }
    }
}
