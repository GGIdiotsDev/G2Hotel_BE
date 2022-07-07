using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace g2hotel_server.Data.Migrations
{
    public partial class add_soluong_phong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Amount",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Rooms");
        }
    }
}
