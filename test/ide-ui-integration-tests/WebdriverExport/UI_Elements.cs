using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTests
{
    [TestFixture]
    public class UIElements
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private string baseURL;
        private bool acceptNextAlert = true;
        
        [SetUp]
        public void SetupTest()
        {
            driver = new FirefoxDriver();
            baseURL = "https://secure.dev-fsw.com/";
            verificationErrors = new StringBuilder();
        }
        
        [TearDown]
        public void TeardownTest()
        {
            try
            {
                driver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
            Assert.AreEqual("", verificationErrors.ToString());
        }
        
        [Test]
        public void TheUIElementsTest()
        {
            // If changing a UI element, do it via find/replace in a text editor to ensure all references are changed too
            String LoginButton = "//a[.='Login']";
            String RegisterNowButton = "link=Register Now »";
            String CreateAnAccountButton = "//button[.='Create an Account']";
            String LoginPage_Username = "id=username";
            String LoginPage_Password = "id=password";
            String LoginPage_SignInButton = "//button[.='Sign In']";
            String ConfirmEmailButton = "//button[.='Confirm Email']";
            String SaveButton = "//input[@value='Save']";
            String ForgotPassword = "link=Forgot Password?";
            String SubmitButton = "css=button.btn.btn-primary";
            // All Pages
            String PageBody = "css=div.container.body-content";
            String Header_Dropdown = "css=a.dropdown-toggle";
            String Header_ManageUsers = "link=Manage Users";
            String CreateUserButton = "link=Create User »";
            String Header_ManageClients = "link=Manage Clients";
            String Header_ManageScopes = "link=Manage Scopes";
            String Header_Logout = "link=Logout";
            String Header_AccountDetails = "link=Account Details";
            String Header_ConfirmYourEmailAccount  = "link=Confirm Your Email Account";
            // Create Account Page
            String Account_Username = "id=Username";
            String Account_Email = "id=Email";
            String Account_GivenName = "id=GivenName";
            String Account_FirstName = Account_GivenName;
            String Account_FamilyName = "id=FamilyName";
            String Account_LastName = Account_FamilyName;
            String Account_IsAuthCentralAdmin = "id=IsAuthCentralAdmin";
            String Account_Organization = "id=Organization";
            String Account_Department = "id=Department";
            String Account_Password = "id=Password";
            String Account_ConfirmPassword = "id=ConfirmPassword";
            String Account_CreateAccountButton = "//button[.='Create Account']";
            // Home Page
            String HomePage_Email = "//dd/a[contains(@href,'mailto')]";
            // Edit User Page
            String Account_ClaimsTab = "link=Claims";
            String Account_ProfileTab = "link=Profile";
            String Account_SaveButton = "//input[@value='Save']";
            String Account_CreateClaimButton = "link=Create Claim";
            String Account_DeleteClaim_ecommadmin = "//input[contains(@value,'ecommadmin')]/../../../following-sibling::div/div/input[@value='Delete']";
            String Account_DeleteClaim_authcentraladmin = "//input[contains(@value,'authcentral:admin')]/../../../following-sibling::div/div/input[@value='Delete']";
            String Account_ConfirmationMessage = "//div/em";
            String Account_ClaimType = "//input[contains(@id,\"Type\")]";
            String Account_ClaimValue = "//input[contains(@id,\"Value\")]";
            String Account_UniqueClaim = "//div[@class='container body-content']/form";
            String Account_NewClaim_DeleteButton = "//input[@value='${NewUser_ClaimType}']/../../../following-sibling::div/div/input";
            String Account_DeleteButton = "id=delete";
            // User Lookup Page
            String NewUser_DeleteButton = "//a[.='${AdminUser_Email}']/../../td/form/input[@value='Delete']";
            String DeleteButton = "//input[@value='Delete']";
            String EmailSearch = "id=clientId";
            String EmailSearchButton = "css=button.btn.btn-primary";
            // Outlook Web App
            String Owa_Username = "id=username";
            String Owa_Password = "id=password";
            String Owa_SignInButton = "css=span.signinTxt";
            String Owa_EmailTimeStamp = "//div[@class='_lv_O']/span";
            String Owa_EmailConfirmationLink = "id=confirm";
            // Account Details Page
            String AccountDetails_Name = "//div[@id='nameText']/div/div";
            String AccountDetails_Email = "//div[@id='emailText']/div/div";
            String AccountDetails_Name_Edit = "//div[@id='nameText']/div/div/a[.='edit']";
            String AccountDetails_Email_Edit = "//div[@id='emailText']/div/div/a[.='edit']";
            String AccountDetails_Password_Edit = "//div[@id='passwordText']/div/div/a[.='edit']";
            String AccountDetails_FirstName = "id=Name_GivenName";
            String AccountDetails_LastName = "id=Name_FamilyName";
            // Change Password Page
            String ChangePassword_CurrentPassword = "id=Password_OldPassword";
            String ChangePassword_NewPassword = "id=Password_NewPassword";
            String ChangePassword_ConfirmNewPassword = "id=Password_NewPasswordConfirm";
            String ChangePasswordButton = "//input[@value='Change Password'] | //button[.='Change Password']";
            String HereButton = "link=here";
            // Change Email Page
            String ChangeEmail_NewEmail = "id=Email";
            String ChangeEmail_SaveButton = "xpath=(//input[@value='Save'])[2]";
            // Clients Page
            String CreateClientButton = "link=+ Create a New Client";
            String Client_Id = "id=ClientId";
            String Client_Name = "id=ClientName";
            String Client_Uri = "id=ClientUri";
            String Client_Secret_Value = "id=ClientSecrets_0__Value";
            String Client_RedirectUri = "id=RedirectUris_0";
            String Client_PostLogoutRedirectUri = "id=PostLogoutRedirectUris_0";
            String CreateButton = "css=input.btn.btn-success";
            String Client_DeleteButton = "css=input.btn.btn-warning";
            String Client_ClientSecretsTab = "link=Client Secrets";
            String CreateSecretButton = "link=Create Secret";
            String Client_Claim_Type = "id=ClientClaims_0__Type";
            String Client_Claim_Value = "id=ClientClaims_0__Value";
            String Client_ClaimsTab = "link=Claims";
            String CreateClaimButton = "link=Create Claim";
            String Client_Claim_InList = "//dt[contains(.,'Type')]/following-sibling::dd[contains(.,'${NewClient_Claim_Type}')]/following-sibling::dt[contains(.,'Value')]/following-sibling::dd[contains(.,'${NewClient_Claim_Value}')]/..";
            String Client_AllowedScopesTab = "link=Allowed Scopes";
            String NewInput_Field = "//input[@value=''][not(contains(@id,'original'))]";
            String Client_Scope_NewScope_Saved = "//input[@value='${NewClient_Scope}'][contains(@id,'original')]";
            String NewInput_SaveButton = NewInput_Field + "/../../following-sibling::div/input[@value='Save']";
            String Client_ClientAdminHomeTab = "link=Client Admin Home";
            String Client_IdSearch = "id=clientId";
            String SearchButton = "css=button.btn.btn-primary";
            String Client_UrisDropdown = "link=Uri's";
            String Client_RedirectUris = "link=Redirect Uri's";
            String Client_RedirectUris_ExistingUri = "//input[@value='${NewClient_RedirectUri}']";
            String Client_NewUri_Saved = "//input[@value='${NewClient_NewUri}'][contains(@id,'original')]";
            String Client_LogoutUris_ExistingUri = "//input[@value='${NewClient_PostLogoutRedirectUri}']";
            String Client_LogoutUris = "link=Logout Uri's";
            String Client_AllowedCorsOrigins = "link=Allowed CORS Origins";
        }
        private bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
        
        private bool IsAlertPresent()
        {
            try
            {
                driver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException)
            {
                return false;
            }
        }
        
        private string CloseAlertAndGetItsText() {
            try {
                IAlert alert = driver.SwitchTo().Alert();
                string alertText = alert.Text;
                if (acceptNextAlert) {
                    alert.Accept();
                } else {
                    alert.Dismiss();
                }
                return alertText;
            } finally {
                acceptNextAlert = true;
            }
        }
    }
}
