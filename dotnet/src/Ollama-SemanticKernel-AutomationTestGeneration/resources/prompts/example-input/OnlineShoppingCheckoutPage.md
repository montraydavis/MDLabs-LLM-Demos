Create a page object model for the `Online Shopping Checkout Page`.

Page Name: OnlineShoppingCheckoutPage
Elements:
- Checkout Form (selector: '#checkout-form')
  - Form Section (selector: '.form-section', multiple elements)
    - Section Title (selector: '.section-title')
    - Input Fields (selector: '.input-field', multiple elements)
    - Submit Button (selector: '.submit-button')
Methods:
- FillFormSection method that takes a section title and a dictionary of field names and values, then fills in the corresponding input fields in the specified section.

---

using Microsoft.Playwright;
using System.Collections.Generic;

public class OnlineShoppingCheckoutPage : BasePage
{
    private readonly IPage _page;

    public OnlineShoppingCheckoutPage(IPage page) : base(page)
    {
        _page = page;
    }

    public class CheckoutForm : BaseElement
    {
        public class FormSections : BaseElement
        {
            private readonly ILocator _locator;

            public FormSections(ILocator locator) : base(locator)
            {
                _locator = locator;
            }

            public new async Task<IReadOnlyList<FormSections>> AllAsync()
            {
                var all = await base.AllAsync();
                return all.Select(a => new FormSections(a)).ToList();
            }

            public ILocator SectionTitle => _locator.Locator(".section-title");
            public ILocator InputFields => _locator.Locator(".input-field");
            public ILocator SubmitButton => _locator.Locator(".submit-button");
        }

        private readonly ILocator _locator;

        public CheckoutForm(ILocator locator)
        {
            _locator = locator;
        }

        public FormSections FormSections => new FormSections(_locator.Locator(".form-section"));
    }

    public CheckoutForm CheckoutForm => new CheckoutForm(_page.Locator("#checkout-form"));

    public async Task FillFormSectionAsync(string sectionTitle, Dictionary<string, string> fieldValues)
    {
        var sections = await CheckoutForm.FormSections.AllAsync();
        var targetSection = sections.FirstOrDefault(s => s.SectionTitle.GetAttributeAsync("textContent") == sectionTitle);
        
        if (targetSection != null)
        {
            foreach (var field in fieldValues)
            {
                await targetSection.InputFields.Filter(new() { HasAttribute = $"name='{field.Key}'" }).FillAsync(field.Value);
            }
            await targetSection.SubmitButton.ClickAsync();
        }
    }
}