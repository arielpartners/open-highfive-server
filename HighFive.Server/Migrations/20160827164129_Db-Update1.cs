using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HighFive.Server.Migrations
{
    public partial class DbUpdate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recognitions_CorporateValue_ValueId",
                table: "Recognitions");

            migrationBuilder.DropTable(
                name: "CorporateValue");

            migrationBuilder.CreateTable(
                name: "CorporateValues",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorporateValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CorporateValues_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Organizations",
                maxLength: 100,
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                maxLength: 100,
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_CorporateValues_OrganizationId",
                table: "CorporateValues",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recognitions_CorporateValues_ValueId",
                table: "Recognitions",
                column: "ValueId",
                principalTable: "CorporateValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recognitions_CorporateValues_ValueId",
                table: "Recognitions");

            migrationBuilder.DropTable(
                name: "CorporateValues");

            migrationBuilder.CreateTable(
                name: "CorporateValue",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorporateValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CorporateValue_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Organizations",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CorporateValue_OrganizationId",
                table: "CorporateValue",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recognitions_CorporateValue_ValueId",
                table: "Recognitions",
                column: "ValueId",
                principalTable: "CorporateValue",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
