using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComplainManagement.Migrations
{
    public partial class ComplainTableUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Complains_AspNetUsers_UserId",
                table: "Complains");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3798939c-baaf-45c5-8854-bb85351246c9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "76dce6d5-a2bf-4498-ba69-f9d140ed2a28");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Complains",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "03f3dd31-5897-4365-9d4d-8734ad12a360", "1", "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "40cdc4e6-4535-4f80-8e33-6893bd3b00f6", "2", "User", "User" });

            migrationBuilder.AddForeignKey(
                name: "FK_Complains_AspNetUsers_UserId",
                table: "Complains",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Complains_AspNetUsers_UserId",
                table: "Complains");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "03f3dd31-5897-4365-9d4d-8734ad12a360");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "40cdc4e6-4535-4f80-8e33-6893bd3b00f6");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Complains",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3798939c-baaf-45c5-8854-bb85351246c9", "2", "User", "User" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "76dce6d5-a2bf-4498-ba69-f9d140ed2a28", "1", "Admin", "Admin" });

            migrationBuilder.AddForeignKey(
                name: "FK_Complains_AspNetUsers_UserId",
                table: "Complains",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
