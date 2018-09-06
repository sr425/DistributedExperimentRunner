using System;
using System.Collections.Generic;
using System.Linq;
using ExperimentController.Model;

namespace ExperimentController.Services.TaskCreation
{
    public class TaskGenerator
    {
        public List<InstanceTask> GenerateTasks(TaskSet set)
        {
            if (set.InputDataset == null)
            {
                return new List<InstanceTask>() { new InstanceTask() { Name = set.Name, Set = set } };
            }
            else
            {
                return set.InputDataset.Files.Select(file =>
               {
                   return new InstanceTask() { Name = set.Name, Set = set, InputData = file, InputPrefix = set.InputDataset.Prefix };
               }).ToList();
            }
        }

        public List<TaskSet> GenerateTaskSets(ExperimentPart part)
        {
            if (part.DynamicParameters != null && part.DynamicParameters.Any())
                throw new NotImplementedException("At the moment no dynamic parameters are supported");
            if (part.InputDatasets == null)
                return new List<TaskSet>();

            var sharedParams = part.Experiment.SharedFixedParameter;

            List<FixedParameter> combined = new List<FixedParameter>();
            if (part.FixedParameters != null)
            {
                combined = part.FixedParameters;
            }
            if (sharedParams != null)
            {
                combined = combined
                   .Union(sharedParams.Where(p => !combined.Any(p2 => p2.Name == p.Name)))
                   .ToList();
            }

            return part.InputDatasets.Select(dataset =>
               new TaskSet() { ExperimentPart = part, Parameters = combined, InputDataset = dataset }
            ).ToList();
        }

        /*private List<List<FixedParameter>> ComputeValueCombinations (List<SetOptimicationParameter> parameters)
        {
            var tempList = parameters[0].Values.Select (v => new List<FixedParameter> () { v.Clone () }).ToList ();

            for (int i = 1; i < parameters.Count; i++)
            {
                var processParam = parameters[i];
                tempList = tempList.SelectMany (tempParamConfig =>
                {
                    return processParam.Values.Select (value =>
                    {
                        var newParamConfig = tempParamConfig.Select (t => t.Clone ()).ToList ();
                        newParamConfig.Add (value.Clone ());
                        return newParamConfig;
                    });
                }).ToList ();
            }
            return tempList;
        }*/
    }
}