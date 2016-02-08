using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.Pages
{
    class UserClaimsUIElementMap : UserUIElementMap
    {
        [FindsBy(How = How.LinkText, Using = "Create Claim")]
        public IWebElement CreateClaimButton;
        [FindsBy(How = How.XPath, Using = "//input[contains(@value,'ecommadmin')]/../../../following-sibling::div/div/input[@value='Delete']")]
        public IWebElement DeleteEcommAdminButton;
        [FindsBy(How = How.XPath, Using = "//input[contains(@value,'authcentral:admin')]/../../../following-sibling::div/div/input[@value='Delete']")]
        public IWebElement DeleteAuthCentralAdminButton;
    }
}
