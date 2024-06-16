namespace Splitit.Automation.NG.Backend.Services.IdServer.responses;

public class ResponseClientSave
{
    public class Root
    {
        public ResponseHeader ResponseHeader { get; set; }
        public Client Client { get; set; }
    }
    
    public class Claim
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public string ValueType { get; set; }
    }

    public class Client
    {
        public bool AllowAccessTokenSlidingExpiration { get; set; }
        public int SlidingAccessTokenExtension { get; set; }
        public List<object> Versions { get; set; }
        public List<string> Roles { get; set; }
        public List<string> Permissions { get; set; }
        public bool IsLocked { get; set; }
        public bool IsBehalfOfAllowed { get; set; }
        public bool IsLockoutEnabled { get; set; }
        public bool Enabled { get; set; }
        public string ClientId { get; set; }
        public string ProtocolType { get; set; }
        public List<object> ClientSecrets { get; set; }
        public bool RequireClientSecret { get; set; }
        public object ClientName { get; set; }
        public object Description { get; set; }
        public object ClientUri { get; set; }
        public object LogoUri { get; set; }
        public bool RequireConsent { get; set; }
        public bool AllowRememberConsent { get; set; }
        public List<string> AllowedGrantTypes { get; set; }
        public bool RequirePkce { get; set; }
        public bool AllowPlainTextPkce { get; set; }
        public bool RequireRequestObject { get; set; }
        public bool AllowAccessTokensViaBrowser { get; set; }
        public List<string> RedirectUris { get; set; }
        public List<string> PostLogoutRedirectUris { get; set; }
        public object FrontChannelLogoutUri { get; set; }
        public bool FrontChannelLogoutSessionRequired { get; set; }
        public object BackChannelLogoutUri { get; set; }
        public bool BackChannelLogoutSessionRequired { get; set; }
        public bool AllowOfflineAccess { get; set; }
        public List<string> AllowedScopes { get; set; }
        public bool AlwaysIncludeUserClaimsInIdToken { get; set; }
        public int IdentityTokenLifetime { get; set; }
        public List<object> AllowedIdentityTokenSigningAlgorithms { get; set; }
        public int AccessTokenLifetime { get; set; }
        public int AuthorizationCodeLifetime { get; set; }
        public int AbsoluteRefreshTokenLifetime { get; set; }
        public int SlidingRefreshTokenLifetime { get; set; }
        public object ConsentLifetime { get; set; }
        public string RefreshTokenUsage { get; set; }
        public bool UpdateAccessTokenClaimsOnRefresh { get; set; }
        public string RefreshTokenExpiration { get; set; }
        public string AccessTokenType { get; set; }
        public bool EnableLocalLogin { get; set; }
        public List<object> IdentityProviderRestrictions { get; set; }
        public bool IncludeJwtId { get; set; }
        public List<Claim> Claims { get; set; }
        public bool AlwaysSendClientClaims { get; set; }
        public string ClientClaimsPrefix { get; set; }
        public object PairWiseSubjectSalt { get; set; }
        public object UserSsoLifetime { get; set; }
        public object UserCodeType { get; set; }
        public int DeviceCodeLifetime { get; set; }
        public object CibaLifetime { get; set; }
        public object PollingInterval { get; set; }
        public List<object> AllowedCorsOrigins { get; set; }
        public Properties Properties { get; set; }
    }

    public class Properties
    {
    }

    public class ResponseHeader
    {
        public bool Succeeded { get; set; }
        public object Errors { get; set; }
        public string TraceId { get; set; }
    }
}