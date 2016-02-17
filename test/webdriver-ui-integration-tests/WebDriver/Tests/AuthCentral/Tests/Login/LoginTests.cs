using Fsw.Enterprise.AuthCentral.Webdriver.Core;
using Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.Pages.LoggedIn;
using Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.Pages.Public;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using Xunit;

namespace Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral
{
    public abstract partial class AuthCentralTestsMain : TestClassBase
    {        
        /// <summary>
        ///     The following test loosely follows the Page Object driven test described here : https://code.google.com/p/selenium/wiki/PageObjects
        /// </summary>
        [Fact(DisplayName = "Log in and load profile")]
        public void Login_CorrectCredentials_Succeeds()
        {
            ProfilePage profilePage = Page.Login(_config.AdminUser, _config.AdminPassword);
            Assert.True(profilePage.Map.AccountDetailsHeader.Displayed);
            Assert.Equal("Account Details", profilePage.Map.AccountDetailsHeader.Text);
        }
        [Fact(DisplayName = "Log in as admin")]
        public void Login_AsAdmin()
        {
            ProfilePage page = Page.Login("AutomationUser", "fs19!t?3h2@");
            Assert.True(page.Map.AccountDetailsHeader.Displayed);
            Assert.Equal("Account Details", page.Map.AccountDetailsHeader.Text);
        }
        [Fact(DisplayName = "Log in with wrong password")]
        public void Login_WrongPassword_Fails()
        {
            Page.Login("AutomationUser", "badPW");
            var FailedPage = new LoginPage(_fixture.Driver);
            Assert.True(FailedPage.Map.ErrorMessage.Displayed);
            Assert.Contains("Invalid username or password", FailedPage.Map.ErrorMessage.Text);
        }
        [Fact(DisplayName = "Log in with wrong username")]
        public void Login_WrongUsername_Fails()
        {
            Page.Login("AutomationLoser", "fs19!t?3h2@");
            var FailedPage = new LoginPage(_fixture.Driver);
            Assert.True(FailedPage.Map.ErrorMessage.Displayed);
            Assert.Contains("Invalid username or password", FailedPage.Map.ErrorMessage.Text);
        }
    }
}