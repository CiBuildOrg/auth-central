using Fsw.Enterprise.AuthCentral.Webdriver.Core;
using Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.UIMaps.Admin;
using Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.UIMaps.LoggedIn;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.Pages.LoggedIn
{
    public class UserListPage : PageObjectBase
    {
        private UserListUIElementMap _userListUI;

        public UserListPage(IWebDriver driver) : base(driver)
        {
            _userListUI = new UserListUIElementMap();
            PageFactory.InitElements(driver, _userListUI);
        }

        internal UserListUIElementMap Map { get { return _userListUI; } }

        //Change these methods to what you need

        //internal ProfilePage ExpandNameForm()
        //{
        //    _userListUI.NameEditLink.Click();
        //    return this;
        //}

        //internal ProfilePage UpdateName(string firstName, string lastName)
        //{
        //    _userListUI.FirstNameBox.Clear();
        //    _userListUI.FirstNameBox.SendKeys(firstName);
        //    _userListUI.LastNameBox.Clear();
        //    _userListUI.LastNameBox.SendKeys(lastName);
        //    _userListUI.NameSaveButton.Click();
        //    return this;
        //}
    }
}
