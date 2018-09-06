using System.Linq;
using AutoMapper;
using ExperimentController.Model;

namespace ExperimentController.ViewModel
{
    public class AutomapperViewModelProfiles : Profile
    {
        public AutomapperViewModelProfiles ()
        {
            CreateMap<Experiment, ExperimentViewModel> ()
                .ForSourceMember (e => e.Parts, cfg => cfg.Ignore ())
                .ReverseMap ()
                .ForMember (e => e.Parts, cfg => cfg.Ignore ());

            CreateMap<Dataset, DatasetViewModel> ()
                .ForSourceMember (d => d.Files, cfg => cfg.Ignore ())
                .ForSourceMember (d => d.FileSerializationString, cfg => cfg.Ignore ());

            CreateMap<ExperimentPart, ExperimentPartViewModel> ()
                .ForSourceMember (p => p.AggregatedValuesSerializationString, cfg => cfg.Ignore ())
                .ForSourceMember (p => p.DynamicParametersSerializationString, cfg => cfg.Ignore ())
                .ForSourceMember (p => p.Experiment, cfg => cfg.Ignore ())
                .ForSourceMember (p => p.FixedParametersSerializationString, cfg => cfg.Ignore ())
                .ForSourceMember (p => p.InputDatasetRelations, cfg => cfg.Ignore ())
                .ForSourceMember (p => p.InputDatasets, cfg => cfg.Ignore ())
                .ForSourceMember (p => p.TaskSets, cfg => cfg.Ignore ())
                .ForMember (p => p.InputDatasets, o => o.ResolveUsing ((src, dest, destMember, context) => src.InputDatasets ?
                    .Select (d => new DatasetViewModel ()
                    {
                        Id = d.Id,
                            Name = d.Name,
                            Description = d.Description
                    }).ToList ()))
                .ReverseMap ()
                .ForMember (p => p.AggregatedValuesSerializationString, cfg => cfg.Ignore ())
                .ForMember (p => p.DynamicParametersSerializationString, cfg => cfg.Ignore ())
                .ForMember (p => p.Experiment, cfg => cfg.Ignore ())
                .ForMember (p => p.FixedParametersSerializationString, cfg => cfg.Ignore ())
                .ForMember (p => p.InputDatasetRelations, cfg => cfg.Ignore ())
                .ForMember (p => p.InputDatasets, cfg => cfg.Ignore ())
                .ForMember (p => p.TaskSets, cfg => cfg.Ignore ());
        }
    }
}