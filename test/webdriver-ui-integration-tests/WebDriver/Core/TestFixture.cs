using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;

namespace Fsw.Enterprise.AuthCentral.Webdriver.Core
{
    public class TestFixture : IDisposable
    {
        private IWebDriver _driver;

        public IWebDriver Driver
        {
            get
            {
                //Sadly this will have to wait until we can find a way to only fire an event on an UnhandledException.
                //Currently it will fire the event on any exception which is not idea because some times we catch those and just swallow them
                //for the sake of iteration of collections.
                // We may be able to use this for logging though, so whenever a button is clicked, or a field filled out we can log that.

                //var firing_driver = new EventFiringWebDriver(driver);
                //var eventHandler = new FiringDriverEvents(driver);

                //firing_driver.ExceptionThrown += eventHandler.TakeScreenshot;
                //driver = firing_driver; 

                return _driver;
            }
        }

        /// <summary>
        /// Base setup for tests. Creates a new DriverInstance if needed, and will pass out the driver instance for the tests to use.
        /// This also deletes all cookies and sets the Archer cookie.
        /// SetUp is called at the beginning of every test, 
        /// </summary>
        /// <param name="url">http://www.stg-fsw.com</param>
        /// <param name="timeOutInSeconds">5</param>
        public void SetUp(IWebDriver driver, string url = "http://www.dev-fsw.com", int timeOutInSeconds = 3)
        {
            if(driver == null)
            {
                throw new ArgumentNullException();
            }
            _driver = driver;

            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(timeOutInSeconds));
            _driver.Manage().Window.Maximize();

            //Clean up from the previous test. We don't currently store anything in local storage, so cookies are enough.
            _driver.Manage().Cookies.DeleteAllCookies();
            _driver.Navigate().GoToUrl(url);
        }

        bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.Collect();
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            if (disposing)
            {
                if (_driver != null)
                {
                    _driver.Dispose();
                }
            }
            disposed = true;
        }
    }
}
