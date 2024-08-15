Create a page object model for the `Search Results Page`.

Page Name: SearchResultsPage
Elements:
- Search Bar (selector: '#search-bar')
- Results List (selector: '.results-list')
  - Result Item (selector: '.result-item', multiple elements)
    - Result Title (selector: '.result-title')
    - Result Description (selector: '.result-description')
    - View Details Button (selector: '.view-details')
- Pagination (selector: '.pagination')
  - Previous Page Button (selector: '.prev-page')
  - Next Page Button (selector: '.next-page')
  - Page Numbers (selector: '.page-number', multiple elements)
Methods:
- PerformSearch method that takes a search query as a parameter and performs a search.
- ViewResultDetails method that takes a result title as a parameter and clicks the View Details button for the matching result.
- GoToPage method that takes a page number as a parameter and navigates to the specified page.

---

using Microsoft.Playwright;

public class SearchResultsPage : BasePage
{
    private readonly IPage _page;

    public SearchResultsPage(IPage page) : base(page)
    {
        _page = page;
    }

    public class ResultsList : BaseElement
    {
        public class ResultItems : BaseElement
        {
            private readonly ILocator _locator;

            public ResultItems(ILocator locator) : base(locator)
            {
                _locator = locator;
            }

            public new async Task<IReadOnlyList<ResultItems>> AllAsync()
            {
                var all = await base.AllAsync();
                return all.Select(a => new ResultItems(a)).ToList();
            }

            public ILocator ResultTitle => _locator.Locator(".result-title");
            public ILocator ResultDescription => _locator.Locator(".result-description");
            public ILocator ViewDetailsButton => _locator.Locator(".view-details");
        }

        private readonly ILocator _locator;

        public ResultsList(ILocator locator)
        {
            _locator = locator;
        }

        public ResultItems ResultItems => new ResultItems(_locator.Locator(".result-item"));
    }

    public class Pagination : BaseElement
    {
        private readonly ILocator _locator;

        public Pagination(ILocator locator)
        {
            _locator = locator;
        }

        public ILocator PreviousPageButton => _locator.Locator(".prev-page");
        public ILocator NextPageButton => _locator.Locator(".next-page");
        public ILocator PageNumbers => _locator.Locator(".page-number");
    }

    public ILocator SearchBar => _page.Locator("#search-bar");
    public ResultsList ResultsList => new ResultsList(_page.Locator(".results-list"));
    public Pagination Pagination => new Pagination(_page.Locator(".pagination"));

    public async Task PerformSearchAsync(string query)
    {
        await SearchBar.FillAsync(query);
        await SearchBar.PressAsync("Enter");
    }

    public async Task ViewResultDetailsAsync(string resultTitle)
    {
        var results = await ResultsList.ResultItems.AllAsync();
        var targetResult = results.FirstOrDefault(r => r.ResultTitle.GetAttributeAsync("textContent") == resultTitle);
        if (targetResult != null)
        {
            await targetResult.ViewDetailsButton.ClickAsync();
        }
    }

    public async Task GoToPageAsync(int pageNumber)
    {
        await Pagination.PageNumbers.Nth(pageNumber - 1).ClickAsync();
    }
}