using System.Collections.Concurrent;

namespace StreamingTweetsLibrary.Interfaces
{
    public interface IStorage
    {
        //consider only using primitives in storage interfaces
        //might create IQueue interface for non-primitives  
        
        
        ConcurrentQueue<string> IncomingTweets { get; set; }
        string MostRecentTweet { get; set; }

        long TotalCount { get; set; }
        long AvgTweetsPerMin { get; set; }

        DateTime AvgTweetsStart { get; set; }
        void IncrementTotalCount();
        void BeginStats();
    }
}
