using Fsw.Enterprise.AuthCentral.Webdriver.Core;
using Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.UIMaps.Admin;
using Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.UIMaps.LoggedIn;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.Pages.Admin
{
    public class UserListPage : PageObjectBase
    {
        private UserListUIElementMap _userListUI;
        private LoggedInUIElementMap _loggedInUI;
        private UserUIElementMap _userUI;

        public UserListPage(IWebDriver driver) : base(driver)
        {
            _userListUI = new UserListUIElementMap();
            PageFactory.InitElements(driver, _userListUI);
            _loggedInUI = new LoggedInUIElementMap();
            PageFactory.InitElements(driver, _loggedInUI);
            _userUI = new UserUIElementMap();
            PageFactory.InitElements(driver, _userUI);
        }


        internal UserListUIElementMap Map { get { return _userListUI; } }

        //Change these methods to what you need

        internal NewUserPage ClickCreateUserButton()
        {
            _userListUI.CreateUserButton.Click();
            return new NewUserPage(Driver);
        }

        //internal ProfilePage UpdateName(string firstName, string lastName)
        //{
        //    _userListUI.FirstNameBox.Clear();
        //    _userListUI.FirstNameBox.SendKeys(firstName);
        //    _userListUI.LastNameBox.Clear();
        //    _userListUI.LastNameBox.SendKeys(lastName);
        //    _userListUI.NameSaveButton.Click();
        //    return this;
        //}
        
        public UserListPage DeleteUserIfExists(string email, string otherEmail)
        {
            _userListUI.EmailSearchBox.SendKeys(email);
            _userListUI.EmailSearchButton.Click();
            if (!_userUI.ClaimsMenuLink.Displayed)
            {
                _userListUI.EmailSearchBox.SendKeys(otherEmail);
                _userListUI.EmailSearchButton.Click();
            }
            if (_userUI.ClaimsMenuLink.Displayed)
            {
                _userListUI.DeleteButton.Click();
            }

            return this;
        }
        public UserListPage GotoManageUsers()
        {
            _loggedInUI.MainMenuLink.Click();
            _loggedInUI.ManageUsersLink.Click();

            return this;
        }        
    }
}
