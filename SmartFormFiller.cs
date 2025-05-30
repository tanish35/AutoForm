using OpenQA.Selenium;
using Locators;



public static class SmartFormFiller
{
    public static IWebElement? FindNameField(IWebDriver driver)
    {
        return
        // RegularDomLocator.FindNameField(driver);
            IframeWithoutIdLocator.FindNameField(driver);
    }
}
