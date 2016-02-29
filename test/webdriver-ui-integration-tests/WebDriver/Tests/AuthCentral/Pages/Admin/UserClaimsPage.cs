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
    public class UserClaimsPage : PageObjectBase
    {

        private UserClaimsUIElementMap _userClaimsUI;

        public UserClaimsPage(IWebDriver driver) : base(driver)
        {
            _userClaimsUI = new UserClaimsUIElementMap();
            PageFactory.InitElements(driver, _userClaimsUI);
        }

        internal UserClaimsUIElementMap Map { get { return _userClaimsUI; } }

        public bool IsOnUserClaimsPage()
        {
            try { return _userClaimsUI.CreateClaimButton.Displayed; }
            catch { return false; }
        }
    }
}