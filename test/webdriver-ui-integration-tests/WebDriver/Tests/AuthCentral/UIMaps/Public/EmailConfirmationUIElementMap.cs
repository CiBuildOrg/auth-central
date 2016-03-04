using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.UIMaps.Public

{
    public class EmailConfirmationUIElementMap : LoginUIElementMap
    {
        [FindsBy(How = How.XPath, Using = "//button[.='Confirm Email']")]
        public IWebElement ConfirmEmailButton;
        [FindsBy(How = How.Id, Using = "Password")]
        public IWebElement PasswordBox;
    }
}
