using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace ExperimentController.Model
{
    public class Dataset
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string Prefix { get; set; }

        [NotMapped]
        public List<Dictionary<string, string>> Files { get; set; }

        [JsonIgnore]
        [Column(nameof(FileSerializationString))]
        public string FileSerializationString
        {
            get { return JsonConvert.SerializeObject(Files); }
            set
            {
                Files = value == null ? null : JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(value);
            }
        }

        public List<ExperimentPart_Dataset_Relation> ExperimentPartRelations { get; set; }
    }
}