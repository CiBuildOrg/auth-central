using Fsw.Enterprise.AuthCentral.Webdriver.Core;
using Xunit;

namespace Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral
{
    [Collection("TestsMain")]
    public class AuthCentralTestsFirefoxLauncher : AuthCentralTestsMain, IClassFixture<FirefoxTestFixture>
    {
        public AuthCentralTestsFirefoxLauncher(FirefoxTestFixture fixture) : base(fixture) { }
    }

    [Collection("TestsMain")]
    public class AuthCentralTestsChromeLauncher : AuthCentralTestsMain, IClassFixture<ChromeTestFixture>
    {
        public AuthCentralTestsChromeLauncher(ChromeTestFixture fixture) : base(fixture) { }
    }
}
