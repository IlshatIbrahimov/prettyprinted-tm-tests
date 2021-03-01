using OpenQA.Selenium;

namespace UITests.Pages
{
    public class WebPage
    {
        public BaseDriver Driver { get; private set; }
        private string path;

        public WebPage(BaseDriver baseDriver, string path)
        {
            Driver = baseDriver;
            this.path = path;
        }

        public void OpenPage()
        {
            Driver.GoToUrl(path);
        }

        public bool HasLocator(By locator)
        {
            return Driver.GetElementsCount(locator) > 0;
        }
    }
}
