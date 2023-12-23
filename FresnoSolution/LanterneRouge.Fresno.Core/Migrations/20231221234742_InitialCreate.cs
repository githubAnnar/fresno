using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LanterneRouge.Fresno.Core.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Street = table.Column<string>(type: "TEXT", nullable: true),
                    PostCode = table.Column<string>(type: "TEXT", nullable: true),
                    PostCity = table.Column<string>(type: "TEXT", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Height = table.Column<int>(type: "INTEGER", nullable: true),
                    Sex = table.Column<string>(type: "TEXT", nullable: false),
                    MaxHr = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.CheckConstraint("CK_SEX", "[Sex] = 'M' OR [Sex] = 'F'");
                });

            migrationBuilder.CreateTable(
                name: "StepTest",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TestType = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "Bike"),
                    EffortUnit = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "W"),
                    StepDuration = table.Column<long>(type: "INTEGER", nullable: false),
                    LoadPreset = table.Column<float>(type: "REAL", nullable: false),
                    Increase = table.Column<float>(type: "REAL", nullable: false),
                    Temperature = table.Column<float>(type: "REAL", nullable: false),
                    Weight = table.Column<float>(type: "REAL", nullable: false),
                    TestDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StepTest", x => x.Id);
                    table.CheckConstraint("CK_EFFORTUNIT", "[EffortUnit] = 'W' OR [EffortUnit] = 'm-s'");
                    table.CheckConstraint("CK_TESTTYPE", "[TestType] = 'Bike' OR [TestType] = 'Run'");
                    table.ForeignKey(
                        name: "FK_StepTest_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Measurement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Sequence = table.Column<int>(type: "INTEGER", nullable: false),
                    StepTestId = table.Column<Guid>(type: "TEXT", nullable: false),
                    HeartRate = table.Column<int>(type: "INTEGER", nullable: false),
                    Lactate = table.Column<float>(type: "REAL", nullable: false),
                    Load = table.Column<float>(type: "REAL", nullable: false),
                    InCalculation = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Measurement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Measurement_StepTest_StepTestId",
                        column: x => x.StepTestId,
                        principalTable: "StepTest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Measurement_StepTestId",
                table: "Measurement",
                column: "StepTestId");

            migrationBuilder.CreateIndex(
                name: "IX_StepTest_UserId",
                table: "StepTest",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Measurement");

            migrationBuilder.DropTable(
                name: "StepTest");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
