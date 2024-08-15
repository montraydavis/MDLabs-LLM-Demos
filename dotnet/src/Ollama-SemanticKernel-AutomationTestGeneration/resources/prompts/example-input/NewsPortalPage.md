Create a page object model for the `News Portal Page`.

Page Name: NewsPortalPage
Elements:
- Article List (selector: '.article-list')
  - Article Item (selector: '.article-item', multiple elements)
    - Article Title (selector: '.article-title')
    - Article Summary (selector: '.article-summary')
    - Read More Button (selector: '.read-more')
Methods:
- ReadArticle method that takes an article title as a parameter and clicks on the Read More button for the matching article.

---

using Microsoft.Playwright;

public class NewsPortalPage : BasePage
{
    private readonly IPage _page;

    public NewsPortalPage(IPage page) : base(page)
    {
        _page = page;
    }

    public class ArticleList : BaseElement
    {
        public class ArticleItems : BaseElement
        {
            private readonly ILocator _locator;

            public ArticleItems(ILocator locator) : base(locator)
            {
                _locator = locator;
            }

            public new async Task<IReadOnlyList<ArticleItems>> AllAsync()
            {
                var all = await base.AllAsync();
                return all.Select(a => new ArticleItems(a)).ToList();
            }

            public ILocator ArticleTitle => _locator.Locator(".article-title");
            public ILocator ArticleSummary => _locator.Locator(".article-summary");
            public ILocator ReadMoreButton => _locator.Locator(".read-more");
        }

        private readonly ILocator _locator;

        public ArticleList(ILocator locator)
        {
            _locator = locator;
        }

        public ArticleItems ArticleItems => new ArticleItems(_locator.Locator(".article-item"));
    }

    public ArticleList ArticleList => new ArticleList(_page.Locator(".article-list"));

    public async Task ReadArticleAsync(string articleTitle)
    {
        var articles = await ArticleList.ArticleItems.AllAsync();
        var targetArticle = articles.FirstOrDefault(a => a.ArticleTitle.GetAttributeAsync("textContent") == articleTitle);
        if (targetArticle != null)
        {
            await targetArticle.ReadMoreButton.ClickAsync();
        }
    }
}