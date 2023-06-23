using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TB.Persistence.Migrations
{
    public partial class Added_DateCreated_DateUpdated_AuthToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "AuthTokens",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdated",
                table: "AuthTokens",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "3e95c516-0765-41cc-b50a-aff626a99568");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "46fd6f19-2b0a-4820-8912-fd62072a585c");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "bd9adb55-a98d-4c0e-85f0-e8107f1cecc4");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedOn", "PasswordHash", "SecurityStamp" },
                values: new object[] { "32a04720-2351-46cc-b3d0-ab27746afa88", new DateTime(2023, 6, 19, 16, 0, 8, 515, DateTimeKind.Local).AddTicks(5152), "AQAAAAEAACcQAAAAEGCIvax2+yp2P69dvefOjj0N9/5Pb6EBZ/24NhmOUJSFdqZ5cVzGZP8+KDneDOxIwQ==", "f38d7031-914d-48e2-a6da-52bc1b95316a" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedOn", "PasswordHash", "SecurityStamp" },
                values: new object[] { "312554f6-fb2c-40e5-8dbb-122cd5891ba5", new DateTime(2023, 6, 19, 16, 0, 8, 515, DateTimeKind.Local).AddTicks(5169), "AQAAAAEAACcQAAAAEJDqWutR1ssBc033FEZ2RXDeUvAQJMwf7BlnBqmGFFyuCB+2mrtfFZzYXXI3RRVcVw==", "9a123eea-2c90-4f00-8931-a8d6a1e9c287" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "AuthTokens");

            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "AuthTokens");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "8d2fea9c-9a68-49cd-a9f3-f3a8663e7e0f");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "aef54e89-aa81-4aec-9b3e-2df726c92c92");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "32e19c10-8270-4046-b06d-881b93ad019a");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedOn", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c0d14a5e-7103-4b5f-ac3e-19f134923bf0", new DateTime(2023, 6, 19, 15, 20, 21, 194, DateTimeKind.Local).AddTicks(7239), "AQAAAAEAACcQAAAAEDn3BjcnqM2MurOy8b6arTP0BRw9Xi8W3Fm09P46T8LmbYqOrM8hBfQRw9Bnx4LY6A==", "c058ca1e-5a07-4761-ae52-e6c941abb932" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedOn", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6318fc63-88f9-403e-b80f-f4891feea4f6", new DateTime(2023, 6, 19, 15, 20, 21, 194, DateTimeKind.Local).AddTicks(7266), "AQAAAAEAACcQAAAAEGkkgCBnMIACCxKdY2BvzlOE41qjrNDkSa0VYnWa29Zvtga3GLclQHE601H6pVVjOg==", "49a9e2c7-46ba-42bd-b27d-b86e55e3011c" });
        }
    }
}
