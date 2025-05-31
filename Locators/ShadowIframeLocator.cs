using OpenQA.Selenium;

namespace Locators;

public static class ShadowIframeLocator
{

    public static void SwitchAndFillInsideShadowIframe(IWebDriver driver, int maxDepth = 6)
    {
        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

        string script = @"
            function findIframeInShadow(root, depth, maxDepth) {
                if (depth > maxDepth) return null;
                const elements = root.querySelectorAll('*');
                for (let el of elements) {
                    if (el.shadowRoot) {
                        let iframe = el.shadowRoot.querySelector('iframe');
                        if (iframe) return iframe;
                        let nested = findIframeInShadow(el.shadowRoot, depth + 1, maxDepth);
                        if (nested) return nested;
                    }
                }
                return null;
            }
            return findIframeInShadow(document, 0, arguments[0]);
        ";

        try
        {
            var iframe = js.ExecuteScript(script, maxDepth) as IWebElement;

            if (iframe != null)
            {
                driver.SwitchTo().DefaultContent();
                driver.SwitchTo().Frame(iframe);
                Console.WriteLine("Inside Iframe inside shadowDom.");

                RegularDomLocator.FillFormFields(driver);

                var iframeWithoutId = driver.FindElements(By.TagName("iframe"))
                    .FirstOrDefault(f => string.IsNullOrEmpty(f.GetAttribute("id")));
                if (iframeWithoutId != null)
                {
                    driver.SwitchTo().Frame(iframeWithoutId);
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

                var iframeWithId = driver.FindElements(By.TagName("iframe"))
                    .FirstOrDefault(f => !string.IsNullOrEmpty(f.GetAttribute("id")));
                if (iframeWithId != null)
                {
                    driver.SwitchTo().Frame(iframeWithId);
                    RegularDomLocator.FillFormFields(driver);
                    driver.SwitchTo().ParentFrame();
                }

                ShadowDomLocator.FillAllShadowFields(driver, "Charles", "Leclerc", "Male", maxDepth);

                driver.SwitchTo().DefaultContent();
            }
            else
            {
                Console.WriteLine(" No iframe found inside shadow DOM.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error handling shadow DOM iframe: {ex.Message}");
            driver.SwitchTo().DefaultContent();
        }
    }
}
