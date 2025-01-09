using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlertApi.Migrations
{
    /// <inheritdoc />
    public partial class relationsupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AlertType",
                table: "Alerts",
                newName: "WebsiteName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WebsiteName",
                table: "Alerts",
                newName: "AlertType");
        }
    }
}
