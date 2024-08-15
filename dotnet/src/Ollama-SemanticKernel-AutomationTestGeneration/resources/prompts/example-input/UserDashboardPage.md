Create a page object model for the `User Dashboard Page`.

Page Name: UserDashboardPage
Elements:
- User Info Panel (selector: '.user-info')
  - User Name (selector: '.user-name')
  - User Email (selector: '.user-email')
  - Edit Profile Button (selector: '.edit-profile')
- Activity Feed (selector: '.activity-feed')
  - Activity Item (selector: '.activity-item', multiple elements)
    - Activity Type (selector: '.activity-type')
    - Activity Description (selector: '.activity-description')
    - Activity Date (selector: '.activity-date')
- Quick Actions Panel (selector: '.quick-actions')
  - New Post Button (selector: '.new-post')
  - Messages Button (selector: '.messages')
  - Settings Button (selector: '.settings')
Methods:
- EditProfile method that clicks the Edit Profile button.
- CreateNewPost method that clicks the New Post button.
- GetLatestActivityDescription method that returns the description of the most recent activity.

---

using Microsoft.Playwright;

public class UserDashboardPage : BasePage
{
    private readonly IPage _page;

    public UserDashboardPage(IPage page) : base(page)
    {
        _page = page;
    }

    public class UserInfoPanel : BaseElement
    {
        private readonly ILocator _locator;

        public UserInfoPanel(ILocator locator) : base(locator)
        {
            _locator = locator;
        }

        public ILocator UserName => _locator.Locator(".user-name");
        public ILocator UserEmail => _locator.Locator(".user-email");
        public ILocator EditProfileButton => _locator.Locator(".edit-profile");
    }

    public class ActivityFeed : BaseElement
    {
        public class ActivityItems : BaseElement
        {
            private readonly ILocator _locator;

            public ActivityItems(ILocator locator) : base(locator)
            {
                _locator = locator;
            }

            public new async Task<IReadOnlyList<ActivityItems>> AllAsync()
            {
                var all = await base.AllAsync();
                return all.Select(a => new ActivityItems(a)).ToList();
            }

            public ILocator ActivityType => _locator.Locator(".activity-type");
            public ILocator ActivityDescription => _locator.Locator(".activity-description");
            public ILocator ActivityDate => _locator.Locator(".activity-date");
        }

        private readonly ILocator _locator;

        public ActivityFeed(ILocator locator)
        {
            _locator = locator;
        }

        public ActivityItems ActivityItems => new ActivityItems(_locator.Locator(".activity-item"));
    }

    public class QuickActionsPanel : BaseElement
    {
        private readonly ILocator _locator;

        public QuickActionsPanel(ILocator locator) : base(locator)
        {
            _locator = locator;
        }

        public ILocator NewPostButton => _locator.Locator(".new-post");
        public ILocator MessagesButton => _locator.Locator(".messages");
        public ILocator SettingsButton => _locator.Locator(".settings");
    }

    public UserInfoPanel UserInfoPanel => new UserInfoPanel(_page.Locator(".user-info"));
    public ActivityFeed ActivityFeed => new ActivityFeed(_page.Locator(".activity-feed"));
    public QuickActionsPanel QuickActionsPanel => new QuickActionsPanel(_page.Locator(".quick-actions"));

    public async Task EditProfileAsync()
    {
        await UserInfoPanel.EditProfileButton.ClickAsync();
    }

    public async Task CreateNewPostAsync()
    {
        await QuickActionsPanel.NewPostButton.ClickAsync();
    }

    public async Task<string> GetLatestActivityDescriptionAsync()
    {
        var activities = await ActivityFeed.ActivityItems.AllAsync();
        if (activities.Any())
        {
            return await activities.First().ActivityDescription.TextContentAsync();
        }
        return string.Empty;
    }
}