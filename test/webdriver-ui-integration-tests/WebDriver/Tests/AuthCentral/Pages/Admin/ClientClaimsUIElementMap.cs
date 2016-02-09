using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.Pages.Admin
{
    class ClientClaimsUIElementMap
    {
        [FindsBy(How = How.LinkText, Using = "Create Claim")]
        public IWebElement CreateClaimButton;
    }
}
