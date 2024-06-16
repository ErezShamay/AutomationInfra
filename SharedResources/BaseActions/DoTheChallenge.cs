using NUnit.Framework;
using OpenQA.Selenium;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentsPlanFunctionality;
using Splitit.Automation.NG.Backend.Services.V1.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V1.InstallmentPlan.InstallmentPlanResponses;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanResponses;
using Splitit.Automation.NG.SharedResources.Pages;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.SharedResources.BaseActions;

public class DoTheChallenge
{
    private readonly DriverFactory.DriverFactory _driverFactory = new();
    private readonly BaseActions _baseActions = new();
    private readonly CheckoutChallengeLocators _checkoutChallengeLocators = new();
    private readonly PaymentFormV4 _paymentFormV4 = new();
    private readonly PaymentFormV4V1 _paymentFormV4V1 = new();
    private readonly PaymentFormV4Functionality _paymentFormV4Functionality = new();
    private readonly Adyen3DsLocators _adyen3DsLocators = new();
    private readonly CyberSource3DsLocators _cyberSource3DsLocators = new();
    private readonly BlueSnapDirect3DsLocators _blueSnapDirect3DsLocators = new();
    private readonly BlueSnapMor3DsLocators _blueSnapMor3DsLocators = new();
    private readonly Checkout3DsLocators _checkout3DsLocators = new();
    private readonly Checkout3DsForAli _checkout3DsForAli = new();
    private readonly PayLikeV23DsLocators _payLikeV23DsLocators = new();
    private readonly Reach3DsLocators _reach3DsLocators = new();
    private readonly WorldPay3DsLocators _worldPay3DsLocators = new();
    private readonly SagePay3DsLocators _sagePay3DsLocators = new();
    private readonly AuditLogController _auditLogController = new();
    private readonly FullPlanInfoIpn _fullPlanInfoIpn = new();
    private readonly VisLocators _visLocators = new();
    private readonly EnvConfig _envConfig = new();

    public async Task<bool> DoTheChallengeV35Async(string url, string cardNumber = null!, string exp = null!,
        string cvv = null!, string is3Ds = null!,
        RequestHeader requestHeader = null!, string ipn = null!)
    {
        var driver = _driverFactory.InitDriver();
        try
        {
            Console.WriteLine("\nStaring DoTheChallengeV35");
            _baseActions.NavigateToUrl(driver, url);
            await Task.Delay(5*1000);
            _baseActions.ClickOnButton(driver, _checkoutChallengeLocators.CardNumberTextBox);
            _baseActions.SendKeysToTextBox(driver, _checkoutChallengeLocators.CardNumberTextBox, cardNumber);
            _baseActions.ClickOnButton(driver, _checkoutChallengeLocators.ExpTextBox);
            _baseActions.SendKeysToTextBox(driver, _checkoutChallengeLocators.ExpTextBox, exp);
            _baseActions.ClickOnButton(driver, _checkoutChallengeLocators.CvvTextBox);
            _baseActions.SendKeysToTextBox(driver, _checkoutChallengeLocators.CvvTextBox, cvv);
            await Task.Delay(3*1000);
            _baseActions.RunJavaScript(driver, _checkoutChallengeLocators.ScriptToRun);
            _baseActions.ClickOnButton(driver, _checkoutChallengeLocators.PayButton);
            await Task.Delay(8*1000);
            if (is3Ds != null!)
            {
                _baseActions.SwitchToIframe(driver, _checkoutChallengeLocators.Iframe);
                _baseActions.ClickOnButton(driver,
                    _baseActions.FindListElementsByAttribute(driver, _paymentFormV4.SuccessButton).Count > 0
                        ? _paymentFormV4.SuccessButton
                        : _paymentFormV4V1.SuccessButton);
            }

            await Task.Delay(5*1000);
            _baseActions.ValidateNavigationByUrl(driver, _checkoutChallengeLocators.Success);
            _baseActions.TearDown(driver);
            Console.WriteLine("DoTheChallengeV35 Succeeded\n");
            return true;
        }
        catch (Exception exception)
        {
            Console.WriteLine("\nPrinting audit log result messages:");
            await _auditLogController.PrintAllAuditLogResultMessagesAsync(requestHeader, ipn);
            Console.WriteLine("Error in DoTheChallengeV35\n" + exception + "\n");
            _baseActions.TearDown(driver);
            throw;
        }
    }

