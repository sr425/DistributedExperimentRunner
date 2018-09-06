using System;
using System.Collections.Generic;
using System.Linq;
using ExperimentController.Model;
using ExperimentController.Services.TaskCreation;
using Xunit;

namespace ExperimentController.Test
{
    public class TestTaskSetGenerator
    {
        private Dataset GenerateTestDataset()
        {
            return new Dataset()
            {
                Files = new List<Dictionary<string, string>>()
                {
                    new Dictionary<string, string> ()
                    { { "frame1", "hello world.png" }
                    }
                    }
            };
        }

        [Fact]
        public void TestFixedGeneration()
        {
            var testdataset = GenerateTestDataset();
            var relation = new Experiment_Dataset_Rel();
            relation.Dataset = testdataset;

            var testExp = new Experiment()
            {
                DatasetRelations = new List<Experiment_Dataset_Rel>() { relation },
                Parameters = new List<OptimicationParameter>()
                {

                new SetOptimicationParameter ()
                {
                Name = "Param1",
                Values = new List<FixedParameter> ()
                {
                new StringParameter () { Name = "Param1", Value = "Hello" },
                new StringParameter () { Name = "Param1", Value = "Test" }
                }
                },
                new SetOptimicationParameter ()
                {
                Name = "Param2",
                Values = new List<FixedParameter> ()
                {
                new StringParameter () { Name = "Param2", Value = "World" },
                new StringParameter () { Name = "Param2", Value = "Set" }
                }
                },
                }
            };
            relation.Experiment = testExp;

            var generator = new TaskGenerator();
            var tasksets = generator.GenerateTaskSets(testExp.Parameters.OfType<SetOptimicationParameter>().ToList(), testExp);
            Assert.Equal(4, tasksets.Count);

            Assert.NotNull(tasksets.FirstOrDefault(ts =>
          {
              return (string)ts.Parameters.First(p => p.Name == "Param1").Value == "Hello" &&
                  (string)ts.Parameters.First(p => p.Name == "Param2").Value == "World";
          }));

            Assert.NotNull(tasksets.FirstOrDefault(ts =>
          {
              return (string)ts.Parameters.First(p => p.Name == "Param1").Value == "Hello" &&
                  (string)ts.Parameters.First(p => p.Name == "Param2").Value == "Set";
          }));

            Assert.NotNull(tasksets.FirstOrDefault(ts =>
          {
              return (string)ts.Parameters.First(p => p.Name == "Param1").Value == "Test" &&
                  (string)ts.Parameters.First(p => p.Name == "Param2").Value == "World";
          }));

            Assert.NotNull(tasksets.FirstOrDefault(ts =>
          {
              return (string)ts.Parameters.First(p => p.Name == "Param1").Value == "Test" &&
                  (string)ts.Parameters.First(p => p.Name == "Param2").Value == "Set";
          }));
        }
    }
}