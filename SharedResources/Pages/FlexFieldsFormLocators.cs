using OpenQA.Selenium;

namespace Splitit.Automation.NG.SharedResources.Pages;

public class FlexFieldsFormLocators
{
    public readonly By CardNumberTextBox = By.XPath("/html/body/input[2]");
    public readonly string CardNumberValue = "4111111111111111";
    public readonly By ExpTextBox = By.XPath("/html/body/input[4]");
    public readonly string ExpValue = "03/30";
    public readonly By CvvTextBox = By.XPath("/html/body/input[3]");
    public readonly string CvvValue = "737";
    public readonly string ScriptToRun = "document.querySelector('#terms-conditions > label').click()";
    public readonly By TermsIframe = By.XPath("/html/body/div[3]/div/iframe");
    public readonly By XButtonInTerms = By.XPath("/html/body/div[2]/main/div[2]/div[4]/label");
    public readonly By PayButton = By.Id("btn-pay");
    public readonly By Iframe3Ds = By.XPath("/html/body/div[3]/div/div[2]/div/iframe");
    public readonly By SimulateSuccessButton = By.XPath("/html/body/div/main/section/div/div[4]/button[1]");
    public readonly By SimulateErrorButton = By.Id("error");
    public readonly By IframeCardNumber = By.XPath("/html/body/div[2]/main/div[2]/div[1]/div[1]/div[1]/iframe");
    public readonly By IframeExp = By.XPath("/html/body/div[2]/main/div[2]/div[1]/div[2]/div[1]/iframe");
    public readonly By IframeCvv = By.XPath("/html/body/div[2]/main/div[2]/div[1]/div[3]/div[1]/iframe");
}