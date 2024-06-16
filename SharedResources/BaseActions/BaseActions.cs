using Newtonsoft.Json;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using Splitit.Automation.NG.Backend.BaseActions;
using Splitit.Automation.NG.Backend.Services.V3.DefaultValues;
using Splitit.Automation.NG.SharedResources.Tests.TestsHelper;

namespace Splitit.Automation.NG.SharedResources.BaseActions;

public class BaseActions
{
    private readonly HttpSender _httpSender = new();
    private readonly SpreedlyCreateTokenBaseObjects.Root _spreedlyCreateTokenBaseObjects =
        new SpreedlyCreateTokenBaseObjects.Root();
    
    public void NavigateToUrl(WebDriver driver, string url)
    {
        try
        {
            driver.Navigate().GoToUrl(url);
            Console.WriteLine("NavigateToUrl Succeeded -> " + url + "\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("NavigateToUrl Failed -> " + url + " exception \n" + exception + "\n");
            throw;
        }
    }

    public void ValidateNavigationByUrl(WebDriver driver, string url)
    {
        try
        {
            var actual = driver.Url;
            Assert.That(actual, Does.Contain(url));
            Console.WriteLine("ValidateNavigationByUrl Succeeded\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("ValidateNavigationByUrl Failed\n" + exception + "\n");
            throw;
        }
    }

    public void ValidateNavigationByElement(WebDriver driver, By element)
    {
        try
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            wait.Until(ExpectedConditions.ElementExists(element));
        }
        catch (Exception exception)
        {
            Console.WriteLine("Failed ValidateNavigationByElement\n" + exception + "\n");
        }
    }

    public IReadOnlyList<IWebElement> FindListElementsByAttribute(WebDriver driver, By element)
    {
        IReadOnlyList<IWebElement> attributesElementsList = new List<IWebElement>();
        try
        {
            Console.WriteLine("\nStarting FindListElementsByAttribute");
            attributesElementsList = driver.FindElements(element);
            Console.WriteLine("Done with FindListElementsByAttribute\n");
        }
        catch (Exception exception)
        {
          Console.WriteLine("Failed ValidateNavigationByElement\n" + exception + "\n");
        }
        return attributesElementsList;
    }

    public void SendKeysToTextBox(WebDriver driver, By element, string keysToSend)
    {
        try
        {
            ValidateNavigationByElement(driver, element);
            driver.FindElement(element).SendKeys(keysToSend);
            Console.WriteLine("SendKeysToTextBox Succeeded\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("SendKeysToTextBox Failed\n" + exception + "\n");
            throw;
        }
    }

    public void ClickOnButton(WebDriver driver, By element)
    {
        try
        {
            WaitForElementToBeClicked(driver, element);
            driver.FindElement(element).Click();
            Console.WriteLine("ClickOnButton -> "+element+" -> Succeeded\n");
        }
        catch (Exception)
        {
            try
            {
                Console.WriteLine("ClickOnButton Failed doing Retry for element ->" + element);
                ClickOnButtonPerform(driver, element);
            }
            catch (Exception e)
            {
                Console.WriteLine("Retry ClickOnButton Failed\n" + e);
                throw;
            }
            throw;
        }
    }

    private void ClickOnButtonPerform(WebDriver driver, By element)
    {
        try
        {
            WaitForElementToBeClicked(driver, element);
            var action = new Actions(driver);
            action.Click(driver.FindElement(element));
            Console.WriteLine("ClickOnButtonPerform  -> "+element+" -> Succeeded \n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("ClickOnButtonPerform for element -> "+element+" ->  Failed\n" + exception + "\n");
            throw;
        }
    }

    public void WaitForElementToBeClicked(WebDriver driver, By element)
    {
        try
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            wait.Until(ExpectedConditions.ElementToBeClickable(element));
            Console.WriteLine("Succeeded WaitForElementToBeClicked\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("Failed WaitForElementToBeClicked for element -> " + element + "\nWith exception -> " + exception);
        }
    }

    public void RefreshPage(WebDriver driver)
    {
        try
        {
            driver.Navigate().Refresh();
            Console.WriteLine("Succeeded RefreshPage\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("Failed RefreshPage\n" + exception + "\n");
            throw;
        }
    }

    public void TearDown(WebDriver driver)
    {
        try
        {
            driver.Quit();
            Console.WriteLine("Succeeded TearDown\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("Failed TearDown\n" + exception + "\n");
            throw;
        }
    }

    public void DoScrollDown(WebDriver driver)
    {
        try
        {
            var js = (IJavaScriptExecutor) driver;
            js.ExecuteScript("window.scrollBy(0,250)", "");
            Console.WriteLine("Succeeded DoScrollDown\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("Failed DoScrollDown\n" + exception + "\n");
            throw;
        }
    }

    public void DoScrollUp(WebDriver driver)
    {
        try
        {
            var js = (IJavaScriptExecutor) driver;
            js.ExecuteScript("window.scrollBy(0,-250)", "");
            Console.WriteLine("Succeeded DoScrollUp\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("Failed DoScrollUp\n" + exception + "\n");
            throw;
        }
    }

    public void DoTab(WebDriver driver, By element)
    {
        try
        {
            WaitForElementToBeClicked(driver, element);
            var elem = driver.FindElement(element);
            elem.SendKeys(Keys.Tab);
            Console.WriteLine("Succeeded DoTab\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("Failed DoTab\n" + exception + "\n");
            throw;
        }
    }

    public void ClickEnter(WebDriver driver, By element)
    {
        try
        {
            WaitForElementToBeClicked(driver, element);
            var textBox = driver.FindElement(element);
            textBox.SendKeys(Keys.Enter);
            Console.WriteLine("ClickEnter Succeeded\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("ClickEnter Failed\n" + exception + "\n");
            throw;
        }
    }

    public void NavigateBack(WebDriver driver)
    {
        try
        {
            driver.Navigate().Back();
            Console.WriteLine("Succeeded NavigateBack\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("Failed NavigateBack\n" + exception + "\n");
            throw;
        }
    }

    public void ValidateTextFromField(WebDriver driver, string textToLookFor)
    {
        try
        {
            Console.WriteLine("Succeeded ValidateTextFromField\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("Failed ValidateTextFromField\n" + exception + "\n");
            throw;
        }
    }

    public void ValidateTextFromElement(WebDriver driver)
    {
        try
        {
            Console.WriteLine("Succeeded ValidateTextFromElement\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("Failed ValidateTextFromElement\n" + exception + "\n");
            throw;
        }
    }

    public bool SwitchToIframe(WebDriver driver, By element)
    {
        var counter = 0;
        try
        {
            Console.WriteLine("Starting SwitchToIframe\n");
            while (counter < 3)
            {
                try
                {
                    counter++;
                    driver.SwitchTo().Frame(driver.FindElement(element));
                    Console.WriteLine("Succeeded SwitchToIframe\n");
                    return true;
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Doing retry number -> " + counter);
                    if (counter == 3)
                    {
                        Assert.Fail("DID NOT managed to do SwitchToIframe \n" + exception + "\n");
                    }
                }
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine("Failed to do SwitchToIframe\n" + exception + "\n");
            throw;
        }
        return false;
    }

    public void SwitchToDefaultFrame(WebDriver driver)
    {
        try
        {
            driver.SwitchTo().DefaultContent();
            Console.WriteLine("Succeeded SwitchToDefaultFrame\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("Failed SwitchToDefaultFrame\n" + exception + "\n");
            throw;
        }
    }

    public void SwitchToParentFrame(WebDriver driver)
    {
        try
        {
            driver.SwitchTo().ParentFrame();
            Console.WriteLine("Succeeded SwitchToParentFrame\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("Failed SwitchToParentFrame\n" + exception + "\n");
            throw;
        }
    }

    public string GetTextFromElement(WebDriver driver, By element)
    {
        var textFromElement = "";
        try
        {
            WaitForElementToBeClicked(driver, element);
            textFromElement = driver.FindElement(element).Text;
            Console.WriteLine("Succeeded getTextFromElement\n");
            return textFromElement;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Failed getTextFromElement\n" + exception + "\n");
            return textFromElement;
        }
    }

    public void ClearTextBox(WebDriver driver, By element)
    {
        try
        {
            WaitForElementToBeClicked(driver, element);
            driver.FindElement(element).Clear();
            Console.WriteLine("Succeeded ClearTextBox\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("Failed ClearTextBox\n" + exception + "\n");
            throw;
        }
    }

    public void ValidateReadyState(WebDriver driver)
    {
        try
        {
            Console.WriteLine("Succeeded ValidateReadyState\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("Failed ValidateReadyState\n" + exception + "\n");
            throw;
        }
    }

    public void SwitchToOtherWindow(WebDriver driver)
    {
        try
        {
            Console.WriteLine("Succeeded SwitchToOtherWindow\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("Failed SwitchToOtherWindow\n" + exception + "\n");
            throw;
        }
    }

    public void SwitchToBeforeWindow(WebDriver driver)
    {
        try
        {
            Console.WriteLine("Succeeded SwitchToBeforeWindow\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("Failed SwitchToBeforeWindow\n" + exception + "\n");
            throw;
        }
    }

    public void ValidateText(WebDriver driver, By textToLookFor)
    {
        try
        {
            WaitForElementToBeVisible(driver, textToLookFor);
            driver.FindElement(textToLookFor);
            Console.WriteLine("Succeeded ValidateText\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("Failed ValidateText\n" + exception + "\n");
            throw;
        }
    }

    public void WaitForElementToBeVisible(WebDriver driver, By elementToWaitFor)
    {
        try
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            wait.Until(ExpectedConditions.ElementIsVisible(elementToWaitFor));
            Console.WriteLine("Succeeded WaitForElementToBeVisible\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("Failed WaitForElementToBeVisible\n" + exception + "\n");
        }
    }
    
    public void WaitForElementToBeExists(WebDriver driver, By elementToWaitFor)
    {
        try
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            wait.Until(ExpectedConditions.ElementExists(elementToWaitFor));
            Console.WriteLine("Succeeded WaitForElementToBeExists\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("Failed WaitForElementToBeExists\n" + exception + "\n");
        }
    }

    public string GetCurrentUrl(WebDriver driver)
    {
        var strUrl = "";
        try
        {
            Console.WriteLine("\nStarting GetCurrentUrl");
            strUrl = driver.Url;
            Console.WriteLine("Done GetCurrentUrl\n");
            return strUrl;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in GetCurrentUrl ֿ\n" + exception + "\n");
            return strUrl;
        }
    }

    public void RunJavaScript(WebDriver driver, string scriptToRun)
    {
        try
        {
            Console.WriteLine("\nStarting RunJavaScript");
            var js = (IJavaScriptExecutor)driver;
            js.ExecuteScript(scriptToRun);
            Console.WriteLine("Done with RunJavaScript\n");
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in RunJavaScript\n" + exception + "\n");
        }
    }

    public async Task<string> GenerateSpreedlyToken(string endPoint, RequestHeader requestHeader, string auth)
    {
        try
        {
            Console.WriteLine("Starting GenerateSpreedlyToken");
            var populatedSpreedlyTokenObject = PopulateGetSpreedlyTokenObject();
            var response = await _httpSender.SendPostHttpsRequestAsync(
                endPoint, populatedSpreedlyTokenObject, requestHeader, null,
                null, null, null!, null!,
                null!, null!, null!, 
                null!, auth, "yes");
            var jResponse = JsonConvert.DeserializeObject<SpreedlyCreateTokenResponse.Root>(response);
            Console.WriteLine("Done with GenerateSpreedlyToken");
            return jResponse!.transaction.payment_method.token;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in GenerateSpreedlyToken" + e);
            throw;
        }
    }

    private SpreedlyCreateTokenBaseObjects.Root PopulateGetSpreedlyTokenObject()
    {
        try
        {
            Console.WriteLine("Staring PopulateGetSpreedlyTokenObject");
            _spreedlyCreateTokenBaseObjects.payment_method = new SpreedlyCreateTokenBaseObjects.PaymentMethod
                {
                    credit_card = new SpreedlyCreateTokenBaseObjects.CreditCard(),
                    metadata = new SpreedlyCreateTokenBaseObjects.Metadata()
                };
            _spreedlyCreateTokenBaseObjects.payment_method.credit_card.first_name = "RnD";
            _spreedlyCreateTokenBaseObjects.payment_method.credit_card.last_name = "Test CARD";
            //_spreedlyCreateTokenBaseObjects.payment_method.credit_card.number = "";
            _spreedlyCreateTokenBaseObjects.payment_method.credit_card.verification_value = "421";
            _spreedlyCreateTokenBaseObjects.payment_method.credit_card.month = "02";
            _spreedlyCreateTokenBaseObjects.payment_method.credit_card.year = "2024";
            _spreedlyCreateTokenBaseObjects.payment_method.credit_card.company = "SplitIt";
            _spreedlyCreateTokenBaseObjects.payment_method.credit_card.address1 = "33 Lane Road";
            _spreedlyCreateTokenBaseObjects.payment_method.credit_card.address2 = "Apartment 4";
            _spreedlyCreateTokenBaseObjects.payment_method.credit_card.city = "Wanaque";
            _spreedlyCreateTokenBaseObjects.payment_method.credit_card.state = "NJ";
            _spreedlyCreateTokenBaseObjects.payment_method.credit_card.zip = "31331";
            _spreedlyCreateTokenBaseObjects.payment_method.credit_card.country = "US";
            _spreedlyCreateTokenBaseObjects.payment_method.credit_card.phone_number = "919.331.3313";
            _spreedlyCreateTokenBaseObjects.payment_method.credit_card.shipping_address1 = "33 Lane Road";
            _spreedlyCreateTokenBaseObjects.payment_method.credit_card.shipping_address2 = "Apartment 4";
            _spreedlyCreateTokenBaseObjects.payment_method.credit_card.shipping_city = "Wanaque";
            _spreedlyCreateTokenBaseObjects.payment_method.credit_card.shipping_state = "NJ";
            _spreedlyCreateTokenBaseObjects.payment_method.credit_card.shipping_zip = "31331";
            _spreedlyCreateTokenBaseObjects.payment_method.credit_card.shipping_country = "US";
            _spreedlyCreateTokenBaseObjects.payment_method.credit_card.shipping_phone_number = "919.331.3313";
            _spreedlyCreateTokenBaseObjects.payment_method.email = "joey@example.com";
            _spreedlyCreateTokenBaseObjects.payment_method.metadata.key = "string value";
            _spreedlyCreateTokenBaseObjects.payment_method.metadata.another_key = 123;
            _spreedlyCreateTokenBaseObjects.payment_method.metadata.final_key = true;
            _spreedlyCreateTokenBaseObjects.payment_method.retained = true;
            Console.WriteLine("Done with PopulateGetSpreedlyTokenObject");
            return _spreedlyCreateTokenBaseObjects;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in " + e);
            throw;
        }
    }
}