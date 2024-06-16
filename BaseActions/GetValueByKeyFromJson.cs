using Newtonsoft.Json.Linq;

namespace Splitit.Automation.NG.Backend.BaseActions;

public class GetValueByKeyFromJson
{
    public string GetValueOfObjectKey(string res, string key)
    {
        var json = JObject.Parse(res);
        return json.GetValue(key)!.ToString();
    }
}