    public async Task DoTheChallengeCheckout3DsAsync(string url)
    {
        var driver = _driverFactory.InitDriver();
        try
        {
            Console.WriteLine("\nStarting DoCheckout3Ds");
            _baseActions.NavigateToUrl(driver, url);
            await Task.Delay(5*1000);
            _baseActions.WaitForElementToBeExists(driver, _checkout3DsForAli.FirstIframe);
            _baseActions.SwitchToIframe(driver, _checkout3DsForAli.FirstIframe);
            _baseActions.ClickOnButton(driver, _checkout3DsForAli.PasswordTextBox);
            _baseActions.SendKeysToTextBox(driver, _checkout3DsForAli.PasswordTextBox,
                _checkout3DsForAli.PasswordValue);
            _baseActions.ClickOnButton(driver, _checkout3DsForAli.ContinueButton);
            await Task.Delay(7*1000);
            _baseActions.ValidateNavigationByUrl(driver, _checkoutChallengeLocators.Success);
            _baseActions.TearDown(driver);
            Console.WriteLine("Done with DoCheckout3Ds\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in DoCheckout3Ds\n" + exception + "\n");
            throw;
        }
    }

    public async Task<bool> DoTheChallengeV4Async(ResponseV3Initiate.Root json, 
        CreatePlanDefaultValues createPlanDefaultValues, RequestHeader requestHeader = null!)
    {
        var driver = _driverFactory.InitDriver();
        try
        {
            Console.WriteLine("\nStaring DoTheChallengeV4");
            _baseActions.NavigateToUrl(driver, json.CheckoutUrl);
            await Task.Delay(5*1000);
            _baseActions.ClickOnButton(driver, _paymentFormV4.CardNumber);
            _baseActions.SendKeysToTextBox(driver, _paymentFormV4.CardNumber,
                createPlanDefaultValues.paymentMethod.card.cardNumber);
            _baseActions.ClickOnButton(driver, _paymentFormV4.Cvv);
            _baseActions.SendKeysToTextBox(driver, _paymentFormV4.Cvv,
                createPlanDefaultValues.paymentMethod.card.cardCvv);
            _baseActions.ClickOnButton(driver, _paymentFormV4.Exp);
            var expYear = createPlanDefaultValues.paymentMethod.card.cardExpYear.ToString()[2..];
            var exp = "0" + createPlanDefaultValues.paymentMethod.card.cardExpMonth + "/" + expYear;
            _baseActions.SendKeysToTextBox(driver, _paymentFormV4.Exp, exp);
            _baseActions.ClickOnButton(driver, _paymentFormV4.AcceptTermCheckBox);
            _baseActions.ClickOnButton(driver, _paymentFormV4.PayButton);
            await Task.Delay(10*1000);
            _baseActions.SwitchToIframe(driver, _paymentFormV4.Iframe);
            _baseActions.ClickOnButton(driver, _paymentFormV4.SuccessButton);
            _baseActions.ValidateNavigationByElement(driver, _paymentFormV4.SuccessImg);
            _baseActions.TearDown(driver);
            Console.WriteLine("DoTheChallengeV4 Succeeded\n");
            return true;
        }
        catch (Exception exception)
        {
            Console.WriteLine("\nPrinting audit log result messages:");
            await _auditLogController.PrintAllAuditLogResultMessagesAsync( requestHeader, json.InstallmentPlanNumber);
            Console.WriteLine("Error in DoTheChallengeV4\n" + exception + "\n");
            _baseActions.TearDown(driver);
            throw;
        }
    }

