using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using System;

namespace Fsw.Enterprise.AuthCentral.Webdriver.Core
{
    public class FirefoxTestFixture : TestFixtureBase
    {
        public FirefoxTestFixture()
        {
            // use this line instead to run firefox locally
            // this.Driver = new FirefoxDriver();
            this.Driver = new RemoteWebDriver(
                new Uri("http://fswstgpssvc01.foodservicewarehouse.com:4444/wd/hub"),
                DesiredCapabilities.Firefox()
            );
        }
    }
}
