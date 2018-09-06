using System.Collections.Generic;
using ExperimentController.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExperimentController
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ExperimentFolderHierarchy> ExperimentFolderHierarchies { get; set; }

        public DbSet<ExperimentPart> ExperimentParts { get; set; }
        public DbSet<Experiment> Experiments { get; set; }
        public DbSet<TaskSet> TaskSets { get; set; }
        public DbSet<InstanceTask> Tasks { get; set; }
        public DbSet<ClientResult> Results { get; set; }

        public DbSet<ExecutionPayload> Payload { get; set; }
        public DbSet<Dataset> Datasets { get; set; }

        public DbSet<ExperimentPart_Dataset_Relation> ExperimentPartDatasetRelations { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ExperimentPart_Dataset_Relation>().HasKey(t => new { t.DatasetId, t.ExperimentPartId });

            builder.Entity<Experiment>()
                .HasMany(e => e.Parts)
                .WithOne(t => t.Experiment)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ExperimentPart>()
                .HasMany(p => p.TaskSets)
                .WithOne(s => s.ExperimentPart)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ExperimentPart>()
                .HasMany(e => e.InputDatasetRelations)
                .WithOne(r => r.ExperimentPart)
                .HasForeignKey(r => r.ExperimentPartId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Dataset>()
                .HasMany(d => d.ExperimentPartRelations)
                .WithOne(r => r.Dataset)
                .HasForeignKey(r => r.DatasetId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<TaskSet>()
                .HasMany(t => t.Tasks)
                .WithOne(it => it.Set)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<TaskSet>()
                .HasOne(t => t.InputDataset)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Dataset>().HasData(
                new Dataset()
                {
                    Id = 1,
                    Name = "Middlebury Flow 8",
                    Description = "Middlebury Optical flow benchark with 8 sequences",
                    Prefix = "middlebury/training",
                    Files = new List<Dictionary<string, string>>
                        {
                            new Dictionary<string, string> () { { "frame10", "RubberWhale/frame10.png" }, { "frame11", "RubberWhale/frame11.png" }, { "groundtruth", "RubberWhale/flow10.flo" } },
                            new Dictionary<string, string> () { { "frame10", "Hydrangea/frame10.png" }, { "frame11", "Hydrangea/frame11.png" }, { "groundtruth", "Hydrangea/flow10.flo" } },
                            new Dictionary<string, string> () { { "frame10", "Grove2/frame10.png" }, { "frame11", "Grove2/frame11.png" }, { "groundtruth", "Grove2/flow10.flo" } },
                            new Dictionary<string, string> () { { "frame10", "Grove3/frame10.png" }, { "frame11", "Grove3/frame11.png" }, { "groundtruth", "Grove3/flow10.flo" } },
                            new Dictionary<string, string> () { { "frame10", "Urban2/frame10.png" }, { "frame11", "Urban2/frame11.png" }, { "groundtruth", "Urban2/flow10.flo" } },
                            new Dictionary<string, string> () { { "frame10", "Urban3/frame10.png" }, { "frame11", "Urban3/frame11.png" }, { "groundtruth", "Urban3/flow10.flo" } },
                            new Dictionary<string, string> () { { "frame10", "Venus/frame10.png" }, { "frame11", "Venus/frame11.png" }, { "groundtruth", "Venus/flow10.flo" } },
                            new Dictionary<string, string> () { { "frame10", "Dimetrodon/frame10.png" }, { "frame11", "Dimetrodon/frame11.png" }, { "groundtruth", "Dimetrodon/flow10.flo" } }
                        }
                });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}