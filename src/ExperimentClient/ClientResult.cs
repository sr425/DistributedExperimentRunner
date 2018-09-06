using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace ExperimentController.Model
{
    public class ClientResult
    {
        public long Id { get; set; }

        public long TaskId { get; set; }

        [NotMapped]
        public Dictionary<string, double> Values { get; set; }

        [JsonIgnore]
        [Column(nameof(ValuesSerializationString))]
        public string ValuesSerializationString
        {
            get { return JsonConvert.SerializeObject(Values); }
            set
            {
                Values = value == null ? null : JsonConvert.DeserializeObject<Dictionary<string, double>>(value);
            }
        }

        public bool Failed { get; set; }

        public string ErrorMessage { get; set; }
    }
}