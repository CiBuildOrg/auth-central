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
            string username = "test" + new Random().Next().ToString();
            string email = username + "@gmail.com";
            Page.Map.CreateAccountLink.Click();
            RegisterPage page = new RegisterPage(_fixture.Driver).Register(username, email, "Test123");
            Assert.Contains("Registration Success", page.Map.PageText.Text);
            Assert.Contains(email, page.Map.PageText.Text);
        }
        [Fact(DisplayName = "Register fails without username")]
        public void Register_NoUsername_Fails()
        {
            string email = "test" + new Random().Next().ToString() + "@gmail.com";
            Page.Map.CreateAccountLink.Click();
            RegisterPage page = new RegisterPage(_fixture.Driver).Register("", email, "Test123");
            Assert.Equal("Username is required.", page.Map.ErrorMessage.Text);
        }
        [Fact(DisplayName = "Register fails without email")]
        public void Register_NoEmail_Fails()
        {
            string username = "test" + new Random().Next().ToString();
            Page.Map.CreateAccountLink.Click();
            RegisterPage page = new RegisterPage(_fixture.Driver).Register(username, "", "Test123");
            Assert.Equal("The Email field is required.", page.Map.ErrorMessage.Text);
        }
        [Fact(DisplayName = "Register fails without password")]
        public void Register_NoPassword_Fails()
        {
            string username = "test" + new Random().Next().ToString();
            string email = username + "@gmail.com";
            Page.Map.CreateAccountLink.Click();
            RegisterPage page = new RegisterPage(_fixture.Driver).Register(username, email, "");
            Assert.Equal("The Password field is required.", page.Map.ErrorMessage.Text);
        }
        [Fact(DisplayName = "Register fails if passwords don't match")]
        public void Register_NonMatchingPasswords_Fails()
        {
            string username = "test" + new Random().Next().ToString();
            string email = username + "@gmail.com";
            Page.Map.CreateAccountLink.Click();
            RegisterPage page = new RegisterPage(_fixture.Driver).Register(username, email, "Test123", "Tset123");
            Assert.Equal("Password confirmation must match password.", page.Map.ErrorMessage.Text);
        }
    }
}