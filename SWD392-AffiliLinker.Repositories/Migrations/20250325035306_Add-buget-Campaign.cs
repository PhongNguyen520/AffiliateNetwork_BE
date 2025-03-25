using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWD392_AffiliLinker.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddbugetCampaign : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Budget",
                table: "Campaigns",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Budget",
                table: "Campaigns");
        }
    }
}
