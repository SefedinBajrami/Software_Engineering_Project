using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECPC.Migrations
{
    /// <inheritdoc />
    public partial class MakeMaxParticipantsNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MaxParticipants",
                table: "Events",
                type: "int",
                nullable: true, // Make column nullable
                oldClrType: typeof(int),
                oldType: "int");
        }


        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MaxParticipants",
                table: "Events",
                type: "int",
                nullable: false, // Revert to non-nullable
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
