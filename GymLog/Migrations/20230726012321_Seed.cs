using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymLog.Migrations
{
    public partial class Seed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BodyParts",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Chest" },
                    { 2, "Back" },
                    { 3, "Legs" },
                    { 4, "Core" },
                    { 5, "Shoulders" },
                    { 6, "Arms" }
                });

            migrationBuilder.InsertData(
                table: "Exercises",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Bench Press" },
                    { 2, "Incline Bench Press" },
                    { 3, "T-Bar Row" },
                    { 4, "Push Ups" },
                    { 5, "Wide Push Ups" },
                    { 6, "Diamond Push Ups" },
                    { 7, "Archer Push Ups" },
                    { 8, "Decline PushUps" },
                    { 9, "Dips" },
                    { 10, "Ring Dips" },
                    { 11, "Straight Bar Dips" },
                    { 12, "Dumbell Pullover" },
                    { 13, "Single Arm Row" },
                    { 14, "Bent Over Row" },
                    { 15, "Single Leg Deadlift" },
                    { 16, "Pulldowns Behind Head" },
                    { 17, "Pull Row" },
                    { 18, "Pull Ups" },
                    { 19, "Calf Raises" },
                    { 20, "Lunges" },
                    { 21, "One Leg Claf Raises" },
                    { 22, "Pistol Squat" },
                    { 23, "Squat" },
                    { 24, "Box Jumps" },
                    { 25, "Deadlift" },
                    { 26, "Bridge" },
                    { 27, "Hanging Knee Raises" },
                    { 28, "Leg Raises" },
                    { 29, "L-Sit Pull Ups" },
                    { 30, "Russian Twist" },
                    { 31, "Sit Ups" },
                    { 32, "Toes To Bar" },
                    { 33, "V-Ups" },
                    { 34, "Knee Elbow Push Ups" },
                    { 35, "Front Raises" },
                    { 36, "Lateral Raises" }
                });

            migrationBuilder.InsertData(
                table: "Exercises",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 37, "Upright Row" },
                    { 38, "Bent Over Reverse Fly" },
                    { 39, "Military Press - OHP" },
                    { 40, "Bicep Curl" },
                    { 41, "French Press" },
                    { 42, "Hummer Curls" },
                    { 43, "Preacher Bicep Curl" },
                    { 44, "Chin Ups" }
                });

            migrationBuilder.InsertData(
                table: "Templates",
                columns: new[] { "Id", "AppUserId", "Name" },
                values: new object[,]
                {
                    { 1, null, "Sample Workout 1" },
                    { 2, null, "Sample Workout 2" },
                    { 3, null, "Sample Workout 3" }
                });

            migrationBuilder.InsertData(
                table: "BodyPartExercises",
                columns: new[] { "BodyPartId", "ExerciseId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 1, 3 },
                    { 1, 4 },
                    { 1, 5 },
                    { 1, 6 },
                    { 1, 7 },
                    { 1, 8 },
                    { 1, 9 },
                    { 1, 10 },
                    { 1, 11 },
                    { 1, 33 },
                    { 2, 12 },
                    { 2, 13 },
                    { 2, 14 },
                    { 2, 15 },
                    { 2, 16 },
                    { 2, 17 },
                    { 2, 24 },
                    { 2, 33 },
                    { 2, 37 },
                    { 2, 43 },
                    { 3, 14 },
                    { 3, 18 },
                    { 3, 19 },
                    { 3, 20 },
                    { 3, 21 },
                    { 3, 22 },
                    { 3, 23 },
                    { 3, 24 },
                    { 4, 4 },
                    { 4, 5 },
                    { 4, 6 },
                    { 4, 7 },
                    { 4, 8 },
                    { 4, 9 },
                    { 4, 10 },
                    { 4, 16 },
                    { 4, 17 },
                    { 4, 23 },
                    { 4, 24 },
                    { 4, 25 }
                });

            migrationBuilder.InsertData(
                table: "BodyPartExercises",
                columns: new[] { "BodyPartId", "ExerciseId" },
                values: new object[,]
                {
                    { 4, 26 },
                    { 4, 27 },
                    { 4, 28 },
                    { 4, 29 },
                    { 4, 30 },
                    { 4, 31 },
                    { 4, 32 },
                    { 4, 33 },
                    { 4, 38 },
                    { 5, 34 },
                    { 5, 35 },
                    { 5, 36 },
                    { 5, 37 },
                    { 5, 38 },
                    { 6, 1 },
                    { 6, 2 },
                    { 6, 3 },
                    { 6, 4 },
                    { 6, 5 },
                    { 6, 6 },
                    { 6, 7 },
                    { 6, 8 },
                    { 6, 9 },
                    { 6, 10 },
                    { 6, 15 },
                    { 6, 16 },
                    { 6, 17 },
                    { 6, 39 },
                    { 6, 40 },
                    { 6, 41 },
                    { 6, 42 },
                    { 6, 43 }
                });

            migrationBuilder.InsertData(
                table: "WorkoutSegments",
                columns: new[] { "Id", "Description", "ExerciseId", "TemplateId", "WeightType", "WorkoutId" },
                values: new object[,]
                {
                    { 1, "2 x 1-4", 1, 1, 0, null },
                    { 2, "2 x 1-4", 2, 1, 0, null },
                    { 3, "2 x 1-4", 3, 1, 1, null },
                    { 4, "2 x 1-4", 10, 1, 2, null },
                    { 5, "2 x 1-4", 11, 1, 1, null },
                    { 6, "2 x 1-4", 12, 1, 1, null },
                    { 7, "2 x 1-4", 33, 1, 2, null },
                    { 8, "2 x 10-12", 4, 2, 2, null },
                    { 9, "2 x 10-12", 6, 2, 2, null },
                    { 10, "2 x 10-12", 7, 2, 2, null }
                });

            migrationBuilder.InsertData(
                table: "WorkoutSegments",
                columns: new[] { "Id", "Description", "ExerciseId", "TemplateId", "WeightType", "WorkoutId" },
                values: new object[,]
                {
                    { 11, "2 x 10-12", 14, 2, 1, null },
                    { 12, "2 x 10-12", 15, 2, 1, null },
                    { 13, "2 x 10-12", 24, 2, 0, null },
                    { 14, "2 x 10-12", 33, 2, 2, null },
                    { 15, "2 x 10-12", 34, 2, 0, null },
                    { 16, "2 x 10-12", 40, 2, 0, null },
                    { 17, "2 x 6-8", 1, 3, 1, null },
                    { 18, "2 x 6-8", 10, 3, 2, null },
                    { 19, "2 x 6-8", 20, 3, 2, null },
                    { 20, "2 x 6-8", 30, 3, 2, null },
                    { 21, "2 x 6-8", 40, 3, 1, null },
                    { 22, "2 x 6-8", 41, 3, 1, null }
                });

            migrationBuilder.InsertData(
                table: "Workouts",
                columns: new[] { "Id", "AppUserId", "EndDate", "Name", "StartDate", "TemplateId" },
                values: new object[,]
                {
                    { 1, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sample Workout 1", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sample Workout 2", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 3, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sample Workout 3", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 }
                });

            migrationBuilder.InsertData(
                table: "Sets",
                columns: new[] { "Id", "Description", "Reps", "Weight", "WorkoutSegmentId" },
                values: new object[,]
                {
                    { 1, null, 4, 0f, 1 },
                    { 2, null, 4, 0f, 1 },
                    { 3, null, 4, 0f, 2 },
                    { 4, null, 4, 0f, 2 },
                    { 5, null, 4, 0f, 3 },
                    { 6, null, 4, 0f, 3 },
                    { 7, null, 4, 0f, 4 },
                    { 8, null, 4, 0f, 4 },
                    { 9, null, 4, 0f, 5 },
                    { 10, null, 4, 0f, 5 },
                    { 11, null, 4, 0f, 6 },
                    { 12, null, 4, 0f, 6 },
                    { 13, null, 4, 0f, 7 },
                    { 14, null, 4, 0f, 7 },
                    { 15, null, 12, 0f, 8 },
                    { 16, null, 12, 0f, 8 },
                    { 17, null, 12, 0f, 9 },
                    { 18, null, 12, 0f, 9 },
                    { 19, null, 12, 0f, 10 },
                    { 20, null, 12, 0f, 10 },
                    { 21, null, 12, 0f, 11 },
                    { 22, null, 12, 0f, 11 },
                    { 23, null, 12, 0f, 12 },
                    { 24, null, 12, 0f, 12 },
                    { 25, null, 12, 0f, 13 },
                    { 26, null, 12, 0f, 13 },
                    { 27, null, 12, 0f, 14 },
                    { 28, null, 12, 0f, 14 },
                    { 29, null, 12, 0f, 15 },
                    { 30, null, 12, 0f, 15 },
                    { 31, null, 12, 0f, 16 },
                    { 32, null, 12, 0f, 16 },
                    { 33, null, 8, 0f, 17 },
                    { 34, null, 8, 0f, 17 },
                    { 35, null, 8, 0f, 18 },
                    { 36, null, 8, 0f, 18 },
                    { 37, null, 8, 0f, 19 },
                    { 38, null, 4, 0f, 19 },
                    { 39, null, 8, 0f, 20 },
                    { 40, null, 8, 0f, 20 },
                    { 41, null, 8, 0f, 21 },
                    { 42, null, 8, 0f, 21 }
                });

            migrationBuilder.InsertData(
                table: "Sets",
                columns: new[] { "Id", "Description", "Reps", "Weight", "WorkoutSegmentId" },
                values: new object[] { 43, null, 8, 0f, 22 });

            migrationBuilder.InsertData(
                table: "Sets",
                columns: new[] { "Id", "Description", "Reps", "Weight", "WorkoutSegmentId" },
                values: new object[] { 44, null, 8, 0f, 22 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 1, 3 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 1, 4 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 1, 5 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 1, 6 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 1, 7 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 1, 8 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 1, 9 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 1, 10 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 1, 11 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 1, 33 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 2, 12 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 2, 13 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 2, 14 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 2, 15 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 2, 16 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 2, 17 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 2, 24 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 2, 33 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 2, 37 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 2, 43 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 3, 14 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 3, 18 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 3, 19 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 3, 20 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 3, 21 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 3, 22 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 3, 23 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 3, 24 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 4, 4 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 4, 5 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 4, 6 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 4, 7 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 4, 8 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 4, 9 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 4, 10 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 4, 16 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 4, 17 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 4, 23 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 4, 24 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 4, 25 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 4, 26 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 4, 27 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 4, 28 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 4, 29 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 4, 30 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 4, 31 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 4, 32 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 4, 33 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 4, 38 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 5, 34 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 5, 35 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 5, 36 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 5, 37 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 5, 38 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 6, 1 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 6, 2 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 6, 3 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 6, 4 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 6, 5 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 6, 6 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 6, 7 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 6, 8 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 6, 9 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 6, 10 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 6, 15 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 6, 16 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 6, 17 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 6, 39 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 6, 40 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 6, 41 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 6, 42 });

            migrationBuilder.DeleteData(
                table: "BodyPartExercises",
                keyColumns: new[] { "BodyPartId", "ExerciseId" },
                keyValues: new object[] { 6, 43 });

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Sets",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Workouts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Workouts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Workouts",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "BodyParts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "BodyParts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "BodyParts",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "BodyParts",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "BodyParts",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "BodyParts",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "WorkoutSegments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "WorkoutSegments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "WorkoutSegments",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "WorkoutSegments",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "WorkoutSegments",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "WorkoutSegments",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "WorkoutSegments",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "WorkoutSegments",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "WorkoutSegments",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "WorkoutSegments",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "WorkoutSegments",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "WorkoutSegments",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "WorkoutSegments",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "WorkoutSegments",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "WorkoutSegments",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "WorkoutSegments",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "WorkoutSegments",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "WorkoutSegments",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "WorkoutSegments",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "WorkoutSegments",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "WorkoutSegments",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "WorkoutSegments",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Templates",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Templates",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Templates",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
