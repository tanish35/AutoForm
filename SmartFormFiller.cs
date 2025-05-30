using OpenQA.Selenium;
using Locators;

public static class SmartFormFiller
{
    public static void FillAllNameFields(IWebDriver driver)
    {
        driver.SwitchTo().DefaultContent();
        FillField(() => RegularDomLocator.FindNameField(driver));


        driver.SwitchTo().DefaultContent();

        driver.SwitchTo().DefaultContent();
        var outerIframe = driver.FindElements(By.TagName("iframe"))
            .FirstOrDefault(f => string.IsNullOrEmpty(f.GetAttribute("id")));

        if (outerIframe != null)
        {
            driver.SwitchTo().Frame(outerIframe);

            var outerFormField = RegularDomLocator.FindNameField(driver);
            if (outerFormField != null)
            {
                FillField(() => outerFormField);
            }
            var nestedIframes = driver.FindElements(By.TagName("iframe"));
            foreach (var innerIframe in nestedIframes)
            {
                driver.SwitchTo().Frame(innerIframe);
                var innerFormField = RegularDomLocator.FindNameField(driver);
                if (innerFormField != null)
                {
                    FillField(() => innerFormField);
                }
                driver.SwitchTo().ParentFrame();
            }
            driver.SwitchTo().DefaultContent();
        }


        FillField(() =>
        {
            driver.SwitchTo().DefaultContent();
            return IframeWithIdLocator.FindNameField(driver);
        });
    }

    private static void FillField(Func<IWebElement?> locator)
    {
        try
        {
            var field = locator();
            if (field != null)
            {
                field.Clear();
                field.SendKeys("Tanish Majumdar");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error filling field: {ex.Message}");
        }
    }
}
