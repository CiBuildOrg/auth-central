using Fsw.Enterprise.AuthCentral.Webdriver.Core;
using Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.Pages.LoggedIn;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.Pages.Public
{
    public class LoginPage : PageObjectBase
    {
        private LoginUIElementMap _loginUI;

        public LoginPage(IWebDriver driver) : base (driver)
        {
            _loginUI = new LoginUIElementMap();
            PageFactory.InitElements(driver, _loginUI);
        }

        public ProfilePage Login(string username, string password)
        {
            this._loginUI.UsernameBox.SendKeys(username);
            this._loginUI.PasswordBox.SendKeys(password);
            this._loginUI.SignInButton.Click();
            
            return new ProfilePage(Driver);
        }
    }
}
