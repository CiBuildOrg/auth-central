using Fsw.Enterprise.AuthCentral.Webdriver.Core.Extensions;
using Fsw.Enterprise.AuthCentral.WebDriver;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fsw.Enterprise.AuthCentral.Webdriver.Core
{
    public abstract class PageObjectBase
    {
        protected int TIMEOUT_IN_SECONDS = 10;
        protected IWebDriver Driver;
        protected WebDriverWait Wait;

        protected PageObjectBase(IWebDriver driver) {
            this.Driver = driver;
        }

        protected TimeSpan GetTimeSpan() {
            return TimeSpan.FromSeconds(TIMEOUT_IN_SECONDS);
        }
    }
}
