using Fsw.Enterprise.AuthCentral.Webdriver.Core;
using Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.Pages;
using OpenQA.Selenium.Support.UI;
using System;
using Xunit;

namespace Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral
{
    public abstract class AuthCentralTestsMain : TestClassBase
    {
        private LoginPage _page;
        private TestFixtureBase _fixture;
        private const string URL = "https://secure.dev-fsw.com";

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

        /// <summary>
        ///     Everything in this constructor will be called before every test
        /// 	and then we can call a setup method here which will be called before every test, 
        ///     but the reference to TestFixture will persist.
        /// </summary>
        /// <param name="fixture"></param>
        protected AuthCentralTestsMain(TestFixtureBase fixture)
        {
            this.Fixture = fixture;
            this.Fixture.SetUp(URL);

            Page = new LoginPage(fixture.Driver);
            Wait = new WebDriverWait(fixture.Driver, GetTimeout());
        }

        /// <summary>
        ///     The following test loosely follows the Page Object driven test described here : https://code.google.com/p/selenium/wiki/PageObjects
        /// </summary>
        [Fact]
        public void CanLogin()
        {
            var results = this.Page.Login("jburbage", "");
            Assert.NotEmpty(results);
        }
    }
}