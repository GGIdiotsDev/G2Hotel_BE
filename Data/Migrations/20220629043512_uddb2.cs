using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace g2hotel_server.Data.Migrations
{
    public partial class uddb2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Rooms_RoomId",
                table: "Photos");

            migrationBuilder.DropForeignKey(
                name: "FK_Photos_RoomTypes_RoomTypeId",
                table: "Photos");

            migrationBuilder.AlterColumn<int>(
                name: "RoomTypeId",
                table: "Photos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "Photos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Rooms_RoomId",
                table: "Photos",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_RoomTypes_RoomTypeId",
                table: "Photos",
                column: "RoomTypeId",
                principalTable: "RoomTypes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Rooms_RoomId",
                table: "Photos");

            migrationBuilder.DropForeignKey(
                name: "FK_Photos_RoomTypes_RoomTypeId",
                table: "Photos");

            migrationBuilder.AlterColumn<int>(
                name: "RoomTypeId",
                table: "Photos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "Photos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Rooms_RoomId",
                table: "Photos",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_RoomTypes_RoomTypeId",
                table: "Photos",
                column: "RoomTypeId",
                principalTable: "RoomTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
