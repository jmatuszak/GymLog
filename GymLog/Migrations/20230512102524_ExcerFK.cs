using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymLog.Migrations
{
    public partial class ExcerFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WorkoutSegments_ExcerciseId",
                table: "WorkoutSegments");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutSegments_ExcerciseId",
                table: "WorkoutSegments",
                column: "ExcerciseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WorkoutSegments_ExcerciseId",
                table: "WorkoutSegments");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutSegments_ExcerciseId",
                table: "WorkoutSegments",
                column: "ExcerciseId",
                unique: true);
        }
    }
}
