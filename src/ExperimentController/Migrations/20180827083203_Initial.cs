using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExperimentController.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Datasets",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Prefix = table.Column<string>(nullable: true),
                    FileSerializationString = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Datasets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExperimentFolderHierarchies",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SubFoldersSerialized = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperimentFolderHierarchies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Experiments",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Creator = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    PayloadFilename = table.Column<string>(nullable: true),
                    PayloadHash = table.Column<string>(nullable: true),
                    SharedFixedParameterSerializationString = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experiments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payload",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Filename = table.Column<string>(nullable: true),
                    BinaryExecutionData = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payload", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExperimentParts",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Running = table.Column<bool>(nullable: false),
                    ExperimentId = table.Column<long>(nullable: false),
                    FixedParametersSerializationString = table.Column<string>(nullable: true),
                    DynamicParametersSerializationString = table.Column<string>(nullable: true),
                    Failed = table.Column<bool>(nullable: false),
                    Finished = table.Column<bool>(nullable: false),
                    AggregatedValuesSerializationString = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperimentParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExperimentParts_Experiments_ExperimentId",
                        column: x => x.ExperimentId,
                        principalTable: "Experiments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExperimentPartDatasetRelations",
                columns: table => new
                {
                    ExperimentPartId = table.Column<long>(nullable: false),
                    DatasetId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperimentPartDatasetRelations", x => new { x.DatasetId, x.ExperimentPartId });
                    table.ForeignKey(
                        name: "FK_ExperimentPartDatasetRelations_Datasets_DatasetId",
                        column: x => x.DatasetId,
                        principalTable: "Datasets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExperimentPartDatasetRelations_ExperimentParts_ExperimentPartId",
                        column: x => x.ExperimentPartId,
                        principalTable: "ExperimentParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskSets",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Running = table.Column<bool>(nullable: false),
                    ExecutionRound = table.Column<long>(nullable: false),
                    ExperimentPartId = table.Column<long>(nullable: false),
                    InputDatasetId = table.Column<long>(nullable: true),
                    ParameterSerializationString = table.Column<string>(nullable: true),
                    Failed = table.Column<bool>(nullable: false),
                    Finished = table.Column<bool>(nullable: false),
                    AggregatedValuesSerializationString = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskSets_ExperimentParts_ExperimentPartId",
                        column: x => x.ExperimentPartId,
                        principalTable: "ExperimentParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskSets_Datasets_InputDatasetId",
                        column: x => x.InputDatasetId,
                        principalTable: "Datasets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Failed = table.Column<bool>(nullable: false),
                    Running = table.Column<bool>(nullable: false),
                    Finished = table.Column<bool>(nullable: false),
                    SetId = table.Column<long>(nullable: true),
                    InputPrefix = table.Column<string>(nullable: true),
                    InputDataSerializationString = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_TaskSets_SetId",
                        column: x => x.SetId,
                        principalTable: "TaskSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Results",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TaskId = table.Column<long>(nullable: false),
                    ValuesSerializationString = table.Column<string>(nullable: true),
                    Failed = table.Column<bool>(nullable: false),
                    ErrorMessage = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Results_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Datasets",
                columns: new[] { "Id", "Description", "FileSerializationString", "Name", "Prefix" },
                values: new object[] { 1L, "Middlebury Optical flow benchark with 8 sequences", "[{\"frame10\":\"RubberWhale/frame10.png\",\"frame11\":\"RubberWhale/frame11.png\",\"groundtruth\":\"RubberWhale/flow10.flo\"},{\"frame10\":\"Hydrangea/frame10.png\",\"frame11\":\"Hydrangea/frame11.png\",\"groundtruth\":\"Hydrangea/flow10.flo\"},{\"frame10\":\"Grove2/frame10.png\",\"frame11\":\"Grove2/frame11.png\",\"groundtruth\":\"Grove2/flow10.flo\"},{\"frame10\":\"Grove3/frame10.png\",\"frame11\":\"Grove3/frame11.png\",\"groundtruth\":\"Grove3/flow10.flo\"},{\"frame10\":\"Urban2/frame10.png\",\"frame11\":\"Urban2/frame11.png\",\"groundtruth\":\"Urban2/flow10.flo\"},{\"frame10\":\"Urban3/frame10.png\",\"frame11\":\"Urban3/frame11.png\",\"groundtruth\":\"Urban3/flow10.flo\"},{\"frame10\":\"Venus/frame10.png\",\"frame11\":\"Venus/frame11.png\",\"groundtruth\":\"Venus/flow10.flo\"},{\"frame10\":\"Dimetrodon/frame10.png\",\"frame11\":\"Dimetrodon/frame11.png\",\"groundtruth\":\"Dimetrodon/flow10.flo\"}]", "Middlebury Flow 8", "middlebury/training/" });

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentPartDatasetRelations_ExperimentPartId",
                table: "ExperimentPartDatasetRelations",
                column: "ExperimentPartId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentParts_ExperimentId",
                table: "ExperimentParts",
                column: "ExperimentId");

            migrationBuilder.CreateIndex(
                name: "IX_Results_TaskId",
                table: "Results",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_SetId",
                table: "Tasks",
                column: "SetId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskSets_ExperimentPartId",
                table: "TaskSets",
                column: "ExperimentPartId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskSets_InputDatasetId",
                table: "TaskSets",
                column: "InputDatasetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExperimentFolderHierarchies");

            migrationBuilder.DropTable(
                name: "ExperimentPartDatasetRelations");

            migrationBuilder.DropTable(
                name: "Payload");

            migrationBuilder.DropTable(
                name: "Results");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "TaskSets");

            migrationBuilder.DropTable(
                name: "ExperimentParts");

            migrationBuilder.DropTable(
                name: "Datasets");

            migrationBuilder.DropTable(
                name: "Experiments");
        }
    }
}
