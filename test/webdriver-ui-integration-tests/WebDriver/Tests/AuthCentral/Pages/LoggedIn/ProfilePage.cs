using Fsw.Enterprise.AuthCentral.Webdriver.Core;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.Pages.LoggedIn
{
    public class ProfilePage : PageObjectBase
    {
        private ProfileUIElementMap _profileUI;

        public ProfilePage(IWebDriver driver) : base (driver)
        {
            _profileUI = new ProfileUIElementMap();
            PageFactory.InitElements(driver, _profileUI);
        }

        internal ProfileUIElementMap Map { get { return _profileUI; } }

        internal ProfilePage ExpandNameForm()
        {
            _profileUI.NameEditLink.Click();
            return this;
        }

        internal ProfilePage UpdateName(string firstName, string lastName)
        {
            _profileUI.FirstNameBox.Clear();
            _profileUI.FirstNameBox.SendKeys(firstName);
            _profileUI.LastNameBox.Clear();
            _profileUI.LastNameBox.SendKeys(lastName);
            _profileUI.NameSaveButton.Click();
            return this;
        }
    }
}
