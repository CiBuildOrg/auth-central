using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;

namespace Fsw.Enterprise.AuthCentral.Webdriver.Core
{
    public class FirefoxTestFixture : TestFixtureBase
    {
        public FirefoxTestFixture()
        {
            this.Driver = new FirefoxDriver();
        }
    }
}
