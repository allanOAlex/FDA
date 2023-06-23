using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TB.Persistence.Migrations
{
    public partial class TokenName_ToStringDataType_AuthToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TokenValue",
                table: "AuthTokens",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "TokenName",
                table: "AuthTokens",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "c884f611-6cca-4d7c-9f0c-6765e01e35b3");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "dc036833-2b28-4db6-bd6d-562eed3bd318");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "17672ffe-eaa8-4133-958d-a3eb9dd8989d");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedOn", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1733eaa3-1883-4c58-86a6-cb277a24de7d", new DateTime(2023, 6, 20, 16, 5, 21, 716, DateTimeKind.Local).AddTicks(6589), "AQAAAAEAACcQAAAAEJOmCvTsPl4w0NdbIlx49nyYUVEc2q3XSOPL/N9PJBY2OwaDsLu+fF3r+bM7JQZRPw==", "828b7910-a5ae-4525-866e-2b77b13e751f" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedOn", "PasswordHash", "SecurityStamp" },
                values: new object[] { "888c66e9-7d9d-4b8f-812d-cc84591eadad", new DateTime(2023, 6, 20, 16, 5, 21, 716, DateTimeKind.Local).AddTicks(6653), "AQAAAAEAACcQAAAAEMrjR31FnptzLrtfCAC+e5TzH0gWv5heGcT4MUsjyfZDp8aPiOOitERl+K2KqpjPjA==", "4b19534a-9692-4b1e-a184-9219e69e99e7" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TokenValue",
                table: "AuthTokens",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TokenName",
                table: "AuthTokens",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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
    }
}
