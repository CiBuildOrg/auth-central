using Fsw.Enterprise.AuthCentral.Webdriver.Core;
using Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.Pages.Admin;
using Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.Pages.LoggedIn;
using Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.UIMaps.Public;
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
        public LoginUIElementMap Map { get { return _loginUI; } }

        public ProfilePage Login(string username, string password)
        {
            this._loginUI.UsernameBox.SendKeys(username);
            this._loginUI.PasswordBox.SendKeys(password);
            this._loginUI.SignInButton.Click();
            
            return new ProfilePage(Driver);
        }
        public LoginPage DeleteUser_IfExists(EnvConfig config)
        {
            UserListPage userListPage = Login(config.AdminUser, config.AdminPassword).ClickManageUsersLink();
            if (userListPage.UserExists(config.NewUserEmail, config.NewUserNewEmail))
            {
                UserProfilePage profilePage = new UserProfilePage(Driver);
                profilePage.DeleteUser();
            }
            Driver.Navigate().GoToUrl($"{config.RootUrl}/useraccount/logout"); //this will need to be un-hardcoded
            return this;
        }
    }
}
