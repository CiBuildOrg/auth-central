using Fsw.Enterprise.AuthCentral.Webdriver.Core;
using Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.UIMaps.Admin;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.Pages.Admin
{
    public class NewUserPage : PageObjectBase
    {

        private AdminNewUserUIElementMap _newuserUI;

        public NewUserPage(IWebDriver driver) : base(driver)
        {
            _newuserUI = new AdminNewUserUIElementMap();
            PageFactory.InitElements(driver, _newuserUI);
        }

        internal AdminNewUserUIElementMap Map { get { return _newuserUI; } }

        internal UserCreatedPage CreateNewAccount(string username, string email, string firstname, string lastname, string organization, string department, bool isadmin)
        {

            _newuserUI.UsernameBox.Clear();
            _newuserUI.UsernameBox.SendKeys(username);
            _newuserUI.EmailBox.Clear();
            _newuserUI.EmailBox.SendKeys(email);
            _newuserUI.FirstNameBox.Clear();
            _newuserUI.FirstNameBox.SendKeys(firstname);
            _newuserUI.LastNameBox.Clear();
            _newuserUI.LastNameBox.SendKeys(lastname);
            _newuserUI.OrganizationBox.Clear();
            _newuserUI.OrganizationBox.SendKeys(organization);
            _newuserUI.DepartmentBox.Clear();
            _newuserUI.DepartmentBox.SendKeys(department);

            if (isadmin)
            {
                _newuserUI.IsAdminCheckbox.Click();
            }

            _newuserUI.CreateAccountButton.Click();
            return new UserCreatedPage(Driver);
        }

    }
}
