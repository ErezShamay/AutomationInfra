using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.AdminApi.InstallmentPlan.InstallmentsPlanFunctionality;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.Backend.Tests.TestsSetups;
using Splitit.Automation.NG.SharedResources.Pages;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.SharedResources.Tests.FlexFormsTests;

[TestFixture]
[AllureNUnit]
[AllureSuite("FlexFormsTestsSuite")]
[AllureDisplayIgnored]
public class FlexFormsTests
{
    private RequestHeader? _requestHeader;
    private readonly BaseActions.BaseActions _baseActions;
    private readonly DriverFactory.DriverFactory _driverFactory;
    private readonly FlexFormsLocators _flexFormsLocators;
    private readonly FullPlanInfoIpn _fullPlanInfoIpn;
    private readonly EnvConfig _envConfig;

    public FlexFormsTests()
    {
        Console.WriteLine("Starting Setup");
        _baseActions = new BaseActions.BaseActions();
        _driverFactory = new DriverFactory.DriverFactory();
        _flexFormsLocators = new FlexFormsLocators();
        _fullPlanInfoIpn = new FullPlanInfoIpn();
        _envConfig = new EnvConfig();
        Console.WriteLine("Done with Setup\n");
    }
    
    [OneTimeSetUp]
    public async Task InitSetUp()
    {
        var testsSetup = new TestsSetup();
        testsSetup.Setup();
        var sendAdminLoginRequest = new SendAdminLoginRequest();
        _requestHeader = await sendAdminLoginRequest.DoAdminLogin(
            Environment.GetEnvironmentVariable("AccessTokenURI")!,
            Environment.GetEnvironmentVariable("ClientSecret")!,
            Environment.GetEnvironmentVariable("SplititMockTerminal")!,
            Environment.GetEnvironmentVariable("clientId")!);
    }

