using Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.UIMaps.Admin;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.UIMaps.LoggedIn
{
    class ProfileUIElementMap : UserUIElementMap
    {
        [FindsBy(How = How.XPath, Using = "//div[@id='nameText']/div/div")]
        public IWebElement NameDiv;
        [FindsBy(How = How.XPath, Using = "//div[@id='emailText']/div/div")]
        public IWebElement EmailDiv;
        [FindsBy(How = How.XPath, Using = "//div[@id='nameText']/div/div/a[.='edit']")]
        public IWebElement NameEditLink;
        [FindsBy(How = How.XPath, Using = "//div[@id='emailText']/div/div/a[.='edit']")]
        public IWebElement EmailEditLink;
        [FindsBy(How = How.XPath, Using = "//div[@id='passwordText']/div/div/a[.='edit']")]
        public IWebElement PasswordEditLink;
        [FindsBy(How = How.Id, Using = "Name_GivenName")]
        public IWebElement FirstNameBox;
        [FindsBy(How = How.Id, Using = "Name_FamilyName")]
        public IWebElement LastNameBox;

        [FindsBy(How = How.Id, Using = "Password_OldPassword")]
        public IWebElement OldPasswordBox;
        [FindsBy(How = How.Id, Using = "Password_NewPassword")]
        public IWebElement NewPasswordBox;
        [FindsBy(How = How.Id, Using = "Password_NewPasswordConfirm")]
        public IWebElement NewPasswordConfirmBox;
        [FindsBy(How = How.XPath, Using = "//input[@value='Change Password'] | //button[.='Change Password']")]
        public IWebElement ChangePasswordButton;

        [FindsBy(How = How.Id, Using = "Email")]
        public IWebElement EmailBox;

        [FindsBy(How = How.CssSelector, Using = "div#nameForm input[type=submit]")]
        public IWebElement NameSaveButton;

        [FindsBy(How = How.XPath, Using = "(//input[@value='Save'])[2]")]
        public IWebElement EmailSaveButton;

        [FindsBy(How = How.CssSelector, Using = "div.col-xs-12.text-center > h1")]
        public IWebElement AccountDetailsHeader;
        [FindsBy(How = How.CssSelector, Using = "a.dropdown-toggle")]
        public IWebElement MainMenuLink;
    }
}
