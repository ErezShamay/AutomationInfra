namespace Splitit.Automation.NG.Backend.Services.V3.DefaultValues;

public class RedirectUrlsDefaultValues
{
    public string authorizeFailed;
    public string authorizeSucceeded;

    public RedirectUrlsDefaultValues()
    {
        authorizeSucceeded = "https://www.success.com";
        authorizeFailed = "https://www.google.com";
    }
}