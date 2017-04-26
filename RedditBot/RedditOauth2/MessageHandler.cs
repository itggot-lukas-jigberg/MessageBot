using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditOauth2
{
    class MessageHandler
    {

        public string message, subject, author, id;

        public MessageHandler(string re_message, string re_subject, string re_author, string re_id)
        {
            message = re_message;
            subject = re_subject;
            author = re_author;
            id = re_id;

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

        }
    }
}
