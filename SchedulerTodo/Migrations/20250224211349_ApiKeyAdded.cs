using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchedulerTodo.Migrations
{
    /// <inheritdoc />
    public partial class ApiKeyAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApiKeys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Expiration = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiKeys", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ApiKeys",
                columns: new[] { "Id", "Expiration", "Key" },
                values: new object[] { 1, null, "k8FZGeZg#I#6b1SwblyU^49TeZLtHLP!y!sB2boP*djNMFosfd" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiKeys");
        }
    }
}
