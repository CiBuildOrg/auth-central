using Xunit;

using OpenQA.Selenium.Support.UI;

using Fsw.Enterprise.AuthCentral.Webdriver.Core;
using OpenQA.Selenium.Chrome;

namespace Fsw.Enterprise.AuthCentral.Webdriver.ExampleTests.Google
{
    public class GoogleExampleTestsChromeLauncher : GoogleExampleTests, IClassFixture<ChromeTestFixture>
    //using IClassFixture allows us to create a shared context between tests. https://xunit.github.io/docs/shared-context.html
    {
        //we reference the shared context from IClassFixture above inside the test class constructor.
        public GoogleExampleTestsChromeLauncher(ChromeTestFixture fixture)
        {
            //and then we can call a setup method here which will be called before every test, 
            //but the reference to TestFixutre will persist.
            fixture.SetUp(URL);

            Wait = new WebDriverWait(fixture.Driver, GetTimeout());
            _googleHome = new GmailLogInPageObject(fixture.Driver);
        }

    }
}
