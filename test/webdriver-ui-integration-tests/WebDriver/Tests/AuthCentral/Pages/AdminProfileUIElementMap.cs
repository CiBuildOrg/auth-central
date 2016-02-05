using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.Pages
{
    class AdminProfileUIElementMap
    {
        [FindsBy(How = How.Id, Using = "GivenName")]
        public IWebElement FirstNameBox;
        [FindsBy(How = How.Id, Using = "FamilyName")]
        public IWebElement LastNameBox;
        [FindsBy(How = How.Id, Using = "Organization")]
        public IWebElement OrganizationBox;
        [FindsBy(How = How.Id, Using = "Department")]
        public IWebElement DepartmentBox;
        [FindsBy(How = How.XPath, Using = "//input[@value='Save']")]
        public IWebElement SaveButton;

        [FindsBy(How = How.Id, Using = "delete")]
        public IWebElement DeleteButton;

        [FindsBy(How = How.Id, Using ="Email")]
        public IWebElement EmailAddressBox;
        [FindsBy(How = How.XPath, Using = "//input[@value='Trigger Change Request']")]
        public IWebElement ChangeEmailButton;

        [FindsBy(How = How.Name, Using = "confirm")]
        public IWebElement DisableConfirmCheckbox;
        [FindsBy(How = How.XPath, Using = "//input[@value='Disable User']")]
        public IWebElement DisableButton;
    }
}
