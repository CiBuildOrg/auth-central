using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using Fsw.Enterprise.AuthCentral.Webdriver.Core;

namespace Fsw.Enterprise.AuthCentral.Webdriver.ExampleTests.Google
{
    public class GmailLogInPageObject : PageObjectBase
    {
        private GmailHomeUIMap _googleHomeUI;

        public GmailLogInPageObject(IWebDriver driver) : base (driver)
        {
            PageFactory.InitElements(driver, _googleHomeUI = new GmailHomeUIMap()); //This will initialize all elements in the UI map for later use.
            //other wise we continue with the instantiation of the class.
        }

        public void Search(string searchTerm)
        {
            this._googleHomeUI.SearchInputBox.SendKeys(searchTerm);
            this._googleHomeUI.SearchInputBox.Submit();
        }

    }
}
