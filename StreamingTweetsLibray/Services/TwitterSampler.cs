using Microsoft.Extensions.Logging;
using StreamingTweetsLibrary.Abstract;
using StreamingTweetsLibrary.Interfaces;

namespace StreamingTweetsLibrary.Services
{
    /// <summary>
    /// Calculates statistics periodically
    /// </summary>
    public class TwitterSampler : TimerHostedService
    {
        private readonly IStorage _storage;


        protected override int IntervalSeconds { get; set; } = 5;

        public TwitterSampler(IStorage storage, ILogger<TimerHostedService> logger) : base(logger)
        {
            _storage = storage;
        }


        protected override void Execute(object? state)
        {

            var numberOfMinutes = (DateTime.Now - _storage.AvgTweetsStart).TotalMinutes;
            if (numberOfMinutes > 0)
            {
                _storage.AvgTweetsPerMin = (long)(_storage.TotalCount / numberOfMinutes);
            }
        }
    }
}
