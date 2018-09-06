using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Extensions.Configuration;

namespace ExperimentClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddCommandLine(args);

            var config = builder.Build();

            var nrCpus = getNumberOfCores();
            if (config["NrCpus"] != null)
            {
                if (int.TryParse(config["NrCpus"], out int cpus) && cpus > 0)
                {
                    nrCpus = cpus;
                }
            }
            Console.WriteLine($"Using {nrCpus} CPUs");

            var settingQueueConfig = config.GetSection("QueueConfig");
            var queueConfig = new QueueConfig()
            {
                TaskQueueConnectionString = settingQueueConfig["TaskQueueConnectionString"],
                TaskQueueName = settingQueueConfig["TaskQueueName"],
                ResultQueueConnectionString = settingQueueConfig["ResultQueueConnectionString"],
                ResultQueueName = settingQueueConfig["ResultQueueName"]
            };

            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                Console.WriteLine("Aborting");
                cts.Cancel();
                return;
            };

            var threadList = new List<Thread>();
            Console.WriteLine($"Using dataset path: {config["DatasetPath"]}");
            Console.WriteLine($"Using queue config: {queueConfig.ToString()}");
            for (int i = 0; i < nrCpus; i++)
            {
                threadList.Add(new Thread(() => ManualExperimentRunner.RunClient(cts.Token, queueConfig, config["DatasetPath"]).Wait()));
            }
            threadList.StartAll();
            threadList.WaitAll();

            Console.WriteLine("Exiting");

        }

        private static int getNumberOfCores()
        {
            return Environment.ProcessorCount;
        }

    }
}