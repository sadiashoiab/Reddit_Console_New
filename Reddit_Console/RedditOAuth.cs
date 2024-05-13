namespace Reddit_Console
{
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public class RedditOAuth
    {
        private readonly string clientId;
        private readonly string clientSecret;
        private readonly string username;
        private readonly string password;

        public RedditOAuth(string clientId, string clientSecret, string username, string password)
        {
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            this.username = username;
            this.password = password;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            
            string strResponse = string.Empty; 
            using (var httpClient = new HttpClient())
            {
                // Set up basic authentication
                var authenticationString = $"{clientId}:{clientSecret}";
                var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.ASCII.GetBytes(authenticationString));

                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
                httpClient.DefaultRequestHeaders.Add("User-Agent", "MyRedditApp/0.1 by YOUR_USERNAME");

                var postData = new Dictionary<string, string>
                {
                    { "grant_type", "password" },
                    { "username", username },
                    { "password", password }
                };

                var content = new FormUrlEncodedContent(postData);
                var response = await httpClient.PostAsync("https://www.reddit.com/api/v1/access_token", content);
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Response Data: " + responseString);
                    var tokenData = JsonConvert.DeserializeObject<dynamic>(responseString);
                    strResponse = tokenData.access_token; ;
                }
                else
                {
                    Console.WriteLine("Error: " + responseString);
                }
            }
            return strResponse; 
        }
    }

}