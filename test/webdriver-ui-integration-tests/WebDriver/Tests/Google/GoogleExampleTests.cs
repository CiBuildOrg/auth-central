using Xunit;

using OpenQA.Selenium.Support.UI;

using Fsw.Enterprise.AuthCentral.Webdriver.Core;
using OpenQA.Selenium.Firefox;

namespace Fsw.Enterprise.AuthCentral.Webdriver.ExampleTests.Google
{
    public abstract class GoogleExampleTests : TestClassBase
    //using IClassFixture allows us to create a shared context between tests. https://xunit.github.io/docs/shared-context.html
    {
        protected static string URL = "https://www.google.com/";
        protected GmailLogInPageObject _googleHome;

        //we reference the shared context from IClassFixture above inside the test class constructor.
        //The following test loosely follows the Page Object driven test described here : https://code.google.com/p/selenium/wiki/PageObjects
        [Fact]
        public void CanSearchForStuff()
        {
            this._googleHome.Search("Hello Google!");
        }
    }
}
