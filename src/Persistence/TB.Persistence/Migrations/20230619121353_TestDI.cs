using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TB.Persistence.Migrations
{
    public partial class TestDI : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion2",
                table: "UserLogins");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "7cfb6d0c-b916-4658-bdac-b8a67cc70d98");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "3bfdbc9f-a24c-49f3-86a5-e7a5be86a6c6");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "e8820c7c-0888-4666-8791-854aa0f5c7a1");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedOn", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5cddfa00-097f-4d7f-8530-6f2c53b7ebae", new DateTime(2023, 6, 19, 14, 34, 51, 713, DateTimeKind.Local).AddTicks(3523), "AQAAAAEAACcQAAAAEFfK1MsBwlX6oCB1NMye3HM+g+el8o0EVGZZdKIxZhiOj34u194xHEiv6tLMy+P6Xw==", "eba48263-a0f2-4032-86a7-ca507bf17ab5" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedOn", "PasswordHash", "SecurityStamp" },
                values: new object[] { "77e0d360-e3fb-492a-9245-679eb684bf8b", new DateTime(2023, 6, 19, 14, 34, 51, 713, DateTimeKind.Local).AddTicks(3551), "AQAAAAEAACcQAAAAELeH9Kd+ZA8dUobwukHdLIe3heQdmIYbVBf13i+DKL87dQd0IGVWJf32hUzi+3bM8A==", "fea63c85-c14b-4188-a1b7-016245e1f24c" });
        }
    }
}
