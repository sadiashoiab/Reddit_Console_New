using System.Text.Json;
using Serilog;

namespace Reddit_Console
{

    public class RedditINClient
    {
        private readonly string accessToken;

        public RedditINClient(string accessToken)
        {
            this.accessToken = accessToken;
        }
        public async Task<List<RedditPost>> GetTopPostsFromSubredditsAsync(IEnumerable<string> subreddits, int limit = 5)
        {

            var allPosts = new List<RedditPost>();
            try
            {
               
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                    httpClient.DefaultRequestHeaders.Add("User-Agent", "MyRedditApp/0.1 by YOUR_USERNAME");

                    foreach (var subreddit in subreddits)
                    {
                        var response = await httpClient.GetAsync($"https://oauth.reddit.com/r/{subreddit}/top?limit={limit}");

                        if (!response.IsSuccessStatusCode)
                        {
                            Console.WriteLine($"Failed to fetch data from subreddit: {subreddit}");
                            continue;
                        }

                        var content = await response.Content.ReadAsStringAsync();
                        var jsonDocument = JsonDocument.Parse(content);

                        var posts = jsonDocument.RootElement.GetProperty("data").GetProperty("children");

                        foreach (var post in posts.EnumerateArray())
                        {
                            var title = post.GetProperty("data").GetProperty("title").GetString();
                            var upvotes = post.GetProperty("data").GetProperty("ups").GetInt32();
                            var url = post.GetProperty("data").GetProperty("url").GetString();

                            allPosts.Add(new RedditPost { Title = title, Upvotes = upvotes, Url = url });
                        }
                    }
                }
              
            }
            catch (Exception ex)
            { 
                Log.Logger = new LoggerConfiguration().WriteTo.Console(outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}").CreateLogger();
                throw ex;
            }
            return allPosts;
        }


        public class RedditPost
        {
            public string Title { get; set; }
            public int Upvotes { get; set; }
            public string Url { get; set; }
        }
        public class RedditTopPost
        {
            public string Title { get; set; }
            public int Upvotes { get; set; }
            public string Author { get; set; }
        }
        public async Task<List<RedditTopPost>> GetTopPostsAsync1(string subreddit, int limit = 5)
        {
            var topPosts = new List<RedditTopPost>();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                httpClient.DefaultRequestHeaders.Add("User-Agent", "MyRedditApp/0.1 by YOUR_USERNAME");

                var response = await httpClient.GetAsync($"https://oauth.reddit.com/r/{subreddit}/top?limit={limit}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Failed to fetch data from Reddit.");
                }

                var content = await response.Content.ReadAsStringAsync();
                var jsonDocument = JsonDocument.Parse(content);

                var posts = jsonDocument.RootElement.GetProperty("data").GetProperty("children");


                foreach (var post in posts.EnumerateArray())
                {
                    var postData = post.GetProperty("data");
                    var title = postData.GetProperty("title").GetString();
                    var upvotes = postData.GetProperty("ups").GetInt32();
                    var author = postData.GetProperty("author").GetString();

                    topPosts.Add(new RedditTopPost
                    {
                        Title = title,
                        Upvotes = upvotes,
                        Author = author
                    });
                }
            }
            return topPosts;
        }
        public async Task GetTopPostsAsync(string subreddit, int limit = 5)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                httpClient.DefaultRequestHeaders.Add("User-Agent", "MyRedditApp/0.1 by YOUR_USERNAME");


                var response = await httpClient.GetAsync($"https://oauth.reddit.com/r/{subreddit}/top?limit={limit}");
                var responseString = await response.Content.ReadAsStringAsync();
                var jsonDocument = JsonDocument.Parse(responseString);
                var posts = jsonDocument.RootElement.GetProperty("data").GetProperty("children");
                foreach (var post in posts.EnumerateArray())
                {
                    var title = post.GetProperty("data").GetProperty("title").GetString();
                    var upvotes = post.GetProperty("data").GetProperty("ups").GetInt32();

                    Console.WriteLine($"Title: {title}, Upvotes: {upvotes}");
                }
                // Sleep for the specified interval before making the next request
                await Task.Delay(60);
            }

        }
        public async Task GetWorldnewsPostAsync(string subreddit, int limit = 10)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                httpClient.DefaultRequestHeaders.Add("User-Agent", "MyRedditApp/0.1 by YOUR_USERNAME");


                var response = await httpClient.GetAsync($"https://oauth.reddit.com/r/{subreddit}/top?t=all&limit={limit}");
                var responseString = await response.Content.ReadAsStringAsync();
                var jsonDocument = JsonDocument.Parse(responseString);
                var posts = jsonDocument.RootElement.GetProperty("data").GetProperty("children");
                foreach (var post in posts.EnumerateArray())
                {
                    var title = post.GetProperty("data").GetProperty("title").GetString();
                    var upvotes = post.GetProperty("data").GetProperty("ups").GetInt32();

                    Console.WriteLine($"Title: {title}, Upvotes: {upvotes}");
                }
                // Sleep for the specified interval before making the next request
                await Task.Delay(60);
            }

        }
    }

}