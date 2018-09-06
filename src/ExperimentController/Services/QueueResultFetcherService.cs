using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ExperimentController.Model;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace ExperimentController.Services
{
    public class QueueResultFetcherService : BackgroundService
    {
        private string _resultQueueConnectionString;
        private IServiceScopeFactory _scopeFactory;

        private IConfiguration _configuration;

        public QueueResultFetcherService(IConfiguration configuration, IServiceScopeFactory scopeFactory) : base()
        {
            _resultQueueConnectionString = configuration["resultQueueConnectionstring"];
            _scopeFactory = scopeFactory;
            _configuration = configuration;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                Console.WriteLine(_resultQueueConnectionString);
                var messageReceiver = new MessageReceiver(_resultQueueConnectionString, "results", ReceiveMode.PeekLock);
                var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var resultHandler = new ResultHandlerService(_context, _configuration);

                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var msg = await messageReceiver.ReceiveAsync();
                        if (msg == null) { continue; }
                        string content = Encoding.UTF8.GetString(msg.Body);
                        var result = JsonConvert.DeserializeObject<ClientResult>(content);

                        var success = await resultHandler.HandleResultAsync(result);

                        if (success)
                        {
                            await messageReceiver.CompleteAsync(msg.SystemProperties.LockToken);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Received exception: " + e.Message);
                    }
                }
            }
        }
    }
}