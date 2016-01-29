using System;

using Xunit;

using Fsw.Enterprise.AuthCentral.Webdriver.Core;
using Fsw.Enterprise.AuthCentral.Webdriver.Core.Extensions;
using OpenQA.Selenium.Support.UI;

namespace Fsw.Enterprise.AuthCentral.Webdriver.ExampleTests.GoogleHome
{
    public abstract class GoogleExampleTestsBase : TestClassBase
    {
        private GoogleHomePage _page;
        private TestFixtureBase _fixture;
        private const string URL = "https://www.google.com/";

        public TestFixtureBase Fixture
        {
            get
            {
                return this._fixture;
            }
            private set
            {
                if(value == null)
                {
                    throw new NullReferenceException();
                }

                this._fixture = value;
            }
        }

        protected GoogleHomePage Page
        {
            get
            {
                return _page;
            }
            private set
            {
                if(value == null)
                {
                    throw new NullReferenceException();
                }

                _page = value;
            }
        }

        /// <summary>
        ///     Everything in this constructor will be called before every test
        /// 	and then we can call a setup method here which will be called before every test, 
        ///     but the reference to TestFixutre will persist.
        /// </summary>
        /// <param name="fixture"></param>
        protected GoogleExampleTestsBase(TestFixtureBase fixture)
        {
            this.Fixture = fixture;
            this.Fixture.SetUp(URL);

            Page = new GoogleHomePage(fixture.Driver);
            Wait = new WebDriverWait(fixture.Driver, GetTimeout());
        }

        /// <summary>
        ///     The following test loosely follows the Page Object driven test described here : https://code.google.com/p/selenium/wiki/PageObjects
        /// </summary>
        [Fact]
        public void CanSearchForStuff()
        {
            this.Page.Search("Hello GoogleHome!");
            this.Fixture.Driver.GoToPage("Https://www.google.com/");
        }
    }
}
