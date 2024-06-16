using OpenQA.Selenium;
using Splitit.Automation.NG.SharedResources.Pages;
using Splitit.Automation.NG.Utilities.EnvironmentsConfig;

namespace Splitit.Automation.NG.SharedResources.BaseActions;

public class FlexFieldsFunctionality
{
    private readonly BaseActions _baseActions = new();
    private readonly FlexFieldsConfigLocators _flexFieldsConfigLocators = new();
    private readonly FlexFieldsFormLocators _flexFieldsFormLocators = new();
    private readonly EnvConfig _envConfig = new();
    
    public async Task SetFlexFieldsConfig(WebDriver driver)
    {
        try
        {
            Console.WriteLine("\nStarting SetFlexFieldsConfig");
            _baseActions.NavigateToUrl(driver, _flexFieldsConfigLocators.SetCredentialsUrl);
            _baseActions.ClearTextBox(driver, _flexFieldsConfigLocators.ApiUserNameTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFieldsConfigLocators.ApiUserNameTextBox, _envConfig.UserAutomationTestUserNameProduction);
            _baseActions.ClearTextBox(driver, _flexFieldsConfigLocators.ApiPasswordTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFieldsConfigLocators.ApiPasswordTextBox, _envConfig.UserAutomationTestPasswordProduction);
            _baseActions.ClearTextBox(driver, _flexFieldsConfigLocators.ApiKeyTextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFieldsConfigLocators.ApiKeyTextBox, _envConfig.SplititMockTerminalProduction);
            _baseActions.ClearTextBox(driver, _flexFieldsConfigLocators.FlexFieldsUrlV2TextBox);
            _baseActions.SendKeysToTextBox(driver, _flexFieldsConfigLocators.FlexFieldsUrlV2TextBox, _flexFieldsConfigLocators.FlexFieldsUrlV2Value);
            _baseActions.ClickOnButton(driver, _flexFieldsConfigLocators.SubmitButton);
            _baseActions.NavigateToUrl(driver, _flexFieldsConfigLocators.ScenarioV2ThreeDsUrl);
            await Task.Delay(3*1000);
            _baseActions.ClickOnButton(driver, _flexFieldsConfigLocators.ChooseSplititPaymentOptionButton);
            Console.WriteLine("Done with SetFlexFieldsConfig\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SetFlexFieldsConfig\n" + exception + "\n");
            throw;
        }
    }

    public async Task FillFlexFieldsFields(WebDriver driver)
    {
        try
        {
            Console.WriteLine("\nStarting FillFlexFieldsFields");
            _baseActions.SwitchToIframe(driver, _flexFieldsFormLocators.IframeCardNumber);
            _baseActions.SendKeysToTextBox(driver, _flexFieldsFormLocators.CardNumberTextBox, _flexFieldsFormLocators.CardNumberValue);
            _baseActions.SwitchToDefaultFrame(driver);
            _baseActions.SwitchToIframe(driver, _flexFieldsFormLocators.IframeExp);
            _baseActions.SendKeysToTextBox(driver, _flexFieldsFormLocators.ExpTextBox, _flexFieldsFormLocators.ExpValue);
            _baseActions.SwitchToDefaultFrame(driver);
            _baseActions.SwitchToIframe(driver, _flexFieldsFormLocators.IframeCvv);
            _baseActions.SendKeysToTextBox(driver, _flexFieldsFormLocators.CvvTextBox, _flexFieldsFormLocators.CvvValue);
            _baseActions.SwitchToDefaultFrame(driver);
            await Task.Delay(3*1000);
            _baseActions.RunJavaScript(driver, _flexFieldsFormLocators.ScriptToRun);
            await Task.Delay(3*1000);
            _baseActions.ClickOnButton(driver, _flexFieldsFormLocators.PayButton);
            await Task.Delay(3*1000);
            var eList = driver.FindElements(
                By.XPath("//*[text()='3D secure verification was not completed successfully.']"));
            if(eList.Count > 0)
            {
                _baseActions.ClickOnButton(driver, _flexFieldsFormLocators.PayButton);
            }
            Console.WriteLine("Done with FillFlexFieldsFields\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in FillFlexFieldsFields\n" + exception + "\n");
            throw;
        }
    }

    public async Task FlexFieldsDoTheChallenge(WebDriver driver)
    {
        try
        {
            Console.WriteLine("\nStarting FlexFieldsDoTheChallenge");
            await Task.Delay(3*1000);
            _baseActions.SwitchToIframe(driver, _flexFieldsFormLocators.Iframe3Ds);
            _baseActions.ClickOnButton(driver, _flexFieldsFormLocators.SimulateSuccessButton);
            _baseActions.TearDown(driver);
            Console.WriteLine("Done with FlexFieldsDoTheChallenge\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in FlexFieldsDoTheChallenge\n" + exception + "\n");
            _baseActions.TearDown(driver);
            throw;
        }
    }
}