using Microsoft.TypeChat;
using Microsoft.TypeChat.Schema;

public class LogExtractionApp<T> : ConsoleApp
{
    JsonTranslator<T> _translator;

    public static LogExtractionApp<T> CreateInstance()
    {
        return new LogExtractionApp<T>();
    }

    protected LogExtractionApp()
    {
        _translator = new JsonTranslator<T>(
            new LanguageModel(new()
            {
                Model = "gpt-3.5-turbo",
                ApiKey = "sk-proj-",
                Endpoint = "https://api.openai.com/v1/chat/completions",
                Azure = false,
                TimeoutMs = 30000
            })
        );

        // Uncomment to see ALL raw messages to and from the AI
        //base.SubscribeAllEvents(_translator);
    }

    public TypeSchema Schema => _translator.Validator.Schema;

    public override async Task<string> ProcessInputAsync(string input, CancellationToken cancelToken)
    {
        var context = new List<IPromptSection>(){
            PromptLibrary.Instructions(),
        };

        T actions = await _translator.TranslateAsync(input, context, null, cancelToken);

        var json = Microsoft.TypeChat.Json.Stringify(actions);

        return json;
    }
}