    public async Task<bool> DoTheChallengeV4Pf4Async(ResponseV3Initiate.Root json, CreatePlanDefaultValues createPlanDefaultValues,
        int installmentsAmount, string gatewayName = null!, string? checkInstallmentOptions = null!,
        RequestHeader requestHeader = null!)
    {
        var driver = _driverFactory.InitDriver();
        try
        {
            Console.WriteLine("\nStaring DoTheChallengeV4Pf4");
            _baseActions.NavigateToUrl(driver, json.CheckoutUrl);
            if (checkInstallmentOptions != null)
            {
                _paymentFormV4Functionality.ValidateInstallmentOptions(driver, installmentsAmount);
            }
            await Task.Delay(5*1000);
            _baseActions.ClickOnButton(driver, _paymentFormV4.CardNumber);
            _baseActions.SendKeysToTextBox(driver, _paymentFormV4.CardNumber,
                createPlanDefaultValues.paymentMethod.card.cardNumber);
            _baseActions.ClickOnButton(driver, _paymentFormV4.Cvv);
            _baseActions.SendKeysToTextBox(driver, _paymentFormV4.Cvv,
                createPlanDefaultValues.paymentMethod.card.cardCvv);
            _baseActions.ClickOnButton(driver, _paymentFormV4.Exp);
            var expYear = createPlanDefaultValues.paymentMethod.card.cardExpYear.ToString()[2..];
            var checkExp = createPlanDefaultValues.paymentMethod.card.cardExpMonth.ToString();
            string expMth;
            if (checkExp.Length == 1)
            {
                expMth = "0" + createPlanDefaultValues.paymentMethod.card.cardExpMonth + "/" + expYear;
            }
            else
            {
                expMth = createPlanDefaultValues.paymentMethod.card.cardExpMonth + "/" + expYear;
            }
            _baseActions.SendKeysToTextBox(driver, _paymentFormV4.Exp, expMth);
            await Task.Delay(4*1000);
            _baseActions.ClickOnButton(driver, _paymentFormV4.AcceptTermCheckBox);
            _baseActions.ClickOnButton(driver, _paymentFormV4.PayButton);
            if (createPlanDefaultValues.attempt3DSecure)
            {
                await Task.Delay(5*1000);
                var temp = _baseActions.FindListElementsByAttribute(driver, _paymentFormV4.ErrorMessage);
                if (temp.Count == 0)
                {
                    switch (gatewayName)
                    {
                        case "Adyen":
                            DoAdyen3Ds(driver);
                            break;
                        case "BlueSnapDirect":
                            DoBlueSnapDirect3Ds(driver);
                            break;
                        case "BlueSnapMor":
                            DoBlueSnapMor3Ds(driver);
                            break;
                        case "Checkout":
                            DoCheckout3Ds(driver);
                            break;
                        case "CyberSource":
                            DoCyberSource3Ds(driver);
                            break;
                        case "PayLikeV2":
                            DoPayLikeV23Ds(driver);
                            break;
                        case "PaySafe":
                            DoBlueSnapMor3Ds(driver);
                            break;
                        case "Reach":
                            DoReach3Ds(driver);
                            break;
                        case "SagePay":
                            DoSagePay3Ds(driver);
                            break;
                        case "StripeDirect":
                            await DoStripeDirect3DsAsync(driver);
                            break;
                        case "WorldPay":
                            await DoWorldPay3DsAsync(driver);
                            break;
                        default:
                            _baseActions.WaitForElementToBeVisible(driver, _paymentFormV4V1.Iframe);
                            _baseActions.SwitchToIframe(driver, _paymentFormV4V1.Iframe);
                            _baseActions.ClickOnButton(driver, _paymentFormV4V1.SuccessButton);
                            break;
                    }
                }
                else
                {
                    Assert.Fail("Error message in Payment Form to use a different card");
                }
            }

            await Task.Delay(5 * 1000);
            _baseActions.TearDown(driver);
            var fullPlanInfoResponse =
                await _fullPlanInfoIpn.SendGetFullPlanInfoIpnAsync(requestHeader, json.InstallmentPlanNumber);
            if (fullPlanInfoResponse.InstallmentPlan.InstallmentPlanStatus.Code != "InProgress" && fullPlanInfoResponse.InstallmentPlan.InstallmentPlanStatus.Code != "Cleared"
                && fullPlanInfoResponse.InstallmentPlan.InstallmentPlanStatus.Code != "Canceled")
            {
                Assert.Fail("InstallmentPlanStatus is not in the right state -> actual -> "
                            + fullPlanInfoResponse.InstallmentPlan.InstallmentPlanStatus.Code);   
            }
            Console.WriteLine("DoTheChallengeV4Pf4 Succeeded\n");
            return true;
        }
        catch (Exception exception)
        {
            Console.WriteLine("\nPrinting audit log result messages:");
            await _auditLogController.PrintAllAuditLogResultMessagesAsync( requestHeader, json.InstallmentPlanNumber);
            Console.WriteLine("Error in DoTheChallengeV4Pf4\n" + exception + "\n");
            _baseActions.TearDown(driver);
            throw;
        }
    }

