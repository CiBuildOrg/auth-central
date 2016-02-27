using Fsw.Enterprise.AuthCentral.Webdriver.Core;
using Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.Pages.Admin;
using Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.Pages.LoggedIn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral
{
    public abstract partial class AuthCentralTestsMain : TestClassBase
    {
        [Fact(DisplayName = "Create New User as admin")]
        public void Admin_CreateNewUser_FailValidation()
        {
            NewUserPage newuserpage = Page.Login(_config.AdminUser, _config.AdminPassword).ClickManageUsersLink().ClickCreateUserButton();
            newuserpage.CreateNewAccount(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, false);
            NewUserPage faileduserpage = new NewUserPage(_fixture.Driver);
            Assert.Equal("The Email field is required.", faileduserpage.Map.ValidationErrors.FindElement(OpenQA.Selenium.By.XPath("//span[@data-valmsg-for='Email']")).Text);
        }
    }
}
