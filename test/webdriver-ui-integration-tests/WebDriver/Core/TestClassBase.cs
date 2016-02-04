using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace Fsw.Enterprise.AuthCentral.Webdriver.Core
{
    public abstract class TestClassBase
    {
        protected static int TIMEOUT_IN_SECONDS = 10;

        //We can store fields that all test classes should be able to have to consolidate some code.
        //Only thing is that it may obscure some things.  
        protected WebDriverWait Wait;

        protected TimeSpan GetTimeout() {
            return TimeSpan.FromSeconds(TIMEOUT_IN_SECONDS);
        }
    }
}
