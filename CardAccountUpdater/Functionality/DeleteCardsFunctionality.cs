using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.CardAccountUpdater.BaseObjects;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.Backend.Services.CardAccountUpdater.Functionality;

public class DeleteCardsFunctionality
{
    private const string EndPoint = "/api/v1/cards";
    private readonly HttpSender _httpSender = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task<string> SendDeleteRequestDeleteCards(
        RequestHeader requestHeader, DeleteCardsBaseObjects.Root deleteCardsBaseObjects)
    {
        try
        {
            Console.WriteLine("\nStarting SendDeleteRequestDeleteCards");
            var response = await _httpSender.SendDeleteRequestAsync(
                _envConfig.AccountUpdater + EndPoint,
                deleteCardsBaseObjects, requestHeader);
            Console.WriteLine("Done with SendDeleteRequestDeleteCards\n");
            return response;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendDeleteRequestDeleteCards \n" + exception + "\n");
            throw;
        }
    }
}