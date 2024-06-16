namespace Splitit.Automation.NG.Backend.Services.V1._5.Login.LoginBaseObjects;

public class LoginBaseObject
{
    public class RequestHeader
    {
        public TouchPoint touchPoint { get; set; }
        public string sessionId { get; set; }
        public string apiKey { get; set; }
        public string cultureName { get; set; }
        public int authenticationType { get; set; }
    }

    public class Root
    {
        public string userName { get; set; }
        public string password { get; set; }
        public RequestHeader requestHeader { get; set; }
        public TouchPoint touchPoint { get; set; }
    }

    public class TouchPoint
    {
        public string code { get; set; }
        public string version { get; set; }
        public string subVersion { get; set; }
        public int versionedTouchpointId { get; set; }
    }
}