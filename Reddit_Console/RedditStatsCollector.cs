using System.Text.Json;
using Reddit;

namespace Reddit_Console
{
    public class RedditStatsCollector
    {
        private readonly RedditClient _redditClient;
        private readonly string _subredditName;
        private readonly string _accessToken;
        public RedditStatsCollector(string clientId, string clientSecret, string refreshToken, string accessToken, string subredditName)
        {
            _redditClient = new RedditClient(clientId, clientSecret, refreshToken, accessToken, "YourUserAgent");
            _subredditName = subredditName;
            _accessToken = accessToken;
        }

        public async Task CollectStats(int requestIntervalSeconds)
        { 
                // Make a request to fetch statistics
                var stats = await GetSubredditStats(); 
                // Process the statistics (e.g., display or save to a database)
                Console.WriteLine($"Statistics for r/{_subredditName} - Posts: {stats.PostCount}, Subscribers: {stats.SubscriberCount}"); 
                // Sleep for the specified interval before making the next request
                await Task.Delay(requestIntervalSeconds); 
        }

        private async Task<(int PostCount, int SubscriberCount)> GetSubredditStats()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_accessToken}");
                httpClient.DefaultRequestHeaders.Add("User-Agent", "MyRedditApp/0.1 by YOUR_USERNAME");


                var response = await httpClient.GetAsync($"https://oauth.reddit.com/r/{_subredditName}/about.json");

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {

                    var content = await response.Content.ReadAsStringAsync();
                    var jsonDocument = JsonDocument.Parse(content); 
                     
                    var postCount = 12;
                    var subscriberCount = 15;

                    return (postCount, subscriberCount);
                }
                else
                {
                    // Handle error response
                    Console.WriteLine($"Failed to fetch subreddit statistics: {response.StatusCode} - {response.ReasonPhrase}");
                    return (0, 0);
                }
            }
        }
    }
}