using OpenQA.Selenium;
using System;
using System.Text;

namespace UITests
{
    public static class Utilities
    {
        public static string BaseUrl { get; set; }

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
