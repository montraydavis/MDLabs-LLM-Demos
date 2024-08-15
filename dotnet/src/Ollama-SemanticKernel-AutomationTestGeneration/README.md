# 🚀 Ollama-SemanticKernel-AutomationTestGeneration 🤖

Welcome to the Ollama-SemanticKernel-AutomationTestGeneration project! 🎉 This innovative tool combines the power of Ollama, Semantic Kernel, and LLMs to automagically generate C# Playwright page object models for your web automation tests. Say goodbye to manual test writing and hello to AI-powered efficiency! 💪

**Example Generated Model**
```plain
Create a ShoppingCartPage class for an e-commerce web application.
Page Name: ShoppingCartPage

[Elements]
- Cart Items List (selector: '#cart-items')
- Cart Item (selector: '.cart-item', multiple elements)
    - Product Name (selector: '.product-name')
    - Quantity Input (selector: '.quantity-input')
    - Remove Button (selector: '.remove-item')
- Checkout Button (selector: '#checkout-button')
[/Elements]

[Methods]
- UpdateItemQuantity method that takes a product name and new quantity as parameters and updates the quantity for the specified item.
- RemoveItem method that takes a product name as a parameter and removes the item from the cart.
- ProceedToCheckout method that clicks the checkout button.
[/Methods]
```

```csharp

    public class CartItems : BaseElement
    {
        private readonly ILocator _locator;

        public CartItems(ILocator locator) : base(locator)
        {
            _locator = locator;
        }

        public ILocator ProductName => _locator.Locator(".product-name");
        public ILocator QuantityInput => _locator.Locator(".quantity-input");
        public ILocator RemoveButton => _locator.Locator(".remove-item");
    }

    public CartItemsList CartItemsList => new CartItemsList(_page.Locator("#cart-items"));

    public class CheckoutButton : BaseElement
    {
        private readonly ILocator _locator;

        public CheckoutButton(ILocator locator) : base(locator)
        {
            _locator = locator;
        }
    }

    public CheckoutButton CheckoutButton => new CheckoutButton(_page.Locator("#checkout-button"));

    public async Task UpdateItemQuantityAsync(string productname, int quantity)
    {
        await CartItemsList.AllAsync().First(a => ((CartItems)a).ProductName.Text == productname).QuantityInput.Fill(quantity.ToString());
    }

    public async Task RemoveItemAsync(string productname)
    {
        await CartItemsList.AllAsync().First(a => ((CartItems)a).ProductName.Text == productname).RemoveButton.ClickAsync();
    }

    public async Task ProceedToCheckoutAsync()
    {
        await CheckoutButton.ClickAsync();
    }
}
```

## 🌟 Features

- 🧠 Utilizes Ollama and Semantic Kernel for intelligent code generation
- 🎭 Generates C# Playwright page object models
- 💬 Uses natural language descriptions to create test models
- 🔧 Customizable output formats (currently supports Handlebars)
- 📚 Includes example-based learning for improved accuracy

## 🧠 Prompts

This project leverages few-shot example prompting to generate accurate and contextually relevant C# Playwright page object models. Here's how our prompt engineering works:

1. 📝 **System Prompt** (`llm.md`): 
   - Sets the context for the AI, specifying its role and task
   - Provides general instructions for code generation

2. 🎨 **Output Format Instructions**:
   - `Handlebars.md`: Template for Handlebars output format
   - `Jinja.md`: Template for Jinja output format
   - `Razor.md`: Template for Razor output format

   These templates serve as a crucial mechanism to enforce the desired structure and format of the LLM's output response. By providing these instructions to the LLM, we ensure that the generated code adheres to a consistent and predefined format, regardless of the specific content. This approach allows for:
   
   - Consistency across generated page object models
   - Easy integration with existing codebases
   - Customizable output formats to suit different project needs or preferences

   The LLM uses these templates as a guide, filling in the placeholders with appropriate content based on the input description while maintaining the overall structure defined in the template.

3. 📚 **Few-Shot Examples** (`./resources/example-input/*.md`):
   - Provides multiple concrete examples of inputs and expected outputs
   - Stored in the `./resources/example-input/` directory as Markdown files
   - Helps the AI understand various scenarios, specific requirements, and conventions
   - Enhances the model's ability to generate accurate and relevant code for different page structures and complexities

This approach allows the AI to learn from a diverse set of examples and generate more accurate and relevant code based on natural language descriptions of web pages, while ensuring the output follows a consistent and desired format.

## 🛠️ Prerequisites

Before you begin, ensure you have the following installed:

- 📦 .NET SDK
- 🦙 Ollama
- 🧠 Semantic Kernel
- 🎭 Playwright for .NET

## 🚀 Getting Started

1. Clone this repository:
   ```
   git clone https://github.com/yourusername/Ollama-SemanticKernel-AutomationTestGeneration.git
   ```

2. Navigate to the project directory:
   ```
   cd Ollama-SemanticKernel-AutomationTestGeneration
   ```

3. Install the required NuGet packages:
   ```
   dotnet add package Microsoft.SemanticKernel
   dotnet add package OllamaSharp
   ```

4. Run the Jupyter notebook:
   ```
   jupyter notebook Ollama-SemanticKernel-AutomationTestGeneration.ipynb
   ```

## 🧩 How It Works

1. 📝 The system uses a pre-defined prompt template (`./resources/prompts/llm.md`)
2. 🔍 It loads example inputs from `./resources/prompts/example-input`
3. 📘 Output instructions are loaded from `./resources/prompts/output-instruct`
4. 🤖 The LLM (Ollama) generates C# Playwright page object models based on natural language descriptions
5. 🎨 The output is formatted according to the specified template (default: Handlebars)

## 🚀 Usage

1. Modify or add page object models to `resources/prompts/models` with `.md` extension
2. Run the notebook cells or C# project to generate your page object model(s)
3. 🎉 Voila! Your C# Playwright page object model is ready to use

## 🛠️ Customization

- 🎨 To change the output format, modify the `_outputFormat` variable
- 📁 Add or modify example inputs in the `./resources/prompts/example-input` directory
- 🔧 Adjust the system prompt by editing `./resources/prompts/llm.md`

## 🤝 Contributing

We welcome contributions! Please feel free to submit a Pull Request.

## 📜 License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.

## 🙏 Acknowledgements

- 🦙 [Ollama](https://ollama.ai/)
- 🧠 [Semantic Kernel](https://github.com/microsoft/semantic-kernel)
- 🎭 [Playwright for .NET](https://playwright.dev/dotnet/)

Happy testing! 🎉🚀