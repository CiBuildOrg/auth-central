using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fsw.Enterprise.AuthCentral.Webdriver.Core.Helpers
{
    public class DriverHelpers
    {
        private IWebDriver _driver;
        public DriverHelpers(IWebDriver driver)
        {
            _driver = driver;
        }

        public static void GoToPage(IWebDriver driver, string url)
        {
            if (url.StartsWith("/"))
            {
                driver.Url += url.Remove(0, 1);
            }
            else
            {
                driver.Url += url;
            }
        }
        public void RemoveImplicitWait()
        {
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
        }
        public void RefreshPage()
        {
            _driver.Navigate().Refresh();
        }

        public void ScrollTo(IWebElement element)
        {
            var height = _driver.Manage().Window.Size.Height - 15;
            var newYPosition = element.Location.Y > height ? element.Location.Y - -(element.Location.Y - (height + 100)) : 0;
            var js = $"window.scrollTo({element.Location.X}, {newYPosition})";
            if (newYPosition != 0)
            {
                IJavaScriptExecutor jsExecutor = _driver as IJavaScriptExecutor;
                jsExecutor.ExecuteScript(js); 
            }
        }
    }
}
