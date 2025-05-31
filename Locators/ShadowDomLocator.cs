using OpenQA.Selenium;

namespace Locators;

public static class ShadowDomLocator
{
    public static void FillAllShadowFields(
        IWebDriver driver,
        string firstName = "Tanish",
        string lastName = "Majumdar",
        string gender = "Male",
        int maxDepth = 6)
    {
        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

        const string script = @"
            function fillShadowFields(root, depth, maxDepth, fname, lname, genderValue) {
                if (depth > maxDepth) return 0;
                let filledCount = 0;

                filledCount += fillFields(root, [
                    'input[name=""First Name""]',
                    'input[id*=""fname""]',
                    'input[placeholder*=""First""]'
                ], fname);

                filledCount += fillFields(root, [
                    'input[name=""Last Name""]',
                    'input[id*=""lname""]',
                    'input[placeholder*=""Last""]'
                ], lname);

                filledCount += handleGender(root, genderValue);

                root.querySelectorAll('*').forEach(el => {
                    if (el.shadowRoot) {
                        filledCount += fillShadowFields(
                            el.shadowRoot, 
                            depth + 1, 
                            maxDepth, 
                            fname, 
                            lname, 
                            genderValue
                        );
                    }
                });

                return filledCount;
            }

            function fillFields(root, selectors, value) {
                let count = 0;
                selectors.forEach(selector => {
                    root.querySelectorAll(selector).forEach(input => {
                        try {
                            input.value = value;
                            input.dispatchEvent(new Event('input', { bubbles: true }));
                            count++;
                        } catch(e) {
                            console.error('Error filling field:', e);
                        }
                    });
                });
                return count;
            }

            function handleGender(root, genderValue) {
                let count = 0;
                const normalizedGender = genderValue.toLowerCase();
                
                root.querySelectorAll(
                    'input[name=""gender""], select[name=""gender""]'
                ).forEach(input => {
                    try {
                        if (input.tagName === 'SELECT') {
                            const option = Array.from(input.options).find(
                                opt => opt.value.toLowerCase() === normalizedGender
                            );
                            if (option) {
                                option.selected = true;
                                input.dispatchEvent(new Event('change', { bubbles: true }));
                                count++;
                            }
                        } else if (input.type === 'radio' && 
                                 input.value.toLowerCase() === normalizedGender) {
                            input.checked = true;
                            input.dispatchEvent(new Event('change', { bubbles: true }));
                            count++;
                        }
                    } catch(e) {
                        console.error('Error handling gender:', e);
                    }
                });
                return count;
            }

            return fillShadowFields(document, 0, arguments[0], arguments[1], arguments[2], arguments[3]);
        ";

        try
        {
            var result = js.ExecuteScript(script, maxDepth, firstName, lastName, gender.ToLower());
            LogResults(result, firstName, lastName, gender);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️ Error executing shadow DOM script: {ex.Message}");
        }
    }

    private static void LogResults(object result, string fname, string lname, string gender)
    {
        if (result is long count)
        {
            var message = count switch
            {
                0 => "No shadow fields found",
                1 => "Filled 1 shadow field",
                _ => $"Filled {count} shadow fields"
            };

            Console.WriteLine($"{message} | " +
                $"Name: {fname} {lname} | " +
                $"Gender: {gender}");
        }
    }
}
