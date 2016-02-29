using Fsw.Enterprise.AuthCentral.Webdriver.Core;
using Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.Pages.LoggedIn;
using Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.UIMaps.Admin;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.Pages.Admin
{
    public class LoggedInAdminPage : PageObjectBase
    {

        private AdminUIElementMap _loggedinadminUI;

        public LoggedInAdminPage(IWebDriver driver) : base(driver)
        {
            _loggedinadminUI = new AdminUIElementMap();
            PageFactory.InitElements(driver, _loggedinadminUI);
        }

        internal AdminUIElementMap Map { get { return _loggedinadminUI; } }

        internal LoggedInAdminPage ExpandMainMenu()
        {
            _loggedinadminUI.MainMenuLink.Click();
            return this;
        }

        internal UserListPage ManageUsersLink()
        {
            _loggedinadminUI.ManageUsersLink.Click();
            return new UserListPage(Driver);
        }

        //Manage Clients
        //internal UserListPage ManageUsersLink()
        //{
        //    _loggedinadminUI.ManageUsersLink.Click();
        //    return new UserListPage(Driver);
        //}

        //Manage Scopes
        //internal UserListPage ManageUsersLink()
        //{
        //    _loggedinadminUI.ManageUsersLink.Click();
        //    return new UserListPage(Driver);
        //}

        //Discovery Document
        //internal UserListPage ManageUsersLink()
        //{
        //    _loggedinadminUI.ManageUsersLink.Click();
        //    return new UserListPage(Driver);
        //}

    }

}
