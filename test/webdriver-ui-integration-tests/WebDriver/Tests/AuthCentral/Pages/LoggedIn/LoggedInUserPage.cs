using Fsw.Enterprise.AuthCentral.Webdriver.Core;
using Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.Pages.Public;
using Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.UIMaps.LoggedIn;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.Pages.LoggedIn
{
    public class LoggedInUserPage : PageObjectBase
    {
        private LoggedInUIElementMap _loggedinUI;

        public LoggedInUserPage(IWebDriver driver) : base(driver)
        {
            _loggedinUI = new LoggedInUIElementMap();
            PageFactory.InitElements(driver, _loggedinUI);
        }

        internal LoggedInUIElementMap Map { get { return _loggedinUI; } }

        internal LoggedInUserPage ExpandMainMenu()
        {
            _loggedinUI.MainMenuLink.Click();
            return this;
        }
        internal ProfilePage ClickAccountDetailsLink()
        {
            _loggedinUI.AccountDetailsLink.Click();
            return new ProfilePage(Driver);
        }

        //need to make the page for this to return - dsc
        //internal AccountPermissionsPage ClickApplicationsPermissions()
        //{
        //    _loggedinUI.AppPermissionsLink.Click();
        //}
        internal LoggedOutPage ClickLogoutLink()
        {
            _loggedinUI.LogoutLink.Click();
            return new LoggedOutPage(Driver);
        }

    }
}