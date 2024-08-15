using System;

using OllamaSharp;
using OllamaSharp.Models.Chat;

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

/// <summary>
/// Base class for Ollama chat completion services.
/// </summary>
public abstract class BaseOllamaChatCompletionService : IChatCompletionService
{
    protected readonly HttpClient _httpClient;
    protected readonly string _modelName;
    
    public IReadOnlyDictionary<string, object?> Attributes { get; set; } = new Dictionary<string, object?>();

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseOllamaChatCompletionService"/> class.
    /// </summary>
    /// <param name="modelUrl">The URL of the model.</param>
    /// <param name="modelName">The name of the model.</param>
    protected BaseOllamaChatCompletionService(string modelUrl, string modelName)
    {
        _modelName = modelName;
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(modelUrl),
            Timeout = TimeSpan.FromMinutes(2)
        };
    }


    /// <summary>
    /// Gets the chat message contents asynchronously.
    /// </summary>
    /// <param name="chatHistory">The chat history.</param>
    /// <param name="executionSettings">The execution settings.</param>
    /// <param name="kernel">The kernel.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public virtual async Task<IReadOnlyList<ChatMessageContent>> GetChatMessageContentsAsync(
        ChatHistory chatHistory,
        PromptExecutionSettings? executionSettings = null,
        Kernel? kernel = null,
        CancellationToken cancellationToken = default)
    {
        var ollama = new OllamaApiClient(_httpClient, _modelName);
        var chat = new Chat(ollama, _ => { });

        // Add system message
        var systemPrompt = await GetSystemPromptAsync();
        await chat.SendAs(ChatRole.System, systemPrompt, cancellationToken);

        // Iterate through chatHistory Messages
        foreach (var message in chatHistory)
        {
            if (message.Role == AuthorRole.System)
            {
                await chat.SendAs(ChatRole.System, message.Content ?? string.Empty, cancellationToken);
            }
        }

        var lastMessage = chatHistory.LastOrDefault();
        string question = lastMessage?.Content ?? string.Empty;
        var history = (await chat.Send(question, cancellationToken)).ToArray();
        var chatResponse = history.Last().Content ?? string.Empty;

        chatHistory.AddAssistantMessage(chatResponse);

        return chatHistory;
    }

    /// <summary>
    /// Gets the streaming chat message contents asynchronously.
    /// </summary>
    /// <param name="chatHistory">The chat history.</param>
    /// <param name="executionSettings">The execution settings.</param>
    /// <param name="kernel">The kernel.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public virtual IAsyncEnumerable<StreamingChatMessageContent> GetStreamingChatMessageContentsAsync(
        ChatHistory chatHistory,
        PromptExecutionSettings? executionSettings = null,
        Kernel? kernel = null,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    protected abstract Task<string> GetSystemPromptAsync();
}
