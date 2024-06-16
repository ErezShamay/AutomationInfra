using NUnit.Framework;
using OpenQA.Selenium;
using Splitit.Automation.NG.Backend.Services.V3.InstallmentPlan.InstallmentPlanResponses;

namespace Splitit.Automation.NG.SharedResources.BaseActions;

public class PaymentFormV4Functionality
{
    private readonly DriverFactory.DriverFactory _driverFactory = new();
    private readonly BaseActions _baseActions = new();
    private readonly Pages.PaymentFormV4 _paymentFormV4 = new();
    
    public void ValidateInstallmentOptions(WebDriver driver, int installmentsAmount)
    {
        var attribute = "installment-option-"; 
        try
        {
            Console.WriteLine("\nStarting ValidateInstallmentOptions");
            var incInstallmentAmount = installmentsAmount + 1;
            _baseActions.WaitForElementToBeExists(driver, By.XPath("//*[@qa-id='" + attribute + incInstallmentAmount + "']"));
            driver.FindElement(By.XPath("//*[@qa-id='" + attribute + incInstallmentAmount + "']"));
            Assert.Fail("Error in ValidateInstallmentOptions\n");
        }
        catch (Exception)
        {
            Console.WriteLine("Done ValidateInstallmentOptions\n");
        }
    }

    public bool ValidatePf4Fields(ResponseV3Initiate.Root json, string? validateBilling = null!, 
        string? validatePartialAddress = null!, string? validateEmail = null!)
    {
        var driver = _driverFactory.InitDriver();
        
        try
        {
            Console.WriteLine("\nStarting ValidatePf4Fields");
            _baseActions.NavigateToUrl(driver, json.CheckoutUrl);
            if (validateBilling != null)
            {
                Console.WriteLine("Validating Address field is null");
                var address = _baseActions.GetTextFromElement(driver, _paymentFormV4.AddressLine);
                if(!address.Equals(""))
                {
                    Assert.Fail("Address field is NOT null");
                }
                Console.WriteLine("Success Address field is null");
                if (validatePartialAddress != null)
                {
                    Console.WriteLine("Validating City field is not null");
                    var city = _baseActions.GetTextFromElement(driver, _paymentFormV4.City);
                    if(!city.Equals(""))
                    {
                        Assert.Fail("City field is null");
                    }
                    Console.WriteLine("Success City field is not null");
                }
            }
            if (validateEmail != null)
            {
                Console.WriteLine("Validating Email field is null");
                var email = _baseActions.GetTextFromElement(driver, _paymentFormV4.Email);
                if(!email.Equals(""))
                {
                    Assert.Fail("Email field is NOT null");
                }
                Console.WriteLine("Success Email field is null");
            }
            Console.WriteLine("Done ValidatePf4Fields");
            _baseActions.TearDown(driver);
            return true;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in ValidatePf4Fields\n" + exception + "\n");
            _baseActions.TearDown(driver);
            throw;
        }
    }

    public bool ValidateMandatoryFields(ResponseV3Initiate.Root json, By clickElement, By clickElement2, By errorMessageElement)
    {
        var driver = _driverFactory.InitDriver();
        
        try
        {
            Console.WriteLine("\nStarting ValidateMandatoryFields");
            _baseActions.NavigateToUrl(driver, json.CheckoutUrl);
            _baseActions.ClickOnButton(driver, clickElement);
            _baseActions.ClickOnButton(driver, clickElement2);
            _baseActions.ValidateNavigationByElement(driver, errorMessageElement);
            Console.WriteLine("Done with ValidateMandatoryFields\n");
            _baseActions.TearDown(driver);
            return true;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in ValidateMandatoryFields \n" + exception + "\n");
            _baseActions.TearDown(driver);
            throw;
        }
    }

    public bool ValidateLinksFlow(ResponseV3Initiate.Root json, By linkElement, By iframeElement)
    {
        var driver = _driverFactory.InitDriver();
        
        try
        {
            Console.WriteLine("\nStarting ValidateLinksFlow");
            _baseActions.NavigateToUrl(driver, json.CheckoutUrl);
            _baseActions.ClickOnButton(driver, linkElement);
            _baseActions.SwitchToIframe(driver, iframeElement);
            Console.WriteLine("Done with ValidateLinksFlow\n");
            _baseActions.TearDown(driver);
            return true;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in ValidateLinksFlow \n" + exception + "\n");
            _baseActions.TearDown(driver);
            throw;
        }
    }
}