using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymLog.Migrations
{
    public partial class tempset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SetId",
                table: "Templates",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Templates_SetId",
                table: "Templates",
                column: "SetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Templates_Sets_SetId",
                table: "Templates",
                column: "SetId",
                principalTable: "Sets",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Templates_Sets_SetId",
                table: "Templates");

            migrationBuilder.DropIndex(
                name: "IX_Templates_SetId",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "SetId",
                table: "Templates");
        }
    }
}
