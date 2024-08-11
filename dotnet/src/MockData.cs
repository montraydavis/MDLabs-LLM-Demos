using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.SemanticKernel;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }
    public string Description { get; set; }
    public int PublicMetrics_FollowerCount { get; set; }
    public int PublicMetrics_FollowingCount { get; set; }
    public int PublicMetrics_TweetCount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class Tweet
{
    public string Id { get; set; }
    public string Text { get; set; }
    public string AuthorId { get; set; }
    public DateTime CreatedAt { get; set; }
    public int PublicMetrics_RetweetCount { get; set; }
    public int PublicMetrics_ReplyCount { get; set; }
    public int PublicMetrics_LikeCount { get; set; }
    public int PublicMetrics_QuoteCount { get; set; }
    public List<string> ReferencedTweets { get; set; } = new List<string>();
}

public class TwitterApiV2MockFunctionFilter : IFunctionInvocationFilter
{
    private IKernelBuilder kernel;

    public async Task OnFunctionInvocationAsync(FunctionInvocationContext context, Func<FunctionInvocationContext, Task> next)
    {
        await next(context);

        var pluginMethod = typeof(TwitterApiV2Mock)
            .GetMethod(context.Function.Name);

        if (pluginMethod == null)
            return;

        var routeAttribute = (RouteAttribute)pluginMethod
            .GetCustomAttributes(false)
            .FirstOrDefault(a => a.GetType() == typeof(RouteAttribute));

        var resultJson = JsonSerializer.Serialize(context.Result.GetValue<object>(), new JsonSerializerOptions()
        {
            WriteIndented = true
        });

        var result = $"""
            system:
            ----------------
            You are a specialized assistant for generating markdown documentation for HTTP responses.

            user:
            ----------------
            [Input]
            ```http
            {routeAttribute.Route}

            HTTP/1.1 200 OK
            Date: {DateTime.Now}
            Server: Apache/2.4.41 (Ubuntu)
            Content-Type: application/json

            {resultJson}
            ```
            [/Input]

            [INS]
            - Your output must strictly adhere to the instructions: no introductory text, no follow-up questions, no additional comments—only the markdown documentation as requested.
            - Assume the user does NOT need additional help.
            [/INS]
            """.Trim();

        var markdownFormattedKernelResponse = await kernel.Build().InvokePromptAsync(result, new());

        context.Result = new FunctionResult(context.Function, markdownFormattedKernelResponse.GetValue<string>());

    }

    public TwitterApiV2MockFunctionFilter(IKernelBuilder kernel)
    {
        this.kernel = kernel;
    }
}

public class TwitterApiV2Mock
{
    public static List<User> Users {get;}= new List<User>();
    public static List<Tweet> Tweets {get;}= new List<Tweet>();

    public static TwitterApiV2Mock CreateInstance()
    {
        return new TwitterApiV2Mock();
    }

    private TwitterApiV2Mock()
    {
        // Initialize with some mock data
        Users.Add(new User
        {
            Id = 1,
            Name = "John Doe",
            Username = "johndoe",
            Description = "Software developer | Coffee lover",
            PublicMetrics_FollowerCount = 1000,
            PublicMetrics_FollowingCount = 500,
            PublicMetrics_TweetCount = 5000,
            CreatedAt = DateTime.Now.AddYears(-5)
        });

        Tweets.Add(new Tweet
        {
            Id = "9876543210",
            Text = "Hello, Twitter! #FirstTweet",
            AuthorId = "1234567890",
            CreatedAt = DateTime.Now,
            PublicMetrics_RetweetCount = 5,
            PublicMetrics_ReplyCount = 2,
            PublicMetrics_LikeCount = 10,
            PublicMetrics_QuoteCount = 1
        });
    }

    // User endpoints

    [KernelFunction(nameof(GetUser))]
    [Route("2/users/{id}", HttpMethod.GET)]
    [Description("Get a user by their ID")]
    public static User GetUser(int id)
    {
        return Users.FirstOrDefault(u => u.Id == id);
    }

    [KernelFunction(nameof(GetUserByUsername))]
    [Route("2/users/by/username/{username}", HttpMethod.GET)]
    [Description("Get a user by their username")]
    public static User GetUserByUsername(string username)
    {
        return Users.FirstOrDefault(u => u.Username == username);
    }

