using OpenQA.Selenium;

namespace Locators;

public static class RegularDomLocator
{
    public static IWebElement? FindNameField(IWebDriver driver)
    {
        string[] selectors = {
            "input[name='First Name']",
            "input[id*='fname']",
            "input[placeholder*='Name']",
            "input[type='text']"
        };

        foreach (var selector in selectors)
        {
            try
            {
                var el = driver.FindElement(By.CssSelector(selector));
                if (el.Displayed && el.Enabled) return el;
            }
            catch { }
        }

        return null;
    }
}
