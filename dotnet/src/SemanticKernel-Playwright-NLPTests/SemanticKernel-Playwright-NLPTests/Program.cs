using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace SemanticKernel_ApiExecution;

class Program
{
    static async Task Main(string[] args)
    {

        bool useAzureOpenAI = false;

        var processes = Process.GetProcessesByName("chromium");

        foreach (var process in processes)
        {
            process.Kill();
        }

        var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync(new()
        {
            Headless = false
        });
        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            RecordVideoDir = "videos/",
            RecordVideoSize = new RecordVideoSize { Width = 1280, Height = 720 }
        });
        var page = await context.NewPageAsync();

        var executionSettings = new OpenAIPromptExecutionSettings()
        {
            ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
        };

        var builder = Kernel.CreateBuilder();

        builder
            .AddOpenAIChatCompletion(
                "gpt-4",
                "sk-proj-"
            )
            .AddOpenAITextGeneration(
                "gpt-4",
                "sk-proj-"
            );

        builder.Plugins.AddFromObject(PlaywrightPlugin.CreateInstance(page));


        Kernel kernel = builder.Build();

        var navigateCommand = await kernel.InvokePromptAsync("Navigate to 'https://google.com/'", new(executionSettings));
        var fillCommand = await kernel.InvokePromptAsync("Fill '[aria-label=\"Search\"]' with 'Semantic Kernel'.", new(executionSettings));
        var pressCommand = await kernel.InvokePromptAsync("Press '[aria-label=\"Search\"]' with \"Enter\".", new(executionSettings));
        var clickLinkCommand = await kernel.InvokePromptAsync("Click link \"text=Introduction to Semantic Kernel\"", new(executionSettings));
        var pageTitleAndUrlCommands = await kernel.InvokePromptAsync("What is the current page title and url?", new(executionSettings));

        Console.WriteLine($"Image of the page has been saved to './ss.png'.\n\n{Convert.ToBase64String(File.ReadAllBytes("./ss.png"))}");

        await browser.CloseAsync();

        var result = new
        {
            navigateCommand,
            fillCommand,
            pressCommand,
            clickLinkCommand,
            pageTitleAndUrlCommands
        };
    }
}
