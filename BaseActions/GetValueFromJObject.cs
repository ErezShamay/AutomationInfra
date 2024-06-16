using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Splitit.Automation.NG.Backend.BaseActions;

public class GetValueFromJObject
{
    public string GetValue(JObject jObject, string key, string innerKey)
    {
        try
        {
            Console.WriteLine("\nStarting to get value from JObject");
            var value = jObject[key];
            if (value != null)
            {
                foreach (var item in value.First!.Children())
                {
                    foreach (var temp in item)
                    {
                        var result = temp[innerKey];
                        if (result!.ToString() != "")
                        {
                            Console.WriteLine("Done Getting value from JObject");
                            Console.WriteLine("The Value is: " + result + "\n");
                            return result.ToString();
                        }
                    }
                }
            }
            Assert.That(value, Is.Not.Null);
            return value!.ToString();
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in getting the value from JObject \n" + exception + "\n");
        }
        return null!;
    }
    
    public string GetInnerValue(JObject jObject, string key, string innerKey, string innerValue, string otherKey)
    {
        try
        {
            Console.WriteLine("\nStarting to get value from JObject");
            var value = jObject[key];
            if (value != null)
            {
                foreach (var index in value)
                {
                    foreach (var item in index.Children())
                    {
                        foreach (var temp in item)
                        {
                            if (temp[innerKey]!.ToString().Equals(innerValue) && !temp[otherKey]!.ToString().Equals(""))
                            {
                                Console.WriteLine("Found the requested item");
                                return temp[otherKey]!.ToString();
                            }
                        }
                    }
                }
            }
            Console.WriteLine("The return value is: " + value);
            return value!.ToString();
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in getting the value from JObject \n" + exception + "\n");
        }
        return null!;
    }
}