namespace Splitit.Automation.NG.Backend.Services.IdServer.requests;

public class PostConnectTokenBaseObjects
{
    public class Root
    {
        public string grant_type { get; set; }
        public string scope { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }
    }
}