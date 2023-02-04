using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StreamingTweetsLibrary.Abstract;
using StreamingTweetsLibrary.Interfaces;
using System.Collections.Concurrent;
using System.Text;

namespace StreamingTweetsLibrary.Services
{
    /// <summary>
    /// Reads http stream from twitter and puts whole json into queue
    /// </summary>
    public class TwitterReader : BackgroundHostedService
    {

        private readonly IConfiguration _configuration;
        private readonly IStorage _storage;
        private readonly ILogger _logger;
        public TwitterReader(IConfiguration configuration, IStorage storage, ILogger<TwitterReader> logger) : base(logger)
        {
            _configuration = configuration;
            _storage = storage;
            _logger = logger;
        }

        protected override async Task Execute(CancellationToken stoppingToken)
        {

            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.twitter.com/2/tweets/sample/stream");

            request.Headers.Add("Authorization", $"Bearer {_configuration["StreamingTweetsDemo:BearerToken"]}");

            using HttpClient httpClient = new();

            httpClient.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
            using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            using var stream = await response.Content.ReadAsStreamAsync();
            using var streamReader = new StreamReader(stream);

            string? theLine;

            while ((theLine = await streamReader.ReadLineAsync()) != null)
            {
                try 
                { 
                    if (theLine.Length > 0)
                    {
                        _storage.BeginStats();
                        _storage.IncomingTweets.Enqueue(theLine);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during TwitterReader process");

                    //todo replace throw with handling
                    throw;
                }

            }

            //to make more robust, you could put this on a timer instead so it retries
            //but be sure to add backoff
        }
    }
}
