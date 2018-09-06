using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace ExperimentController.Model
{
    public class InstanceTask
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public List<ClientResult> Results { get; set; }

        public bool Failed { get; set; } = false;
        public bool Running { get; set; } = false;
        public bool Finished { get; set; } = false;

        public long? SetId { get; set; }
        public TaskSet Set { get; set; }

        public string InputPrefix { get; set; }

        [NotMapped]
        public Dictionary<string, string> InputData { get; set; }

        [JsonIgnore]
        [Column(nameof(InputDataSerializationString))]
        public string InputDataSerializationString
        {
            get
            {
                return JsonConvert.SerializeObject(InputData);
            }
            set
            {
                InputData = value == null ? null : JsonConvert.DeserializeObject<Dictionary<string, string>>(value);
            }
        }
    }
}