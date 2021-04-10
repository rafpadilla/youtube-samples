using MicroserviceSample.Services;
using MicroserviceSample.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceSample.HostedServices
{
    internal sealed class TimedHostedService : IHostedService, IDisposable
    {
        private Timer _timer;
        private int _executionCount = 0;
        private readonly ILogger<TimedHostedService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ProcessSettings _processSettings;

        public TimedHostedService(ILogger<TimedHostedService> logger, IServiceScopeFactory scopeFactory, IOptions<ProcessSettings> settings)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _processSettings = settings.Value;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (_processSettings.EnableTimer)
            {
                _logger.LogInformation("Timed Hosted Service running.");

                _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(_processSettings.TimerIntervalSecs));
            }
            else
            {
                _logger.LogError("Timer is disabled, check your configuration and restart service.");
            }

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var count = Interlocked.Increment(ref _executionCount);

            _logger.LogDebug("Timed Hosted Service is working. Count: {Count}", count);
            try
            {
                using var scope = _scopeFactory.CreateScope();

                var service = scope.ServiceProvider.GetService<IBackgroundTaskQueue>();
                var cloudflaseService = scope.ServiceProvider.GetService<ICloudflareService>();

                service.QueueBackgroundWorkItem(cancellationToken => cloudflaseService.CheckIpAndUpdateDDNSIfNeeded(cancellationToken));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error");
            }

            _logger.LogDebug("Timed Hosted Service is working. Waiting...");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
