using OpenQA.Selenium;
using Locators;

namespace Locators;

public static class IframeWithIdLocator
{
    public static void FindNameField(IWebDriver driver)
    {
        var iframes = driver.FindElements(By.CssSelector("iframe[id]"));
        Console.WriteLine($"Found {iframes.Count} iframes with an ID.");

        for (int i = 0; i < iframes.Count; i++)
        {
            try
            {
                var iframe = iframes[i];
                var iframeId = iframe.GetAttribute("id");
                Console.WriteLine($"Checking iframe #{i} with ID: {iframeId}");

                driver.SwitchTo().Frame(iframe);
                var forms = driver.FindElements(By.TagName("form"));
                Console.WriteLine($"Found {forms.Count} form(s) in iframe");


                var targetForm = driver.FindElements(By.Id("automationtestform")).FirstOrDefault()
                               ?? forms.FirstOrDefault();

                if (targetForm != null)
                {
                    Console.WriteLine($"Found form: {targetForm.GetAttribute("id") ?? "no-id"}");

                    RegularDomLocator.FillFormFields(driver);

                }

                driver.SwitchTo().DefaultContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in iframe #{i}: {ex.Message}");
                driver.SwitchTo().DefaultContent();
            }
        }

        return;
    }
}
