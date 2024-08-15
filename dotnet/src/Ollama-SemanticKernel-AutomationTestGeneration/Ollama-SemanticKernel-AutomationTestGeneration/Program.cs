using System;
using System.IO;
using System.Net.Http;
using System.Threading;

using OllamaSharp;
using OllamaSharp.Models.Chat;

using Microsoft.Extensions.DependencyInjection;

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

var prompts = Directory.EnumerateFiles("resources/prompts/models")
    .Select(promptFile => File.ReadAllText(promptFile));

var sysPrompt = await PromptEngine.GetSystemPromptWithExamplesAsync();

// await SolveProblemWithToT(input);
var ollamaChat = new GenericChatCompletionService("http://localhost:11434", "llama3");

var builder = Kernel.CreateBuilder();

builder.Services.AddKeyedSingleton<IChatCompletionService>("ollamaChat", ollamaChat);
Kernel kernel = builder.Build();

foreach(var prompt in prompts){
    var chat = kernel.GetRequiredService<IChatCompletionService>();

    var history = new ChatHistory();
    history.AddSystemMessage(sysPrompt);
    history.AddUserMessage(prompt);

    var result = await chat.GetChatMessageContentsAsync(history);

    Console.WriteLine(result[^1].Content);
}

Console.ReadLine();