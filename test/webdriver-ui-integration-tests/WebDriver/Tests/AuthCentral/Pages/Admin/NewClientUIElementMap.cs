using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.Pages.Admin
{
    class NewClientUIElementMap
    {
        [FindsBy(How = How.Id, Using = "ClientId")]
        public IWebElement ClientIdBox;
        [FindsBy(How = How.Id, Using = "ClientName")]
        public IWebElement ClientNameBox;
        [FindsBy(How = How.Id, Using = "ClientUri")]
        public IWebElement ClientUriBox;
        [FindsBy(How = How.Id, Using = "LogoUri")]
        public IWebElement LogoUriBox;
        [FindsBy(How = How.Id, Using = "SecretValue")]
        public IWebElement SecretValueBox;
        [FindsBy(How = How.Id, Using = "SecretDescription")]
        public IWebElement SecretDescriptionBox;
        [FindsBy(How = How.Id, Using = "SecretExpiration")]
        public IWebElement SecretExpirationBox;
        [FindsBy(How = How.Id, Using = "SecretType")]
        public IWebElement SecretTypeBox;
        [FindsBy(How = How.Id, Using = "RedirectUri")]
        public IWebElement RedirectUriBox;
        [FindsBy(How = How.Id, Using = "PostLogoutUri")]
        public IWebElement PostLogoutUriBox;
        [FindsBy(How = How.CssSelector, Using = "input.btn.btn-success")]
        public IWebElement CreateButton;
    }
}
