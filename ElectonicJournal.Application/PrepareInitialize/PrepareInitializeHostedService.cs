using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ElectronicJournal.Application.PrepareInitialize
{
    public class PrepareInitializeHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        public PrepareInitializeHostedService(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var prepareInitializeService = scope.ServiceProvider.GetRequiredService<IPrepareInitializeAppService>();
                var isInit = await prepareInitializeService.IsInitialize();
                if (!isInit)
                {
                    await prepareInitializeService.Initialize();
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
