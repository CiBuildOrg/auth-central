using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using Fsw.Enterprise.AuthCentral.Webdriver.Core;
using Fsw.Enterprise.AuthCentral.Webdriver.Core.Extensions;
using System.Collections.ObjectModel;

namespace Fsw.Enterprise.AuthCentral.Webdriver.ExampleTests.GoogleHome
{
    public class GoogleHomePage : PageObjectBase
    {
        private GoogleHomeUIElementMap _googleHomeUI;

        public GoogleHomePage(IWebDriver driver) : base (driver)
        {
            _googleHomeUI = new GoogleHomeUIElementMap();

            //otherwise we continue with the instantiation of the class.
            PageFactory.InitElements(driver, _googleHomeUI);
        }

        public ReadOnlyCollection<IWebElement> Search(string searchTerm, string expectedLinkText)
        {
            this._googleHomeUI.SearchInputBox.SendKeys(searchTerm);
            this._googleHomeUI.SearchButton.Click();

            return this.Driver.FindElements(By.PartialLinkText(expectedLinkText));
        }

    }
}
