using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.SemanticKernel;

public class PlaywrightPlugin {

    protected PlaywrightPlugin(IPage page){
        this.page = page;
    }
    
    public static PlaywrightPlugin CreateInstance(IPage page){
        return new PlaywrightPlugin(page);
    }

    protected IPage page;

    [Description("Get the current URL")]
    [KernelFunction(nameof(GetUrlAsync))]
    public async Task<string> GetUrlAsync(){
        return await Task.FromResult(page.Url);
    }

    [Description("Get the title of the page")]
    [KernelFunction(nameof(GetTitleAsync))]
    public async Task<string> GetTitleAsync(){
        return await page.TitleAsync();
    }


    [Description("Navigate to a specified URL")]
    [KernelFunction(nameof(GoToAsync))]
    public async Task GoToAsync(string url){
        await page.GotoAsync(url);
    }

    [Description("Fill a form field with text")]
    [KernelFunction(nameof(FillAsync))]
    public async Task FillAsync(string selector, string text){
        await page.FillAsync(selector, text);
    }

    [Description("Clear the content of a form field")]
    [KernelFunction(nameof(ClearAsync))]
    public async Task ClearAsync(string selector){
        await page.FillAsync(selector, string.Empty);
    }

    [Description("Click on an element")]
    [KernelFunction(nameof(ClickAsync))]
    public async Task ClickAsync(string selector){
        await page.ClickAsync(selector);
    }

    [Description("Double-click on an element")]
    [KernelFunction(nameof(DblClickAsync))]
    public async Task DblClickAsync(string selector){
        await page.DblClickAsync(selector);
    }

    [Description("Hover over an element")]
    [KernelFunction(nameof(HoverAsync))]
    public async Task HoverAsync(string selector){
        await page.HoverAsync(selector);
    }

    [Description("Select an option from a dropdown")]
    [KernelFunction(nameof(SelectAsync))]
    public async Task SelectAsync(string selector, string value){
        await page.SelectOptionAsync(selector, value);
    }

    [Description("Focus on an element")]
    [KernelFunction(nameof(FocusAsync))]
    public async Task FocusAsync(string selector){
        await page.FocusAsync(selector);
    }

    [Description("Remove focus from an element")]
    [KernelFunction(nameof(BlurAsync))]
    public async Task BlurAsync(string selector){
        await page.Locator(selector).BlurAsync();
    }

    [Description("Check a checkbox or radio button")]
    [KernelFunction(nameof(CheckAsync))]
    public async Task CheckAsync(string selector){
        await page.CheckAsync(selector);
    }

    [Description("Uncheck a checkbox")]
    [KernelFunction(nameof(UncheckAsync))]
    public async Task UncheckAsync(string selector){
        await page.UncheckAsync(selector);
    }

    [Description("Press a key on an element")]
    [KernelFunction(nameof(PressAsync))]
    public async Task PressAsync(string selector, string key){
        await page.PressAsync(selector, key);
    }

    [Description("Tap on an element (for touch devices)")]
    [KernelFunction(nameof(TapAsync))]
    public async Task TapAsync(string selector){
        await page.TapAsync(selector);
    }

    [Description("Set files for a file input")]
    [KernelFunction(nameof(SetInputFilesAsync))]
    public async Task SetInputFilesAsync(string selector, string[] files){
        await page.SetInputFilesAsync(selector, files);
    }

    [Description("Wait for an element to appear")]
    [KernelFunction(nameof(WaitForAsync))]
    public async Task WaitForAsync(string selector){
        await page.WaitForSelectorAsync(selector);
    }

    [Description("Wait for a specific page load state")]
    [KernelFunction(nameof(WaitForLoadStateAsync))]
    public async Task WaitForLoadStateAsync(LoadState state){
        await page.WaitForLoadStateAsync(state);
    }

    [Description("Take a screenshot of the page")]
    [KernelFunction(nameof(ScreenshotAsync))]
    public async Task<string> ScreenshotAsync(string path){
        try{
            var screenshot = await page.ScreenshotAsync(new PageScreenshotOptions { Path = path });
            var base64Screenshot = Convert.ToBase64String(screenshot);
            
            return path;
        }
        catch(Exception ex){
            Console.WriteLine($"Error: {ex.Message}");

            return ex.Message;
        }

    }
}