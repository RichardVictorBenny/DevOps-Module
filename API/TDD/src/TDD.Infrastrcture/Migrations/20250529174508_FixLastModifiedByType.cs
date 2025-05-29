using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TDD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixLastModifiedByType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
            name: "LastModifiedBy",
            table: "TaskItem");

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedBy",
                table: "TaskItem",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: Guid.Empty);
        }
        

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
            name: "LastModifiedBy",
            table: "TaskItem");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedBy",
                table: "TaskItem",
                type: "datetime2",
                nullable: false,
                defaultValue: DateTime.UtcNow);
        }
    }
}
