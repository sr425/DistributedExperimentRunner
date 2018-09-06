using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;

namespace ExperimentClient
{
    public class ManualExperimentRunner
    {
        public static async Task RunClient(CancellationToken token, QueueConfig queueConfig, string datasetPath)
        {
            var runner = new ExperimentRunner();
            var client = new QueueClient(queueConfig.ResultQueueConnectionString, queueConfig.ResultQueueName);
            var receiver = new QueueClient(queueConfig.TaskQueueConnectionString, queueConfig.TaskQueueName);
            var messageReceiver = new MessageReceiver(queueConfig.TaskQueueConnectionString, queueConfig.TaskQueueName, ReceiveMode.PeekLock);
            runner.sendClient = client;
            runner.DatasetPath = datasetPath;

            while (!token.IsCancellationRequested)
            {
                try
                {
                    var message = await messageReceiver.ReceiveAsync();
                    if (message != null)
                    {
                        (var success,
                            var msg) = await runner.Runner(message, token);
                        if (msg != null)
                        {
                            await client.SendAsync(msg);
                        }
                        if (success)
                        {
                            await messageReceiver.CompleteAsync(message.SystemProperties.LockToken);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Idel");
                        await Task.Delay(500);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

    }
}