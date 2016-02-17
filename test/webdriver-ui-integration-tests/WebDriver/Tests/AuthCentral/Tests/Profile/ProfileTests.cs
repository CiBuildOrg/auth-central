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
        [Fact(DisplayName = "Change Name")]
        public void NameChange_ValidInput_ChangesName()
        {
            ProfilePage profilePage = Page.Login(_config.AdminUser, _config.AdminPassword)
                                          .ExpandNameForm()
                                          .UpdateName("Example", "User");

            Assert.Equal("Example User", profilePage.Map.NameDiv.Text);

            profilePage.ExpandNameForm();
            Assert.Equal("Example", profilePage.Map.FirstNameBox.GetAttribute("value"));
            Assert.Equal("User", profilePage.Map.LastNameBox.GetAttribute("value"));
        }
    }
}