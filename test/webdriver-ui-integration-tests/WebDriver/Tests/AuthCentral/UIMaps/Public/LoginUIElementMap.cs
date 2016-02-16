using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.UIMaps.Public

{
    public class LoginUIElementMap
    {
        [FindsBy(How = How.Id, Using = "username")]
        public IWebElement UsernameBox;
        [FindsBy(How = How.Id, Using = "password")]
        public IWebElement PasswordBox;
        [FindsBy(How = How.LinkText, Using = "Forgot Password?")]
        public IWebElement ForgotPasswordLink;
        [FindsBy(How = How.Id, Using = "rememberMe")]
        public IWebElement RememberMeCheckBox;
        [FindsBy(How = How.LinkText, Using = "Create an Account")]
        public IWebElement CreateAccountLink;
        [FindsBy(How = How.XPath, Using = "//button[.='Sign In']")]
        public IWebElement SignInButton;
        [FindsBy(How = How.ClassName, Using = "alert alert-danger ng-binding")]
        public IWebElement ErrorMessage;
    }
}
