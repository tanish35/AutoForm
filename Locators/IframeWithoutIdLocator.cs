using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Locators;

namespace Locators;

public static class IframeWithoutIdLocator
{
    public static IWebElement? FindNameField(IWebDriver driver)
    {
        var iframes = driver.FindElements(By.TagName("iframe"))
                            .Where(f => string.IsNullOrEmpty(f.GetAttribute("id")))
                            .ToList();

        try
        {
            driver.SwitchTo().Frame(iframes[0]);
            var field = RegularDomLocator.FindNameField(driver);

            if (field is not null)
                return field;
            driver.SwitchTo().DefaultContent();
        }
        catch (Exception ex)
        {
            driver.SwitchTo().DefaultContent();
        }

        return null;
    }

}
