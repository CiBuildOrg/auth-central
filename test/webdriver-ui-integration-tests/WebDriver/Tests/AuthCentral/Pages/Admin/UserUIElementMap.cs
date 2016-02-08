using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.Pages
{
    abstract class UserUIElementMap
    {
        [FindsBy(How = How.LinkText, Using = "Claims")]
        public IWebElement ClaimsMenuLink;
        [FindsBy(How = How.LinkText, Using = "Profile")]
        public IWebElement ProfileMenuLink;
    }
}