    [KernelFunction(nameof(CreateUser))]
    [Route("2/users", HttpMethod.POST)]
    [Description("Create a new user")]
    public static User CreateUser(User user)
    {
        user.Id = Users.Max(u => u.Id) + 1;
        user.CreatedAt = DateTime.Now;
        Users.Add(user);
        return user;
    }

    [KernelFunction(nameof(UpdateUser))]
    [Route("2/users/{id}", HttpMethod.PUT)]
    [Description("Update an existing user")]
    public static User UpdateUser(int id, User updatedUser)
    {
        var user = GetUser(id);
        if (user != null)
        {
            user.Name = updatedUser.Name;
            user.Description = updatedUser.Description;
        }
        return user;
    }

    // Tweet endpoints

    [KernelFunction(nameof(GetTweet))]
    [Route("2/tweets/{id}", HttpMethod.GET)]
    [Description("Get a tweet by its ID")]
    public static Tweet GetTweet(string id)
    {
        return Tweets.FirstOrDefault(t => t.Id == id);
    }

    [KernelFunction(nameof(CreateTweet))]
    [Route("2/tweets", HttpMethod.POST)]
    [Description("Create a new tweet")]
    public static Tweet CreateTweet(Tweet tweet)
    {
        tweet.Id = Guid.NewGuid().ToString();
        tweet.CreatedAt = DateTime.Now;
        Tweets.Add(tweet);
        return tweet;
    }

    [KernelFunction(nameof(DeleteTweet))]
    [Route("2/tweets/{id}", HttpMethod.DELETE)]
    [Description("Delete a tweet by its ID")]
    public static bool DeleteTweet(string id)
    {
        return Tweets.RemoveAll(t => t.Id == id) > 0;
    }

    [KernelFunction(nameof(GetUserTweets))]
    [Route("2/users/{id}/tweets", HttpMethod.GET)]
    [Description("Get tweets by a specific user ID")]
    public static List<Tweet> GetUserTweets(string id)
    {
        return Tweets.Where(t => t.AuthorId == id).ToList();
    }

    [KernelFunction(nameof(SearchRecentTweets))]
    [Route("2/tweets/search/recent", HttpMethod.GET)]
    [Description("Search for recent tweets based on a query")]
    public static List<Tweet> SearchRecentTweets(string query)
    {
        // This is a very simplified search. In reality, it would be much more complex.
        return Tweets.Where(t => t.Text.Contains(query)).ToList();
    }

    // Like functionality

    [KernelFunction(nameof(LikeTweet))]
    [Route("2/users/{id}/likes", HttpMethod.POST)]
    [Description("Like a tweet")]
    public static void LikeTweet(string userId, string tweetId)
    {
        var tweet = GetTweet(tweetId);
        if (tweet != null)
        {
            tweet.PublicMetrics_LikeCount++;
        }
    }

    [KernelFunction(nameof(UnlikeTweet))]
    [Route("2/users/{id}/likes/{tweetId}", HttpMethod.DELETE)]
    [Description("Unlike a previously liked tweet")]
    public static void UnlikeTweet(string userId, string tweetId)
    {
        var tweet = GetTweet(tweetId);
        if (tweet != null && tweet.PublicMetrics_LikeCount > 0)
        {
            tweet.PublicMetrics_LikeCount--;
        }
    }

    // Retweet functionality

    [KernelFunction(nameof(Retweet))]
    [Route("2/users/{id}/retweets", HttpMethod.POST)]
    [Description("Retweet a tweet")]
    public static void Retweet(string userId, string tweetId)
    {
        var tweet = GetTweet(tweetId);
        if (tweet != null)
        {
            tweet.PublicMetrics_RetweetCount++;
        }
    }

    [KernelFunction(nameof(UndoRetweet))]
    [Route("2/users/{id}/retweets/{tweetId}", HttpMethod.DELETE)]
    [Description("Undo a retweet on a tweet")]
    public static void UndoRetweet(string userId, string tweetId)
    {
        var tweet = GetTweet(tweetId);
        if (tweet != null && tweet.PublicMetrics_RetweetCount > 0)
        {
            tweet.PublicMetrics_RetweetCount--;
        }
    }
}