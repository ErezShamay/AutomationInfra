using OpenQA.Selenium;

namespace Splitit.Automation.NG.SharedResources.Pages;

public class FlexFieldsConfigLocators
{
    public readonly string SetCredentialsUrl = "https://examples.production.splitit.com/Home/SetCredentials";
    public readonly By ApiUserNameTextBox = By.Id("SplititApiUsername");
    public readonly By ApiPasswordTextBox = By.Id("SplititApiPassword");
    public readonly By ApiKeyTextBox = By.Id("SplititApiKey");
    public readonly By ApiUrlTextBox = By.Id("SplititApiUrl");
    public readonly By FlexFieldsUrlTextBox = By.Id("FlexFieldsUrlRoot");
    public readonly By FlexFieldsUrlV2TextBox = By.Id("FlexFieldsV2UrlRoot");
    public readonly string FlexFieldsUrlV2Value = "https://flexfields.production.splitit.com/v2.0";
    public readonly By UpstreamUrlTextBox = By.Id("UpstreamUrlRoot");
    public readonly By PaymentFormUrlTextBox = By.Id("PaymentFormEmbedderUrlRoot");
    public readonly By SubmitButton = By.XPath("/html/body/div/div/div/form[1]/button");
    public readonly By ResetButton = By.XPath("/html/body/div/div/div/form[2]/button");
    public readonly string ScenarioV2ThreeDsUrl = "https://examples.production.splitit.com/ScenarioV2/Secure3D?options=5&amount="+new Random().Next(100, 1000)+"&currency=USD";
    public readonly By ChooseSplititPaymentOptionButton = By.XPath("/html/body/div[2]/main/div[1]/button");
}