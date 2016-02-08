using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.Pages.Admin
{
    class EditClientUIElementMap
    {
        [FindsBy(How = How.Id, Using = "ClientId")]
        public IWebElement ClientIdBox;
        [FindsBy(How = How.Id, Using = "ClientName")]
        public IWebElement ClientNameBox;
        [FindsBy(How = How.Id, Using = "ClientUri")]
        public IWebElement ClientUriBox;
        [FindsBy(How = How.Id, Using = "LogoUri")]
        public IWebElement LogoUriBox;
        [FindsBy(How = How.LinkText, Using = "Client Secrets")]
        public IWebElement ClientSecretsLink;
        [FindsBy(How = How.LinkText, Using = "Claims")]
        public IWebElement ClaimsLink;
        [FindsBy(How = How.LinkText, Using = "Allowed Scopes")]
        public IWebElement ScopesLink;
        [FindsBy(How = How.LinkText, Using = "Uri's")]
        public IWebElement UriMenuLink;
        [FindsBy(How = How.LinkText, Using = "Redirect Uri's")]
        public IWebElement RedirectUriLink;
        [FindsBy(How = How.LinkText, Using = "Logout Uri's")]
        public IWebElement LogoutUriLink;
        [FindsBy(How = How.LinkText, Using = "Allowed CORS Origins")]
        public IWebElement CorsOriginLink;
        [FindsBy(How = How.LinkText, Using = "ClientAdminHome")]
        public IWebElement ClientAdminLink;


    }
}
