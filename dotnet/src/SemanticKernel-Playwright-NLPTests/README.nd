# 📚 Semantic Kernel + Playwright NLP Web Automation

## 📋 Table of Contents
1. [Introduction](#introduction)
2. [Prerequisites](#prerequisites)
3. [Installation](#installation)
4. [Project Structure](#project-structure)
5. [Getting Started](#getting-started)
6. [Core Concepts](#core-concepts)
7. [Implemented Playwright Commands](#implemented-playwright-commands)
8. [Usage Examples](#usage-examples)
9. [Adding New Commands](#adding-new-commands)
10. [Troubleshooting](#troubleshooting)
11. [Contributing](#contributing)
12. [License](#license)

## 1. Introduction <a name="introduction"></a>

Welcome to the Semantic Kernel + Playwright NLP Web Automation project! 🎉 This innovative project combines the power of Microsoft's Semantic Kernel (SK) with Playwright to enable web automation using natural language processing (NLP). With this tool, you can control web browsers using simple English commands, making web automation more accessible and intuitive than ever before.

![Alt text](../../../Assets/video/sk-playwright/001.gif)

### Key Features:
- 🧠 Natural language processing for web automation
- 🌐 Cross-browser support (Chrome, Firefox, WebKit)
- 🚀 Easy-to-use interface for complex web tasks
- 🔧 Extensible architecture for custom commands

## 2. Prerequisites <a name="prerequisites"></a>

Before you begin, ensure you have the following installed:
- 📦 .NET 6.0 or later
- 📦 Microsoft.SemanticKernel NuGet package
- 📦 Microsoft.Playwright NuGet package
- 🔑 OpenAI API key (for Semantic Kernel)

## 3. Installation <a name="installation"></a>

1. Clone the repository:
   ```
   git clone https://github.com/yourusername/sk-playwright-nlp.git
   ```

2. Navigate to the project directory:
   ```
   cd sk-playwright-nlp
   ```

3. Install the required NuGet packages:
   ```
   dotnet add package Microsoft.SemanticKernel
   dotnet add package Microsoft.Playwright
   ```

4. Install Playwright browsers:
   ```
   playwright install
   ```

5. Set up your OpenAI API key as an environment variable:
   ```
   setx OPENAI_API_KEY "your-api-key-here"
   ```

## 4. Project Structure <a name="project-structure"></a>

```
sk-playwright-nlp/
│
├── src/
│   ├── PlaywrightPlugin.cs
│   ├── Program.cs
│   └── SK-Playwright-NLP.csproj
│
├── tests/
│   └── PlaywrightPluginTests.cs
│
├── examples/
│   └── Example.ipynb
│
└── README.md
```

## 5. Getting Started <a name="getting-started"></a>

To run the project:

1. Open the solution in your preferred IDE (e.g., Visual Studio, VS Code).
2. Build the solution.
3. Run the `Program.cs` file or execute the Jupyter notebook in the `examples` folder.

Here's a basic example of how to use the system:

```csharp
var playwright = await Playwright.CreateAsync();
var browser = await playwright.Chromium.LaunchAsync(new() { Headless = false });
var page = await browser.NewPageAsync();

var kernel = Kernel.Builder
    .WithOpenAIChatCompletionService("gpt-4", "your-api-key-here")
    .Build();

kernel.ImportFunctions(new PlaywrightPlugin(page), "web");

await kernel.InvokePromptAsync("Navigate to 'https://www.example.com'");
await kernel.InvokePromptAsync("Click the 'About' link");
```

## 6. Core Concepts <a name="core-concepts"></a>

### Semantic Kernel
Semantic Kernel is an AI orchestration framework that combines natural language processing with traditional programming. It allows you to create AI-powered applications that can understand and execute natural language commands.

### Playwright
Playwright is a powerful browser automation library that allows you to control web browsers programmatically. It supports multiple browser engines and provides a high-level API for web interactions.

### PlaywrightPlugin
The `PlaywrightPlugin` class is the bridge between Semantic Kernel and Playwright. It exposes Playwright's functionality as semantic functions that can be invoked using natural language.

## 7. Implemented Playwright Commands <a name="implemented-playwright-commands"></a>

Here's a list of implemented Playwright commands along with example prompts:

1. 🌐 **Navigate to URL**
   - Command: `GoToAsync`
   - Example: "Navigate to 'https://www.example.com'"

2. 📝 **Fill form field**
   - Command: `FillAsync`
   - Example: "Fill '#username' with 'johndoe@example.com'"

3. 🧹 **Clear form field**
   - Command: `ClearAsync`
   - Example: "Clear the '#password' field"

4. 🖱️ **Click element**
   - Command: `ClickAsync`
   - Example: "Click the 'Submit' button"

5. 👆 **Double-click element**
   - Command: `DblClickAsync`
   - Example: "Double-click the image with id 'zoom-image'"

6. 🖱️ **Hover over element**
   - Command: `HoverAsync`
   - Example: "Hover over the 'Menu' dropdown"

7. 📊 **Select dropdown option**
   - Command: `SelectAsync`
   - Example: "Select 'Option 2' from the '#dropdown' menu"

8. 🔍 **Focus on element**
   - Command: `FocusAsync`
   - Example: "Focus on the search input field"

9. 👀 **Remove focus from element**
   - Command: `BlurAsync`
   - Example: "Remove focus from the '#email' input"

10. ✅ **Check checkbox or radio button**
    - Command: `CheckAsync`
    - Example: "Check the 'I agree' checkbox"

11. ❎ **Uncheck checkbox**
    - Command: `UncheckAsync`
    - Example: "Uncheck the 'Subscribe to newsletter' box"

12. ⌨️ **Press key on element**
    - Command: `PressAsync`
    - Example: "Press 'Enter' on the search input"

13. 📱 **Tap element (for touch devices)**
    - Command: `TapAsync`
    - Example: "Tap the 'Menu' button"

14. 📎 **Upload file**
    - Command: `SetInputFilesAsync`
    - Example: "Upload 'document.pdf' to the file input"

15. ⏳ **Wait for element**
    - Command: `WaitForAsync`
    - Example: "Wait for the '#loading' spinner to disappear"

16. 🔄 **Wait for page load state**
    - Command: `WaitForLoadStateAsync`
    - Example: "Wait for the page to finish loading"

17. 📸 **Take screenshot**
    - Command: `ScreenshotAsync`
    - Example: "Take a screenshot of the entire page"

18. 🔗 **Get current URL**
    - Command: `GetUrlAsync`
    - Example: "What is the current page URL?"

19. 📑 **Get page title**
    - Command: `GetTitleAsync`
    - Example: "What is the title of the current page?"

## 8. Usage Examples <a name="usage-examples"></a>

Here are some more complex examples of how to use the system:

```csharp
// Perform a Google search
await kernel.InvokePromptAsync("Navigate to 'https://www.google.com'");
await kernel.InvokePromptAsync("Fill the search input with 'Semantic Kernel'");
await kernel.InvokePromptAsync("Press 'Enter' on the search input");

// Fill out a form
await kernel.InvokePromptAsync("Navigate to 'https://www.example.com/form'");
await kernel.InvokePromptAsync("Fill '#name' with 'John Doe'");
await kernel.InvokePromptAsync("Fill '#email' with 'john@example.com'");
await kernel.InvokePromptAsync("Select 'Option 2' from the '#country' dropdown");
await kernel.InvokePromptAsync("Check the 'I agree to terms' checkbox");
await kernel.InvokePromptAsync("Click the 'Submit' button");

// Take a screenshot of a specific element
await kernel.InvokePromptAsync("Navigate to 'https://www.example.com'");
await kernel.InvokePromptAsync("Wait for the '.hero-image' to be visible");
await kernel.InvokePromptAsync("Take a screenshot of the '.hero-image' element");
```

## 9. Adding New Commands <a name="adding-new-commands"></a>

To add a new Playwright command:

1. Open `PlaywrightPlugin.cs`.
2. Add a new public async method.
3. Annotate it with `[Description]` and `[KernelFunction]` attributes.
4. Implement the command using the `page` object.
5. Handle async operations with `await`.

Example of adding a new command to get the inner text of an element:

```csharp
[Description("Get the inner text of an element")]
[KernelFunction(nameof(GetInnerTextAsync))]
public async Task<string> GetInnerTextAsync(string selector)
{
    var element = await page.QuerySelectorAsync(selector);
    if (element == null)
    {
        return "Element not found";
    }
    return await element.InnerTextAsync();
}
```

After adding the new command, you can use it like this:

```csharp
var innerText = await kernel.InvokePromptAsync("Get the inner text of the element with id 'header'");
```

## 10. Troubleshooting <a name="troubleshooting"></a>

- **Issue**: Playwright fails to launch browser
  **Solution**: Ensure you've run `playwright install` to download the necessary browser binaries.

- **Issue**: Semantic Kernel fails to process commands
  **Solution**: Check that your OpenAI API key is correctly set and that you have sufficient credits.

- **Issue**: Commands timeout
  **Solution**: Increase the timeout in the Playwright options when launching the browser.

---

I hope this guide helps you get started with the Semantic Kernel + Playwright NLP Web Automation project. Happy automating! 🚀🤖