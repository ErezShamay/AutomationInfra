using OpenQA.Selenium;

namespace Splitit.Automation.NG.SharedResources.Pages;

public class PaymentFormV35
{
    public readonly By OrderTotal = By.XPath("//*[@qa-id='total-amount']");
    public readonly By CardNumber = By.XPath("//*[@qa-id='card-number']");
    public readonly By FullNameTextBox = By.XPath("//*[@qa-id='card-holder-name']");
    public readonly By Exp = By.XPath("/html/body/app-root/app-main-page/div/div/app-content/div/app-credit-card-step/section/div/div/div/app-credit-card-fields/div/div/mat-form-field[1]");
    public readonly By Cvv = By.XPath("/html/body/app-root/app-main-page/div/div/app-content/div/app-credit-card-step/section/div/div/div/app-credit-card-fields/div/div/mat-form-field[2]");
    public readonly By SelectNumberOfPayments = By.XPath("/html/body/app-root/app-main-page/div/div/app-content/div/app-credit-card-step/section/div/div/div/app-installments-step/section/div/div/div/range-select/mat-slider/div/div[3]/div[2]");
    public readonly By ChangeBillingAddressButton = By.XPath("//*[text()='Change']");
    public readonly By AcceptTermCheckBox = By.XPath("/html/body/app-root/app-main-page/div/div/app-content/div/app-buttons-section/div/div/div/label");
    public readonly By PayButton = By.XPath("//*[@qa-id='pay-button']");
    public readonly By SuccessMessage = By.XPath("//[@qa-id='success-message']");
}

public class Checkout3DsForAli
{
    public readonly By FirstIframe = By.XPath("/html/body/iframe");
    public readonly By PasswordTextBox = By.XPath("/html/body/div[2]/div/form/div/input");
    public readonly string PasswordValue = "Checkout1!";
    public readonly By ContinueButton = By.XPath("/html/body/div[2]/div/form/input[2]");
}

public class PaymentFormV4
{
    public readonly By OrderTotal = By.XPath("/html/body/div/div[1]/section/div/section[2]/section/div[2]/div[2]/div[2]/div[2]/div/p[3]");
    public readonly By AddressLine = By.XPath("//*[@qa-id='AddressLine']");
    public readonly By AddressLine2 = By.XPath("//*[@qa-id='AddressLine2']");
    public readonly By City = By.XPath("//*[@qa-id='City']");
    public readonly By Zip = By.XPath("//*[@qa-id='Zip']");
    public readonly By Email = By.XPath("//*[@qa-id='Email']");
    public readonly By ErrorBillingInfoMessage = By.XPath("/html/body/div[1]/div[1]/section/div/section/section/div[1]/div[2]/div[1]/div");
    public readonly By CardNumber = By.XPath("//*[@qa-id='CardNumber']");
    public readonly By FullNameTextBox = By.XPath("//*[@qa-id='CardHolderFullName']");
    public readonly By Exp = By.XPath("//*[@qa-id='ExpDate']");
    public readonly By Cvv = By.XPath("//*[@qa-id='CardCvv']");
    public readonly By ErrorPaymentsMessage = By.XPath("/html/body/div/div[1]/section/div/section/section/div[2]/div[1]/form/div[2]/div[1]/div/div");
    public readonly By ErrorExpMessage = By.XPath("/html/body/div/div[1]/section/div/section/section/div[2]/div[1]/form/div[2]/div[2]/div[1]/div");
    public readonly By CvvErrorMessage = By.XPath("/html/body/div/div[1]/section/div/section/section/div[2]/div[1]/form/div[2]/div[2]/div[2]/div");
    public readonly By AcceptTermCheckBox = By.Id("id-agree-terms");
    public readonly By TermAndConditionsLink = By.XPath("/html/body/div/div[1]/section/div/section/section/div[2]/div[2]/div[2]/div[3]/div/div/label/div[2]/button[1]");
    public readonly By TermAndConditionsIframe = By.XPath("/html/body/div/div[1]/div/div/div[2]/iframe");
    public readonly By PrivacyPolicyLink = By.XPath("/html/body/div/div[1]/section/div/section/section/div[2]/div[2]/div[2]/div[3]/div/div/label/div[2]/button[2]");
    public readonly By PrivacyPolicyIframe = By.XPath("/html/body/div/div[1]/div/div/div[2]/iframe");
    public readonly By PayButton = By.XPath("//*[@qa-id='pay-button']");
    public readonly By ErrorMessage = By.XPath("//*[contains(., 'Processing failure. Please use a different card')]");
    public readonly By MagicTextBox = By.Id("magic");
    public readonly string MagicValue = "";
    public readonly By Iframe = By.XPath("/html/body/div[1]/div[1]/div/div/div[2]/iframe");
    public readonly By MagicValueTextBox = By.XPath("//*[@qa-id='magic-value-input']");
    public readonly By SuccessButton = By.XPath("/html/body/div/main/section/div/div[4]/button[1]");
    public readonly By FailureButton = By.Id("error");
    public readonly By SuccessImg = By.XPath("//*[@alt='success']");
    public readonly By SuccessText = By.XPath("//*[@src=\"/imgs/success.png\"']");
}

