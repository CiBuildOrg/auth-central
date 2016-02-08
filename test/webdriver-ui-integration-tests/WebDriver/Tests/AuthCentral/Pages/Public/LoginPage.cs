using Fsw.Enterprise.AuthCentral.Webdriver.Core;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public string Login(string username, string password)
        {
            this._loginUI.UsernameBox.SendKeys(username);
            this._loginUI.PasswordBox.SendKeys(password);
            this._loginUI.SignInButton.Click();
            
            return this.Driver.CurrentWindowHandle;
        }
    }
}
