using System.Collections.Generic;
using ExperimentController.Model;

namespace ExperimentController.ViewModel
{
    public class ExperimentViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Creator { get; set; }
        public string Description { get; set; }

        public string PayloadFilename { get; set; }
        public string PayloadHash { get; set; }

        public List<FixedParameter> SharedFixedParameter;
    }
}