using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECPC.Migrations
{
    /// <inheritdoc />
    public partial class CreateUserProfileTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    Bio = table.Column<string>(nullable: true),
                    Facebook = table.Column<string>(nullable: true),
                    Instagram = table.Column<string>(nullable: true),
                    Twitter = table.Column<string>(nullable: true),
                    EventsAttended = table.Column<int>(nullable: false),
                    EventsCreated = table.Column<int>(nullable: false),
                    AttendancePercentage = table.Column<float>(nullable: false),
                    ProfileImageUrl = table.Column<string>(nullable: true),
                    Following = table.Column<int>(nullable: false),
                    Followers = table.Column<int>(nullable: false),
                    Connections = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProfiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                }); 
        }
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
