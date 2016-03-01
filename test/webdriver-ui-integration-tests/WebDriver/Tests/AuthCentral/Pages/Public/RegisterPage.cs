using Fsw.Enterprise.AuthCentral.Webdriver.Core;
using Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.Pages.LoggedIn;
using Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.UIMaps.Public;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;

namespace Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.Pages.Public
{
    public class RegisterPage : PageObjectBase
    {
        private RegisterUIElementMap _registerUI;

        public RegisterPage(IWebDriver driver) : base(driver)
        {
            _registerUI = new RegisterUIElementMap();
            PageFactory.InitElements(driver, _registerUI);
        }
        public RegisterUIElementMap Map { get { return _registerUI; } }

        public RegisterPage DeleteAndRegisterNewUser(EnvConfig config)
        {
            LoginPage page = new LoginPage(Driver).DeleteUser_IfExists(config);
            page.Map.CreateAccountLink.Click();
            _registerUI.UsernameBox.SendKeys(config.NewUserUsername);
            _registerUI.EmailBox.SendKeys(config.NewUserEmail);
            _registerUI.FirstNameBox.SendKeys("test");
            _registerUI.LastNameBox.SendKeys("user");
            _registerUI.PasswordBox.SendKeys(config.NewUserPassword);
            _registerUI.ConfirmPasswordBox.SendKeys(config.NewUserPassword);
            _registerUI.CreateAccountButton.Click();
            return new RegisterPage(Driver);
        }
        public RegisterPage DeleteAndRegisterNewUser(EnvConfig config, string username, string email, string password, string confirmPassword = null)
        {
            LoginPage page = new LoginPage(Driver).DeleteUser_IfExists(config);
            page.Map.CreateAccountLink.Click();
            _registerUI.UsernameBox.SendKeys(username);
            _registerUI.EmailBox.SendKeys(email);
            _registerUI.FirstNameBox.SendKeys("test");
            _registerUI.LastNameBox.SendKeys("user");
            _registerUI.PasswordBox.SendKeys(password);
            _registerUI.ConfirmPasswordBox.SendKeys(confirmPassword == null ? password : confirmPassword);
            _registerUI.CreateAccountButton.Click();
            return new RegisterPage(Driver);
        }
        public RegisterPage Register(EnvConfig config, string username, string email, string password, string confirmPassword = null)
        {
            LoginPage page = new LoginPage(Driver);
            page.Map.CreateAccountLink.Click();
            _registerUI.UsernameBox.SendKeys(username);
            _registerUI.EmailBox.SendKeys(email);
            _registerUI.FirstNameBox.SendKeys("test");
            _registerUI.LastNameBox.SendKeys("user");
            _registerUI.PasswordBox.SendKeys(password);
            _registerUI.ConfirmPasswordBox.SendKeys(confirmPassword == null ? password : confirmPassword);
            _registerUI.CreateAccountButton.Click();
            return new RegisterPage(Driver);
        }
    }
}
