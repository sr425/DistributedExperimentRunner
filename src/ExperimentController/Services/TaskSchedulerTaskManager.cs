using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ExperimentController.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ExperimentController.Services
{
    public class TaskSchedulerTaskManager : ITaskManager
    {
        private string _clientSecret;
        private HttpClient _client;
        private string _endpointUrl;
        private JsonSerializerSettings _jsonSerializerSettings;

        public TaskSchedulerTaskManager(IConfiguration configuration)
        {
            _clientSecret = configuration["clientSecret"];
            _client = new HttpClient();
            // _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_clientSecret);
            _client.DefaultRequestHeaders.Add("Authorization", _clientSecret);
            _endpointUrl = configuration["taskSchedulerEndpoint"];
            _jsonSerializerSettings = new JsonSerializerSettings();
            _jsonSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        public async Task<bool> CancelTask(InstanceTask task)
        {
            task.Running = false;
            var response = await _client.DeleteAsync($"{_endpointUrl}/{task.Id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CancelTasks(IEnumerable<InstanceTask> tasks)
        {
            var jobs = tasks.Select(t => CancelTask(t));
            var success = true;
            foreach (var j in jobs)
            {
                success = success && await j;
            }
            return success;
        }

        public async Task<bool> QueueTask(InstanceTask task)
        {
            task.Running = true;
            var workPackage = new WorkPackage()
            {
                InstanceTaskId = task.Id,
                PayloadHash = task.Set.ExperimentPart.Experiment.PayloadHash,
                FilesPrefix = task.InputPrefix,
                Files = task.InputData,
                Parameters = task.Set.Parameters
            };
            var json = JsonConvert.SerializeObject(workPackage, _jsonSerializerSettings);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            content.Headers.Remove("Content-Type");
            content.Headers.Add("Content-Type", "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, _endpointUrl);
            request.Content = content;

            var msg = await _client.SendAsync(request);
            if (!msg.IsSuccessStatusCode)
            {
                Console.WriteLine(this._clientSecret);
                Console.WriteLine(json);
                Console.WriteLine(msg.StatusCode);
                Console.WriteLine(await msg.Content.ReadAsStringAsync());
                return false;
            }
            return true;
        }

        public async Task<bool> QueueTaskRange(IEnumerable<InstanceTask> tasks)
        {
            var jobs = tasks.Select(t => QueueTask(t));
            var successfull = true;
            foreach (var j in jobs)
            {
                successfull = successfull && await j;
            }
            return successfull;
        }
    }
}