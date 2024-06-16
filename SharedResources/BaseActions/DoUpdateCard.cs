using OpenQA.Selenium;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V1.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.SharedResources.Pages;

namespace Splitit.Automation.NG.SharedResources.BaseActions;

public class DoUpdateCard
{
    private readonly DriverFactory.DriverFactory _driverFactory = new();
    private readonly BaseActions _baseActions = new();
    private readonly UpdateCardLocators _updateCardLocators = new();
    private readonly Adyen3DsLocatorsUpdateCard _adyen3DsLocatorsUpdateCard = new();
    private readonly CyberSource3DsLocatorsUpdateCard _cyberSource3DsLocatorsUpdateCard = new();
    private readonly BlueSnapDirect3DsLocatorsUpdateCard _blueSnapDirect3DsLocatorsUpdateCard = new();
    private readonly Checkout3DsLocatorsUpdateCard _checkout3DsLocatorsUpdateCard = new();
    private readonly PaySafe3DsLocatorsUpdateCard _paySafe3DsLocatorsUpdateCard = new();
    private readonly Reach3DsLocatorsUpdateCard _reach3DsLocatorsUpdateCard = new();
    //private readonly WorldPay3DsLocatorsUpdateCard _worldPay3DsLocatorsUpdateCard = new();
    private readonly SagePay3DsLocatorsUpdateCard _sagePay3DsLocatorsUpdateCard = new();
    private readonly PaymentFormV4V1 _paymentFormV4V1 = new();
    private readonly AuditLogController _auditLogController = new();

    public async Task<bool> DoUpdateCreditCardAsync(string url, string cardNumberToUpdate, string cardHolderNameToUpdateTo, int expMonthToUpdateTo,
        int expYearToUpdateTo, string cvvToUpdateTo, string gatewayName = null!, CreatePlanDefaultValues createPlanDefaultValues = null!, 
        RequestHeader requestHeader = null!, string ipn = null!)
    {
        var driver = _driverFactory.InitDriver();
        
        try
        {
            Console.WriteLine("ֿ\nStarting DoUpdateCreditCard");
            var checkExp = createPlanDefaultValues.paymentMethod.card.cardExpMonth.ToString();
            string expToUpdateTo;
            if (checkExp.Length == 1)
            {
                expToUpdateTo = "0" + createPlanDefaultValues.paymentMethod.card.cardExpMonth + "/" + expYearToUpdateTo.ToString()[2..];
            }
            else
            {
                expToUpdateTo = createPlanDefaultValues.paymentMethod.card.cardExpMonth + "/" + expYearToUpdateTo.ToString()[2..];
            }
            _baseActions.NavigateToUrl(driver, url);
            _baseActions.ClickOnButton(driver, _updateCardLocators.CardNumber);
            _baseActions.SendKeysToTextBox(driver, _updateCardLocators.CardNumber, cardNumberToUpdate);
            _baseActions.ClickOnButton(driver, _updateCardLocators.CardHolderName);
            _baseActions.SendKeysToTextBox(driver, _updateCardLocators.CardHolderName, cardHolderNameToUpdateTo);
            _baseActions.ClickOnButton(driver, _updateCardLocators.CardExpDate);
            _baseActions.SendKeysToTextBox(driver, _updateCardLocators.CardExpDate, expToUpdateTo);
            _baseActions.ClickOnButton(driver, _updateCardLocators.CardCvv);
            _baseActions.SendKeysToTextBox(driver, _updateCardLocators.CardCvv, cvvToUpdateTo);
            _baseActions.ClickOnButton(driver, _updateCardLocators.UpdateCardButton);
            if (await Is3DsPopUpVisible(createPlanDefaultValues, requestHeader, ipn))
            {
                switch(gatewayName) 
                {
                    case "Adyen":
                        DoAdyen3Ds(driver);
                        break;
                    case "BlueSnapDirect":
                        await DoBlueSnapDirect3Ds(driver);
                        break;
                    case "BlueSnapMor":
                        await DoBlueSnapMor3Ds();
                        break;
                    case "Checkout":
                        DoCheckout3Ds(driver);
                        break;
                    case "CyberSource":
                        DoCyberSource3Ds(driver);
                        break;
                    case "PaySafe":
                        DoPaySafe3Ds(driver);
                        break;
                    case "Reach":
                        DoReach3Ds(driver);
                        break;
                    case "SagePay":
                        DoSagePay3Ds(driver);
                        break;
                    case "StripeDirect":
                        Console.WriteLine("No more 3DS functionality is needed");
                        await Task.Delay(3 * 1000);
                        break;
                    case "WorldPay":
                        await DoWorldPay3Ds();
                        break;
                    default:
                        _baseActions.WaitForElementToBeVisible(driver, _paymentFormV4V1.Iframe);
                        _baseActions.SwitchToIframe(driver, _paymentFormV4V1.Iframe);
                        _baseActions.ClickOnButton(driver, _paymentFormV4V1.SuccessButton);
                        break;
                }
            }
            _baseActions.ValidateNavigationByElement(driver, _updateCardLocators.ApprovalMessage);
            _baseActions.TearDown(driver);
            Console.WriteLine("Done with DoUpdateCreditCard\n");
            return true;
        }
        catch (Exception exception)
        {
            Console.WriteLine("\nPrinting audit log result messages:");
            await _auditLogController.PrintAllAuditLogResultMessagesAsync(requestHeader, ipn);
            Console.WriteLine("Error in DoUpdateCreditCard\n" + exception + "\n");
            _baseActions.TearDown(driver);
            throw;
        }
    }
    
