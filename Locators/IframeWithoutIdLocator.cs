using OpenQA.Selenium;

namespace Locators;

public static class IframeWithoutIdLocator
{
    public static List<IWebElement>? FindNameField(IWebDriver driver)
    {
        var fields = new List<IWebElement>();

        try
        {
            driver.SwitchTo().DefaultContent();

            var outerIframe = driver.FindElements(By.TagName("iframe"))
                .FirstOrDefault(f => string.IsNullOrEmpty(f.GetAttribute("id")));

            if (outerIframe == null)
                return null;

            driver.SwitchTo().Frame(outerIframe);

            var outerFormField = RegularDomLocator.FindNameField(driver);
            if (outerFormField != null)
                fields.Add(outerFormField);

            var nestedIframes = driver.FindElements(By.TagName("iframe"));
            foreach (var innerIframe in nestedIframes)
            {
                try
                {
                    driver.SwitchTo().Frame(innerIframe);
                    var innerFormField = RegularDomLocator.FindNameField(driver);
                    if (innerFormField != null)
                        fields.Add(innerFormField);
                }
                finally
                {
                    driver.SwitchTo().ParentFrame();
                }
            }

            driver.SwitchTo().DefaultContent();
            return fields;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            driver.SwitchTo().DefaultContent();
            return null;
        }
    }
}
