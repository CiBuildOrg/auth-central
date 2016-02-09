using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.Pages.Admin
{
    class CreateUserClaimUIElementMap
    {
        [FindsBy(How = How.XPath, Using = "//input[contains(@id,\"Type\")]")]
        public IWebElement ClaimTypeBox;
        [FindsBy(How = How.XPath, Using = "//input[contains(@id,\"Value\")]")]
        public IWebElement ClaimValueBox;
        [FindsBy(How = How.XPath, Using = "//input[@value='Save']")]
        public IWebElement SaveButton;
    }
}
