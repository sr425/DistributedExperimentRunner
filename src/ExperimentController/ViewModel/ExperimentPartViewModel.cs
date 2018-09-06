using System.Collections.Generic;
using ExperimentController.Model;

namespace ExperimentController.ViewModel
{
    public class ExperimentPartViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public bool Running { get; set; }

        public long ExperimentId { get; set; }

        public List<DatasetViewModel> InputDatasets { get; set; }


        public List<FixedParameter> FixedParameters;

        public List<DynamicOptimicationParameter> DynamicParameters;

        public bool Failed { get; set; }
        public bool Finished { get; set; }
        public Dictionary<string, double> AggregatedValues { get; set; }
    }
}