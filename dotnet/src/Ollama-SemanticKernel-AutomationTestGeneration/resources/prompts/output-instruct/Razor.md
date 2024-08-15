@using System.Collections.Generic
@model PageObjectModel

@functions {
    public class PageObjectModel
    {
        public string PageName { get; set; }
        public List<Element> Elements { get; set; }
        public List<Method> Methods { get; set; }
    }

    public class Element
    {
        public string Name { get; set; }
        public string Selector { get; set; }
        public List<Element> Children { get; set; }
        public List<Property> Properties { get; set; }
    }

    public class Property
    {
        public string Name { get; set; }
        public string Selector { get; set; }
    }

    public class Method
    {
        public string Name { get; set; }
        public string Parameters { get; set; }
        public string Implementation { get; set; }
    }
}

using Microsoft.Playwright;

public class @Model.PageName : BasePage
{
    private readonly IPage _page;

    public @(Model.PageName)(IPage page) : base(page)
    {
        _page = page;
    }

    @foreach (var element in Model.Elements)
    {
    <text>
    public class @(element.Name) : BaseElement
    {
        @if (element.Children != null && element.Children.Any())
        {
        <text>
        public class @(element.Name)Item : BaseElement
        {
            private readonly ILocator _locator;

            public @(element.Name)Item(ILocator locator) : base(locator)
            {
                _locator = locator;
            }

            public new async Task<IReadOnlyList<@(element.Name)Item>> AllAsync()
            {
                var all = await base.AllAsync();
                return all.Select(a => new @(element.Name)Item(a)).ToList();
            }

            @foreach (var child in element.Children)
            {
            <text>
            public ILocator @(child.Name) => _locator.Locator("@(child.Selector)");
            </text>
            }
        }
        </text>
        }

        private readonly ILocator _locator;

        public @(element.Name)(ILocator locator) : base(locator)
        {
            _locator = locator;
        }

        @if (element.Children != null && element.Children.Any())
        {
        <text>
        public @(element.Name)Item @(element.Name)Items => new @(element.Name)Item(_locator.Locator("@(element.Children[0].Selector)"));
        </text>
        }
        else if (element.Properties != null)
        {
            foreach (var property in element.Properties)
            {
            <text>
        public ILocator @(property.Name) => _locator.Locator("@(property.Selector)");
            </text>
            }
        }
    }

    public @(element.Name) @(element.Name) => new @(element.Name)(_page.Locator("@(element.Selector)"));
    </text>
    }

    @foreach (var method in Model.Methods)
    {
    <text>
    public async Task @(method.Name)Async(@(method.Parameters))
    {
        @(Html.Raw(method.Implementation))
    }
    </text>
    }
}