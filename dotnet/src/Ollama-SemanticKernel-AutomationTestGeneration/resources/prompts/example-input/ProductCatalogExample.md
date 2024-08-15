Create a page object model for the `Product Catalog Page` .

Page Name: ProductCatalogPage
Elements:
- Product Grid (selector: '.product-grid')
  - Product Card (selector: '.product-card', multiple elements)
    - Product Name (selector: '.product-name')
    - Product Price (selector: '.product-price')
    - Add to Cart Button (selector: '.add-to-cart')
- Filter Panel (selector: '#filter-panel')
  - Category Dropdown (selector: '#category-select')
  - Price Range Slider (selector: '#price-range')
Methods:
- FilterProducts method that takes category and price range as parameters and applies the filters.
- AddProductToCart method that takes a product name as a parameter and clicks the Add to Cart button for the matching product.

---

using Microsoft.Playwright;

public class ProductCatalogPage : BasePage
{
    private readonly IPage _page;

    public ProductCatalogPage(IPage page) : base(page)
    {
        _page = page;
    }

    public class ProductGrid : BaseElement
    {
        public class ProductCard : BaseElement
        {
            private readonly ILocator _locator;

            public ProductCard(ILocator locator) : base(locator)
            {
                _locator = locator;
            }

            public new async Task<IReadOnlyList<ProductCard>> AllAsync()
            {
                var all = await base.AllAsync();
                return all.Select(a => new ProductCard(a)).ToList();
            }

            public ILocator ProductName => _locator.Locator(".product-name");
            public ILocator ProductPrice => _locator.Locator(".product-price");
            public ILocator AddToCartButton => _locator.Locator(".add-to-cart");
        }

        private readonly ILocator _locator;

        public ProductGrid(ILocator locator) : base(locator)
        {
            _locator = locator;
        }

        public ProductCard ProductCards => new ProductCard(_locator.Locator(".product-card"));
    }

    public class FilterPanel : BaseElement
    {
        private readonly ILocator _locator;

        public FilterPanel(ILocator locator) : base(locator)
        {
            _locator = locator;
        }

        public ILocator CategoryDropdown => _locator.Locator("#category-select");
        public ILocator PriceRangeSlider => _locator.Locator("#price-range");
    }

    public ProductGrid ProductGrid => new ProductGrid(_page.Locator(".product-grid"));
    public FilterPanel FilterPanel => new FilterPanel(_page.Locator("#filter-panel"));

    public async Task FilterProductsAsync(string category, (int min, int max) priceRange)
    {
        await FilterPanel.CategoryDropdown.SelectOptionAsync(category);
        await FilterPanel.PriceRangeSlider.EvaluateAsync($"el => {{ el.min = {priceRange.min}; el.max = {priceRange.max}; }}");
    }

    public async Task AddProductToCartAsync(string productName)
    {
        var productCards = await ProductGrid.ProductCards.AllAsync();
        var targetProduct = productCards.FirstOrDefault(card => card.ProductName.GetAttributeAsync("textContent") == productName);
        if (targetProduct != null)
        {
            await targetProduct.AddToCartButton.ClickAsync();
        }
    }
}