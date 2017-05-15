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
    class Postoffice
    {
        private TokenBucket _bucket;
        HttpClient inst_client;

        public Postoffice()
        {
            _bucket = new TokenBucket(30, 60);
        }

        public JArray ReciveMessage(HttpClient client)
        {
            inst_client = client;
            _bucket.Afford();
            var response = inst_client.GetAsync("https://oauth.reddit.com/message/unread.json").GetAwaiter().GetResult();
            var responseData = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var content = JArray.Parse(JObject.Parse(responseData).SelectToken("data.children").ToString());
            Console.WriteLine("Took from mailbox");
            return content;
        }

        public void RemoveMessage(string re_name)
        {
            var message_id = new FormUrlEncodedContent(new Dictionary<string, string> { { "id", re_name } });
            _bucket.Afford();
            var redurl = "https://oauth.reddit.com/api/read_message/";
            var redmessage = inst_client.PostAsync(redurl, message_id).GetAwaiter().GetResult();
            Console.WriteLine("Removed from mailbox");
        }

        public void SendMessage(Dictionary<string, string> selectedtokens)
        {
            var message_content = new FormUrlEncodedContent(selectedtokens);
            _bucket.Afford();
            var authUrl = "https://oauth.reddit.com/api/compose/";
            var answer = inst_client.PostAsync(authUrl, message_content).GetAwaiter().GetResult();
            Console.WriteLine("Send to mailbox");
        }
    }
}