    public async Task<bool> DoUpdateCreditCardWithV1Async(string url, string cardNumberToUpdate, string cardHolderNameToUpdateTo, int expMonthToUpdateTo,
        int expYearToUpdateTo, string cvvToUpdateTo, string gatewayName = null!, V1InitiateDefaultValues createPlanDefaultValues = null!, 
        RequestHeader requestHeader = null!, string ipn = null!)
    {
        var driver = _driverFactory.InitDriver();
        
        try
        {
            Console.WriteLine("ֿ\nStarting DoUpdateCreditCard");
            var checkExp = createPlanDefaultValues.CreditCardDetails.cardExpMonth;
            string expToUpdateTo;
            if (checkExp.Length == 1)
            {
                expToUpdateTo = "0" + createPlanDefaultValues.CreditCardDetails.cardExpMonth + "/" + expYearToUpdateTo.ToString()[2..];
            }
            else
            {
                expToUpdateTo = createPlanDefaultValues.CreditCardDetails.cardExpMonth + "/" + expYearToUpdateTo.ToString()[2..];
            }
            _baseActions.NavigateToUrl(driver, url);
            _baseActions.ClickOnButton(driver, _updateCardLocators.CardNumber);
            _baseActions.SendKeysToTextBox(driver, _updateCardLocators.CardNumber, cardNumberToUpdate);
            _baseActions.ClickOnButton(driver, _updateCardLocators.CardHolderName);
            _baseActions.SendKeysToTextBox(driver, _updateCardLocators.CardHolderName, cardHolderNameToUpdateTo);
            _baseActions.ClickOnButton(driver, _updateCardLocators.CardExpDate);
            _baseActions.SendKeysToTextBox(driver, _updateCardLocators.CardExpDate, expToUpdateTo);
            _baseActions.ClickOnButton(driver, _updateCardLocators.CardCvv);
            _baseActions.SendKeysToTextBox(driver, _updateCardLocators.CardCvv, cvvToUpdateTo);
            _baseActions.ClickOnButton(driver, _updateCardLocators.UpdateCardButton);
            if (await Is3DsPopUpVisibleV1Async(createPlanDefaultValues, requestHeader, ipn))
            {
                switch(gatewayName) 
                {
                    case "Adyen":
                        DoAdyen3Ds(driver);
                        break;
                    case "BlueSnapDirect":
                        await Task.Delay(10 * 1000);
                        //await DoBlueSnapDirect3Ds(driver);
                        break;
                    case "BlueSnapMor":
                        await DoBlueSnapMor3Ds();
                        break;
                    case "Checkout":
                        DoCheckout3Ds(driver);
                        break;
                    case "CyberSource":
                        DoCyberSource3Ds(driver);
                        break;
                    case "PaySafe":
                        await Task.Delay(10 * 1000);
                        //DoPaySafe3Ds(driver);
                        break;
                    case "Reach":
                        DoReach3Ds(driver);
                        break;
                    case "SagePay":
                        DoSagePay3Ds(driver);
                        break;
                    case "StripeDirect":
                        Console.WriteLine("No more 3DS functionality is needed");
                        await Task.Delay(3 * 1000);
                        break;
                    case "WorldPay":
                        await DoWorldPay3Ds();
                        break;
                    default:
                        _baseActions.WaitForElementToBeVisible(driver, _paymentFormV4V1.Iframe);
                        _baseActions.SwitchToIframe(driver, _paymentFormV4V1.Iframe);
                        _baseActions.ClickOnButton(driver, _paymentFormV4V1.SuccessButton);
                        break;
                }
            }
            _baseActions.ValidateNavigationByElement(driver, _updateCardLocators.ApprovalMessage);
            _baseActions.TearDown(driver);
            Console.WriteLine("Done with DoUpdateCreditCard\n");
            return true;
        }
        catch (Exception exception)
        {
            Console.WriteLine("\nPrinting audit log result messages:");
            await _auditLogController.PrintAllAuditLogResultMessagesAsync(requestHeader, ipn);
            Console.WriteLine("Error in DoUpdateCreditCard\n" + exception + "\n");
            _baseActions.TearDown(driver);
            throw;
        }
    }
    
