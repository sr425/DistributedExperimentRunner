using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using JsonSubTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ExperimentController.Model
{
    public class DynamicOptimicationParameter
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public string ParameterValueType { get; set; }

        [NotMapped]
        public Dictionary<string, object> OptimizerParams;

        [JsonIgnore]
        [Column(nameof(OptimizerParamsSerializationString))]
        public string OptimizerParamsSerializationString
        {
            get { return JsonConvert.SerializeObject(OptimizerParams); }
            set
            {
                OptimizerParams = value == null ? null : JsonConvert.DeserializeObject<Dictionary<string, object>>(value);
            }
        }
    }
}