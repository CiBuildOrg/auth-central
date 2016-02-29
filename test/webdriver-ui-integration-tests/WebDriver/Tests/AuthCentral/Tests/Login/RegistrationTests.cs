﻿using Fsw.Enterprise.AuthCentral.Webdriver.Core;
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
        [Fact(DisplayName = "Register fails if username already exists")]
        public void Register_UsernameAlreadyExists_Fails()
        {
            string username = "test" + new Random().Next().ToString();
            string email = username + "@gmail.com";
            string url = _fixture.Driver.Url.ToString();
            Page.Map.CreateAccountLink.Click();
            RegisterPage page = new RegisterPage(_fixture.Driver).Register(username, email, "Test123");
            Assert.Contains("Registration Success", page.Map.PageText.Text);
            _fixture.Driver.Navigate().GoToUrl(url);
            Page.Map.CreateAccountLink.Click();
            page.Register(username, "differentEmail@fakemail.com", "Test123");
            Assert.Equal("Username already in use.", page.Map.ErrorMessage.Text);
        }
        [Fact(DisplayName = "Register fails if email already exists")]
        public void Register_EmailAlreadyExists_Fails()
        {
            string username = "test" + new Random().Next().ToString();
            string email = username + "@gmail.com";
            string url = _fixture.Driver.Url.ToString();
            Page.Map.CreateAccountLink.Click();
            RegisterPage page = new RegisterPage(_fixture.Driver).Register(username, email, "Test123");
            Assert.Contains("Registration Success", page.Map.PageText.Text);
            _fixture.Driver.Navigate().GoToUrl(url);
            Page.Map.CreateAccountLink.Click();
            page.Register("DifferentUser", email, "Test123");
            Assert.Equal("Email already in use.", page.Map.ErrorMessage.Text);
        }
        [Fact(DisplayName = "Register and confirm email")]
        public void RegisterAndConfirmEmail_Succeeds()
        {
            string email = "automationuser@fsw.com";
            string otherEmail = "AUser2@fsw.com";
            Page.Login("AutomationUser", "fs19!t?3h2@");
            UserListPage userList = new UserListPage(_fixture.Driver).GotoManageUsers();
            userList.DeleteUserIfExists(email, otherEmail);
            ProfilePage profile = new ProfilePage(_fixture.Driver).Logout();
                        
            Page.Map.CreateAccountLink.Click();
            RegisterPage page = new RegisterPage(_fixture.Driver).Register("NewUser", "automationuser@fsw.com", "J3huh@8h$$");

        }
    }
}