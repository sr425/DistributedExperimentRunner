using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;

namespace ExperimentController.Model
{
    public class Experiment
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Creator { get; set; }
        public string Description { get; set; }

        public string PayloadFilename { get; set; }
        public string PayloadHash { get; set; }

        public List<ExperimentPart> Parts { get; set; }


        [NotMapped]
        public List<FixedParameter> SharedFixedParameter;
        [JsonIgnore]
        [Column(nameof(SharedFixedParameterSerializationString))]
        public string SharedFixedParameterSerializationString
        {
            get { return JsonConvert.SerializeObject(SharedFixedParameter); }
            set
            {
                SharedFixedParameter = value == null ? null : JsonConvert.DeserializeObject<List<FixedParameter>>(value);
            }
        }
    }
}