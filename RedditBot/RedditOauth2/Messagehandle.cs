using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditOauth2
{
    class Messagehandle
    {

        public string df_message, df_subject, df_author, df_id;

        public Messagehandle(string re_message, string re_subject, string re_author)
        {
            df_message = re_message;
            df_subject = re_subject;
            df_author = re_author;

            Console.WriteLine(df_subject);
            Console.WriteLine(df_message);

            if (df_subject != "IWish")
            {
                df_message = "If you want me to do your bidding the subject of your letter has to be 'IWish' // Vilhelm";
            }
            else
            {
                df_message = "https://www.youtube.com/watch?v=g-sgw9bPV4A";
                //df_message = "I will never forgive you";
            }

            df_subject = $"{df_author} wish";

        }
    }
}
