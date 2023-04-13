using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymLog.Migrations
{
    public partial class TemplateIDNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TemplateSegments_Templates_TemplateId",
                table: "TemplateSegments");

            migrationBuilder.AlterColumn<int>(
                name: "TemplateId",
                table: "TemplateSegments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateSegments_Templates_TemplateId",
                table: "TemplateSegments",
                column: "TemplateId",
                principalTable: "Templates",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TemplateSegments_Templates_TemplateId",
                table: "TemplateSegments");

            migrationBuilder.AlterColumn<int>(
                name: "TemplateId",
                table: "TemplateSegments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateSegments_Templates_TemplateId",
                table: "TemplateSegments",
                column: "TemplateId",
                principalTable: "Templates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
