using Microsoft.Playwright;

public class {{pageName}} : BasePage
{
    private readonly IPage _page;

    public {{pageName}}(IPage page) : base(page)
    {
        _page = page;
    }

    {{#each elements}}
    public class {{this.name}} : BaseElement
    {
        {{#if this.children}}
        public class {{this.childName}} : BaseElement
        {
            private readonly ILocator _locator;

            public {{this.childName}}(ILocator locator) : base(locator)
            {
                _locator = locator;
            }

            public new async Task<IReadOnlyList<{{this.childName}}>> AllAsync()
            {
                var all = await base.AllAsync();
                return all.Select(a => new {{this.childName}}(a)).ToList();
            }

            {{#each this.children}}
            public ILocator {{this.name}} => _locator.Locator("{{this.selector}}");
            {{/each}}
        }
        {{/if}}

        private readonly ILocator _locator;

        public {{this.name}}(ILocator locator) : base(locator)
        {
            _locator = locator;
        }

        {{#if this.children}}
        public {{this.childName}} {{this.childName}} => new {{this.childName}}(_locator.Locator("{{this.childSelector}}"));
        {{else}}
        {{#each this.properties}}
        public ILocator {{this.name}} => _locator.Locator("{{this.selector}}");
        {{/each}}
        {{/if}}
    }

    public {{this.name}} {{this.name}} => new {{this.name}}(_page.Locator("{{this.selector}}"));
    {{/each}}

    {{#each methods}}
    public async Task {{this.name}}Async({{this.parameters}})
    {
        {{{this.implementation}}}
    }
    {{/each}}
}