    public async Task<bool> DoTheChallengeV4ForV1Async(InitiateResponse.Root json, V1InitiateDefaultValues v1InitiateDefaultValues,
        Dictionary<string, string> envDict = null!, RequestHeader requestHeader = null!)
    {
        var driver = _driverFactory.InitDriver();
        try
        {
            Console.WriteLine("\nStaring DoTheChallengeV4ForV1");
            _baseActions.NavigateToUrl(driver, json.CheckoutUrl);
            await Task.Delay(5*1000);
            _baseActions.ClickOnButton(driver, _paymentFormV4V1.CardNumber);
            _baseActions.SendKeysToTextBox(driver, _paymentFormV4V1.CardNumber, v1InitiateDefaultValues.CreditCardDetails.cardNumber);
            _baseActions.ClickOnButton(driver, _paymentFormV4V1.Cvv);
            _baseActions.SendKeysToTextBox(driver, _paymentFormV4V1.Cvv, v1InitiateDefaultValues.CreditCardDetails.cardCvv);
            _baseActions.ClickOnButton(driver, _paymentFormV4V1.Exp);
            var expYear = v1InitiateDefaultValues.CreditCardDetails.cardExpYear;
            var exp = "0" + v1InitiateDefaultValues.CreditCardDetails.cardExpMonth + "/" + expYear;
            _baseActions.SendKeysToTextBox(driver, _paymentFormV4V1.Exp, exp);
            await Task.Delay(3*1000);
            _baseActions.ClickOnButton(driver, _paymentFormV4V1.AcceptTermCheckBox);
            _baseActions.ClickOnButton(driver, _paymentFormV4V1.PayButton);
            if (v1InitiateDefaultValues.planData.Attempt3DSecure)
            {
                _baseActions.WaitForElementToBeVisible(driver, _paymentFormV4V1.Iframe);
                _baseActions.SwitchToIframe(driver, _paymentFormV4V1.Iframe);
                _baseActions.ClickOnButton(driver, _paymentFormV4V1.SuccessButton);
            }
            else
            {
                await Task.Delay(15*1000);
                Console.WriteLine("Validating in success page");
                var currentUrl = _baseActions.GetCurrentUrl(driver);
                Assert.That(currentUrl.Contains("success"));
                Console.WriteLine("Validated we are in success page");
            }

            _baseActions.TearDown(driver);
            Console.WriteLine("DoTheChallengeV4ForV1 Succeeded\n");
            return true;
        }
        catch (Exception exception)
        {
            Console.WriteLine("\nPrinting audit log result messages:");
            await _auditLogController.PrintAllAuditLogResultMessagesAsync( requestHeader,
                json.InstallmentPlan.InstallmentPlanNumber);
            Console.WriteLine("Error in DoTheChallengeV4ForV1\n" + exception + "\n");
            _baseActions.TearDown(driver);
            throw;
        }
    }

    private void DoAdyen3Ds(WebDriver driver)
    {
        try
        {
            Console.WriteLine("\nStarting DoAdyen3Ds");
            _baseActions.WaitForElementToBeVisible(driver, _adyen3DsLocators.FirstIframe);
            _baseActions.SwitchToIframe(driver, _adyen3DsLocators.FirstIframe);
            _baseActions.WaitForElementToBeVisible(driver, _adyen3DsLocators.SecondIframe);
            _baseActions.SwitchToIframe(driver, _adyen3DsLocators.SecondIframe);
            _baseActions.ClickOnButton(driver, _adyen3DsLocators.PasswordTextBox);
            _baseActions.SendKeysToTextBox(driver, _adyen3DsLocators.PasswordTextBox, Adyen3DsLocators.PasswordValue);
            _baseActions.ClickOnButton(driver, _adyen3DsLocators.ContinueButton);
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
            _baseActions.WaitForElementToBeVisible(driver, _cyberSource3DsLocators.FirstIframe);
            _baseActions.SwitchToIframe(driver, _cyberSource3DsLocators.FirstIframe);
            _baseActions.WaitForElementToBeVisible(driver, _cyberSource3DsLocators.SecondIframe);
            _baseActions.SwitchToIframe(driver, _cyberSource3DsLocators.SecondIframe);
            _baseActions.ClickOnButton(driver, _cyberSource3DsLocators.PasswordTextBox);
            _baseActions.SendKeysToTextBox(driver, _cyberSource3DsLocators.PasswordTextBox,
                CyberSource3DsLocators.PasswordValue);
            _baseActions.ClickOnButton(driver, _cyberSource3DsLocators.ContinueButton);
            Console.WriteLine("Done with DoCyberSource3Ds\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in DoCyberSource3Ds\n" + exception + "\n");
            throw;
        }
    }

