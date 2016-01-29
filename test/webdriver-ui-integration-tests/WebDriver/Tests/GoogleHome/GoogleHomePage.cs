using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using Fsw.Enterprise.AuthCentral.Webdriver.Core;

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

        public void Search(string searchTerm)
        {
            this._googleHomeUI.SearchInputBox.SendKeys(searchTerm);
            this._googleHomeUI.SearchInputBox.Submit();
        }

    }
}
