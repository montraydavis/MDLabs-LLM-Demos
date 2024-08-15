Create a page object model for the `Client Select Page` .

Page Name: ClientSelectPage
Elements:
- Client List (selector: '.clientList')
  - Client Row (selector: '.row', multiple elements)
    - Client Name (selector: '.name')
Methods:
- SelectClient method that takes a client name as a parameter and clicks on the client row with the matching name.

---

using Microsoft.Playwright;

public class ClientSelectPage : BasePage
{
    private readonly IPage _page;

    public ClientSelectPage(IPage page) : base(page)
    {
        _page = page;
    }

    public class ClientList : BaseElement
    {
        public class ClientRows : BaseElement
        {
            private readonly ILocator _locator;

            public ClientRows(ILocator locator) : base(locator)
            {
                _locator = locator;
            }

            public new async Task<IReadOnlyList<ClientRows>> AllAsync()
            {
                var all = await base.AllAsync();
                
                return all.Select(a => new ClientRows(a)).ToList();
            }

            public ILocator ClientName => _locator.Locator(".name");
        }

        private readonly ILocator _locator;

        public ClientList(ILocator locator)
        {
            _locator = locator;
        }

        public ClientRows ClientRows => new ClientRows(_locator.Locator(".row"));
    }

    public ClientList ClientList => new ClientList(_page.Locator(".clientList"));

    public async Task SelectClientAsync(string clientName)
    {
        await ClientList.ClientRows.ClientName.ClickAsync($"[data-client-name="{clientName}"]");
    }
}