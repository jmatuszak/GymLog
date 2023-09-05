using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymLog.Migrations
{
    public partial class userexercise : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Workouts",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Exercises",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_AppUserId",
                table: "Exercises",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_AspNetUsers_AppUserId",
                table: "Exercises",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_AspNetUsers_AppUserId",
                table: "Exercises");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_AppUserId",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Exercises");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Workouts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldMaxLength: 40);
        }
    }
}
