using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.Pages
{
    class AdminUsersUIElementMap
    {
        [FindsBy(How = How.LinkText, Using = "Create User »")]
        public IWebElement CreateUserButton;
        [FindsBy(How = How.Id, Using = "clientId")]
        public IWebElement EmailSearchBox;
        [FindsBy(How = How.CssSelector, Using = "button.btn.btn-primary")]
        public IWebElement EmailSearchButton;
        [FindsBy(How = How.XPath, Using = "//a[.='${AdminUser_Email}']/../../td/form/input[@value='Delete']")]
        public IWebElement NewUserDeleteButton;
        [FindsBy(How = How.XPath, Using = "//input[@value='Delete']")];
        public IWebElement DeleteButtons;
    }
}
