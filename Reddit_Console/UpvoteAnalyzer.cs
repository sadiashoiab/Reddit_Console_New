using static Reddit_Console.RedditINClient;

namespace Reddit_Console
{
    public class UpvoteAnalyzer
    {
        public static void FindMostUpvotedUsers(IEnumerable<RedditTopPost> posts)
        {
            var userUpvotes = posts
                .GroupBy(p => p.Author)  // Group posts by author
                .Select(g => new { Author = g.Key, TotalUpvotes = g.Sum(p => p.Upvotes) })  // Sum upvotes for each user
                .OrderByDescending(g => g.TotalUpvotes)  // Order by total upvotes
                .Take(5)  // Get top 5 users with most upvotes
                .ToList();

            Console.WriteLine("Top users with the most upvotes:");
            foreach (var user in userUpvotes)
            {
                Console.WriteLine($"User: {user.Author}, Total Upvotes: {user.TotalUpvotes}");
            }
        }
    }
}