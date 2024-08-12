using Microsoft.TypeChat;
using Microsoft.TypeChat.Schema;
using System.CommandLine.Parsing;
using System.Text;

public abstract class ConsoleApp : IInputHandler
{
    List<string> _stopStrings;

    private string ConsolePrompt { get; set; } = ">";

    public IList<string> StopStrings => _stopStrings;

    private string CommentPrefix { get; set; } = "#";

    private string CommandPrefix { get; set; } = "@";

    protected ConsoleApp()
    {
        Console.OutputEncoding = Encoding.UTF8;
        _stopStrings = new List<string>(2) { "quit", "exit" };
    }

    public async Task<string> RunBatchAsync(string batchFilePath, string userPrompt, CancellationToken cancelToken = default)
    {
        using var reader = new StreamReader(batchFilePath);
        string line = null;
        var res = new List<string>();

        while (!cancelToken.IsCancellationRequested &&
              (line = reader.ReadLine()) is not null)
        {
            line = line.Trim();
            if (line.Length == 0 ||
               line.StartsWith(CommentPrefix))
            {
                continue;
            }

            Console.WriteLine(ConsolePrompt);
            Console.WriteLine();
            Console.WriteLine(line);

            var evaluatedInput = await EvalInputAsync(line, cancelToken).ConfigureAwait(false);

            res.Add(evaluatedInput);
        }

        return $"[{string.Join(",\n", res)}]";
    }

    public async Task<string> RunAsync(string consolePrompt, string userPrompt, string? inputFilePath = null)
    {
        ConsolePrompt = consolePrompt;
        await InitApp();
        if (string.IsNullOrEmpty(inputFilePath))
        {
            return await RunAsync(userPrompt);
        }
        else
        {
            return await RunBatchAsync(inputFilePath, userPrompt);
        }
    }

    public async Task<string> RunAsync(string userPrompt, CancellationToken cancelToken = default)
    {
        Console.WriteLine(ConsolePrompt);
        Console.WriteLine();
        var evaluatedInput = await EvalInputAsync(userPrompt, cancelToken).ConfigureAwait(false);

        return evaluatedInput;
    }

    async Task<string> EvalInputAsync(string input, CancellationToken cancelToken)
    {
        try
        {
            if (input.StartsWith(CommandPrefix))
            {
                return await EvalCommandAsync(input, cancelToken).ConfigureAwait(false);
            }
            return await EvalLineAsync(input, cancelToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            OnException(input, ex);
        }

        throw new Exception("Failed to evaluate input.");
    }

    async Task<string> EvalLineAsync(string input, CancellationToken cancelToken)
    {
        if (IsStop(input))
        {
            return null;
        }

        return await ProcessInputAsync(input, cancelToken).ConfigureAwait(false);
    }

    async Task<string> EvalCommandAsync(string input, CancellationToken cancelToken)
    {
        List<string> parts = CommandLineStringSplitter.Instance.Split(input).ToList();
        if (parts.Count(p => !string.IsNullOrWhiteSpace(p)) == 0)
        {
            return null;
        }

        string cmd = parts[0].Substring(CommandPrefix.Length);
        if (!string.IsNullOrEmpty(cmd))
        {
            if (IsStop(cmd))
            {
                return null;
            }
            parts.RemoveAt(0);
            await ProcessCommandAsync(cmd, parts).ConfigureAwait(false);
        }

        return null;
    }

    bool IsStop(string line)
    {
        if (line is null)
        {
            return true;
        }
        return _stopStrings.Contains(line, StringComparer.OrdinalIgnoreCase);
    }

    public async Task WriteLineAsync(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            Console.Out.WriteLine();
        }
        else
        {
            await Console.Out.WriteLineAsync(value).ConfigureAwait(false);
        }
    }

    public abstract Task<string> ProcessInputAsync(string input, CancellationToken cancelToken);
    public virtual Task ProcessCommandAsync(string cmd, IList<string> args)
    {
        switch (cmd)
        {
            default:
                Console.WriteLine($"Command {cmd} not handled");
                break;

            case "clear":
                Console.Clear();
                break;
        }
        return Task.CompletedTask;
    }

    protected virtual Task InitApp() => Task.CompletedTask;

    protected void SubscribeAllEvents<T>(JsonTranslator<T> translator)
    {
        translator.SendingPrompt += this.OnSendingPrompt;
        translator.AttemptingRepair += this.OnAttemptingRepairs;
        translator.CompletionReceived += this.OnCompletionReceived;
    }

    protected virtual void OnException(string input, Exception ex)
    {
        Console.WriteLine("## Could not process request");
        if (ex is TypeChatException tex)
        {
            Console.WriteLine($"## TypeChatException");
            Console.WriteLine(ex.ToString());
        }
        else
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine();
        }
    }

    protected void OnSendingPrompt(Prompt prompt)
    {
        Console.WriteLine("### PROMPT");
        Console.WriteLine(prompt.ToString(true));
        Console.WriteLine("###");
    }

    protected void OnCompletionReceived(string value)
    {
        Console.WriteLine("### COMPLETION");
        Console.WriteLine(value);
        Console.WriteLine("###");
    }

    protected void OnAttemptingRepairs(string value)
    {
        Console.WriteLine("### REPAIRING ERROR:");
        Console.WriteLine(value);
        Console.WriteLine("###");
    }

    protected static void WriteError(Exception ex)
    {
        Console.WriteLine("### EXCEPTION:");
        Console.WriteLine(ex.Message);
        Console.WriteLine("###");
    }

    public static void WriteLines(IEnumerable<string> items)
    {
        foreach (string item in items)
        {
            Console.WriteLine(item);
        }
    }
}