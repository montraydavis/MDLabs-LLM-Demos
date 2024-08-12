using Microsoft.TypeChat;

/// <summary>
/// A library of common Prompt Sections and instructions
/// </summary>
public class PromptLibrary
{
    /// <summary>
    /// A common prompt section for instructions
    /// </summary>
    public static PromptSection Instructions()
    {
        return new PromptSection("Extract each log entry from the provided unstructured logs.");
    }
}