    private void DoBlueSnapDirect3Ds(WebDriver driver)
    {
        try
        {
            Console.WriteLine("\nStarting DoBlueSnapDirect3Ds");
            _baseActions.WaitForElementToBeVisible(driver, _blueSnapDirect3DsLocators.FirstIframe);
            _baseActions.SwitchToIframe(driver, _blueSnapDirect3DsLocators.FirstIframe);
            _baseActions.WaitForElementToBeVisible(driver, _blueSnapDirect3DsLocators.SecondIframe);
            _baseActions.SwitchToIframe(driver, _blueSnapDirect3DsLocators.SecondIframe);
            _baseActions.ClickOnButton(driver, _blueSnapDirect3DsLocators.PasswordTextBox);
            _baseActions.SendKeysToTextBox(driver, _blueSnapDirect3DsLocators.PasswordTextBox,
                _blueSnapDirect3DsLocators.PasswordValue);
            _baseActions.ClickOnButton(driver, _blueSnapDirect3DsLocators.ContinueButton);
            Console.WriteLine("Done with DoBlueSnapDirect3Ds\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in DoBlueSnapDirect3Ds\n" + exception + "\n");
            throw;
        }
    }

    private void DoBlueSnapMor3Ds(WebDriver driver)
    {
        try
        {
            Console.WriteLine("\nStarting DoBlueSnapMor3Ds");
            _baseActions.WaitForElementToBeVisible(driver, _blueSnapMor3DsLocators.FirstIframe);
            _baseActions.SwitchToIframe(driver, _blueSnapMor3DsLocators.FirstIframe);
            _baseActions.WaitForElementToBeVisible(driver, _blueSnapMor3DsLocators.SecondIframe);
            _baseActions.SwitchToIframe(driver, _blueSnapMor3DsLocators.SecondIframe);
            _baseActions.ClickOnButton(driver, _blueSnapMor3DsLocators.PasswordTextBox);
            _baseActions.SendKeysToTextBox(driver, _blueSnapMor3DsLocators.PasswordTextBox,
                _blueSnapMor3DsLocators.PasswordValue);
            _baseActions.ClickOnButton(driver, _blueSnapMor3DsLocators.ContinueButton);
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
            _baseActions.WaitForElementToBeVisible(driver, _checkout3DsLocators.FirstIframe);
            _baseActions.SwitchToIframe(driver, _checkout3DsLocators.FirstIframe);
            _baseActions.WaitForElementToBeVisible(driver, _checkout3DsLocators.SecondIframe);
            _baseActions.SwitchToIframe(driver, _checkout3DsLocators.SecondIframe);
            _baseActions.ClickOnButton(driver, _checkout3DsLocators.PasswordTextBox);
            _baseActions.SendKeysToTextBox(driver, _checkout3DsLocators.PasswordTextBox,
                _checkout3DsLocators.PasswordValue);
            _baseActions.ClickOnButton(driver, _checkout3DsLocators.ContinueButton);
            Console.WriteLine("Done with DoCheckout3Ds\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in DoCheckout3Ds\n" + exception + "\n");
            throw;
        }
    }

    private void DoPayLikeV23Ds(WebDriver driver)
    {
        try
        {
            Console.WriteLine("\nStarting DoPayLikeV23Ds");
            _baseActions.WaitForElementToBeExists(driver, _payLikeV23DsLocators.FirstIframe);
            _baseActions.SwitchToIframe(driver, _payLikeV23DsLocators.FirstIframe);
            _baseActions.WaitForElementToBeExists(driver, _payLikeV23DsLocators.SecondIframe);
            _baseActions.SwitchToIframe(driver, _payLikeV23DsLocators.SecondIframe);
            Thread.Sleep(4*1000);
            _baseActions.RunJavaScript(driver, _payLikeV23DsLocators.JsCommand);
            Console.WriteLine("Done with DoPayLikeV23Ds\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in DoPayLikeV23Ds\n" + exception + "\n");
            throw;
        }
    }

