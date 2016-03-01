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
    public class LoggedInAdminPage : LoggedInUserPage
    {

        private AdminUIElementMap _loggedinadminUI;

        public LoggedInAdminPage(IWebDriver driver) : base(driver)
        {
            _loggedinadminUI = new AdminUIElementMap();
            PageFactory.InitElements(driver, _loggedinadminUI);
        }

        /// <summary>
        /// This method clicks the Manager Users link from the main menu.
        /// Note: This will only work if the user is logged in as an admin.
        /// </summary>
        /// <returns>a <see cref="UserListPage"/> object containing the resulting page.</returns>
        public UserListPage ClickManageUsersLink()
        {
            _loggedinadminUI.MainMenuLink.Click();
            _loggedinadminUI.ManageUsersLink.Click();
            return new UserListPage(Driver);
        }

        //TODO: Create page for Manage Clients
        //internal UserListPage ManageUsersLink()
        //{
        //    _loggedinadminUI.ManageUsersLink.Click();
        //    return new UserListPage(Driver);
        //}

        //TODO: Create page for Manage Scopes
        //internal UserListPage ManageUsersLink()
        //{
        //    _loggedinadminUI.ManageUsersLink.Click();
        //    return new UserListPage(Driver);
        //}

        //TODO: Create page for Discovery Document
        //internal UserListPage ManageUsersLink()
        //{
        //    _loggedinadminUI.ManageUsersLink.Click();
        //    return new UserListPage(Driver);
        //}

    }

}
