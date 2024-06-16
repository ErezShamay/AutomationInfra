using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Splitit.Automation.NG.SharedResources.DriverFactory;

public class DriverFactory
{
    private WebDriver _driver = null!;
    public WebDriver InitDriver()
    {
        try
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("--headless");
            chromeOptions.AddArguments("--verbose");
            chromeOptions.AddArguments("--disable-gpu");
            chromeOptions.AddArguments("--no-sandbox");
            chromeOptions.AddArguments("--no-first-run");
            chromeOptions.AddArguments("--disable-infobars");
            chromeOptions.AddArguments("--disable-extensions");
            chromeOptions.AddArguments("--disable-notifications");
            chromeOptions.AddArguments("--disable-dev-shm-usage");
            chromeOptions.AddArguments("--ignore-certificate-errors");
            chromeOptions.AddArguments("--disable-popup-blocking");
            chromeOptions.AddArguments("--disable-in-process-stack-traces");
            chromeOptions.AddArguments("--remote-allow-origins=*");
            chromeOptions.AddArguments("--window-size=1280,1024");
            chromeOptions.AddArguments("--disable-site-isolation-trials");
            chromeOptions.AddArguments("--auto-open-devtools-for-tabs");
            chromeOptions.AddArguments("--disable-blink-features=AutomationControlled");
            chromeOptions.AddExcludedArguments("enable-automation");
            chromeOptions.AddAdditionalChromeOption("useAutomationExtension", false);

            _driver = new ChromeDriver(chromeOptions);
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in creating driver instance exception: \n" + exception + "\n");
            throw;
        }
        return _driver;
    }
}