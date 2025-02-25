using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchedulerTodo.Migrations
{
    /// <inheritdoc />
    public partial class ToDosModelUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Deadline",
                table: "ItemsToDo",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "RemindOn",
                table: "ItemsToDo",
                type: "datetimeoffset",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deadline",
                table: "ItemsToDo");

            migrationBuilder.DropColumn(
                name: "RemindOn",
                table: "ItemsToDo");
        }
    }
}
