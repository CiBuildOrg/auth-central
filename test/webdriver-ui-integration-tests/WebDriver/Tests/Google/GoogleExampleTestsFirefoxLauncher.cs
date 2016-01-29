using Xunit;

using OpenQA.Selenium.Support.UI;

using Fsw.Enterprise.AuthCentral.Webdriver.Core;
using OpenQA.Selenium.Firefox;

namespace Fsw.Enterprise.AuthCentral.Webdriver.ExampleTests.Google
{
    public class GoogleExampleTestsFirefoxLauncher : GoogleExampleTests, IClassFixture<FirefoxTestFixture>
    //using IClassFixture allows us to create a shared context between tests. https://xunit.github.io/docs/shared-context.html
    {
        //we reference the shared context from IClassFixture above inside the test class constructor.
        public GoogleExampleTestsFirefoxLauncher(FirefoxTestFixture fixture)
        {
            //and then we can call a setup method here which will be called before every test, 
            //but the reference to TestFixutre will persist.
            fixture.SetUp(URL);

            Wait = new WebDriverWait(fixture.Driver, GetTimeout());
            _googleHome = new GmailLogInPageObject(fixture.Driver);
        }

    }
}
