using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace g2hotel_server.Data.Migrations
{
    public partial class update_tbl_detailservicepayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Amount",
                table: "DetailServicePayments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "DetailServicePayments");
        }
    }
}
