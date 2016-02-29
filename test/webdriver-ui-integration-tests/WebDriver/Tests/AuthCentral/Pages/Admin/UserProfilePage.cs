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
        public UserProfilePage IfOnClaimsPage_GoToProfilePage() // this method should be removed once it is determined which page we will land on after email search
        {
            UserClaimsPage claimsPage = new UserClaimsPage(Driver);
            if(claimsPage.IsOnUserClaimsPage())
            {
                claimsPage.Map.ProfileMenuLink.Click();
            }
            return this;
        }
        public UserListPage DeleteUser()
        {
            IfOnClaimsPage_GoToProfilePage();
            _profileUI.DeleteButton.Click();
            UserListPage userListPage = new UserListPage(Driver);
            return userListPage;
        }

    }
}