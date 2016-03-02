using Fsw.Enterprise.AuthCentral.Webdriver.Core;
using Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.UIMaps.Outlook;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.Pages.Outlook
{
    public class OwaLoginPage : PageObjectBase
    {
        private OwaLoginUIElementMap _owaLoginUI;

        public OwaLoginPage(IWebDriver driver) : base(driver)
        {
            _owaLoginUI = new OwaLoginUIElementMap();
            PageFactory.InitElements(driver, _owaLoginUI);
        }
        public OwaLoginUIElementMap Map { get { return _owaLoginUI; } }

        public OwaLoggedInPage Login(string username, string password)
        {
            Driver.Navigate().GoToUrl("https://outlook.foodservicewarehouse.com/owa/auth/logon.aspx?");
            _owaLoginUI.UsernameBox.SendKeys(username);
            _owaLoginUI.PasswordBox.SendKeys(password);
            _owaLoginUI.SignInButton.Click();

            return new OwaLoggedInPage(Driver);
        }
    }
}
