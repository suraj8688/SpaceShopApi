﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpaceShop.Migrations
{
    /// <inheritdoc />
    public partial class addLastUpdatedFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedOn",
                table: "Cities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "LastUpdateedBy",
                table: "Cities",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastUpdatedOn",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "LastUpdateedBy",
                table: "Cities");
        }
    }
}
