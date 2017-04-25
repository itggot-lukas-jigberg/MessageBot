using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace RedditOauth2
{
    class TokenBucket
    {
        public int currentTokens, capacity, interval;
        public DateTime lastRefreshed;

        public TokenBucket(int tokencap, int interval)
        {
            this.capacity = tokencap;
            this.currentTokens = capacity;
            this.interval = interval;
            this.lastRefreshed = DateTime.Now;
            Console.WriteLine($"{currentTokens}");
        }

            // kör denna om du ska kolla om lookout for fortsätta

        public void afford()
        {
            Refill();
            if (currentTokens - 1 < 0)
            {
                Thread.Sleep((interval*1000) - Convert.ToInt32(DateTime.Now.Subtract(lastRefreshed).TotalSeconds));
                Refill();
            }
            else
            {
                currentTokens -= 1;
            }
            Console.WriteLine($"{currentTokens}");

        } 

        public bool Refill()
        {
            if (DateTime.Now.Subtract(lastRefreshed).TotalSeconds >= interval)
            {
                currentTokens = capacity;
                lastRefreshed = DateTime.Now;
                return true;
            }
            return false;
        }
    }
}
