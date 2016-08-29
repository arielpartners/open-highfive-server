using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HighFive.Server.Migrations
{
    public partial class FxCopOrgFldUpd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "Organizations");

            migrationBuilder.AddColumn<string>(
                name: "UrlPath",
                table: "Organizations",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Recognitions",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "AspNetUsers",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ReportsTo",
                table: "AspNetUsers",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "AspNetUsers",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "AspNetUsers",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Department",
                table: "AspNetUsers",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "CorporateValues",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "CorporateValues",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Comment",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.RenameColumn(
                name: "isPrivate",
                table: "Recognitions",
                newName: "IsPrivate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlPath",
                table: "Organizations");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Organizations",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Recognitions",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ReportsTo",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Department",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "CorporateValues",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "CorporateValues",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Comment",
                nullable: true);

            migrationBuilder.RenameColumn(
                name: "IsPrivate",
                table: "Recognitions",
                newName: "isPrivate");
        }
    }
}
