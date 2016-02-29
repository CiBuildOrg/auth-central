using Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.Pages.LoggedIn;
using Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.UIMaps.LoggedIn;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.UIMaps.Admin
{
    class AdminUIElementMap : LoggedInUIElementMap
    {
        [FindsBy(How = How.LinkText, Using = "Manage Users")]
        public IWebElement ManageUsersLink;

        [FindsBy(How = How.LinkText, Using = "Manage Clients")]
        public IWebElement ManageClientsLink;

        [FindsBy(How = How.LinkText, Using = "Manage Scopes")]
        public IWebElement ManageScopesLink;

        [FindsBy(How = How.LinkText, Using = "Discovery Document")]
        public IWebElement DiscoveryDocumentLink;
    }
}
