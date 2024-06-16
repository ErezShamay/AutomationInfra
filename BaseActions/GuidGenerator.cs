namespace Splitit.Automation.NG.Backend.BaseActions;

public abstract class GuidGenerator
{
    public static string GenerateNewGuid()
    {
        var guid = Guid.NewGuid();
        return guid.ToString();
    }
}