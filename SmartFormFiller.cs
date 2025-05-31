using OpenQA.Selenium;
using Locators;

public static class SmartFormFiller
{
    public static void FillAllNameFields(IWebDriver driver)
    {
        driver.SwitchTo().DefaultContent();
        // FillField(() => RegularDomLocator.FindNameField(driver));
        RegularDomLocator.FillFormFields(driver);


        // driver.SwitchTo().DefaultContent();

        driver.SwitchTo().DefaultContent();
        var outerIframe = driver.FindElements(By.TagName("iframe"))
            .FirstOrDefault(f => string.IsNullOrEmpty(f.GetAttribute("id")));

        if (outerIframe != null)
        {
            driver.SwitchTo().Frame(outerIframe);

            RegularDomLocator.FillFormFields(driver);

            var nestedIframes = driver.FindElements(By.TagName("iframe"));
            foreach (var innerIframe in nestedIframes)
            {
                driver.SwitchTo().Frame(innerIframe);
                RegularDomLocator.FillFormFields(driver);

                driver.SwitchTo().ParentFrame();
            }
            driver.SwitchTo().DefaultContent();
        }


        // FillField(() =>
        // {
        //     driver.SwitchTo().DefaultContent();
        //     return IframeWithIdLocator.FindNameField(driver);
        // });
        IframeWithIdLocator.FindNameField(driver);
        ShadowDomLocator.FillAllShadowFields(driver, "Tanish", "Majumdar", "Male", 6);
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
