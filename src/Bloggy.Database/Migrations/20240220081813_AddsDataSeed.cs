using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bloggy.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddsDataSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "created_at", "password_hash", "password_salt", "username" },
                values: new object[,]
                {
                    { new Guid("7004a7ce-45e5-4d79-97e6-cb791ee5eb17"), new DateTimeOffset(new DateTime(2024, 2, 20, 8, 0, 40, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "ABC073BE888C36BF213A3B669E2BC892EE18F6C6", "73616C74", "alex" },
                    { new Guid("dd985147-f7f1-49ab-9e25-cd6f5e7f3be5"), new DateTimeOffset(new DateTime(2024, 2, 20, 8, 0, 40, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "ABC073BE888C36BF213A3B669E2BC892EE18F6C6", "73616C74", "simon" },
                    { new Guid("e1318721-b268-4d74-83fb-c35d08525f34"), new DateTimeOffset(new DateTime(2024, 2, 20, 8, 0, 40, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "ABC073BE888C36BF213A3B669E2BC892EE18F6C6", "73616C74", "jordano" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("7004a7ce-45e5-4d79-97e6-cb791ee5eb17"));

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("dd985147-f7f1-49ab-9e25-cd6f5e7f3be5"));

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("e1318721-b268-4d74-83fb-c35d08525f34"));
        }
    }
}
