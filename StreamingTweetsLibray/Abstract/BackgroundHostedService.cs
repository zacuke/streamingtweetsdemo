using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace StreamingTweetsLibrary.Abstract
{
    public abstract class BackgroundHostedService : IHostedService, IDisposable
    {
        private Task _executingTask = default!;
        private readonly CancellationTokenSource _stoppingCts = new CancellationTokenSource();
        private readonly ILogger _logger;

        protected BackgroundHostedService(ILogger logger)
        {
            _logger = logger;
        }

        protected abstract Task Execute(CancellationToken stoppingToken);

        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogTrace("Starting");
            // Store the task we're executing
            _executingTask = Execute(_stoppingCts.Token);

            // If the task is completed then return it, this will bubble cancellation and failure to the caller
            if (_executingTask.IsCompleted)
            {
                return _executingTask;
            }

            // Otherwise it's running
            return Task.CompletedTask;
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            // Stop called without start
            if (_executingTask == null)
            {
                return;
            }
            try
            {
                // Signal cancellation to the executing method
                _stoppingCts.Cancel();
            }
            finally
            {
                // Wait until the task completes or the stop token triggers
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }
        }

        public virtual void Dispose()
        {
            _stoppingCts.Cancel();
        }
    }
}
