using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Aethebot.Worker
{
    public class WorkerService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //_logger.LogDebug($"{nameof(WorkerService)} is starting.");

            //await _receiver.Start();

            stoppingToken.Register(() => Stop());
        }

        private Task Stop()
        {
            //_logger.LogDebug($"{nameof(EventReceiverBackgroundHost)} is stopping.");
            //return _receiver.Stop();
            return Task.CompletedTask;
        }
    }
}
