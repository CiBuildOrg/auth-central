using Fsw.Enterprise.AuthCentral.Webdriver.Core;
using Fsw.Enterprise.AuthCentral.Webdriver.Core.Extensions;
using Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.UIMaps.Outlook;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.Pages.Outlook
{
    public class OwaLoggedInPage : PageObjectBase
    {
        private OwaLoggedInUIElementMap _owaLoggedInUI;

        public OwaLoggedInPage(IWebDriver driver) : base(driver)
        {
            _owaLoggedInUI = new OwaLoggedInUIElementMap();
            PageFactory.InitElements(driver, _owaLoggedInUI);
        }
        public OwaLoggedInUIElementMap Map { get { return _owaLoggedInUI; } }

        public string GetAccountCreatedUrl()
        {
            // complicated xpath to find a field which contains text that matches the currentTime (or one minute from now),
            // which belongs to an email that also has the subject "Account Created"
            string currentTime = DateTime.Now.ToString("HH:mm");
            string oneMinuteFromNow = DateTime.Now.AddMinutes(1).ToString("HH:mm");
            // waiting for the email with the correct time stamp and subject line to land in inbox
            for (int i = 0; i <= 10; i++ )
            {
                try
                {
                    
                    IWebElement CurrentTime_AccountCreatedEmail = Driver.TryFindElement(By.XPath("//span[.='" + currentTime + "']/../../div/span[contains(.,'Account Created')]"));
                    IWebElement OneMinuteFromNow_AccountCreatedEmail = Driver.TryFindElement(By.XPath("//span[.='" + oneMinuteFromNow + "']/../../div/span[contains(.,'Account Created')]"));
                    var a = CurrentTime_AccountCreatedEmail;
                    if (CurrentTime_AccountCreatedEmail != null || OneMinuteFromNow_AccountCreatedEmail != null)
                    {
                        break;
                    }
                }
                catch(NoSuchElementException)
                {
                    if (i == 30)
                    {
                        throw new NullReferenceException("Email from " + currentTime + " or " + oneMinuteFromNow + " not found");
                    }
                    Driver.Navigate().Refresh();
                    continue;
                }
            }
            string confirmEmailUrl = _owaLoggedInUI.ConfirmationLink.GetAttribute("href");
            return confirmEmailUrl;
        }
        
    }
}
