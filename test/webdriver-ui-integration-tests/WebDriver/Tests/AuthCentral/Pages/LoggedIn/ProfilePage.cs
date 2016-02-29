﻿using Fsw.Enterprise.AuthCentral.Webdriver.Core;
using Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.Pages.Public;
using Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.UIMaps.LoggedIn;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.Pages.LoggedIn
{
    public class ProfilePage : PageObjectBase
    {
        private ProfileUIElementMap _profileUI;
        private LoggedInUIElementMap _loggedInUI;

        public ProfilePage(IWebDriver driver) : base (driver)
        {
            _profileUI = new ProfileUIElementMap();
            PageFactory.InitElements(driver, _profileUI);
            _loggedInUI = new LoggedInUIElementMap();
            PageFactory.InitElements(driver, _loggedInUI);
        }

        internal ProfileUIElementMap Map { get { return _profileUI; } }

        internal ProfilePage ExpandNameForm()
        {
            _profileUI.NameEditLink.Click();
            return this;
        }

        internal ProfilePage UpdateName(string firstName, string lastName)
        {
            _profileUI.FirstNameBox.Clear();
            _profileUI.FirstNameBox.SendKeys(firstName);
            _profileUI.LastNameBox.Clear();
            _profileUI.LastNameBox.SendKeys(lastName);
            _profileUI.NameSaveButton.Click();
            return this;
        }
        public ProfilePage Logout()
        {
            _loggedInUI.MainMenuLink.Click();
            _loggedInUI.LogoutLink.Click();
            return this;
        }
    }
}
