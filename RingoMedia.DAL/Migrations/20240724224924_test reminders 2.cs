using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingoMedia.DAL.Migrations
{
    public partial class testreminders2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "Reminders",
                newName: "ReminderDateTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReminderDateTime",
                table: "Reminders",
                newName: "DateTime");
        }
    }
}
