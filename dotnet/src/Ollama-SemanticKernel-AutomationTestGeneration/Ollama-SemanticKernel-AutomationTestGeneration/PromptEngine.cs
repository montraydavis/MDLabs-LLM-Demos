using System;
using System.Text;

/// <summary>
/// Simple prompt engine.
/// </summary>
public static class PromptEngine{

    public static string _outputFormat = "Handlebars";
    public static string _promptsDirectory = "resources/prompts/example-input";
    public static string _outputInstructDirectory = "resources/prompts/output-instruct";
    public static string _mainPromptFile = "resources/prompts/llm.md";

    /// <summary>
    /// Loads the examples from files asynchronously.
    /// </summary>
    /// <param name="maxExamples">The maximum number of examples to load.</param>
    /// <returns>The examples.</returns>
    public static async Task<string> LoadExamplesFromFilesAsync(int maxExamples)
    {
        var exampleFiles = Directory.GetFiles(_promptsDirectory, "*.md");
        var examples = new StringBuilder();

        var idx = 0;

        foreach (var file in exampleFiles)
        {
            if(idx >= maxExamples)
            {
                break;
            }

            examples.AppendLine($"Example: {Path.GetFileNameWithoutExtension(file)}");
            examples.AppendLine(await File.ReadAllTextAsync(file));
            examples.AppendLine();

            idx++;
        }

        return examples.ToString();
    }

    /// <summary>
    /// Loads the output instructions asynchronously.
    /// </summary>
    /// <returns>The output instructions.</returns> 
    public static async Task<string> LoadOutputInstructionsAsync()
    {
        var instructionFile = Path.Combine(_outputInstructDirectory, $"{_outputFormat}.md");
        if (File.Exists(instructionFile))
        {
            return await File.ReadAllTextAsync(instructionFile);
        }
        return string.Empty;
    }

    /// <summary>
    /// Loads the main prompt asynchronously.
    /// </summary>
    /// <returns>The main prompt.</returns>
    public static async Task<string> LoadMainPromptAsync()
    {
        if (File.Exists(_mainPromptFile))
        {
            return await File.ReadAllTextAsync(_mainPromptFile);
        }
        throw new FileNotFoundException($"Main prompt file not found: {_mainPromptFile}");
    }

    /// <summary>
    /// Gets the system prompt with examples asynchronously.
    /// </summary>
    /// <param name="maxExamples">The maximum number of examples to load.</param>
    /// <returns>The system prompt with examples.</returns>
    public static async Task<string> GetSystemPromptWithExamplesAsync(int maxExamples = 3)
    {
        var mainPrompt = await LoadMainPromptAsync();
        var examples = await LoadExamplesFromFilesAsync(maxExamples);
        var outputInstructions = await LoadOutputInstructionsAsync();

        return string.Format(mainPrompt, examples, _outputFormat, outputInstructions);
    }
}