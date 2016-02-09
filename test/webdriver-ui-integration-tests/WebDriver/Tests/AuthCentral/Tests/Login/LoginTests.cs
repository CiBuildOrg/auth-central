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
            ProfilePage profilePage = Page.Login("exampleuser", "FSWis#1");
            Assert.True(profilePage.Map.AccountDetailsHeader.Displayed);
            Assert.Equal("Account Details", profilePage.Map.AccountDetailsHeader.Text);
        }
    }
}