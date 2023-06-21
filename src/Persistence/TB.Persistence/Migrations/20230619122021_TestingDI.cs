using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TB.Persistence.Migrations
{
    public partial class TestingDI : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion2",
                table: "UserLogins");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion2",
                table: "UserLogins",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "262161ce-c221-4f19-b505-0e5cc88239d2");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "fb739e06-8e7c-4ebb-babc-d2497c05bdb8");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "9bb8dfc1-d5a8-4e27-b23d-30a5af4b7d93");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedOn", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a936d500-55a4-4335-8374-db1fd539fb11", new DateTime(2023, 6, 19, 15, 13, 53, 426, DateTimeKind.Local).AddTicks(7660), "AQAAAAEAACcQAAAAELz3POHR3IWetblxOg14ybFhdx9HdfPuA/dQm7N6c7VgSbSJv87wOlHcp7BfAcI6PA==", "77fee62e-f026-4282-b5a6-91a0e0a98c68" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedOn", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3b8fa32c-492e-4f7e-91df-39617fe77ed8", new DateTime(2023, 6, 19, 15, 13, 53, 426, DateTimeKind.Local).AddTicks(7682), "AQAAAAEAACcQAAAAEHV15CqusPAuKbKQPJbVxBLw/iz9YBCbPMd+kZca2hfKbp6tBwRvPWoZsjomQdzcBA==", "5a009722-d8e8-4ebd-a3f3-045ccd762174" });
        }
    }
}