public class PaymentFormV4V1
{
    public readonly By OrderTotal = By.XPath("/html/body/div/div[1]/section/div/section[2]/section/div[2]/div[2]/div[2]/div[2]/div/p[3]");
    public readonly By CardNumber = By.XPath("//*[@qa-id='CardNumber']");
    public readonly By FullNameTextBox = By.XPath("//*[@qa-id='CardHolderFullName']");
    public readonly By Exp = By.XPath("//*[@qa-id='ExpDate']");
    public readonly By Cvv = By.XPath("//*[@qa-id='CardCvv']");
    public readonly By AcceptTermCheckBox = By.Id("id-agree-terms");
    public readonly By PayButton = By.XPath("//*[@qa-id='pay-button']");
    public readonly By MagicTextBox = By.Id("magic");
    public readonly string MagicValue = "";
    public readonly By Iframe = By.XPath("/html/body/div/div[1]/div/div/div[2]/iframe");
    public readonly By MagicValueTextBox = By.XPath("//*[@qa-id='magic-value-input']");
    public readonly By SuccessButton = By.XPath("//*[@qa-id='success-button']");
    public readonly By FailureButton = By.XPath("//*[@qa-id='failure-button']");
    public readonly By SuccessImg = By.XPath("//*[@alt='success']");
}

public class Adyen3DsLocators
{
    public readonly By FirstIframe = By.XPath("//*[@title='3DS verification window']");
    public readonly By SecondIframe = By.XPath("//*[@name='threeDSIframe']");
    public readonly By PasswordTextBox = By.XPath("/html/body/div[1]/form/label/div[2]/input[1]");
    public const string PasswordValue = "password";
    public readonly By ContinueButton = By.Id("buttonSubmit");
}

public class CyberSource3DsLocators
{
    public readonly By FirstIframe = By.XPath("/html/body/div/div[1]/div/div/div[2]/iframe");
    public readonly By SecondIframe = By.XPath("/html/body/div[1]/div/iframe");
    public readonly By ThirdIframe = By.XPath("/html/body/div[1]/div/iframe");
    public readonly By PasswordTextBox = By.XPath("/html/body/div/section[1]/div[2]/form[1]/input[1]");
    public const string PasswordValue = "1234";
    public readonly By ContinueButton = By.XPath("/html/body/div/section[1]/div[2]/form[1]/input[2]");
}

public class BlueSnapDirect3DsLocators
{
    public readonly By FirstIframe = By.XPath("/html/body/div/div[1]/div/div/div[2]/iframe");
    public readonly By SecondIframe = By.XPath("/html/body/div[2]/div[1]/div[2]/iframe");
    public readonly By PasswordTextBox = By.XPath("/html/body/div/section[1]/div[2]/form[1]/input[1]");
    public readonly string PasswordValue = "1234";
    public readonly By ContinueButton = By.XPath("/html/body/div/section[1]/div[2]/form[1]/input[2]");
}

