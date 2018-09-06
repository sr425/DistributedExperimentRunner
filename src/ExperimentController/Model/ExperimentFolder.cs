using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace ExperimentController.Model
{
    public class ExperimentFolder
    {
        public string Name { get; set; }

        public List<ExperimentFolderViewModel> Experiments { get; set; }

        public List<ExperimentFolder> SubFolders { get; set; }
    }

    public class ExperimentFolderHierarchy
    {
        public long Id { get; set; }

        [NotMapped]
        public List<ExperimentFolder> SubFolders { get; set; }

        [JsonIgnore]
        public string SubFoldersSerialized
        {
            get
            {
                return JsonConvert.SerializeObject (SubFolders);
            }
            set
            {
                SubFolders = value == null?null : JsonConvert.DeserializeObject<List<ExperimentFolder>> (value);
            }
        }
    }

    public class ExperimentFolderViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}