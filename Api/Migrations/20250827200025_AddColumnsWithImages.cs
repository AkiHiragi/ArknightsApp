using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ArknightsApp.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnsWithImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                table: "Operators",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "E0ArtUrl",
                table: "Operators",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "E2ArtUrl",
                table: "Operators",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreviewUrl",
                table: "Operators",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Skin",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    OperatorId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skin_Operators_OperatorId",
                        column: x => x.OperatorId,
                        principalTable: "Operators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Skin_OperatorId",
                table: "Skin",
                column: "OperatorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Skin");

            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                table: "Operators");

            migrationBuilder.DropColumn(
                name: "E0ArtUrl",
                table: "Operators");

            migrationBuilder.DropColumn(
                name: "E2ArtUrl",
                table: "Operators");

            migrationBuilder.DropColumn(
                name: "PreviewUrl",
                table: "Operators");
        }
    }
}
