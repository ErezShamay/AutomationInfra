using OpenQA.Selenium;

namespace Splitit.Automation.NG.SharedResources.Pages;

public class CheckoutChallengeLocators
{
    public readonly By CardNumberTextBox = By.XPath("//*[@qa-id='card-number']");
    public readonly By ExpTextBox = By.XPath("//*[@qa-id='expiration-date']");
    public readonly By CvvTextBox = By.XPath("//*[@qa-id='cvv']");
    public readonly By AcceptTermAndConditionsButton = By.XPath("/html/body/app-root/app-main-page/div/div/app-content/div/app-buttons-section/div/div/div/label/div[1]/svg[1]/use");
    public readonly string ScriptToRun = "document.querySelector('#sticky-buttons > div > div > label').click()";
    public readonly By PayButton = By.XPath("//*[@qa-id='pay-button']");
    public readonly By Iframe = By.XPath("/html/body/app-root/app-main-page/div/div/app-modal/div[2]/div/div[2]/iframe");
    public readonly By PasswordTextBox = By.XPath("//input[@id = 'password']");
    public readonly string PasswordValue = "Checkout1!";
    public readonly By ContinueButton = By.Id("txtButton");
    public readonly string Success= "success";
}