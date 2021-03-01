using OpenQA.Selenium;

namespace UITests.Pages.Authorization
{
    class AuthorizationPage : WebPage
    {
        public readonly By emailField = By.XPath("//input[@id='email']");
        public readonly By passwordField = By.XPath("//input[@id='password']");
        public readonly By registerNameField = By.XPath("//input[@id='name']");
        public readonly By registerSurnameField = By.XPath("//input[@id='surname']");
        public readonly By registerPasswordConfirmField = By.XPath("//input[@id='confirm-password']");
        public readonly By loginButton = By.XPath("//button[text()='Log In']");
        public readonly By registerButton = By.XPath("//button[text()='Register ']");
        public readonly By switchToRegistrationButton = By.XPath("//span[text()='Register']");
        public readonly By authErrorMessage = By.XPath("//div[@class='auth__invalid']");

        public AuthorizationPage(BaseDriver baseDriver) : base(baseDriver, "/auth")
        {

        }

        public void OpenRegistrationForm()
        {
            Driver.Click(switchToRegistrationButton);
        }

        public void LogIn(string email, string password)
        {
            Driver.FillField(emailField, email);
            Driver.FillField(passwordField, password);
            Driver.Click(loginButton);
        }

        public void Register(string name, string surname, string email, string password)
        {
            Driver.FillField(registerNameField, name);
            Driver.FillField(registerSurnameField, surname);
            Driver.FillField(emailField, email);
            Driver.FillField(passwordField, password);
            Driver.Click(registerButton);
        }
    }
}