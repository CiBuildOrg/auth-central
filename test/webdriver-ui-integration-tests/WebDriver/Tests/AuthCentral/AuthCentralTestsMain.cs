using Fsw.Enterprise.AuthCentral.Webdriver.Core;
using Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.Pages.LoggedIn;
using Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.Pages.Public;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using Xunit;

namespace Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral
{
    public abstract partial class AuthCentralTestsMain : TestClassBase
    {
        private const string URL = "https://secure.dev-fsw.com";
        private TestFixtureBase _fixture;
        private LoginPage _page;

        public AuthCentralTestsMain(TestFixtureBase fixture)
        {
            this.Fixture = fixture;
            this.Fixture.SetUp(URL);

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