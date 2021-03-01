using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Text;
using System.Threading;
using UITests.Pages.Authorization;

namespace UITests
{
    public static class Utilities
    {
        public static string BaseUrl { get; set; }
        public static string TestUserEmail = "admin@mail.ru";
        public static string TestUserPassword = "12345678";

        public static void Login(BaseDriver driver)
        {
            AuthorizationPage page = new AuthorizationPage(driver);
            page.OpenPage();
            page.LogIn(TestUserEmail, TestUserPassword);
            Thread.Sleep(2000);
        }

        public static string GenerateNumbers(int count)
        {
            StringBuilder sb = new StringBuilder();
            Random rnd = new Random();

            for (int i = 0; i < count; i++)
            {
                sb.Append($"{rnd.Next(1, 10)}");
            }

            return sb.ToString();
        }

        public static By ConcatXPath(By firstXPath, string secondXPath)
        {
            return By.XPath(firstXPath.ToString().Replace("By.XPath: ", "") + secondXPath);
        }

        public static By ConcatXPath(params By[] paths)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var p in paths)
            {
                sb.Append(p.ToString().Replace("By.XPath: ", ""));
            }

            return By.XPath(sb.ToString());
        }
    }
}
