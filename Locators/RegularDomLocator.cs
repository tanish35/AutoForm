using OpenQA.Selenium;

namespace Locators;

public static class RegularDomLocator
{
    public static void FillFormFields(IWebDriver driver)
    {
        FillField(driver, new[]
        {
            "input[name='First Name']",
            "input[id*='fname']",
            "input[placeholder*='First']",
            "input[placeholder*='Name']",
            "input[type='text']"
        }, "Tanish");

        FillField(driver, new[]
        {
            "input[name='Last Name']",
            "input[id*='lname']",
            "input[placeholder*='Last']",
            "input[placeholder*='Surname']"
        }, "Majumdar");

        FillField(driver, new[]
        {
            "input[type='email']",
            "input[name*='email']",
            "input[id*='email']",
            "input[placeholder*='email']"
        }, "tanish@example.com");
    }

    private static void FillField(IWebDriver driver, string[] selectors, string value)
    {
        foreach (var selector in selectors)
        {
            try
            {
                var el = driver.FindElement(By.CssSelector(selector));
                if (el.Displayed && el.Enabled)
                {
                    el.Clear();
                    el.SendKeys(value);
                    break;
                }
            }
            catch { }
        }
    }
}
