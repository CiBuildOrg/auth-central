using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Fsw.Enterprise.AuthCentral.Webdriver.ExampleTests.GoogleHome
{
    public class GoogleHomeUIElementMap
    {
//        [FindsBy(How = How.Id, Using = "lst-ib")]
        [FindsBy(How = How.Id, Using = "search_form_input_homepage")]
        public IWebElement SearchInputBox;

        [FindsBy(How = How.Id, Using = "search_button_homepage")]
        public IWebElement SearchButton;

        [FindsBy(How = How.PartialLinkText, Using = "http://www.hello-world.com/")]
        public IWebElement ExpectedHelloWorldSearchResult;

     }
}
