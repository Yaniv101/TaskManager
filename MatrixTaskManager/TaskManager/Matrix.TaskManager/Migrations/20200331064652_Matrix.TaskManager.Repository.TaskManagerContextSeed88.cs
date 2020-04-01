using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Matrix.TaskManager.Migrations
{
    public partial class MatrixTaskManagerRepositoryTaskManagerContextSeed88 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "RegisterTS",
                value: new DateTime(2020, 3, 31, 9, 46, 51, 938, DateTimeKind.Local).AddTicks(431));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "RegisterTS",
                value: new DateTime(2020, 3, 31, 1, 51, 59, 883, DateTimeKind.Local).AddTicks(678));
        }
    }
}
