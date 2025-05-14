using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotificationService.DatabaseMigration.Migrations
{
    /// <inheritdoc />
    public partial class AddedProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NotificationRead",
                table: "Notifications",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotificationRead",
                table: "Notifications");
        }
    }
}
