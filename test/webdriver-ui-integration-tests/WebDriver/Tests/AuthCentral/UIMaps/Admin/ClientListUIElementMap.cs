using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.UIMaps.Admin
{
    class ClientListUIElementMap
    {
        [FindsBy(How = How.Id, Using = "clientId")]
        public IWebElement ClientIdBox;
        [FindsBy(How = How.CssSelector, Using = "css=button.btn.btn-primary")]
        public IWebElement ClientSearchButton;
        [FindsBy(How = How.LinkText, Using = "+ Create a New Client")]
        public IWebElement CreateClientButton;
    }
}
