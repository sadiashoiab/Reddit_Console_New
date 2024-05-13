using Reddit_Console;
using Serilog;
using System.Configuration;
using System.Diagnostics;

public class Program
{

    public static async Task Main(string[] args)
    {
        string strKey = ConfigurationManager.AppSettings["key"].ToString();
        var clientId = EncryptDecryptUtils.AesDecrypt(ConfigurationManager.AppSettings["clientId"].ToString(), strKey);
        var clientSecret = EncryptDecryptUtils.AesDecrypt(ConfigurationManager.AppSettings["clientSecret"].ToString(), strKey);
        var username = EncryptDecryptUtils.AesDecrypt(ConfigurationManager.AppSettings["username"].ToString(), strKey);
        var password = EncryptDecryptUtils.AesDecrypt(ConfigurationManager.AppSettings["password"].ToString(), strKey);
        
        // Get OAuth access token
        using (var log = new LoggerConfiguration().WriteTo.Console().CreateLogger())
        {
            log.Information("Get OAuth access token RedditOAuth"); 
        } 

        var redditOAuth = new RedditOAuth(clientId, clientSecret, username, password);
        var accessToken = await redditOAuth.GetAccessTokenAsync();

     
        Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));
        Serilog.Debugging.SelfLog.Enable(Console.Error);

        // Fetch top posts from a specified subreddit
        var redditClient = new RedditINClient(accessToken);
        using (var log = new LoggerConfiguration().WriteTo.Console().CreateLogger())
        {
            log.Information("TopPost Information -- with python ");
            log.Warning("TopPost with data limit 5 only");
        } 
        await redditClient.GetTopPostsAsync("python", 5); 
        
        // Change "python" to any subreddit
         // Subreddits to fetch data from
        var subreddits = new List<string> { "python", "programming", "dotnet" };

        using (var log = new LoggerConfiguration().WriteTo.Console().CreateLogger())
        {
            log.Information("TopPost Subreddits Information -- with python,programming,dotnet Subreddits  ");
            log.Warning("TopPost Subreddits with data limit 5 only");
        }
        var topPosts = await redditClient.GetTopPostsFromSubredditsAsync(subreddits, 5);

        // Display the fetched posts
        foreach (var post in topPosts)
        {
            Console.WriteLine($"Title: {post.Title}, Upvotes: {post.Upvotes}, Url: {post.Url}");
        }
        using (var log = new LoggerConfiguration().WriteTo.Console().CreateLogger())
        {
            log.Information("TopPost Subreddits Information -- with programming Subreddits  ");
            log.Warning("TopPost Subreddits with data limit 10");
        }
        var topPosts1 = await redditClient.GetTopPostsAsync1("programming", 10);  // Change "programming" to any subreddit

        using (var log = new LoggerConfiguration().WriteTo.Console().CreateLogger())
        {
            log.Information("Find users with the most upvotes "); 
        }
        // Find users with the most upvotes
        UpvoteAnalyzer.FindMostUpvotedUsers(topPosts1);

        // Fetch Worldnews posts from a specified subreddit 
        using (var log = new LoggerConfiguration().WriteTo.Console().CreateLogger())
        {
            log.Information("Get Worldnews Information -- with worldnews ");
            log.Warning("Get Worldnews with data limit 10 only");
        }
        await redditClient.GetWorldnewsPostAsync("worldnews", 10);


        RedditStatsCollector statsCollector = new RedditStatsCollector(clientId, clientSecret, accessToken, accessToken, "python");

        // Specify the interval (in seconds) between each request
        int requestIntervalSeconds = 60; // e.g., request every 60 seconds

        // Start collecting statistics
        //To acquire near real time statistics from Reddit, you will need to continuously request data from Reddit's rest APIs.
        //Reddit implements rate limiting and provides details regarding rate limit used, rate limit remaining, and rate limit reset period via response headers.
        //Your application should use these values to control throughput in an even and consistent manner while utilizing a high percentage of the available request rate.
        await statsCollector.CollectStats(requestIntervalSeconds);

       

    }
}
