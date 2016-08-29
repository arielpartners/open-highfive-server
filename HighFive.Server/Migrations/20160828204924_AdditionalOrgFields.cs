using Microsoft.EntityFrameworkCore.Migrations;

namespace HighFive.Server.Migrations
{
    public partial class AdditionalOrgFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Organizations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "Organizations");
        }
    }
}