    [Category("FlexFormsTests")]
    [Test(Description = "DoPurchaseWithFlexForm")]
    public async Task DoPurchaseWithFlexForm()
    {
        try
        {
            Console.WriteLine("\nStarting DoPurchaseWithFlexForm");
            var driver = _driverFactory.InitDriver();
            _baseActions.NavigateToUrl(driver, _flexFormsLocators.StoreUrl);
            _baseActions.ValidateNavigationByElement(driver, _flexFormsLocators.AddToCartButton);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.AddToCartButton);
            _baseActions.ValidateNavigationByElement(driver, _flexFormsLocators.ViewCartButton);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.ViewCartButton);
            _baseActions.ValidateNavigationByElement(driver, _flexFormsLocators.ProceedToCheckoutButton);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.ProceedToCheckoutButton);
            _baseActions.ValidateNavigationByElement(driver, _flexFormsLocators.FirstNameTextBox);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.FirstNameTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.FirstNameTextBox,
                _flexFormsLocators.FirstNameValue);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.LastNameTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.LastNameTextBox,
                _flexFormsLocators.LastNameValue);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.StreetAddressTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.StreetAddressTextBox,
                _flexFormsLocators.StreetAddressValue);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.ZipCodeTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.ZipCodeTextBox, _flexFormsLocators.ZipCodeValue);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.CityTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.CityTextBox, _flexFormsLocators.CityValue);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.PhoneTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.PhoneTextBox, _flexFormsLocators.PhoneValue);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.EmailAddressTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.EmailAddressTextBox,
                _flexFormsLocators.EmailAddressValue);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.MonthlyRadioButton);
            await Task.Delay(3 * 1000);
            _baseActions.ValidateNavigationByElement(driver, _flexFormsLocators.IframeCardNumber);
            _baseActions.SwitchToIframe(driver, _flexFormsLocators.IframeCardNumber);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.CardNumberTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.CardNumberTextBox, "4111111111111111");
            _baseActions.SwitchToDefaultFrame(driver);
            await Task.Delay(3 * 1000);
            _baseActions.SwitchToIframe(driver, _flexFormsLocators.IframeExp);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.CardNumberExpTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.CardNumberExpTextBox, "03/27");
            _baseActions.SwitchToDefaultFrame(driver);
            await Task.Delay(3 * 1000);
            _baseActions.SwitchToIframe(driver, _flexFormsLocators.IframeCvv);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.CardNumberCvvTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.CardNumberCvvTextBox, "737");
            _baseActions.SwitchToDefaultFrame(driver);
            await Task.Delay(3 * 1000);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.TermAndConditionsCheckBox);
            await Task.Delay(2 * 1000);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.PlaceOrderButton);
            _baseActions.ValidateNavigationByElement(driver, _flexFormsLocators.OrderConfirmationMessage);
            _baseActions.ValidateNavigationByElement(driver, _flexFormsLocators.InstallmentPlanNumberHeader);
            var elementText = _baseActions.GetTextFromElement(driver, _flexFormsLocators.InstallmentPlanNumberValue);
            var fixedString = elementText.Remove(0, 25);
            var responseFullPlanInfo = await _fullPlanInfoIpn.SendGetFullPlanInfoIpnAsync(_requestHeader!, fixedString);
            Assert.That(responseFullPlanInfo.InstallmentPlan.InstallmentPlanStatus.Code.Equals("InProgress"));
            Console.WriteLine("DoPurchaseWithFlexForm is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in DoPurchaseWithFlexForm \n" + exception + "\n");
        }
    }

    [Category("FlexFormsTests")]
    [Test(Description = "ErrorHandlingTryToDoPurchaseWithFlexFormNoCc")]
    public async Task ErrorHandlingTryToDoPurchaseWithFlexFormNoCc()
    {
        try
        {
            Console.WriteLine("\nStarting ErrorHandlingTryToDoPurchaseWithFlexFormNoCc");
            var driver = _driverFactory.InitDriver();
            _baseActions.NavigateToUrl(driver, _flexFormsLocators.StoreUrl);
            _baseActions.ValidateNavigationByElement(driver, _flexFormsLocators.AddToCartButton);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.AddToCartButton);
            _baseActions.ValidateNavigationByElement(driver, _flexFormsLocators.ViewCartButton);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.ViewCartButton);
            _baseActions.ValidateNavigationByElement(driver, _flexFormsLocators.ProceedToCheckoutButton);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.ProceedToCheckoutButton);
            _baseActions.ValidateNavigationByElement(driver, _flexFormsLocators.FirstNameTextBox);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.FirstNameTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.FirstNameTextBox,
                _flexFormsLocators.FirstNameValue);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.LastNameTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.LastNameTextBox,
                _flexFormsLocators.LastNameValue);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.StreetAddressTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.StreetAddressTextBox,
                _flexFormsLocators.StreetAddressValue);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.ZipCodeTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.ZipCodeTextBox, _flexFormsLocators.ZipCodeValue);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.CityTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.CityTextBox, _flexFormsLocators.CityValue);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.PhoneTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.PhoneTextBox, _flexFormsLocators.PhoneValue);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.EmailAddressTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.EmailAddressTextBox,
                _flexFormsLocators.EmailAddressValue);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.MonthlyRadioButton);
            await Task.Delay(3 * 1000);
            _baseActions.ValidateNavigationByElement(driver, _flexFormsLocators.IframeCardNumber);
            _baseActions.SwitchToIframe(driver, _flexFormsLocators.IframeCardNumber);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.CardNumberTextBox);
            _baseActions.SwitchToDefaultFrame(driver);
            await Task.Delay(3 * 1000);
            _baseActions.SwitchToIframe(driver, _flexFormsLocators.IframeExp);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.CardNumberExpTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.CardNumberExpTextBox, "03/27");
            _baseActions.SwitchToDefaultFrame(driver);
            await Task.Delay(3 * 1000);
            _baseActions.SwitchToIframe(driver, _flexFormsLocators.IframeCvv);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.CardNumberCvvTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.CardNumberCvvTextBox, "737");
            _baseActions.SwitchToDefaultFrame(driver);
            await Task.Delay(3 * 1000);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.TermAndConditionsCheckBox);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.PlaceOrderButton);
            await Task.Delay(2 * 1000);
            _baseActions.ValidateNavigationByElement(driver, _flexFormsLocators.PlaceOrderButton);
            Console.WriteLine("ErrorHandlingTryToDoPurchaseWithFlexFormNoCc is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in ErrorHandlingTryToDoPurchaseWithFlexFormNoCc \n" + exception + "\n");
        }
    }

    [Category("FlexFormsTests")]
    [Test(Description = "ErrorHandlingTryToDoPurchaseWithFlexFormNoExp")]
    public async Task ErrorHandlingTryToDoPurchaseWithFlexFormNoExp()
    {
        try
        {
            Console.WriteLine("\nStarting ErrorHandlingTryToDoPurchaseWithFlexFormNoExp");
            var driver = _driverFactory.InitDriver();
            _baseActions.NavigateToUrl(driver, _flexFormsLocators.StoreUrl);
            _baseActions.ValidateNavigationByElement(driver, _flexFormsLocators.AddToCartButton);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.AddToCartButton);
            _baseActions.ValidateNavigationByElement(driver, _flexFormsLocators.ViewCartButton);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.ViewCartButton);
            _baseActions.ValidateNavigationByElement(driver, _flexFormsLocators.ProceedToCheckoutButton);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.ProceedToCheckoutButton);
            _baseActions.ValidateNavigationByElement(driver, _flexFormsLocators.FirstNameTextBox);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.FirstNameTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.FirstNameTextBox,
                _flexFormsLocators.FirstNameValue);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.LastNameTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.LastNameTextBox,
                _flexFormsLocators.LastNameValue);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.StreetAddressTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.StreetAddressTextBox,
                _flexFormsLocators.StreetAddressValue);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.ZipCodeTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.ZipCodeTextBox, _flexFormsLocators.ZipCodeValue);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.CityTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.CityTextBox, _flexFormsLocators.CityValue);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.PhoneTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.PhoneTextBox, _flexFormsLocators.PhoneValue);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.EmailAddressTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.EmailAddressTextBox,
                _flexFormsLocators.EmailAddressValue);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.MonthlyRadioButton);
            await Task.Delay(3 * 1000);
            _baseActions.ValidateNavigationByElement(driver, _flexFormsLocators.IframeCardNumber);
            _baseActions.SwitchToIframe(driver, _flexFormsLocators.IframeCardNumber);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.CardNumberTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.CardNumberTextBox, "4111111111111111");
            _baseActions.SwitchToDefaultFrame(driver);
            await Task.Delay(3 * 1000);
            _baseActions.SwitchToIframe(driver, _flexFormsLocators.IframeExp);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.CardNumberExpTextBox);
            _baseActions.SwitchToDefaultFrame(driver);
            await Task.Delay(3 * 1000);
            _baseActions.SwitchToIframe(driver, _flexFormsLocators.IframeCvv);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.CardNumberCvvTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.CardNumberCvvTextBox, "737");
            _baseActions.SwitchToDefaultFrame(driver);
            await Task.Delay(3 * 1000);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.TermAndConditionsCheckBox);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.PlaceOrderButton);
            await Task.Delay(2 * 1000);
            _baseActions.ValidateNavigationByElement(driver, _flexFormsLocators.PlaceOrderButton);
            Console.WriteLine("ErrorHandlingTryToDoPurchaseWithFlexFormNoExp is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in ErrorHandlingTryToDoPurchaseWithFlexFormNoExp \n" + exception + "\n");
        }
    }

    [Category("FlexFormsTests")]
    [Test(Description = "ErrorHandlingTryToDoPurchaseWithFlexFormNoCvv")]
    public async Task ErrorHandlingTryToDoPurchaseWithFlexFormNoCvv()
    {
        try
        {
            Console.WriteLine("\nStarting ErrorHandlingTryToDoPurchaseWithFlexFormNoCvv");
            var driver = _driverFactory.InitDriver();
            _baseActions.NavigateToUrl(driver, _flexFormsLocators.StoreUrl);
            _baseActions.ValidateNavigationByElement(driver, _flexFormsLocators.AddToCartButton);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.AddToCartButton);
            _baseActions.ValidateNavigationByElement(driver, _flexFormsLocators.ViewCartButton);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.ViewCartButton);
            _baseActions.ValidateNavigationByElement(driver, _flexFormsLocators.ProceedToCheckoutButton);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.ProceedToCheckoutButton);
            _baseActions.ValidateNavigationByElement(driver, _flexFormsLocators.FirstNameTextBox);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.FirstNameTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.FirstNameTextBox,
                _flexFormsLocators.FirstNameValue);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.LastNameTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.LastNameTextBox,
                _flexFormsLocators.LastNameValue);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.StreetAddressTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.StreetAddressTextBox,
                _flexFormsLocators.StreetAddressValue);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.ZipCodeTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.ZipCodeTextBox, _flexFormsLocators.ZipCodeValue);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.CityTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.CityTextBox, _flexFormsLocators.CityValue);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.PhoneTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.PhoneTextBox, _flexFormsLocators.PhoneValue);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.EmailAddressTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.EmailAddressTextBox,
                _flexFormsLocators.EmailAddressValue);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.MonthlyRadioButton);
            await Task.Delay(3 * 1000);
            _baseActions.ValidateNavigationByElement(driver, _flexFormsLocators.IframeCardNumber);
            _baseActions.SwitchToIframe(driver, _flexFormsLocators.IframeCardNumber);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.CardNumberTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.CardNumberTextBox, "4111111111111111");
            _baseActions.SwitchToDefaultFrame(driver);
            await Task.Delay(3 * 1000);
            _baseActions.SwitchToIframe(driver, _flexFormsLocators.IframeExp);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.CardNumberExpTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.CardNumberExpTextBox, "03/27");
            _baseActions.SwitchToDefaultFrame(driver);
            await Task.Delay(3 * 1000);
            _baseActions.SwitchToIframe(driver, _flexFormsLocators.IframeCvv);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.CardNumberCvvTextBox);
            _baseActions.SwitchToDefaultFrame(driver);
            await Task.Delay(3 * 1000);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.TermAndConditionsCheckBox);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.PlaceOrderButton);
            await Task.Delay(2 * 1000);
            _baseActions.ValidateNavigationByElement(driver, _flexFormsLocators.PlaceOrderButton);
            Console.WriteLine("ErrorHandlingTryToDoPurchaseWithFlexFormNoCvv is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in ErrorHandlingTryToDoPurchaseWithFlexFormNoCvv \n" + exception + "\n");
        }
    }
    
    [Category("FlexFormsTests")]
    [Test(Description = "ErrorHandlingTryToDoPurchaseWithFlexFormDidNotApproveTermsAndCondition")]
    public async Task ErrorHandlingTryToDoPurchaseWithFlexFormDidNotApproveTermsAndCondition()
    {
        try
        {
            Console.WriteLine("\nStarting ErrorHandlingTryToDoPurchaseWithFlexFormDidNotApproveTermsAndCondition");
            var driver = _driverFactory.InitDriver();
            _baseActions.NavigateToUrl(driver, _flexFormsLocators.StoreUrl);
            _baseActions.ValidateNavigationByElement(driver, _flexFormsLocators.AddToCartButton);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.AddToCartButton);
            _baseActions.ValidateNavigationByElement(driver, _flexFormsLocators.ViewCartButton);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.ViewCartButton);
            _baseActions.ValidateNavigationByElement(driver, _flexFormsLocators.ProceedToCheckoutButton);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.ProceedToCheckoutButton);
            _baseActions.ValidateNavigationByElement(driver, _flexFormsLocators.FirstNameTextBox);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.FirstNameTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.FirstNameTextBox,
                _flexFormsLocators.FirstNameValue);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.LastNameTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.LastNameTextBox,
                _flexFormsLocators.LastNameValue);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.StreetAddressTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.StreetAddressTextBox,
                _flexFormsLocators.StreetAddressValue);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.ZipCodeTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.ZipCodeTextBox, _flexFormsLocators.ZipCodeValue);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.CityTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.CityTextBox, _flexFormsLocators.CityValue);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.PhoneTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.PhoneTextBox, _flexFormsLocators.PhoneValue);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.EmailAddressTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.EmailAddressTextBox,
                _flexFormsLocators.EmailAddressValue);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.MonthlyRadioButton);
            await Task.Delay(3 * 1000);
            _baseActions.ValidateNavigationByElement(driver, _flexFormsLocators.IframeCardNumber);
            _baseActions.SwitchToIframe(driver, _flexFormsLocators.IframeCardNumber);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.CardNumberTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.CardNumberTextBox, "4111111111111111");
            _baseActions.SwitchToDefaultFrame(driver);
            await Task.Delay(3 * 1000);
            _baseActions.SwitchToIframe(driver, _flexFormsLocators.IframeExp);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.CardNumberExpTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.CardNumberExpTextBox, "03/27");
            _baseActions.SwitchToDefaultFrame(driver);
            await Task.Delay(3 * 1000);
            _baseActions.SwitchToIframe(driver, _flexFormsLocators.IframeCvv);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.CardNumberCvvTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFormsLocators.CardNumberCvvTextBox, "737");
            _baseActions.SwitchToDefaultFrame(driver);
            await Task.Delay(3 * 1000);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.TermAndConditionsCheckBox);
            _baseActions.ClickOnButton(driver, _flexFormsLocators.PlaceOrderButton);
            await Task.Delay(2 * 1000);
            _baseActions.ValidateNavigationByElement(driver, _flexFormsLocators.PlaceOrderButton);
            Console.WriteLine("ErrorHandlingTryToDoPurchaseWithFlexFormDidNotApproveTermsAndCondition is Done\n");
        }
        catch (Exception exception)
        {
            Assert.Fail("Error in ErrorHandlingTryToDoPurchaseWithFlexFormDidNotApproveTermsAndCondition \n" + exception + "\n");
        }
    }
}