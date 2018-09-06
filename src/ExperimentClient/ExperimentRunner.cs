using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ExperimentController.Model;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExperimentClient
{
    public class ExperimentRunner
    {
        public QueueClient sendClient;

        public string DatasetPath;

        public async Task<(bool successfull, Message msg)> Runner(Message message, CancellationToken token)
        {
            Console.WriteLine("Starting processing of task");
            string content = new string(Encoding.UTF8.GetChars(message.Body));

            (var resultString,
                var error) = await Process(content);

            var result = JsonConvert.DeserializeObject<ClientResult>(resultString);
            if (!string.IsNullOrWhiteSpace(error))
            {
                if (string.IsNullOrWhiteSpace(result.ErrorMessage))
                {
                    result.ErrorMessage = "";
                }
                result.ErrorMessage += $" Error from output: {error}";
                result.Failed = true;
            }

            var resultMsg = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(result)));
            if (string.IsNullOrWhiteSpace(error) && !result.Failed)
            {
                return (true, resultMsg);
            }
            return (false, resultMsg);
        }

        public async Task<(string results, string error)> Process(string jsonContent)
        {
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            var tempFile = Path.GetTempFileName();
            Console.WriteLine(tempFile);
            using (var sw = new StreamWriter(new FileStream(tempFile, FileMode.Truncate)))
            {
                await sw.WriteAsync(jsonContent);
            }
            dynamic workpackage = JObject.Parse(jsonContent);

            var process = new Process();
            process.StartInfo.FileName = isWindows ? "python" : "python3";
            process.StartInfo.Arguments = $"testrunner.py -i {tempFile} -d {this.DatasetPath}";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();

            (string output, string err) = await Task.Run(() =>
           {
               string standard_output = process.StandardOutput.ReadToEnd();
               string error_output = process.StandardError.ReadToEnd();

               process.WaitForExit();
               return (standard_output, error_output);
           });

            Console.WriteLine(output);
            Console.WriteLine(err);

            using (var sr = new StreamReader(new FileStream(tempFile, FileMode.Open)))
            {
                return (results: await sr.ReadToEndAsync(), error: err);
            }
        }

    }
}