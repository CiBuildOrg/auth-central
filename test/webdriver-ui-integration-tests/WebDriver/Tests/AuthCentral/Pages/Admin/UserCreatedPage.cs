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
    class UserCreatedPage : LoggedInAdminPageObjectBase
    {
        private UserCreatedUIElementsMap _usercreatedUI;

        public UserCreatedPage(IWebDriver driver) : base(driver)
        {
            _usercreatedUI = new UserCreatedUIElementsMap();
            PageFactory.InitElements(driver, _usercreatedUI);
        }

        internal UserCreatedUIElementsMap Map { get { return _usercreatedUI; } }
    }
}
