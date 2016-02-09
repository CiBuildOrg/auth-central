using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.UIMaps.Admin
{
    class AdminNewUserUIElementMap
    {
        [FindsBy(How = How.Id, Using = "Username")]
        public IWebElement UsernameBox;
        [FindsBy(How = How.Id, Using = "Email")]
        public IWebElement EmailBox;
        [FindsBy(How = How.Id, Using = "GivenName")]
        public IWebElement FirstNameBox;
        [FindsBy(How = How.Id, Using = "FamilyName")]
        public IWebElement LastNameBox;
        [FindsBy(How = How.Id, Using = "IsAuthCentralAdmin")]
        public IWebElement IsAdminCheckbox;
        [FindsBy(How = How.Id, Using = "Organization")]
        public IWebElement OrganizationBox;
        [FindsBy(How = How.Id, Using = "Department")]
        public IWebElement DepartmentBox;
        [FindsBy(How = How.XPath, Using = "//button[.='Create Account']")]
        public IWebElement CreateAccountButton;
    }
}
