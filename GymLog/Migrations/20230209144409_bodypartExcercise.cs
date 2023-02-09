using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymLog.Migrations
{
    public partial class bodypartExcercise : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BodyPartExcercise");

            migrationBuilder.CreateTable(
                name: "BodyPartExcercises",
                columns: table => new
                {
                    BodyPartId = table.Column<int>(type: "int", nullable: false),
                    ExcerciseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BodyPartExcercises", x => new { x.BodyPartId, x.ExcerciseId });
                    table.ForeignKey(
                        name: "FK_BodyPartExcercises_BodyParts_BodyPartId",
                        column: x => x.BodyPartId,
                        principalTable: "BodyParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BodyPartExcercises_Excercises_ExcerciseId",
                        column: x => x.ExcerciseId,
                        principalTable: "Excercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BodyPartExcercises_ExcerciseId",
                table: "BodyPartExcercises",
                column: "ExcerciseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BodyPartExcercises");

            migrationBuilder.CreateTable(
                name: "BodyPartExcercise",
                columns: table => new
                {
                    BodyPartsId = table.Column<int>(type: "int", nullable: false),
                    ExcercisesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BodyPartExcercise", x => new { x.BodyPartsId, x.ExcercisesId });
                    table.ForeignKey(
                        name: "FK_BodyPartExcercise_BodyParts_BodyPartsId",
                        column: x => x.BodyPartsId,
                        principalTable: "BodyParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BodyPartExcercise_Excercises_ExcercisesId",
                        column: x => x.ExcercisesId,
                        principalTable: "Excercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BodyPartExcercise_ExcercisesId",
                table: "BodyPartExcercise",
                column: "ExcercisesId");
        }
    }
}
