using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TDD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBackTheDroppedColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedDate",
                table: "TaskItem",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "(getdate())");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastModifiedDate",
                table: "TaskItem");
        }
    }
}
