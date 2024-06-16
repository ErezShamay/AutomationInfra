namespace Splitit.Automation.NG.Backend.Services.IdServer.responses;

public class PostConnectTokenResponse
{
    public class Root
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }
        public string scope { get; set; }
    }
}