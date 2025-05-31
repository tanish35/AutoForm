using OpenQA.Selenium;
using Locators;

public static class SmartFormFiller
{
    public static void FillAllNameFields(IWebDriver driver)
    {
        driver.SwitchTo().DefaultContent();

        RegularDomLocator.FillFormFields(driver);


        driver.SwitchTo().DefaultContent();

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
                var nestedIframeWithoutId = driver.FindElements(By.TagName("iframe"))
                    .FirstOrDefault(f => string.IsNullOrEmpty(f.GetAttribute("id")));
                if (nestedIframeWithoutId != null)
                {
                    driver.SwitchTo().Frame(nestedIframeWithoutId);
                    RegularDomLocator.FillFormFields(driver);
                    driver.SwitchTo().ParentFrame();
                }

                driver.SwitchTo().ParentFrame();
            }
            driver.SwitchTo().DefaultContent();
        }

        IframeWithIdLocator.FindNameField(driver);
        ShadowDomLocator.FillAllShadowFields(driver, "Charles", "Leclerc", "Male", 6);
        ShadowIframeLocator.SwitchAndFillInsideShadowIframe(driver, 6);
    }
}
