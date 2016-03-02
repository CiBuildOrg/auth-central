using Fsw.Enterprise.AuthCentral.Webdriver.Core;
using Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.Pages.Admin;
using Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.Pages.LoggedIn;
using Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.UIMaps.Public;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.Pages.Public
{
    public class EmailConfirmationPage : PageObjectBase
    {
        private EmailConfirmationUIElementMap _emailConfirmationUI;

        public EmailConfirmationPage(IWebDriver driver) : base(driver)
        {
            _emailConfirmationUI = new EmailConfirmationUIElementMap();
            PageFactory.InitElements(driver, _emailConfirmationUI);
        }
        public EmailConfirmationUIElementMap Map { get { return _emailConfirmationUI; } }

        public EmailConfirmationPage ConfirmEmail(string confirmationLinkUrl, string password)
        {
            Driver.Navigate().GoToUrl(confirmationLinkUrl);
            _emailConfirmationUI.PasswordBox.SendKeys(password);
            _emailConfirmationUI.ConfirmEmailButton.Click();
            return this;
        }
    }
}
