using Microsoft.EntityFrameworkCore.Migrations;

namespace MAV.Chat.Infrastructure.Migrations
{
    public partial class dateReadToReadDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateRead",
                table: "Messages",
                newName: "ReadDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReadDate",
                table: "Messages",
                newName: "DateRead");
        }
    }
}
