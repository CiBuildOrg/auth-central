using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Fsw.Enterprise.AuthCentral.Webdriver.ExampleTests.Google
{
    public class GmailHomeUIMap
    {
        [FindsBy(How = How.Id, Using = "lst-ib")]
        public IWebElement SearchInputBox;

     }
}
