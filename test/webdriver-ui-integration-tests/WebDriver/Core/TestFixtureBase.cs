using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.IO;

namespace Fsw.Enterprise.AuthCentral.Webdriver.Core
{
    public abstract class TestFixtureBase : IDisposable
    {
        private IWebDriver _driver;

        public IWebDriver Driver
        {
            get
            {
                //Sadly this will have to wait until we can find a way to only fire an event on an UnhandledException.
                //Currently it will fire the event on any exception which is not ideal because some times we catch those and just swallow them
                //for the sake of iteration of collections.
                // We may be able to use this for logging though, so whenever a button is clicked, or a field filled out we can log that.

                //var firing_driver = new EventFiringWebDriver(driver);
                //var eventHandler = new FiringDriverEvents(driver);

                //firing_driver.ExceptionThrown += eventHandler.TakeScreenshot;
                //driver = firing_driver; 

                return _driver;
            }
            protected set
            {
                _driver = value;
            }
        }

        /// <summary>
        /// Base setup for tests. Creates a new DriverInstance if needed, and will pass out the driver instance for the tests to use.
        /// This also deletes all cookies and sets the Archer cookie.
        /// SetUp is called at the beginning of every test, 
        /// </summary>
        /// <param name="url">http://www.stg-fsw.com</param>
        /// <param name="timeOutInSeconds">5</param>
        public void SetUp(string url = "http://www.dev-fsw.com", int timeOutInSeconds = 3)
        {
            Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(timeOutInSeconds));
            Driver.Manage().Window.Maximize();

            //Clean up from the previous test. We don't currently store anything in local storage, so cookies are enough.
            Driver.Manage().Cookies.DeleteAllCookies();
            Driver.Navigate().GoToUrl(url);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.Collect();
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Driver != null)
                {
                    try
                    {
                        Driver.Dispose();
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
        }

        protected virtual string GetDriverDir()
        {
            return Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\Drivers"));
        }
    }
}
