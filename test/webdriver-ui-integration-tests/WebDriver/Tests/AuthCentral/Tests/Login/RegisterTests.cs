﻿using Fsw.Enterprise.AuthCentral.Webdriver.Core;
using Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.Pages.Admin;
using Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.Pages.LoggedIn;
using Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.Pages.Outlook;
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
            RegisterPage page = new RegisterPage(_fixture.Driver).DeleteAndRegisterNewUser(_config);
            Assert.Contains("Registration Success", page.Map.PageText.Text);
            Assert.Contains(_config.NewUserEmail, page.Map.PageText.Text);
        }
        [Fact(DisplayName = "Register fails without username")]
        public void Register_NoUsername_Fails()
        {
            RegisterPage page = new RegisterPage(_fixture.Driver).Register(_config,"",_config.NewUserEmail,_config.NewUserPassword);
            Assert.Contains("Username is required.", page.Map.ErrorMessage.Text);
        }
        [Fact(DisplayName = "Register fails without email")]
        public void Register_NoEmail_Fails()
        {
            RegisterPage page = new RegisterPage(_fixture.Driver).Register(_config, _config.NewUserUsername,"", _config.NewUserPassword);
            Assert.Contains("The Email field is required.", page.Map.ErrorMessage.Text);
        }
        [Fact(DisplayName = "Register fails without password")]
        public void Register_NoPassword_Fails()
        {
            RegisterPage page = new RegisterPage(_fixture.Driver).Register(_config, _config.NewUserUsername, _config.NewUserEmail,"","");
            Assert.Contains("The Password field is required.", page.Map.ErrorMessage.Text);
        }
        [Fact(DisplayName = "Register fails if passwords don't match")]
        public void Register_NonMatchingPasswords_Fails()
        {
            RegisterPage page = new RegisterPage(_fixture.Driver).Register(_config, _config.NewUserUsername, _config.NewUserEmail,_config.NewUserPassword, "12345wrongPW!");
            Assert.Contains("Password confirmation must match password.", page.Map.ErrorMessage.Text);
        }
        [Fact(DisplayName = "Register fails if username already exists")]
        public void Register_UsernameAlreadyExists_Fails()
        {
            RegisterPage page = new RegisterPage(_fixture.Driver).DeleteAndRegisterNewUser(_config);
            _fixture.Driver.Navigate().GoToUrl(_config.RootUrl);
            page.Register(_config,_config.NewUserUsername,"thisUserShouldntExist@gmail.com",_config.NewUserPassword);
            Assert.Contains("Username already in use.", page.Map.ErrorMessage.Text);
        }
        [Fact(DisplayName = "Register fails if email already exists")]
        public void Register_EmailAlreadyExists_Fails()
        {
            RegisterPage page = new RegisterPage(_fixture.Driver).DeleteAndRegisterNewUser(_config);
            _fixture.Driver.Navigate().GoToUrl(_config.RootUrl);
            page.Register(_config, "ThisUserShouldntExist", _config.NewUserEmail, _config.NewUserPassword);
            Assert.Contains("Email already in use.", page.Map.ErrorMessage.Text);
        }
        [Fact(DisplayName = "Register and login")]
        public void RegisterAndLogin_Succeeds()
        {
            RegisterPage registerPage = new RegisterPage(_fixture.Driver).DeleteAndRegisterNewUser(_config);
            OwaLoggedInPage outlook = new OwaLoginPage(_fixture.Driver).Login(_config.NewUser_Outlook_Username, _config.NewUser_Outlook_Password);
            string confirmationUrl = outlook.GetAccountCreatedUrl();
            EmailConfirmationPage confirmationPage = new EmailConfirmationPage(_fixture.Driver).ConfirmEmail(confirmationUrl, _config.NewUserPassword);
            Assert.Contains("Success", registerPage.Map.PageText.Text);
            _fixture.Driver.Navigate().GoToUrl(_config.RootUrl);
            ProfilePage page = Page.Login(_config.NewUserUsername, _config.NewUserPassword);
            Assert.Contains("Account Details", page.Map.AccountDetailsHeader.Text);
        }
    }
}