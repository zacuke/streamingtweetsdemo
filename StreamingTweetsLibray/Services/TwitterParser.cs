using System.IO;
using System.Text;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using StreamingTweetsLibrary.Abstract;
using StreamingTweetsLibrary.Interfaces;
using StreamingTweetsLibrary.Models;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace StreamingTweetsLibrary.Services
{
    /// <summary>
    /// Reads next item from queue and run json deserializer
    /// </summary>
    public class TwitterParser : BackgroundHostedService
    {
        private readonly IStorage _twitterStorage;
        private readonly ILogger<TimerHostedService> _logger;

        public TwitterParser(IStorage twitterStorage, ILogger<TimerHostedService> logger) : base(logger)
        {
            _twitterStorage = twitterStorage;
            _logger = logger;
        }

        protected override async Task Execute(CancellationToken stoppingToken)
        {
            _logger.LogTrace("Execute");
            while (true)
            {
                try
                {
                    if (_twitterStorage.IncomingTweets.TryDequeue(out var next))
                    {
                        var stream = new MemoryStream(Encoding.UTF8.GetBytes(next));
                        var parsedTweet = await JsonSerializer.DeserializeAsync<SingleTweetWrapper>(stream);

                        if (parsedTweet is null)
                            throw new Exception("Unable to deserialize data from Twitter");

                        _twitterStorage.MostRecentTweet = parsedTweet.data.text;

                        _twitterStorage.IncrementTotalCount();

                    }
                    else
                    {
                        //wait for more strings
                        await Task.Delay(1);
                    }
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, "Error during TwitterParser process");

                    //todo replace throw with handling
                    throw;
                }
            }
        }
    }
}