public class BlueSnapMor3DsLocators
{
    public readonly By FirstIframe = By.XPath("/html/body/div/div[1]/div/div/div[2]/iframe");
    public readonly By SecondIframe = By.XPath("/html/body/div[2]/div[1]/div[2]/iframe");
    public readonly By PasswordTextBox = By.XPath("/html/body/div/section[1]/div[2]/form[1]/input[1]");
    public readonly string PasswordValue = "1234";
    public readonly By ContinueButton = By.XPath("/html/body/div/section[1]/div[2]/form[1]/input[2]");
}

public class Checkout3DsLocators
{
    public readonly By FirstIframe = By.XPath("/html/body/div/div[1]/div/div/div[2]/iframe");
    public readonly By SecondIframe = By.XPath("/html/body/iframe");
    public readonly By PasswordTextBox = By.XPath("/html/body/div[2]/div/form/div/input");
    public readonly string PasswordValue = "Checkout1!";
    public readonly By ContinueButton = By.XPath("/html/body/div[2]/div/form/input[2]");
}

public class PayLikeV23DsLocators
{
    public readonly By FirstIframe = By.XPath("/html/body/div/div[1]/div/div/div[2]/div/iframe");
    public readonly By SecondIframe = By.XPath("/html/body/div/div[1]/main/iframe[1]");
    public readonly By CompleteButton = By.XPath("/html/body/form/input[5]");
    public readonly string JsCommand = "document.querySelector('body > form > input[type=submit]:nth-child(6)').click()";
}

public class Reach3DsLocators
{
    public readonly By FirstIframe = By.XPath("/html/body/div/div[1]/div/div/div[2]/iframe");
    public readonly By SecondIframe = By.XPath("/html/body/div/div/iframe");
    public readonly By PasswordTextBox = By.XPath("/html/body/div[1]/form/label/div[2]/input[1]");
    public readonly string PasswordValue = "password";
    public readonly By OkButton = By.Id("buttonSubmit");
}

public class WorldPay3DsLocators
{
    public readonly By FirstIframe = By.XPath("/html/body/div/div[1]/div/div/div[2]/iframe");
    public readonly By SecondIframe = By.XPath("/html/body/app-root/div/div/iframe");
    public readonly By OkButton = By.XPath("/html/body/form/input[4]");
}

public class StripeDirect3DsLocators
{
    public readonly By FirstIframe = By.XPath("/html/body/div/div[1]/div/div/div[2]/iframe");
    public readonly By SecondIframe = By.XPath("/html/body/div/iframe");
    public readonly By ThirdIframe = By.XPath("/html/body/div/div/div/div/div/div/div/div/iframe");
    public readonly By CompleteButton = By.Id("test-source-authorize-3ds");
}

public class SagePay3DsLocators
{
    public readonly By FirstIframe = By.XPath("/html/body/div/div[1]/div/div/div[2]/iframe");
    public readonly By PasswordTextBox = By.Id("cd");
    public readonly string PasswordValue = "challenge";
    public readonly By NextButton = By.XPath("/html/body/div/div[3]/form/input[4]");
}

public class VisLocators
{
    public readonly By FullNameTextBox = By.XPath("/html/body/div[1]/div[1]/form/main/section/section[3]/div[1]/label");
    public readonly By CardNumberTextBox = By.XPath("/html/body/div/div[1]/form/main/section/section[3]/div[2]/div[1]/label");
    public readonly By ExpDateTextBox = By.XPath("/html/body/div/div[1]/form/main/section/section[3]/div[2]/div[2]/label");
    public readonly By CvvTextBox = By.XPath("/html/body/div/div[1]/form/main/section/section[3]/div[2]/div[3]/label");
    public readonly By ContinueButton = By.XPath("/html/body/div/div[1]/form/main/section/section[4]/button");
    public readonly By AgreeCheckBox = By.XPath("/html/body/div[1]/div[1]/form/main/section/section[3]/div[2]/div/button[4]/div[3]/div[2]/label");
    public readonly By PayNowButton = By.XPath("/html/body/div[1]/div[1]/form/main/section/section[4]/button");
}