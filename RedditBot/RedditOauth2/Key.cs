using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RedditOauth2
{
    class InvalidAppVariable : Exception
    {
        public InvalidAppVariable(string message) : base(message)
        {

        }
    }

    class Key
    {

        //instance variables
        private static HttpClient _client;
        
        public Key(HttpClient client)
        {
          _client = client;
        }

        public static void runouath()
        {
            // Fill out these variables in App.config

            var clientId = ConfigurationManager.AppSettings["clientId"];
            var clientSecret = ConfigurationManager.AppSettings["clientSecret"];
            var redditUsername = ConfigurationManager.AppSettings["username"];
            var redditPassword = ConfigurationManager.AppSettings["password"];
            var clientVersion = "1.0";

            var authenticationArray = Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}");
            var encodedAuthenticationString = Convert.ToBase64String(authenticationArray);
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", encodedAuthenticationString);
            _client.DefaultRequestHeaders.Add("User-Agent", $"ChangeMeClient /v{ clientVersion} by { redditUsername}");
            var formData = new Dictionary<string, string>
            {
                { "grant_type", "password" },
                { "username", redditUsername },
                { "password", redditPassword }
            };

            var encodedFormData = new FormUrlEncodedContent(formData);
            var authUrl = "https://www.reddit.com/api/v1/access_token";
            var response = _client.PostAsync(authUrl, encodedFormData).GetAwaiter().GetResult();


            // Response Code
            Console.WriteLine(response.StatusCode);

            // Actual Token

            var responseData = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var accessToken = JObject.Parse(responseData).SelectToken("access_token");

            try
            {
                if (accessToken == null)
                {
                    throw new InvalidAppVariable("Appconfig variables are invalid");
                }
            }
            catch (InvalidAppVariable ex)
            {
                Console.WriteLine($"{ex.Message}");
            }

            var accessTokenString = accessToken.ToString();

            // Update AuthorizationHeader
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", accessTokenString);
            _client.GetAsync("https://oauth.reddit.com/api/v1/me").GetAwaiter().GetResult();

            responseData =
            response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

        }


    }
}
