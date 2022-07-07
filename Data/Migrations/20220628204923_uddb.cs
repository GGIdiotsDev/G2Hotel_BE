using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace g2hotel_server.Data.Migrations
{
    public partial class uddb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoomTypeId",
                table: "Photos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Photos_RoomTypeId",
                table: "Photos",
                column: "RoomTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_RoomTypes_RoomTypeId",
                table: "Photos",
                column: "RoomTypeId",
                principalTable: "RoomTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_RoomTypes_RoomTypeId",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Photos_RoomTypeId",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "RoomTypeId",
                table: "Photos");
        }
    }
}
