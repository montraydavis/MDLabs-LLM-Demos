Create a page object model for the `Social Media Profile Page`.

Page Name: SocialMediaProfilePage
Elements:
- Post Feed (selector: '.post-feed')
  - Post (selector: '.post', multiple elements)
    - Post Content (selector: '.post-content')
    - Like Button (selector: '.like-button')
    - Comment Button (selector: '.comment-button')
Methods:
- LikePost method that takes a post index as a parameter and clicks the Like button for the specified post.

---

using Microsoft.Playwright;

public class SocialMediaProfilePage : BasePage
{
    private readonly IPage _page;

    public SocialMediaProfilePage(IPage page) : base(page)
    {
        _page = page;
    }

    public class PostFeed : BaseElement
    {
        public class Posts : BaseElement
        {
            private readonly ILocator _locator;

            public Posts(ILocator locator) : base(locator)
            {
                _locator = locator;
            }

            public new async Task<IReadOnlyList<Posts>> AllAsync()
            {
                var all = await base.AllAsync();
                return all.Select(a => new Posts(a)).ToList();
            }

            public ILocator PostContent => _locator.Locator(".post-content");
            public ILocator LikeButton => _locator.Locator(".like-button");
            public ILocator CommentButton => _locator.Locator(".comment-button");
        }

        private readonly ILocator _locator;

        public PostFeed(ILocator locator)
        {
            _locator = locator;
        }

        public Posts Posts => new Posts(_locator.Locator(".post"));
    }

    public PostFeed PostFeed => new PostFeed(_page.Locator(".post-feed"));

    public async Task LikePostAsync(int postIndex)
    {
        var posts = await PostFeed.Posts.AllAsync();
        if (postIndex >= 0 && postIndex < posts.Count)
        {
            await posts[postIndex].LikeButton.ClickAsync();
        }
    }
}