    private void DoReach3Ds(WebDriver driver)
    {
        try
        {
            Console.WriteLine("\nStarting DoReach3Ds");
            _baseActions.WaitForElementToBeVisible(driver, _reach3DsLocators.FirstIframe);
            _baseActions.SwitchToIframe(driver, _reach3DsLocators.FirstIframe);
            _baseActions.WaitForElementToBeVisible(driver, _reach3DsLocators.SecondIframe);
            _baseActions.SwitchToIframe(driver, _reach3DsLocators.SecondIframe);
            _baseActions.ClickOnButton(driver, _reach3DsLocators.PasswordTextBox);
            _baseActions.SendKeysToTextBox(driver, _reach3DsLocators.PasswordTextBox, _reach3DsLocators.PasswordValue);
            _baseActions.ClickOnButton(driver, _reach3DsLocators.OkButton);
            Console.WriteLine("Done with DoReach3Ds\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in DoReach3Ds\n" + exception + "\n");
            throw;
        }
    }

    private async Task DoWorldPay3DsAsync(WebDriver driver)
    {
        try
        {
            Console.WriteLine("\nStarting DoWorldPay3Ds");
            _baseActions.WaitForElementToBeVisible(driver, _worldPay3DsLocators.FirstIframe);
            await Task.Delay(3*1000);
            _baseActions.SwitchToIframe(driver, _worldPay3DsLocators.FirstIframe);
            // _baseActions.SwitchToIframe(driver, _worldPay3DsLocators.SecondIframe);
            _baseActions.WaitForElementToBeVisible(driver, _worldPay3DsLocators.OkButton);
            _baseActions.ClickOnButton(driver, _worldPay3DsLocators.OkButton);
            Console.WriteLine("Done with DoWorldPay3Ds\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in DoWorldPay3Ds\n" + exception + "\n");
            throw;
        }
    }
    
    private async Task DoStripeDirect3DsAsync(WebDriver driver)
    {
        try
        {
            Console.WriteLine("\nStarting DoStripeDirect3DsAsync");
            await Task.Delay(3 * 1000);
            Console.WriteLine("Done with DoStripeDirect3DsAsync\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in DoStripeDirect3DsAsync\n" + exception + "\n");
            throw;
        }
    }

    private void DoSagePay3Ds(WebDriver driver)
    {
        try
        {
            Console.WriteLine("\nStarting DoSagePay3Ds");
            _baseActions.WaitForElementToBeVisible(driver, _sagePay3DsLocators.FirstIframe);
            _baseActions.SwitchToIframe(driver, _sagePay3DsLocators.FirstIframe);
            _baseActions.ClickOnButton(driver, _sagePay3DsLocators.PasswordTextBox);
            _baseActions.SendKeysToTextBox(driver, _sagePay3DsLocators.PasswordTextBox,
                _sagePay3DsLocators.PasswordValue);
            _baseActions.ClickOnButton(driver, _sagePay3DsLocators.NextButton);
            Console.WriteLine("Done with DoSagePay3Ds\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in DoSagePay3Ds\n" + exception + "\n");
            throw;
        }
    }

    public async Task<bool> DoTheChallengeVisAsync(ResponseV3Initiate.Root json)
    {
        var driver = _driverFactory.InitDriver();
        
        try
        {
            _baseActions.NavigateToUrl(driver, json.CheckoutUrl);
            await Task.Delay(5 * 1000);
            _baseActions.ClickOnButton(driver, _visLocators.CardNumberTextBox);
            _baseActions.SendKeysToTextBox(driver, _visLocators.CardNumberTextBox, _envConfig.VisMerchantFeeOnlyCardNumber);
            _baseActions.ClickOnButton(driver, _visLocators.ExpDateTextBox);
            _baseActions.SendKeysToTextBox(driver, _visLocators.ExpDateTextBox, _envConfig.VisExp);
            _baseActions.ClickOnButton(driver, _visLocators.CvvTextBox);
            _baseActions.SendKeysToTextBox(driver, _visLocators.CvvTextBox, _envConfig.VisCvv);
            _baseActions.ClickOnButton(driver, _visLocators.ContinueButton);
            await Task.Delay(5 * 1000);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}