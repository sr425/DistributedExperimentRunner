using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;

namespace ExperimentController.Model
{
    public class ExperimentPart
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public bool Running { get; set; } = false;

        public long ExperimentId { get; set; }

        [JsonIgnore]
        public Experiment Experiment { get; set; }

        [JsonIgnore]
        public List<ExperimentPart_Dataset_Relation> InputDatasetRelations { get; set; }

        [JsonIgnore]
        [NotMapped]
        public List<Dataset> InputDatasets
        {
            get { return InputDatasetRelations?.Select(r => r.Dataset).ToList(); }
            set
            {
                if (value == null) { InputDatasetRelations = null; return; }

                if (InputDatasetRelations == null)
                {
                    InputDatasetRelations = value.Select(d => new ExperimentPart_Dataset_Relation() { DatasetId = d.Id, ExperimentPart = this }).ToList();
                }
                var deleteElements = InputDatasetRelations.Where(r => !value.Any(v => v.Id == r.DatasetId)).ToList();
                deleteElements.ForEach(d => InputDatasetRelations.Remove(d));

                var ids = value.Where(v => !InputDatasetRelations.Any(r => r.DatasetId == v.Id)).Select(v => v.Id).ToList();
                ids.ForEach(id => InputDatasetRelations.Add(new ExperimentPart_Dataset_Relation() { ExperimentPart = this, DatasetId = id }));
            }
        }

        public List<TaskSet> TaskSets { get; set; }

        [NotMapped]
        public List<FixedParameter> FixedParameters;
        [JsonIgnore]
        [Column(nameof(FixedParametersSerializationString))]
        public string FixedParametersSerializationString
        {
            get { return JsonConvert.SerializeObject(FixedParameters); }
            set
            {
                FixedParameters = value == null ? null : JsonConvert.DeserializeObject<List<FixedParameter>>(value);
            }
        }

        [NotMapped]
        public List<DynamicOptimicationParameter> DynamicParameters;
        [JsonIgnore]
        [Column(nameof(DynamicParametersSerializationString))]
        public string DynamicParametersSerializationString
        {
            get { return JsonConvert.SerializeObject(DynamicParameters); }
            set
            {
                DynamicParameters = value == null ? null : JsonConvert.DeserializeObject<List<DynamicOptimicationParameter>>(value);
            }
        }

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

    public class ExperimentPart_Dataset_Relation
    {
        public long ExperimentPartId { get; set; }
        public ExperimentPart ExperimentPart { get; set; }

        public long DatasetId { get; set; }
        public Dataset Dataset { get; set; }
    }
}