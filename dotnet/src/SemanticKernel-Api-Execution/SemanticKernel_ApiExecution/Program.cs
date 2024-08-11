using System;
using System.Threading.Tasks;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace SemanticKernel_ApiExecution;

class Program
{
    static async Task Main(string[] args)
    {
        var executionSettings = new OpenAIPromptExecutionSettings()
        {
            ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
        };

        var builder = Kernel.CreateBuilder();

        builder
            .AddOpenAIChatCompletion(
                "gpt-3.5-turbo",
                "sk-proj-"
            )
            .AddOpenAITextGeneration(
                "gpt-3.5-turbo",
                "sk-proj-"
            );

        builder.Plugins.AddFromObject(TwitterApiV2Mock.CreateInstance());


        Kernel kernel = builder.Build();

        kernel.FunctionInvocationFilters.Add(new TwitterApiV2MockFunctionFilter(builder));

        // GetUser
        var getUserById = await kernel.InvokePromptAsync("Find the user profile for ID 1", new(executionSettings));
        var getUserDetails = await kernel.InvokePromptAsync("Show me the details of the user with identifier 1", new(executionSettings));
        var getUserInfo = await kernel.InvokePromptAsync("What information do you have on the user with ID 1?", new(executionSettings));

    }
}
