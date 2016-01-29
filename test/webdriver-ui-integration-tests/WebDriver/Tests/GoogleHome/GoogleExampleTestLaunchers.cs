using Xunit;

using OpenQA.Selenium.Support.UI;

using Fsw.Enterprise.AuthCentral.Webdriver.Core;
using OpenQA.Selenium.Chrome;

namespace Fsw.Enterprise.AuthCentral.Webdriver.ExampleTests.GoogleHome
{
    public class GoogleExampleTestsFirefoxLauncher : GoogleExampleTestsBase, IClassFixture<FirefoxTestFixture>
    {
        public GoogleExampleTestsFirefoxLauncher(FirefoxTestFixture fixture): base(fixture) { }
    }

    public class GoogleExampleTestsChromeLauncher : GoogleExampleTestsBase, IClassFixture<ChromeTestFixture>
    {
        public GoogleExampleTestsChromeLauncher(ChromeTestFixture fixture): base(fixture) { }
    }
}