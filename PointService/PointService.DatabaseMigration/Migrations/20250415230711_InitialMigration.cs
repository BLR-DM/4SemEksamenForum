using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PointService.DatabaseMigration.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PointActions",
                columns: table => new
                {
                    Action = table.Column<string>(type: "text", nullable: false),
                    Points = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointActions", x => x.Action);
                });

            migrationBuilder.CreateTable(
                name: "PointEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    PointActionId = table.Column<string>(type: "text", nullable: false),
                    Points = table.Column<int>(type: "integer", nullable: false),
                    SourceId = table.Column<int>(type: "integer", nullable: false),
                    SourceType = table.Column<string>(type: "text", nullable: false),
                    ContextId = table.Column<int>(type: "integer", nullable: false),
                    ContextType = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PointEntries_PointActions_PointActionId",
                        column: x => x.PointActionId,
                        principalTable: "PointActions",
                        principalColumn: "Action",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PointEntries_PointActionId",
                table: "PointEntries",
                column: "PointActionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PointEntries");

            migrationBuilder.DropTable(
                name: "PointActions");
        }
    }
}
