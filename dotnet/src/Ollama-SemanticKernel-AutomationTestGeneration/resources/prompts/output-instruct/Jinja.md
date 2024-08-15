using Microsoft.Playwright;

public class {{ page_name }} : BasePage
{
    private readonly IPage _page;

    public {{ page_name }}(IPage page) : base(page)
    {
        _page = page;
    }

    {% for element in elements %}
    public class {{ element.name }} : BaseElement
    {
        {% if element.children %}
        public class {{ element.child_name }} : BaseElement
        {
            private readonly ILocator _locator;

            public {{ element.child_name }}(ILocator locator) : base(locator)
            {
                _locator = locator;
            }

            public new async Task<IReadOnlyList<{{ element.child_name }}>> AllAsync()
            {
                var all = await base.AllAsync();
                return all.Select(a => new {{ element.child_name }}(a)).ToList();
            }

            {% for child in element.children %}
            public ILocator {{ child.name }} => _locator.Locator("{{ child.selector }}");
            {% endfor %}
        }
        {% endif %}

        private readonly ILocator _locator;

        public {{ element.name }}(ILocator locator) : base(locator)
        {
            _locator = locator;
        }

        {% if element.children %}
        public {{ element.child_name }} {{ element.child_name }} => new {{ element.child_name }}(_locator.Locator("{{ element.child_selector }}"));
        {% else %}
        {% for child in element.properties %}
        public ILocator {{ child.name }} => _locator.Locator("{{ child.selector }}");
        {% endfor %}
        {% endif %}
    }

    public {{ element.name }} {{ element.name }} => new {{ element.name }}(_page.Locator("{{ element.selector }}"));
    {% endfor %}

    {% for method in methods %}
    public async Task {{ method.name }}Async({{ method.parameters }})
    {
        {{ method.implementation }}
    }
    {% endfor %}
}