    private void DoAdyen3Ds(WebDriver driver)
    {
        try
        {
            Console.WriteLine("\nStarting DoAdyen3Ds");
            _baseActions.WaitForElementToBeVisible(driver, _adyen3DsLocatorsUpdateCard.FirstIframe);
            _baseActions.SwitchToIframe(driver, _adyen3DsLocatorsUpdateCard.FirstIframe);
            _baseActions.WaitForElementToBeVisible(driver, _adyen3DsLocatorsUpdateCard.SecondIframe);
            _baseActions.SwitchToIframe(driver, _adyen3DsLocatorsUpdateCard.SecondIframe);
            _baseActions.ClickOnButton(driver, _adyen3DsLocatorsUpdateCard.PasswordTextBox);
            _baseActions.SendKeysToTextBox(driver, _adyen3DsLocatorsUpdateCard.PasswordTextBox, _adyen3DsLocatorsUpdateCard.PasswordValue);
            _baseActions.ClickOnButton(driver, _adyen3DsLocatorsUpdateCard.ContinueButton);
            Console.WriteLine("Done with DoAdyen3Ds\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in DoAdyen3Ds\n" + exception + "\n");
            throw;
        }
    }
    
    private void DoCyberSource3Ds(WebDriver driver)
    {
        try
        {
            Console.WriteLine("\nStarting DoCyberSource3Ds");
            _baseActions.WaitForElementToBeExists(driver, _cyberSource3DsLocatorsUpdateCard.FirstIframe);
            _baseActions.SwitchToIframe(driver, _cyberSource3DsLocatorsUpdateCard.FirstIframe);
            _baseActions.WaitForElementToBeExists(driver, _cyberSource3DsLocatorsUpdateCard.SecondIframe);
            _baseActions.SwitchToIframe(driver, _cyberSource3DsLocatorsUpdateCard.SecondIframe);
            // _baseActions.WaitForElementToBeExists(driver, _cyberSource3DsLocatorsUpdateCard.ThirdIframe);
            // _baseActions.SwitchToIframe(driver, _cyberSource3DsLocatorsUpdateCard.ThirdIframe);
            _baseActions.ClickOnButton(driver, _cyberSource3DsLocatorsUpdateCard.PasswordTextBox);
            _baseActions.SendKeysToTextBox(driver, _cyberSource3DsLocatorsUpdateCard.PasswordTextBox, CyberSource3DsLocators.PasswordValue);
            _baseActions.ClickOnButton(driver, _cyberSource3DsLocatorsUpdateCard.ContinueButton);
            Console.WriteLine("Done with DoCyberSource3Ds\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in DoCyberSource3Ds\n" + exception + "\n");
            throw;
        }
    }
    
    private async Task DoBlueSnapDirect3Ds(WebDriver driver)
    {
        try
        {
            Console.WriteLine("\nStarting DoBlueSnapDirect3Ds");
            await Task.Delay(3*1000);
            _baseActions.WaitForElementToBeVisible(driver, _blueSnapDirect3DsLocatorsUpdateCard.FirstIframe);
            _baseActions.SwitchToIframe(driver, _blueSnapDirect3DsLocatorsUpdateCard.FirstIframe);
            _baseActions.WaitForElementToBeVisible(driver, _blueSnapDirect3DsLocatorsUpdateCard.SecondIframe);
            _baseActions.SwitchToIframe(driver, _blueSnapDirect3DsLocatorsUpdateCard.SecondIframe);
            _baseActions.ClickOnButton(driver, _blueSnapDirect3DsLocatorsUpdateCard.PasswordTextBox);
            _baseActions.SendKeysToTextBox(driver, _blueSnapDirect3DsLocatorsUpdateCard.PasswordTextBox, _blueSnapDirect3DsLocatorsUpdateCard.PasswordValue);
            _baseActions.ClickOnButton(driver, _blueSnapDirect3DsLocatorsUpdateCard.ContinueButton);
            Console.WriteLine("Done with DoBlueSnapDirect3Ds\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in DoBlueSnapDirect3Ds\n" + exception + "\n");
            throw;
        }
    }
    
