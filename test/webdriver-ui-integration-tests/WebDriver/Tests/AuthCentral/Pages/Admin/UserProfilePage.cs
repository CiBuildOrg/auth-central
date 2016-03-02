using Fsw.Enterprise.AuthCentral.Webdriver.Core;
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
    public class UserProfilePage : PageObjectBase
    {

        private ProfileUIElementMap _profileUI;

        public UserProfilePage(IWebDriver driver) : base(driver)
        {
            _profileUI = new ProfileUIElementMap();
            PageFactory.InitElements(driver, _profileUI);
        }

        internal ProfileUIElementMap Map { get { return _profileUI; } }

        public bool IsOnUserProfilePage()
        {
            try { return _profileUI.DisableButton.Displayed; }
            catch { return false; }
        }
        public UserListPage DeleteUser()
        {
            _profileUI.DeleteButton.Click();
            UserListPage userListPage = new UserListPage(Driver);
            return userListPage;
        }

    }
}