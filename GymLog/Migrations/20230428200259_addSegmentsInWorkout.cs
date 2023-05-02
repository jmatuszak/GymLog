using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymLog.Migrations
{
    public partial class addSegmentsInWorkout : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Workouts",
                newName: "StartDate");

            migrationBuilder.AddColumn<int>(
                name: "WorkoutId",
                table: "WorkoutSegments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Workouts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutSegments_WorkoutId",
                table: "WorkoutSegments",
                column: "WorkoutId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutSegments_Workouts_WorkoutId",
                table: "WorkoutSegments",
                column: "WorkoutId",
                principalTable: "Workouts",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutSegments_Workouts_WorkoutId",
                table: "WorkoutSegments");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutSegments_WorkoutId",
                table: "WorkoutSegments");

            migrationBuilder.DropColumn(
                name: "WorkoutId",
                table: "WorkoutSegments");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Workouts");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Workouts",
                newName: "Date");
        }
    }
}