    private async Task DoBlueSnapMor3Ds()
    {
        try
        {
            Console.WriteLine("\nStarting DoBlueSnapMor3Ds");
            await Task.Delay(3*1000);
            // _baseActions.WaitForElementToBeVisible(driver, _blueSnapMor3DsLocatorsUpdateCard.FirstIframe);
            // _baseActions.SwitchToIframe(driver, _blueSnapMor3DsLocatorsUpdateCard.FirstIframe);
            // _baseActions.WaitForElementToBeVisible(driver, _blueSnapMor3DsLocatorsUpdateCard.SecondIframe);
            // _baseActions.SwitchToIframe(driver, _blueSnapMor3DsLocatorsUpdateCard.SecondIframe);
            // _baseActions.ClickOnButton(driver, _blueSnapMor3DsLocatorsUpdateCard.PasswordTextBox);
            // _baseActions.SendKeysToTextBox(driver, _blueSnapMor3DsLocatorsUpdateCard.PasswordTextBox, _blueSnapMor3DsLocatorsUpdateCard.PasswordValue);
            // _baseActions.ClickOnButton(driver, _blueSnapMor3DsLocatorsUpdateCard.ContinueButton);
            Console.WriteLine("Done with DoBlueSnapMor3Ds\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in DoBlueSnapMor3Ds\n" + exception + "\n");
            throw;
        }
    }
    
    private void DoCheckout3Ds(WebDriver driver)
    {
        try
        {
            Console.WriteLine("\nStarting DoCheckout3Ds");
            _baseActions.WaitForElementToBeVisible(driver, _checkout3DsLocatorsUpdateCard.FirstIframe);
            _baseActions.SwitchToIframe(driver, _checkout3DsLocatorsUpdateCard.FirstIframe);
            _baseActions.WaitForElementToBeVisible(driver, _checkout3DsLocatorsUpdateCard.SecondIframe);
            _baseActions.SwitchToIframe(driver, _checkout3DsLocatorsUpdateCard.SecondIframe);
            _baseActions.ClickOnButton(driver, _checkout3DsLocatorsUpdateCard.PasswordTextBox);
            _baseActions.SendKeysToTextBox(driver, _checkout3DsLocatorsUpdateCard.PasswordTextBox, _checkout3DsLocatorsUpdateCard.PasswordValue);
            _baseActions.ClickOnButton(driver, _checkout3DsLocatorsUpdateCard.ContinueButton);
            Console.WriteLine("Done with DoCheckout3Ds\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in DoCheckout3Ds\n" + exception);
            throw;
        }
    }
    
    private void DoPaySafe3Ds(WebDriver driver)
    {
        try
        {
            Console.WriteLine("\nStarting DoPayLikeV23Ds");
            _baseActions.WaitForElementToBeVisible(driver, _paySafe3DsLocatorsUpdateCard.FirstIframe);
            _baseActions.SwitchToIframe(driver, _paySafe3DsLocatorsUpdateCard.FirstIframe);
            _baseActions.WaitForElementToBeVisible(driver, _paySafe3DsLocatorsUpdateCard.SecondIframe);
            _baseActions.SwitchToIframe(driver, _paySafe3DsLocatorsUpdateCard.SecondIframe);
            _baseActions.SendKeysToTextBox(driver, _paySafe3DsLocatorsUpdateCard.CodeTextBox, _paySafe3DsLocatorsUpdateCard.Code);
            _baseActions.ClickOnButton(driver, _paySafe3DsLocatorsUpdateCard.ContinueButton);
            Console.WriteLine("Done with DoPayLikeV23Ds\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in DoPayLikeV23Ds\n" + exception);
            throw;
        }
    }
    
