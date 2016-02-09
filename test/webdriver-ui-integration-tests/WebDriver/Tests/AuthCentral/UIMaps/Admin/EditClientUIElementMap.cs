using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Fsw.Enterprise.AuthCentral.WebDriver.Tests.AuthCentral.UIMaps.Admin
{
    class EditClientUIElementMap
    {

        [FindsBy(How = How.Id, Using = "ClientId")]
        public IWebElement ClientIdBox;
        [FindsBy(How = How.Id, Using = "ClientName")]
        public IWebElement ClientNameBox;
        [FindsBy(How = How.Id, Using = "ClientUri")]
        public IWebElement ClientUriBox;
        [FindsBy(How = How.Id, Using = "LogoUri")]
        public IWebElement LogoUriBox;

        [FindsBy(How = How.Id, Using = "ExistingClient_RequireConsent")]
        public IWebElement RequireConsentCheckbox;
        [FindsBy(How = How.Id, Using = "ExistingClient_AllowRememberConsent")]
        public IWebElement AllowRememberConsentCheckbox;
        [FindsBy(How = How.Id, Using = "ExistingClient_Flow")]
        public IWebElement FlowBox;
        [FindsBy(How = How.Id, Using = "ExistingClient_AllowClientCredentialsOnly")]
        public IWebElement AllowClientCredentialsOnlyCheckbox;
        [FindsBy(How = How.Id, Using = "ExistingClient_AllowAccessToAllScopes")]
        public IWebElement AllowAccessToAllScopesCheckbox;
        [FindsBy(How = How.Id, Using = "ExistingClient_IdentityTokenLifetime")]
        public IWebElement IdentityTokenLifetimeBox;
        [FindsBy(How = How.Id, Using = "ExistingClient_AccessTokenLifetime")]
        public IWebElement AccessTokenLifetimeBox;
        [FindsBy(How = How.Id, Using = "ExistingClient_AuthorizationCodeLifetime")]
        public IWebElement AuthorizationCodeLifetimeBox;
        [FindsBy(How = How.Id, Using = "ExistingClient_AbsoluteRefreshTokenLifetime")]
        public IWebElement AbsoluteRefreshTokenLifetimeBox;
        [FindsBy(How = How.Id, Using = "ExistingClient_SlidingRefreshTokenLifetime")]
        public IWebElement SlidingRefreshTokenLifetimeBox;
        [FindsBy(How = How.Id, Using = "ExistingClient_Enabled")]
        public IWebElement EnabledCheckbox;
        [FindsBy(How = How.Id, Using = "ExistingClient_RefreshTokenUsage")]
        public IWebElement RefreshTokenUsageBox;
        [FindsBy(How = How.Id, Using = "ExistingClient_UpdateAccessTokenClaimsOnRefresh")]
        public IWebElement UpdateAccessTokenClaimsOnRefreshCheckbox;
        [FindsBy(How = How.Id, Using = "ExistingClient_RefreshTokenExpiration")]
        public IWebElement RefreshTokenExpiration;
        [FindsBy(How = How.Id, Using = "ExistingClient_AccessTokenType")]
        public IWebElement AccessTokenTypeBox;
        [FindsBy(How = How.Id, Using = "ExistingClient_EnableLocalLogin")]
        public IWebElement EnableLocalLoginCheckbox;
        [FindsBy(How = How.Id, Using = "ExistingClient_IncludeJwtId")]
        public IWebElement IncludeJwtIdBox;
        [FindsBy(How = How.Id, Using = "ExistingClient_AlwaysSendClientClaims")]
        public IWebElement AlwaysSendClientClaimsBox;
        [FindsBy(How = How.Id, Using = "ExistingClient_PrefixClientClaims")]
        public IWebElement PrefixClientClaimsBox;
        [FindsBy(How = How.Id, Using = "ExistingClient_AllowAccessToAllCustomGrantTypes")]
        public IWebElement AllowAccessToAllCustomGrantTypesCheckbox;
        
    }
}
