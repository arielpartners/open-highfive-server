using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HighFive.Server.Migrations
{
    public partial class FxCopFixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlPath",
                table: "Organizations");

            migrationBuilder.AddColumn<string>(
                name: "WebPath",
                table: "Organizations",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WebPath",
                table: "Organizations");

            migrationBuilder.AddColumn<string>(
                name: "UrlPath",
                table: "Organizations",
                maxLength: 100,
                nullable: true);
        }
    }
}