    private void DoReach3Ds(WebDriver driver)
    {
        try
        {
            Console.WriteLine("\nStarting DoReach3Ds");
            _baseActions.WaitForElementToBeVisible(driver, _reach3DsLocatorsUpdateCard.FirstIframe);
            _baseActions.SwitchToIframe(driver, _reach3DsLocatorsUpdateCard.FirstIframe);
            _baseActions.WaitForElementToBeVisible(driver, _reach3DsLocatorsUpdateCard.SecondIframe);
            _baseActions.SwitchToIframe(driver, _reach3DsLocatorsUpdateCard.SecondIframe);
            _baseActions.ClickOnButton(driver, _reach3DsLocatorsUpdateCard.PasswordTextBox);
            _baseActions.SendKeysToTextBox(driver, _reach3DsLocatorsUpdateCard.PasswordTextBox, _reach3DsLocatorsUpdateCard.PasswordValue);
            _baseActions.ClickOnButton(driver, _reach3DsLocatorsUpdateCard.OkButton);
            Console.WriteLine("Done with DoReach3Ds\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in DoReach3Ds\n" + exception + "\n");
            throw;
        }
    }
    
    private async Task DoWorldPay3Ds()
    {
        try
        {
            Console.WriteLine("\nStarting DoWorldPay3Ds");
            // _baseActions.WaitForElementToBeVisible(driver, _worldPay3DsLocatorsUpdateCard.FirstIframe);
            // _baseActions.SwitchToIframe(driver, _worldPay3DsLocatorsUpdateCard.FirstIframe);
            //_baseActions.SwitchToIframe(driver, _worldPay3DsLocatorsUpdateCard.SecondIframe);
            // _baseActions.WaitForElementToBeVisible(driver, _worldPay3DsLocatorsUpdateCard.OkButton);
            // _baseActions.ClickOnButton(driver, _worldPay3DsLocatorsUpdateCard.OkButton);
            await Task.Delay(3 * 1000);
            Console.WriteLine("Done with DoWorldPay3Ds\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in DoWorldPay3Ds\n" + exception + "\n");
            throw;
        }
    }
    
    private void DoSagePay3Ds(WebDriver driver)
    {
        try
        {
            Console.WriteLine("\nStarting DoSagePay3Ds");
            _baseActions.WaitForElementToBeVisible(driver, _sagePay3DsLocatorsUpdateCard.FirstIframe);
            _baseActions.SwitchToIframe(driver, _sagePay3DsLocatorsUpdateCard.FirstIframe);
            _baseActions.ClickOnButton(driver, _sagePay3DsLocatorsUpdateCard.PasswordTextBox);
            _baseActions.SendKeysToTextBox(driver, _sagePay3DsLocatorsUpdateCard.PasswordTextBox, _sagePay3DsLocatorsUpdateCard.PasswordValue);
            _baseActions.ClickOnButton(driver, _sagePay3DsLocatorsUpdateCard.NextButton);
            Console.WriteLine("Done with DoSagePay3Ds\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in DoSagePay3Ds\n" + exception + "\n");
            throw;
        }
    }

    private async Task<bool> Is3DsPopUpVisible(CreatePlanDefaultValues createPlanDefaultValues, 
        RequestHeader requestHeader, string ipn)
    {
        try
        {
            Console.WriteLine("\nstarting Is3DsPopUpVisible");
            if (createPlanDefaultValues.attempt3DSecure)
            {
                //await Task.Delay(3*1000);
                var jResponse = await _auditLogController.SendRetrieveAuditLogRequestAsync(requestHeader, ipn);
                if(_auditLogController.ValidateAuditLogLogsWithCounter(ipn, new []{"Finalize 3D-Secure", "Finalize 3D-Secure"}, jResponse!))
                {
                    return false;
                }
                Console.WriteLine("Done with Is3DsPopUpVisible\n");
                return true;
            }
            Console.WriteLine("Done with Is3DsPopUpVisible\n");
            return false;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in Is3DsPopUpVisible \n" + exception + "\n");
            return false;
        }
    }
    
    private async Task<bool> Is3DsPopUpVisibleV1Async(V1InitiateDefaultValues createPlanDefaultValues, 
        RequestHeader requestHeader, string ipn)
    {
        try
        {
            Console.WriteLine("\nstarting Is3DsPopUpVisible");
            if (createPlanDefaultValues.planData.Attempt3DSecure)
            {
                //await Task.Delay(3*1000);
                var jResponse = await _auditLogController.SendRetrieveAuditLogRequestAsync(requestHeader, ipn);
                if(_auditLogController.ValidateAuditLogLogsWithCounter(ipn, new []{"Finalize 3D-Secure", "Finalize 3D-Secure"}, jResponse!))
                {
                    return false;
                }
                Console.WriteLine("Done with Is3DsPopUpVisible\n");
                return true;
            }
            Console.WriteLine("Done with Is3DsPopUpVisible\n");
            return false;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in Is3DsPopUpVisible \n" + exception + "\n");
            return false;
        }
    }
}