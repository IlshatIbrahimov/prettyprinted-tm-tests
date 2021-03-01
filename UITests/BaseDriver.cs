using Allure.Commons;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UITests
{
    public class BaseDriver
    {
        private IWebDriver driver;
        // при использовании Docker Toolbox Selenoid запускается на 192.168.99.100:4444
        // при использовании Docker for Windows Selenoid запускается на localhost:4444
        private string remoteWdUri = "http://192.168.99.100:4444/";

        public BaseDriver()
        {
            driver = StartBrowser();
        }

        private RemoteWebDriver StartBrowser()
        {
            ChromeOptions options = new ChromeOptions();

            options.AddUserProfilePreference("browser.download.folderList", 2);
            options.AddUserProfilePreference("browser.download.manager.showWhenStarting", false);
            options.AddUserProfilePreference("browser.download.manager.focusWhenStarting", false);
            options.AddUserProfilePreference("browser.download.manager.useWindow", false);
            options.AddUserProfilePreference("browser.download.manager.showAlertOnComplete", false);
            options.AddUserProfilePreference("profile.default_content_settings.popups", 0);

            options.AddArgument("start-maximized");
            options.AddArgument("no-sandbox");

            options.AddAdditionalCapability("browser", "chrome", true);
            options.AddAdditionalCapability("version", "84.0", true);
            options.AddAdditionalCapability("platform", "Any", true);
            options.AddAdditionalCapability("enableVNC", true, true);

            TimeSpan waiting = TimeSpan.FromMinutes(5);

            var driver = new RemoteWebDriver(
                new Uri(remoteWdUri + "wd" + Path.DirectorySeparatorChar + "hub"),
                options.ToCapabilities(),
                waiting);

            return driver;
        }


        public void GoToUrl(string url)
        {
            driver.Url = Utilities.BaseUrl + url;
            Refresh();
        }

        public void GoToExternalUrl(string url)
        {
            driver.Url = url;
            Refresh();
        }

        public void Back()
        {
            driver.Navigate().Back();
        }

        public void Refresh()
        {
            driver.Navigate().Refresh();
        }

        public string GetCurrentUrl()
        {
            return driver.Url;
        }

        public IWebElement GetElement(By locator, int secondsToWait = 10)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(secondsToWait));
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(locator));

                return driver.FindElement(locator);
            }
            catch(TimeoutException)
            {
                throw new NoSuchElementException($"Element '{locator}' not found!");
            }
        }

        public IReadOnlyCollection<IWebElement> GetElements(By locator, int secondsToWait = 10)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(secondsToWait));
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(locator));

                return driver.FindElements(locator);
            }
            catch(TimeoutException)
            {
                throw new NoSuchElementException($"Element '{locator}' not found!");
            }
        }

        public int GetElementsCount(By locator, int secondsToWait = 10)
        {
            try
            {
                IReadOnlyCollection<IWebElement> elements = GetElements(locator, secondsToWait);
                return elements.Count;
            }
            catch (NoSuchElementException)
            {
                return 0;
            }
        }

        public IWebElement Click(By locator, int secondsToWait = 10)
        {
            try
            {
                IWebElement element = GetElement(locator, secondsToWait);
                return Click(element);
            }
            catch(Exception e)
            {
                throw new ElementNotInteractableException($"Element '{locator}' is not clickable!\n{e.Message}");
            }
        }

        public IWebElement Click(IWebElement element)
        {
            try
            {
                element.Click();
                return element;
            }
            catch (Exception e)
            {
                throw new ElementNotInteractableException($"Element is not clickable!\n{e.Message}");
            }
        }

        public IWebElement ClearField(By locator)
        {
            IWebElement element = GetElement(locator);

            element.SendKeys(Keys.Control + 'a');
            element.SendKeys(Keys.Backspace);

            return element;
        }

        public IWebElement FillField(By locator, string value, bool pressEnter = false, int secondsToWait = 10)
        {
            try
            {
                IWebElement element = GetElement(locator, secondsToWait);
                return FillField(element, value, pressEnter);
            }
            catch (Exception e)
            {
                throw new ElementNotInteractableException($"Element '{locator}' is not fillable!\n{e.Message}");
            }
        }

        public IWebElement FillField(IWebElement element, string value, bool pressEnter = false)
        {
            try
            {
                element.Clear();
                return FillFieldWithoutClear(element, value, pressEnter);
            }
            catch (Exception e)
            {
                throw new ElementNotInteractableException($"Element is not fillable!\n{e.Message}");
            }
        }

        public IWebElement FillFieldWithoutClear(By locator, string value, bool pressEnter = false, int secondsToWait = 10)
        {
            try
            {
                IWebElement element = GetElement(locator, secondsToWait);
                return FillFieldWithoutClear(element, value, pressEnter);
            }
            catch (Exception e)
            {
                throw new ElementNotInteractableException($"Element '{locator}' is not fillable!\n{e.Message}");
            }
        }

        public IWebElement FillFieldWithoutClear(IWebElement element, string value, bool pressEnter = false)
        {
            try
            {
                element.SendKeys(value);

                if (pressEnter)
                    element.SendKeys(Keys.Enter);

                return element;
            }
            catch (Exception e)
            {
                throw new ElementNotInteractableException($"Element is not fillable!\n{e.Message}");
            }
        }

        public void ScrollToElement(By locator, int secondsToWait = 10)
        {
            IWebElement element = GetElement(locator, secondsToWait);
            ScrollToElement(element);
        }

        public void ScrollToElement(IWebElement element)
        {
            Actions action = new Actions(driver);
            action.MoveToElement(element).Perform();
        }

        public void Quit()
        {
            driver.Quit();
        }

        public void Dispose()
        {
            driver.Dispose();
        }

        public void AttachScreenToReport()
        {
            byte[] screenshot = TakeScreenshot();

            try
            {
                Random rnd = new Random();
                AllureLifecycle.Instance.AddAttachment(
                    $"Screenshot--{DateTime.Now.ToString("hh-mm-ss")}--{rnd.Next(100, 999)}",
                    "image/png",
                    screenshot,
                    "png");
            }
            catch { }
        }

        public void AttachPageSourceToReport()
        {
            try
            {
                Random rnd = new Random();
                AllureLifecycle.Instance.AddAttachment(
                    $"PageSource--{DateTime.Now.ToString("hh-mm-ss")}--{rnd.Next(100, 999)}",
                    "text/html",
                    Encoding.UTF8.GetBytes(driver.PageSource),
                    "html");
            }
            catch { }
        }

        private byte[] TakeScreenshot()
        {
            try
            {
                Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                byte[] screenshotByteArray = screenshot.AsByteArray;
                return screenshotByteArray;
            }
            catch (Exception e)
            {
                throw new Exception($"Не удалось сделать скриншот:\n{e.Message}");
            }
        }
    }
}
