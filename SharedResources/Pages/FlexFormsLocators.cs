using OpenQA.Selenium;

namespace Splitit.Automation.NG.SharedResources.Pages;

public class FlexFormsLocators
{
    public readonly string StoreUrl = "https://storesdemos:3WildTables@wordpress-automation.sandbox.splitit.com/shop/";
    public readonly By AddToCartButton = By.XPath("//*[text()='Add to cart']");
    public readonly By ViewCartButton = By.XPath("//*[@title='View cart']");
    public readonly By ProceedToCheckoutButton = By.XPath("//*[@href='https://wordpress-automation.sandbox.splitit.com/checkout/']");
    public readonly By FirstNameTextBox = By.Id("billing_first_name");
    public readonly string FirstNameValue = "Automation";
    public readonly By LastNameTextBox = By.Id("billing_last_name");
    public readonly string LastNameValue = "Run";
    public readonly By StreetAddressTextBox = By.Id("billing_address_1");
    public readonly string StreetAddressValue = "Stam Street";
    public readonly By ZipCodeTextBox = By.Id("billing_postcode");
    public readonly string ZipCodeValue = "7170000";
    public readonly By CityTextBox = By.Id("billing_city");
    public readonly string CityValue = "NewYork";
    public readonly By PhoneTextBox = By.Id("billing_phone");
    public readonly string PhoneValue = "0501231234";
    public readonly By EmailAddressTextBox = By.Id("billing_email");
    public readonly string EmailAddressValue = "erez@erez.com";
    public readonly By MonthlyRadioButton = By.XPath("/html/body/div[2]/div[2]/div/form[2]/div[2]/div/div/ul/li[2]/label");
    public readonly By IframeCardNumber = By.XPath("/html/body/div[2]/div[2]/div/form[2]/div[2]/div/div/ul/li[2]/div/fieldset/div/div[2]/section/div[1]/section[1]/div[1]/iframe");
    public readonly By CardNumberTextBox = By.XPath("//*[@autocomplete='cc-number']");
    public readonly By IframeExp = By.XPath("/html/body/div[2]/div[2]/div/form[2]/div[2]/div/div/ul/li[2]/div/fieldset/div/div[2]/section/div[1]/section[2]/div[1]/iframe");
    public readonly By CardNumberExpTextBox = By.XPath("//*[@autocomplete='cc-exp']");
    public readonly By IframeCvv = By.XPath("/html/body/div[2]/div[2]/div/form[2]/div[2]/div/div/ul/li[2]/div/fieldset/div/div[2]/section/div[1]/section[3]/div[1]/iframe");
    public readonly By CardNumberCvvTextBox = By.XPath("//*[@autocomplete='cc-csc']");
    public readonly By TermAndConditionsCheckBox = By.XPath("//*[@aria-label='terms and conditions']");
    public readonly By PlaceOrderButton = By.Id("place_order");
    public readonly By OrderConfirmationMessage = By.XPath("//*[text()='Order confirmation']");
    public readonly By InstallmentPlanNumberHeader = By.XPath("//*[text()='Installment plan number:']");
    public readonly By InstallmentPlanNumberValue = By.XPath("/html/body/div[2]/main/div/div/p[2]");
    public readonly By NumberOfInstallmentsHeader = By.XPath("//*[text()='Number of installments:']");
    public readonly By NumberOfInstallmentsValue = By.XPath("/html/body/div[2]/main/div/div/p[3]/text()");
}