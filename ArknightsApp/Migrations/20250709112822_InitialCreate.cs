using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArknightsApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Factions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    LogoUrl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    TypeCode = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    IconUrl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OperatorClasses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    IconUrl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperatorClasses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ModuleLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ModuleId = table.Column<int>(type: "INTEGER", nullable: false),
                    Level = table.Column<int>(type: "INTEGER", nullable: false),
                    StatBonuses = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModuleLevels_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubClasses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    OperatorClassId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubClasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubClasses_OperatorClasses_OperatorClassId",
                        column: x => x.OperatorClassId,
                        principalTable: "OperatorClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Operators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Rarity = table.Column<int>(type: "INTEGER", nullable: false),
                    OperatorClassId = table.Column<int>(type: "INTEGER", nullable: false),
                    SubClassId = table.Column<int>(type: "INTEGER", nullable: false),
                    FactionId = table.Column<int>(type: "INTEGER", nullable: true),
                    ImageUrl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Position = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    CnReleaseDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GlobalReleaseDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operators_Factions_FactionId",
                        column: x => x.FactionId,
                        principalTable: "Factions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Operators_OperatorClasses_OperatorClassId",
                        column: x => x.OperatorClassId,
                        principalTable: "OperatorClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Operators_SubClasses_SubClassId",
                        column: x => x.SubClassId,
                        principalTable: "SubClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OperatorGrowthTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OperatorId = table.Column<int>(type: "INTEGER", nullable: false),
                    E0BaseHealth = table.Column<int>(type: "INTEGER", nullable: false),
                    E0BaseAttack = table.Column<int>(type: "INTEGER", nullable: false),
                    E0BaseDefense = table.Column<int>(type: "INTEGER", nullable: false),
                    E0BaseResistance = table.Column<int>(type: "INTEGER", nullable: false),
                    E0MaxHealth = table.Column<int>(type: "INTEGER", nullable: false),
                    E0MaxAttack = table.Column<int>(type: "INTEGER", nullable: false),
                    E0MaxDefense = table.Column<int>(type: "INTEGER", nullable: false),
                    E0MaxResistance = table.Column<int>(type: "INTEGER", nullable: false),
                    E1BaseHealth = table.Column<int>(type: "INTEGER", nullable: false),
                    E1BaseAttack = table.Column<int>(type: "INTEGER", nullable: false),
                    E1BaseDefense = table.Column<int>(type: "INTEGER", nullable: false),
                    E1BaseResistance = table.Column<int>(type: "INTEGER", nullable: false),
                    E1MaxHealth = table.Column<int>(type: "INTEGER", nullable: false),
                    E1MaxAttack = table.Column<int>(type: "INTEGER", nullable: false),
                    E1MaxDefense = table.Column<int>(type: "INTEGER", nullable: false),
                    E1MaxResistance = table.Column<int>(type: "INTEGER", nullable: false),
                    E2BaseHealth = table.Column<int>(type: "INTEGER", nullable: false),
                    E2BaseAttack = table.Column<int>(type: "INTEGER", nullable: false),
                    E2BaseDefense = table.Column<int>(type: "INTEGER", nullable: false),
                    E2BaseResistance = table.Column<int>(type: "INTEGER", nullable: false),
                    E2MaxHealth = table.Column<int>(type: "INTEGER", nullable: false),
                    E2MaxAttack = table.Column<int>(type: "INTEGER", nullable: false),
                    E2MaxDefense = table.Column<int>(type: "INTEGER", nullable: false),
                    E2MaxResistance = table.Column<int>(type: "INTEGER", nullable: false),
                    TrustBonusHealth = table.Column<int>(type: "INTEGER", nullable: false),
                    TrustBonusAttack = table.Column<int>(type: "INTEGER", nullable: false),
                    TrustBonusDefense = table.Column<int>(type: "INTEGER", nullable: false),
                    TrustBonusResistance = table.Column<int>(type: "INTEGER", nullable: false),
                    AttackSpeed = table.Column<decimal>(type: "TEXT", nullable: false),
                    AttackType = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    BlockCount = table.Column<int>(type: "INTEGER", nullable: false),
                    DeploymentCost = table.Column<int>(type: "INTEGER", nullable: false),
                    RedeployTime = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperatorGrowthTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperatorGrowthTemplates_Operators_OperatorId",
                        column: x => x.OperatorId,
                        principalTable: "Operators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OperatorModules",
                columns: table => new
                {
                    OperatorId = table.Column<int>(type: "INTEGER", nullable: false),
                    ModuleId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsUnlocked = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperatorModules", x => new { x.OperatorId, x.ModuleId });
                    table.ForeignKey(
                        name: "FK_OperatorModules_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OperatorModules_Operators_OperatorId",
                        column: x => x.OperatorId,
                        principalTable: "Operators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    BaseDescription = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    IconUrl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    OperatorId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skills_Operators_OperatorId",
                        column: x => x.OperatorId,
                        principalTable: "Operators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Talents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    BaseDescription = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    IconUrl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    OperatorId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Talents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Talents_Operators_OperatorId",
                        column: x => x.OperatorId,
                        principalTable: "Operators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SkillLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SkillId = table.Column<int>(type: "INTEGER", nullable: false),
                    Level = table.Column<int>(type: "INTEGER", nullable: false),
                    LevelName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    SpCost = table.Column<int>(type: "INTEGER", nullable: false),
                    Duration = table.Column<int>(type: "INTEGER", nullable: false),
                    Parameters = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SkillLevels_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModuleTalentUpgrades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ModuleLevelId = table.Column<int>(type: "INTEGER", nullable: false),
                    TalentId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpgradeParameters = table.Column<string>(type: "TEXT", nullable: false),
                    UpgradeDescription = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleTalentUpgrades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModuleTalentUpgrades_ModuleLevels_ModuleLevelId",
                        column: x => x.ModuleLevelId,
                        principalTable: "ModuleLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModuleTalentUpgrades_Talents_TalentId",
                        column: x => x.TalentId,
                        principalTable: "Talents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TalentLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TalentId = table.Column<int>(type: "INTEGER", nullable: false),
                    PotentialLevel = table.Column<int>(type: "INTEGER", nullable: false),
                    UnlockCondition = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Parameters = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TalentLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TalentLevels_Talents_TalentId",
                        column: x => x.TalentId,
                        principalTable: "Talents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModuleLevels_ModuleId",
                table: "ModuleLevels",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleTalentUpgrades_ModuleLevelId",
                table: "ModuleTalentUpgrades",
                column: "ModuleLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleTalentUpgrades_TalentId",
                table: "ModuleTalentUpgrades",
                column: "TalentId");

            migrationBuilder.CreateIndex(
                name: "IX_OperatorGrowthTemplates_OperatorId",
                table: "OperatorGrowthTemplates",
                column: "OperatorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OperatorModules_ModuleId",
                table: "OperatorModules",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Operators_FactionId",
                table: "Operators",
                column: "FactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Operators_OperatorClassId",
                table: "Operators",
                column: "OperatorClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Operators_SubClassId",
                table: "Operators",
                column: "SubClassId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillLevels_SkillId",
                table: "SkillLevels",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_OperatorId",
                table: "Skills",
                column: "OperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_SubClasses_OperatorClassId",
                table: "SubClasses",
                column: "OperatorClassId");

            migrationBuilder.CreateIndex(
                name: "IX_TalentLevels_TalentId",
                table: "TalentLevels",
                column: "TalentId");

            migrationBuilder.CreateIndex(
                name: "IX_Talents_OperatorId",
                table: "Talents",
                column: "OperatorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModuleTalentUpgrades");

            migrationBuilder.DropTable(
                name: "OperatorGrowthTemplates");

            migrationBuilder.DropTable(
                name: "OperatorModules");

            migrationBuilder.DropTable(
                name: "SkillLevels");

            migrationBuilder.DropTable(
                name: "TalentLevels");

            migrationBuilder.DropTable(
                name: "ModuleLevels");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "Talents");

            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.DropTable(
                name: "Operators");

            migrationBuilder.DropTable(
                name: "Factions");

            migrationBuilder.DropTable(
                name: "SubClasses");

            migrationBuilder.DropTable(
                name: "OperatorClasses");
        }
    }
}
