using OpenQA.Selenium;

namespace Locators;

public static class ShadowDomLocator
{
    public static void FillAllNameFields(IWebDriver driver, string value = "Tanish Majumdar", int maxDepth = 6)
    {
        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

        string script = @"
            function fillShadowInputs(root, depth, maxDepth, value) {
                if (depth > maxDepth) return 0;
                let count = 0;
                let selectors = [
                    ""input[name='First Name']"",
                    ""input[id*='fname']"",
                    ""input[placeholder*='Name']""
                ];
                selectors.forEach(sel => {
                    let inputs = root.querySelectorAll(sel);
                    inputs.forEach(input => {
                        try { input.value = value; input.dispatchEvent(new Event('input', { bubbles: true })); count++; } catch(e){}
                    });
                });
                let elements = root.querySelectorAll('*');
                elements.forEach(el => {
                    if (el.shadowRoot) {
                        count += fillShadowInputs(el.shadowRoot, depth + 1, maxDepth, value);
                    }
                });
                return count;
            }
            return fillShadowInputs(document, 0, arguments[0], arguments[1]);
        ";

        try
        {
            var filledCount = js.ExecuteScript(script, maxDepth, value);
            Console.WriteLine($"Filled {filledCount} shadow DOM input(s) with '{value}'.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error filling shadow DOM fields: {ex.Message}");
        }
    }
}
