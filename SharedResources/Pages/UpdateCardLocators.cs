using OpenQA.Selenium;

namespace Splitit.Automation.NG.SharedResources.Pages;

public class UpdateCardLocators
{
    public readonly By CardNumber = By.XPath("/html/body/div/div[1]/form/main/section/section[2]/div[2]/div[1]/label");
    public readonly By CardHolderName = By.XPath("/html/body/div/div[1]/form/main/section/section[2]/div[1]/label");
    public readonly By CardExpDate = By.XPath("/html/body/div/div[1]/form/main/section/section[2]/div[2]/div[2]/label");
    public readonly By CardCvv = By.XPath("/html/body/div/div[1]/form/main/section/section[2]/div[2]/div[3]/label");
    public readonly By UpdateCardButton = By.XPath("//*[@qa-id='update-card-btn']");
    public readonly By ApprovalMessage = By.XPath("//*[text(), 'YOUR CARD IS UPDATED']");
}

public class Adyen3DsLocatorsUpdateCard
{
    public readonly By FirstIframe = By.XPath("/html/body/div/div[1]/div/div/div[2]/iframe");
    public readonly By SecondIframe = By.XPath("/html/body/div/div/div/div/iframe");
    public readonly By PasswordTextBox = By.XPath("/html/body/div[1]/form/label/div[2]/input[1]");
    public readonly string PasswordValue = "password";
    public readonly By ContinueButton = By.Id("buttonSubmit");
}

public class CyberSource3DsLocatorsUpdateCard
{
    public readonly By FirstIframe = By.XPath("/html/body/div/div[1]/div/div/div[2]/iframe");
    public readonly By SecondIframe = By.XPath("/html/body/div[1]/div/iframe");
    public readonly By ThirdIframe = By.XPath("/html/body/div[1]/div/iframe");
    public readonly By PasswordTextBox = By.XPath("/html/body/div/section[1]/div[2]/form[1]/input[1]");
    public string PasswordValue = "1234";
    public readonly By ContinueButton = By.XPath("/html/body/div/section[1]/div[2]/form[1]/input[2]");
}

public class BlueSnapDirect3DsLocatorsUpdateCard
{
    public readonly By FirstIframe = By.XPath("/html/body/div/div[1]/div/div/div[2]/iframe");
    public readonly By SecondIframe = By.XPath("/html/body/div[2]/div[1]/div[2]/iframe");
    public readonly By PasswordTextBox = By.XPath("/html/body/div/section[1]/div[2]/form[1]/input[1]");
    public readonly string PasswordValue = "1234";
    public readonly By ContinueButton = By.XPath("/html/body/div/section[1]/div[2]/form[1]/input[2]");
}

public class BlueSnapMor3DsLocatorsUpdateCard
{
    public readonly By FirstIframe = By.XPath("/html/body/app-root/app-main-page/div/div/app-modal/div[2]/div/div[2]/iframe");
    public readonly By SecondIframe = By.XPath("/html/body/div/div[1]/div[2]/iframe");
    public readonly By PasswordTextBox = By.XPath("/html/body/div/section[1]/div[2]/form[1]/input[1]");
    public readonly string PasswordValue = "1234";
    public readonly By ContinueButton = By.XPath("/html/body/div/section[1]/div[2]/form[1]/input[2]");
}

public class Checkout3DsLocatorsUpdateCard
{
    public readonly By FirstIframe = By.XPath("/html/body/div/div[1]/div/div/div[2]/iframe");
    public readonly By SecondIframe = By.XPath("/html/body/iframe");
    public readonly By PasswordTextBox = By.XPath("/html/body/div[2]/div/form/div/input");
    public readonly string PasswordValue = "Checkout1!";
    public readonly By ContinueButton = By.XPath("/html/body/div[2]/div/form/input[2]");
}

public class PaySafe3DsLocatorsUpdateCard
{
    public readonly By FirstIframe = By.XPath("/html/body/div/div[1]/div/div/div[2]/iframe");
    public readonly By SecondIframe = By.XPath("/html/body/div[2]/div[1]/div[2]/iframe");
    public readonly By CodeTextBox = By.XPath("/html/body/div/section[1]/div[2]/form[1]/input[1]");
    public readonly string Code = "1234";
    public readonly By ContinueButton = By.XPath("/html/body/div/section[1]/div[2]/form[1]/input[2]");
}

public class Reach3DsLocatorsUpdateCard
{
    public readonly By FirstIframe = By.XPath("/html/body/div/div[1]/div/div/div[2]/iframe");
    public readonly By SecondIframe = By.XPath("/html/body/div/div/iframe");
    public readonly By PasswordTextBox = By.XPath("/html/body/div[1]/form/label/div[2]/input[1]");
    public readonly string PasswordValue = "password";
    public readonly By OkButton = By.Id("buttonSubmit");
}

public class WorldPay3DsLocatorsUpdateCard
{
    public readonly By FirstIframe = By.XPath("/html/body/app-root/app-main-page/div/div/app-modal/div[2]/div/div[2]/iframe");
    public readonly By SecondIframe = By.XPath("/html/body/app-root/div/div/iframe");
    public readonly By OkButton = By.XPath("/html/body/form/input[4]");
}

public class SagePay3DsLocatorsUpdateCard
{
    public readonly By FirstIframe = By.XPath("/html/body/app-root/app-main-page/div/div/app-modal/div[2]/div/div[2]/iframe");
    public readonly By PasswordTextBox = By.Id("cd");
    public readonly string PasswordValue = "challenge";
    public readonly By NextButton = By.XPath("/html/body/div/div[3]/form/input[4]");
}