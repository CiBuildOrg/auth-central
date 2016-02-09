using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.Pages.Admin
{
    class ClientSecretsUIElementMap
    {
        [FindsBy(How = How.LinkText, Using = "Create Secret")]
        public IWebElement CreateSecretButton;
    }
}
