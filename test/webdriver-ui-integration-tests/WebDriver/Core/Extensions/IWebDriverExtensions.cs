using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fsw.Enterprise.AuthCentral.Webdriver.Core.Extensions
{
    public static class IWebDriverExtensions
    {
        public static void GoToPage(this IWebDriver driver, string url)
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
        public static void RemoveImplicitWait(this IWebDriver driver)
        {
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
        }
        public static void RefreshPage(this IWebDriver driver)
        {
            driver.Navigate().Refresh();
        }

        public static void ScrollTo(this IWebDriver driver, IWebElement element)
        {
            var height = driver.Manage().Window.Size.Height - 15;
            var newYPosition = element.Location.Y > height ? element.Location.Y - -(element.Location.Y - (height + 100)) : 0;
            var js = $"window.scrollTo({element.Location.X}, {newYPosition})";
            if (newYPosition != 0)
            {
                IJavaScriptExecutor jsExecutor = driver as IJavaScriptExecutor;
                jsExecutor.ExecuteScript(js); 
            }
        }
        public static ReadOnlyCollection<Cookie> GetAllCookies(this IWebDriver driver)
        {
            return driver.Manage().Cookies.AllCookies;
        }

        public static void AddCookie(this IWebDriver driver, Cookie cookie)
        {
            if (cookie != null && cookie.Name != null && cookie.Value != null)
            {
                driver.Manage().Cookies.AddCookie(cookie);
            }
            else
            {
                throw new Exception("Cookie value cannot be set.");
            }
        }

        public static Cookie GetCookieNamed(this IWebDriver driver, string name)
        {
            return driver.Manage().Cookies.GetCookieNamed(name);
        }


    }
}
