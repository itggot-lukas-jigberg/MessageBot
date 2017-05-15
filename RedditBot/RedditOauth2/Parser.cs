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
    class Parser
    {
        private Messagehandle _handler;
        HttpClient inst_client;

        public List < Dictionary <string, string>> Parse(JArray content)
        {

            var selectedtokenlist = new List<Dictionary<string, string>>();

            foreach (var item in content)
            {
                var re_message = item.SelectToken("data.body").Value<string>();
                var re_subject = item.SelectToken("data.subject").Value<string>();
                var re_author = item.SelectToken("data.author").Value<string>();
                var re_name = item.SelectToken("data.name").Value<string>();

                Console.WriteLine(re_name);

                _handler = new Messagehandle(re_message, re_subject, re_author);

                var selectedtokens = new Dictionary<string, string>
                {
                    { "text", _handler.df_message },
                    { "subject", _handler.df_subject },
                    { "to", _handler.df_author },
                    { "api_type", "json" },
                    { "id", re_name }
                };

                selectedtokenlist.Add(selectedtokens);
            }

            return selectedtokenlist;
            

        }
    }
}
