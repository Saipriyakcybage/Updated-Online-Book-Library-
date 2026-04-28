using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Migrations
{
    /// <inheritdoc />
    public partial class NotificationTableUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Borrows_BorrowTableId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_BorrowTableId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "BorrowTableId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Notifications",
                newName: "BorrowId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BorrowId",
                table: "Notifications",
                newName: "ProductId");

            migrationBuilder.AddColumn<int>(
                name: "BorrowTableId",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Notifications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_BorrowTableId",
                table: "Notifications",
                column: "BorrowTableId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Borrows_BorrowTableId",
                table: "Notifications",
                column: "BorrowTableId",
                principalTable: "Borrows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
