using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RedditOauth2
{
    class RedditBot
    {

        private HttpClient _client;
        private TokenBucket _bucket;

        public RedditBot()
        {
            _client = new HttpClient();
            Key key = new Key(_client);

            _bucket = new TokenBucket(30, 60);
            Key.runouath();

            while (true) { 
                lookout();
            };
        }

        public void lookout()
        {
            _bucket.afford();
            var response = _client.GetAsync("https://oauth.reddit.com/message/unread.json").GetAwaiter().GetResult();
            var responseData = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            Console.WriteLine(responseData);
            var content = JArray.Parse(JObject.Parse(responseData).SelectToken("data.children").ToString());

            if (content.Count() == 0)
            {
                Console.WriteLine("jag går och lägger mig");
                Thread.Sleep(10000);
            }
            else
            {
                foreach (var item in content)
                {
                    var message = item.SelectToken("data.body").Value<string>();
                    var subject = item.SelectToken("data.subject").Value<string>();
                    var author = item.SelectToken("data.author").Value<string>();
                    var id = item.SelectToken("data.id").Value<string>();

                    Console.WriteLine(subject);
                    Console.WriteLine(message);

                    if (subject != "IWish")
                    {
                        message = "If you want me to do your bidding the subject of your letter has to be 'IWish' // Vilhelm";
                    }
                    else
                    {
                        message = "https://www.youtube.com/watch?v=eY52Zsg-KVI";
                    }

                    subject = $"{author} wish";
                    var selectedtokens = new Dictionary<string, string>
                    {
                        { "text", message },
                        { "subject", subject },
                        { "to", author },
                        { "api_type", "json" }
                    };

                    var message_id = new FormUrlEncodedContent(new Dictionary<string, string> { { "id", "t4_" + id } });
                    _bucket.afford();
                    var redurl = "https://oauth.reddit.com/api/read_message/";
                    var redmessage = _client.PostAsync(redurl, message_id).GetAwaiter().GetResult();

                    var message_content = new FormUrlEncodedContent(selectedtokens);
                    _bucket.afford();
                    var authUrl = "https://oauth.reddit.com/api/compose/";
                    var answer = _client.PostAsync(authUrl, message_content).GetAwaiter().GetResult();
                }
            }

        }

    }
}