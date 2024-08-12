using Microsoft.TypeChat.Schema;

public class AppLog
{
    [Comment("The date and time when the log entry was created.")]
    public string CreatedOn { get; set; }
    [Comment("The URL of the request.")]
    public string URL { get; set; }
    [Comment("Logs and Stack Traces from the entry.")]
    public string[] Logs { get; set; }
    [Comment("The HTTP method used to make the request.")]
    public HttpMethod Method { get; set; }
    [Comment("The HTTP status code returned by the server.")]
    public int? StatusCode { get; set; }
    [Comment("The IP address of the client making the request.")]
    public string IPAddress { get; set; }
}
