using Fsw.Enterprise.AuthCentral.Webdriver.Core;
using Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.UIMaps.Public;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.Pages.Public
{
    public class LoggedOutPage : PageObjectBase
    {
        private LoginUIElementMap _loggedoutUI;

        public LoggedOutPage(IWebDriver driver) : base(driver)
        {
            _loggedoutUI = new LoginUIElementMap();
            PageFactory.InitElements(driver, _loggedoutUI);
        }
        public LoginUIElementMap Map { get { return _loggedoutUI; } }
    }
}
