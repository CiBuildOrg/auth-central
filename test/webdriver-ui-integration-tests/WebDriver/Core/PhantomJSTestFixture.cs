using OpenQA.Selenium.PhantomJS;
using System;

namespace Fsw.Enterprise.AuthCentral.Webdriver.Core
{
    public class PhantomJSTestFixture : TestFixtureBase
    {
        public PhantomJSTestFixture()
        {
            var driverService = PhantomJSDriverService.CreateDefaultService(this.GetDriverDir());
            driverService.HideCommandPromptWindow = true;
            driverService.LoadImages = false;
            driverService.ProxyType = "none";

            this.Driver = new PhantomJSDriver(driverService);
        }
    }
}
