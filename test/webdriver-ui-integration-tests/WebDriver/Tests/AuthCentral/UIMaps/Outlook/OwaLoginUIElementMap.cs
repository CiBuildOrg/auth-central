using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.UIMaps.Outlook
{
    public class OwaLoginUIElementMap
    {
        [FindsBy(How = How.Id, Using = "username")]
        public IWebElement UsernameBox;
        [FindsBy(How = How.Id, Using = "password")]
        public IWebElement PasswordBox;
        [FindsBy(How = How.ClassName, Using = "signinTxt")]
        public IWebElement SignInButton;
    }
}
