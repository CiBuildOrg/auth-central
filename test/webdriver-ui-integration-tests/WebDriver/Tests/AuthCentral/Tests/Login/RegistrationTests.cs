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
        [Fact(DisplayName = "Register as new user")]
        public void Register_CorrectCredentials_Succeeds()
        {
            string email = "test" + new Random().Next().ToString() + "@gmail.com";
            Page.Map.CreateAccountLink.Click();
            RegisterPage page = new RegisterPage(_fixture.Driver).Register("testuser", email, "Test123");
            Assert.Contains("Registration Success", page.Map.PageText.Text);
            Assert.Contains(email, page.Map.PageText.Text);
        }
    }
}