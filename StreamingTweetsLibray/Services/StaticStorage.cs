using StreamingTweetsLibrary.Interfaces;
using System.Collections.Concurrent;

namespace StreamingTweetsLibrary.Services
{
    public class StaticStorage : IStorage
    {
        public StaticStorage()
        {
        }

        // you could re-implement IStorage with persistence
        static ConcurrentQueue<string> _incomingTweets = new ConcurrentQueue<string>();
        static long _totalCount;
        static long _avgTweetsPerMin;
        static string _mostRecentTweet = "";
        static DateTime _avgTweetsStart = DateTime.MinValue;   
        public ConcurrentQueue<string> IncomingTweets { get => _incomingTweets; set => _incomingTweets = value; }
        public long TotalCount { get => _totalCount; set => _totalCount = value; }
        public long AvgTweetsPerMin { get => _avgTweetsPerMin; set => _avgTweetsPerMin = value; }
        public string MostRecentTweet { get => _mostRecentTweet; set => _mostRecentTweet = value; }
        public DateTime AvgTweetsStart { get => _avgTweetsStart; set => _avgTweetsStart = value; }


        //be cognizant of the round trips out to a DB
        //in a real implementation

        public void IncrementTotalCount()
        {
            _totalCount++;
        }
        public void BeginStats()
        {
            if (_avgTweetsStart == DateTime.MinValue)
            {
                _avgTweetsStart = DateTime.Now;
            }
        }
    }
}
