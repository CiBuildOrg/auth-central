using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.UIMaps.Public
{
    public class RegisterUIElementMap
    {
        [FindsBy(How = How.Id, Using = "Username")]
        public IWebElement UsernameBox;
        [FindsBy(How = How.Id, Using = "Email")]
        public IWebElement EmailBox;
        [FindsBy(How = How.Id, Using = "GivenName")]
        public IWebElement FirstNameBox;
        [FindsBy(How = How.Id, Using = "FamilyName")]
        public IWebElement LastNameBox;
        [FindsBy(How = How.Id, Using = "Password")]
        public IWebElement PasswordBox;
        [FindsBy(How = How.Id, Using = "ConfirmPassword")]
        public IWebElement ConfirmPasswordBox;
        [FindsBy(How = How.XPath, Using = "//button[.='Create an Account']")]
        public IWebElement CreateAccountButton;
        [FindsBy(How = How.ClassName, Using = "container body-content")]
        public IWebElement PageText;
    }
}
