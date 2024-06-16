using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V1.InstallmentPlan.InstallmentPlanBaseObjects;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;

namespace Splitit.Automation.NG.Backend.Services.V1.InstallmentPlan.InstallmentPlanApiEndPoints;

public class GetPgtl
{
    private readonly HttpSender _httpSender = new();
    private const string GetPgtlEndPoint = "/api/InstallmentPlan/GetPGTL";
    private readonly GetPgtlBaseObjects.Root _getPgtlBaseObjects = new();
    private readonly GetValueFromJObject _getValueFromJObject = new();

    private async Task<JObject> SendPostRequestGetPgtlAsync(string baseUrl, RequestHeader requestHeader, string ipn)
    {
        try
        {
            Console.WriteLine("\nStarting SendPostRequestGetPgtl");
            _getPgtlBaseObjects.InstallmentPlanNumber = ipn;
            var response = await _httpSender.SendPostHttpsRequestAsync(baseUrl + GetPgtlEndPoint,
                _getPgtlBaseObjects, requestHeader);
            var data = JObject.Parse(response);
            Console.WriteLine("Done with SendPostRequestGetPgtl\n");
            return data;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendPostRequestGetPgtl\n" + exception + "\n");
            throw;
        }
    }

    public async Task<string?> ValidatePgtlKeyValueAsync(string baseUrl, RequestHeader requestHeader, string ipn, string innerKey)
    {
        var counter = 0;
        var tempValue = "";
        try
        {
            while (counter < 5)
            {
                var jObject = await SendPostRequestGetPgtlAsync(baseUrl, requestHeader, ipn);
                tempValue = _getValueFromJObject.GetValue(jObject, "paymentGatewaytransactionResponses", innerKey);
                if (tempValue != "" && tempValue != null! && !tempValue.Contains("\n"))
                    return tempValue;
                else
                {
                    await Task.Delay(3 * 1000);
                    counter++;
                }
            }

            Assert.That(tempValue, Is.Not.Null);
            return tempValue;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in ValidatePgtlKeyValue \n" + exception);
            throw;
        }
    }
    
    public async Task<string?> ValidatePgtlKeyValueInnerAsync(string baseUrl, RequestHeader requestHeader, string ipn, string innerKey, string innerValue,string otherKey)
    {
        var counter = 0;
        var tempValue = "";
        try
        {
            while (counter < 10)
            {
                var jObject = await SendPostRequestGetPgtlAsync(baseUrl, requestHeader, ipn);
                tempValue = _getValueFromJObject.GetInnerValue(jObject, "paymentGatewaytransactionResponses", innerKey, innerValue, otherKey);
                if (tempValue != "" && tempValue != null! && !tempValue.Contains("\n"))
                    return tempValue;
                await Task.Delay(5 * 1000);
                counter++;
            }
            Console.WriteLine("the return value is: " + tempValue);
            return tempValue;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in ValidatePgtlKeyValue \n" + exception);
            throw;
        }
    }

    public async Task<int> CountTypeValuesInPgtlAsync(string baseUrl, RequestHeader requestHeader, string ipn,
        string innerKey, string innerValue, bool toCount = default, string anotherKey = null!, string anotherKeyValue = null!)
    {
        var valueCounter = 0;
        var flag = false;

        try
        {
            Console.WriteLine("Starting countTypeValuesInPgtl");
            var jObject = await SendPostRequestGetPgtlAsync(baseUrl, requestHeader, ipn);
            var valuesList = jObject.GetValue("paymentGatewaytransactionResponses")!.Values();
            foreach (var logs in valuesList)
            {
                foreach (var log in logs)
                {
                    var item = log[innerKey];
                    if (item.ToString().Equals(innerValue))
                    {
                        if (toCount)
                        {
                            valueCounter++;
                        }
                        
                        if (anotherKey != null)
                        {
                            if (log[anotherKey]!.ToString() == anotherKeyValue)
                            {
                                valueCounter++;
                            }
                        }
                    }
                }
            }
            Console.WriteLine("Done with countTypeValuesInPgtl");
            return valueCounter;
        }
        catch (Exception exception)
        {
            Console.WriteLine("error in countTypeValuesInPgtl -> " + exception);
            throw;
        }
    }
}