using OpenQA.Selenium.Remote;
using System;

namespace Fsw.Enterprise.AuthCentral.Webdriver.Core
{
    public class ChromeTestFixture : TestFixtureBase
    {
        public ChromeTestFixture()
        {
            // use this line instead to run chrome locally
            // this.Driver = new ChromeDriver();
            this.Driver = new RemoteWebDriver(
                new Uri("http://fswstgpssvc01.foodservicewarehouse.com:4444/wd/hub"), 
                DesiredCapabilities.Chrome()
            );
        }
    }
}
