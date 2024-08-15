using System;

/// <summary>
/// Generic chat completion service.
/// </summary>
public class GenericChatCompletionService : BaseOllamaChatCompletionService
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GenericChatCompletionService"/> class.
    /// </summary>
    /// <param name="modelUrl">The URL of the model.</param>
    /// <param name="modelName">The name of the model.</param>
    public GenericChatCompletionService(string modelUrl, string modelName) : base(modelUrl, modelName)
    {

    }

    /// <summary>
    /// Gets the system prompt asynchronously.
    /// </summary>
    /// <returns>The system prompt.</returns>
    protected override Task<string> GetSystemPromptAsync()
    {
        return Task.FromResult("You are a helpful assistant.");
    }
}