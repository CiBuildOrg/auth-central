using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.UIMaps.LoggedIn
{
    class LoggedInUIElementMap
    {
        [FindsBy(How = How.CssSelector, Using = "a.dropdown-toggle")]
        public IWebElement MainMenuLink;

        [FindsBy(How = How.LinkText, Using = "Account Details")]
        public IWebElement AccountDetailsLink;

        [FindsBy(How = How.LinkText, Using = "Application Permissions")]
        public IWebElement AppPermissionsLink;

        [FindsBy(How = How.LinkText, Using = "Logout")]
        public IWebElement LogoutLink;

        [FindsBy(How = How.LinkText, Using = "Manage Users")]
        public IWebElement ManageUsersLink;
    }
}
