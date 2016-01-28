using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Fsw.Enterprise.AuthCentral.Webdriver.Core.Helpers
{
    public class CookieHelpers : ICookieJar
    {
        private IWebDriver _driver;
        public CookieHelpers(IWebDriver driver)
        {
            _driver = driver;
        }

        public ReadOnlyCollection<Cookie> AllCookies
        {
            get
            {
                return _driver.Manage().Cookies.AllCookies;
            }
        }

        public void AddCookie(Cookie cookie)
        {
            if (cookie != null && cookie.Name != null && cookie.Value != null)
            {
                _driver.Manage().Cookies.AddCookie(cookie);
            }
            else
            {
                throw new Exception("Cookie value cannot be set.");
            }
        }

        public Cookie GetCookieNamed(string name)
        {
            return _driver.Manage().Cookies.GetCookieNamed(name);
        }

        public void DeleteCookie(Cookie cookie)
        {
            throw new NotImplementedException();
        }

        public void DeleteCookieNamed(string name)
        {
            throw new NotImplementedException();
        }

        public void DeleteAllCookies()
        {
            throw new NotImplementedException();
        }

        public bool IsUserLoggedIn()
        {
            List<Cookie> cookies = AllCookies.ToList();
            return cookies.Where(c => c.Name == "loggedIn").Count() >= 1 ? true : false;
        }
    }
}
