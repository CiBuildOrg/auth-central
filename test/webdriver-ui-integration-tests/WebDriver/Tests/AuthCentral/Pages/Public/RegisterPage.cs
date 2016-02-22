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

        public RegisterPage Register(string username, string email, string password)
        {
            this._registerUI.UsernameBox.SendKeys(username);
            this._registerUI.EmailBox.SendKeys(email);
            this._registerUI.FirstNameBox.SendKeys("test");
            this._registerUI.LastNameBox.SendKeys("user");
            this._registerUI.PasswordBox.SendKeys(password);
            this._registerUI.ConfirmPasswordBox.SendKeys(password);
            this._registerUI.CreateAccountButton.Click();
            return new RegisterPage(Driver);
        }
    }
}
