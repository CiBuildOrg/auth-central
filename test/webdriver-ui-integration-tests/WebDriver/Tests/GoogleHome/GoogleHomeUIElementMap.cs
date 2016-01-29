using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Fsw.Enterprise.AuthCentral.Webdriver.ExampleTests.GoogleHome
{
    public class GoogleHomeUIElementMap
    {
        [FindsBy(How = How.Id, Using = "lst-ib")]
        public IWebElement SearchInputBox;

     }
}
