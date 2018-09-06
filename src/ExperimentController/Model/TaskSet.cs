using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace ExperimentController.Model
{
    public class TaskSet
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public bool Running { get; set; } = false;

        public long ExecutionRound { get; set; } = 0;

        public long ExperimentPartId { get; set; }

        [JsonIgnore]
        public ExperimentPart ExperimentPart { get; set; }

        public Dataset InputDataset { get; set; }

        [NotMapped]
        public List<FixedParameter> Parameters;
        [JsonIgnore]
        [Column(nameof(ParameterSerializationString))]
        public string ParameterSerializationString
        {
            get { return JsonConvert.SerializeObject(Parameters); }
            set
            {
                Parameters = value == null ? null : JsonConvert.DeserializeObject<List<FixedParameter>>(value);
            }
        }

        public List<InstanceTask> Tasks { get; set; }

        public bool Failed { get; set; } = false;
        public bool Finished { get; set; } = false;

        [NotMapped]
        public Dictionary<string, double> AggregatedValues { get; set; }

        [JsonIgnore]
        [Column(nameof(AggregatedValuesSerializationString))]
        public string AggregatedValuesSerializationString
        {
            get { return JsonConvert.SerializeObject(AggregatedValues); }
            set
            {
                AggregatedValues = value == null ? null : JsonConvert.DeserializeObject<Dictionary<string, double>>(value);
            }
        }
    }
}