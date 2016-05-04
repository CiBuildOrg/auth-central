using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.UIMaps.Outlook
{
    public class OwaLoggedInUIElementMap
    {
        [FindsBy(How = How.XPath, Using = "//div[@class='_lv_O']/span")]
        public IWebElement EmailTimeStamp;
        [FindsBy(How = How.Id, Using = "confirm")]
        public IWebElement ConfirmationLink;
    }
}
