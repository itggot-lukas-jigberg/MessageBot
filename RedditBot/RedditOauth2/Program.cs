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
    class Program
    {
        public static void Main(string[] args)
        {
            HttpClient _client;
            JArray content;
            List<Dictionary<string, string>> selectedtokenlist;

            _client = new HttpClient();
            Key key = new Key(_client);
            Key.runouath();
            Postoffice bot = new Postoffice();
            Parser translator = new Parser();

            while (true)
            {
                content = bot.ReciveMessage(_client);

                if (content.Count == 0)
                {
                    Console.WriteLine("sleeping 5sec cause mailbox empty");
                    Thread.Sleep(5000);
                }
                else
                {
                    selectedtokenlist = translator.Parse(content);
                    foreach (var selectedtoken in selectedtokenlist)
                    {
                        bot.SendMessage(selectedtoken);
                        bot.RemoveMessage(selectedtoken["id"]);
                    }
                    
                }

            };

        }
    }
}

