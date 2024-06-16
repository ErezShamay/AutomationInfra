namespace Splitit.Automation.NG.Backend.Services.IdServer.requests;

public class RequestClientsSave
{
    public class Root
    {
        public Client Client { get; set; }
    }
    
    public class Client
    {
        public bool IsBehalfOfAllowed { get; set; }
        public string ClientId { get; set; }
        public int AccessTokenLifetime { get; set; }
        public string AccessTokenType { get; set; }
        public bool AllowAccessTokenSlidingExpiration { get; set; }
        public List<string> AllowedGrantTypes { get; set; }
        public List<string> AllowedScopes { get; set; }
        public List<string> RedirectUris { get; set; }
        public bool AllowOfflineAccess { get; set; }
        public bool RequireClientSecret { get; set; }
        public int SlidingAccessTokenExtension { get; set; }
        public List<string> Roles { get; set; }
        public List<string> Permissions { get; set; }
    }
}