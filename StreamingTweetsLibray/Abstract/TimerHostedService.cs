using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace StreamingTweetsLibrary.Abstract
{
    public abstract class TimerHostedService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private Timer _timer = default!;
        protected abstract int IntervalSeconds { get; set; }

        public TimerHostedService(ILogger<TimerHostedService> logger)
        {
            _logger = logger;
        }

        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogTrace("Starting");

            _logger.LogInformation("Timed Background Service is starting.");

            _timer = new Timer(Execute, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(IntervalSeconds));

            return Task.CompletedTask;
        }

        protected abstract void Execute(object? state);


        public virtual Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }

}
