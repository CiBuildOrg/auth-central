using Xunit;

using OpenQA.Selenium.Support.UI;

using Fsw.Enterprise.AuthCentral.Webdriver.Core;
using OpenQA.Selenium.Chrome;

namespace Fsw.Enterprise.AuthCentral.Webdriver.ExampleTests.GoogleHome
{
    public class GoogleExampleTestsFirefoxLauncher : GoogleExampleTestsMain, IClassFixture<FirefoxTestFixture>
    {
        public GoogleExampleTestsFirefoxLauncher(FirefoxTestFixture fixture): base(fixture) { }
    }

    public class GoogleExampleTestsChromeLauncher : GoogleExampleTestsMain, IClassFixture<ChromeTestFixture>
    {
        public GoogleExampleTestsChromeLauncher(ChromeTestFixture fixture): base(fixture) { }
    }
}