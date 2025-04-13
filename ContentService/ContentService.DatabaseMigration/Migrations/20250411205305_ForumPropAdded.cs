﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContentService.DatabaseMigration.Migrations
{
    /// <inheritdoc />
    public partial class ForumPropAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Forums",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "Forums");
        }
    }
}
