using Fsw.Enterprise.AuthCentral.Webdriver.Core;
using Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.Pages.Admin;
using Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.Pages.Public;
using Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.UIMaps.LoggedIn;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.Pages.LoggedIn
{
    public class LoggedInUserPageObjectBase : PageObjectBase
    {
        private LoggedInUIElementMap _loggedinUI;

        public LoggedInUserPageObjectBase(IWebDriver driver) : base(driver)
        {
            _loggedinUI = new LoggedInUIElementMap();
            PageFactory.InitElements(driver, _loggedinUI);
        }

        internal ProfilePage ClickAccountDetailsLink()
        {
            _loggedinUI.MainMenuLink.Click();
            _loggedinUI.AccountDetailsLink.Click();
            return new ProfilePage(Driver);
        }

        //TODO: Add AppPermissionsPage
        //internal UserListPage ClickAccountPermissionsLink()
        //{
        //    _loggedinUI.MainMenuLink.Click();
        //    _loggedinUI.AppPermissionsLink.Click();
        //    return new UserListPage(Driver);
        //}

        internal LoggedOutPage ClickLogoutLink()
        {
            _loggedinUI.MainMenuLink.Click();
            _loggedinUI.LogoutLink.Click();
            return new LoggedOutPage(Driver);
        }


    }
}
