using Reddit;
using Post = Reddit.Controllers.Post;

namespace Reddit_Console
{


    #region "Single Responsibility Principle (SRP)"

    /// <summary>
    /// Single Responsibility Principle (SRP) Create a separate class or module responsible for handling API data storage, ensuring each module has only one reason to change.

    /// </summary>
    public class RedditApiClient
    {
        private readonly RedditClient _reddit;

        public RedditApiClient(string clientId, string clientSecret, string username, string password)
        {
            _reddit = new RedditClient(clientId, clientSecret, username, password);
        }

        public IEnumerable<Post> GetTopPosts(string subredditName, int count)
        {
            var subreddit = _reddit.Subreddit(subredditName);
            return (IEnumerable<Post>)subreddit.Posts.Top.ToList();
        }
    }
    // Class responsible for displaying posts
    public class PostRenderer
    {
        public void RenderPosts(IEnumerable<Post> posts)
        {
            foreach (var post in posts)
            {
                Console.WriteLine($"Title: {post.Title}");
                Console.WriteLine($"Author: {post.Author}");
                Console.WriteLine($"Score: {post.Score}"); 
            }
        }
    }

    #endregion


    #region "Open/Closed Principle (OCP)"
    // Interface for post renderer Design the database storage class to be extensible and modifiable without modifying
    // its source code, using techniques like dependency injection or polymorphism.

    public interface IPostRenderer
    {
        void Render(Post post);
    }



    // Implementation of Console post renderer
    public class ConsolePostRenderer : IPostRenderer
    {
        public void Render(Post post)
        {
            Console.WriteLine($"Title: {post.Title}");
            Console.WriteLine($"Author: {post.Author}");
            Console.WriteLine($"Score: {post.Score}"); 
        }
    }

    // Implementation of HTML post renderer
    public class HtmlPostRenderer : IPostRenderer
    {
        public void Render(Post post)
        {
            // Render post in HTML format
        }
    }

    // Implementation of JSON post renderer
    public class JsonPostRenderer : IPostRenderer
    {
        public void Render(Post post)
        {
            // Render post in JSON format
        }
    }

    #endregion

    #region "Interface Segregation Principle(ISP)"


    /// <summary>
    /// Interface Segregation Principle(ISP)  : Define a specific interface for the database storage class, tailoring it to the needs of the API data storage, 
    /// and avoiding unnecessary dependencies. 
    /// </summary> 
    public interface IRedditApiClientISP
    {
        IEnumerable<Post> GetTopPosts(string subredditName, int count);
    }
    // Implementation of Reddit API client
    public class RedditApiClientISP : IRedditApiClientISP
    {
        private readonly RedditAPI _reddit;

        public RedditApiClientISP(string clientId, string clientSecret, string username, string password)
        {
            _reddit = new RedditAPI(clientId, clientSecret, username, password);
        }

        public IEnumerable<Post> GetTopPosts(string subredditName, int count)
        {
            var subreddit = _reddit.Subreddit(subredditName);
            var topPosts = subreddit.Posts.GetTop();
            return (IEnumerable<Post>)topPosts;
        }
    }
    // Interface for different types of post renderers
    public interface IPostRendererISP
    {
        void Render(Post post);
    }

    // Implementations of IPostRenderer for different formats
    public class ConsolePostRendererISP : IPostRendererISP
    {
        public void Render(Post post)
        {
            Console.WriteLine($"Title: {post.Title}");
            Console.WriteLine($"Author: {post.Author}");
            Console.WriteLine($"Score: {post.Score}");
        }
    }

    public class HtmlPostRendererISP : IPostRendererISP
    {
        public void Render(Post post)
        {
            // Render post in HTML format
        }
    }

    public class JsonPostRendererISP : IPostRendererISP
    {
        public void Render(Post post)
        {
            // Render post in JSON format
        }
    }

    #endregion
}