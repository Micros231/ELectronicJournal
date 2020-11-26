using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ElectronicJournal.Application.Navigation
{
    public class NavigationManagerHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly INavigationManager _manager;
        public NavigationManagerHostedService(
            IServiceProvider serviceProvider,
            INavigationManager manager)
        {
            _serviceProvider = serviceProvider;
            _manager = manager;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            NavigationProviderContext navigation = new NavigationProviderContext(_manager);
            using (var scope = _serviceProvider.CreateScope())
            {
                var provider = scope.ServiceProvider.GetRequiredService<NavigationProvider>();
                provider.SetNavigation(navigation);
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
