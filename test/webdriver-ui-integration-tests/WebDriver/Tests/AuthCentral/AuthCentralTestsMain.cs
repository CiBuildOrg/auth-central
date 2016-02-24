using Fsw.Enterprise.AuthCentral.Webdriver.Core;
using Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.Pages.Public;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Security.Policy;

namespace Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral
{
    public abstract partial class AuthCentralTestsMain : TestClassBase
    {
        private static readonly EnvConfig _config;
        private TestFixtureBase _fixture;
        private LoginPage _page;

        static AuthCentralTestsMain()
        {
            _config = new EnvConfig();
        }

        public AuthCentralTestsMain(TestFixtureBase fixture)
        {
            this.Fixture = fixture;
            this.Fixture.SetUp(string.IsNullOrWhiteSpace(_config.RootUrl)? "https://secure.dev-fsw.com/" : _config.RootUrl);
            
            Page = new LoginPage(fixture.Driver);
            Wait = new WebDriverWait(fixture.Driver, GetTimeout());
        }
        
        public TestFixtureBase Fixture
        {
            get
            {
                return this._fixture;
            }
            private set
            {
                if (value == null)
                {
                    throw new NullReferenceException();
                }

                this._fixture = value;
            }
        }

        protected LoginPage Page
        {
            get
            {
                return _page;
            }
            private set
            {
                if (value == null)
                {
                    throw new NullReferenceException();
                }

                _page = value;
            }
        }
    }
}