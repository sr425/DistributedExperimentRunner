using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Management.ServiceBus.Fluent;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TaskScheduler.Services;

namespace TaskScheduler
{
    public class WorkpackageQueuerService : BackgroundService
    {
        IServiceScopeFactory _scopeFactory;
        //IPersistenceProvider _persistenceProvider;
        ManagementClient _managemenetClient;
        QueueClient _queueClient;
        public int ActiveTargetQueueLength = 10;

        public WorkpackageQueuerService(IServiceScopeFactory scopeFactory, IConfiguration config) : base() // IPersistenceProvider persistenceProvider) : base()
        {
            _managemenetClient = new ManagementClient("Endpoint=sb://optimizersb.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=Gd9g623OqHPMyTmE/lqPhBHTHV08MSP692SAoNXQfmw=");
            _queueClient = new QueueClient("Endpoint=sb://optimizersb.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=Gd9g623OqHPMyTmE/lqPhBHTHV08MSP692SAoNXQfmw=", "tasks");
            _scopeFactory = scopeFactory;
            ActiveTargetQueueLength = config["ActiveTargetQueueLength"] == null ? 10 : int.Parse(config["ActiveTargetQueueLength"]);
        }

        private async Task ManageTasks()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var _persistenceProvider = scope.ServiceProvider.GetRequiredService<IPersistenceProvider>();
                Console.WriteLine("Starting task managment");
                var bufferSize = await _persistenceProvider.GetPackageNumberAsync();
                Console.WriteLine($"Found {bufferSize} elements in db storage");
                if (bufferSize == 0)
                {
                    return;
                }

                var queueInfo = await _managemenetClient.GetQueueRuntimeInfoAsync("tasks");
                var messageCount = queueInfo.MessageCount;
                if (messageCount >= ActiveTargetQueueLength)
                {
                    return;
                }

                int count = (int)Math.Min(bufferSize, ActiveTargetQueueLength - messageCount);

                var packages = await _persistenceProvider.GetPackages(count);
                Console.WriteLine($"Found {packages.Count()} messages to send to SB");
                var messages = packages.Select(pkg =>
               {
                   return new Message(Encoding.UTF8.GetBytes(pkg.JsonContent));
               }).ToList();
                await _queueClient.SendAsync(messages);

                await _persistenceProvider.RemovePackagesAsync(packages);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register((() => Console.WriteLine("Exit")));

            Console.WriteLine("Started scheduler background service");
            while (!stoppingToken.IsCancellationRequested)
            {
                await ManageTasks();

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }

            await _queueClient.CloseAsync();
            await _managemenetClient.CloseAsync();
        }
    }
}