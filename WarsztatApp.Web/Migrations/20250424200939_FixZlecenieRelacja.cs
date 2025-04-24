using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarsztatApp.Web.Migrations
{
    /// <inheritdoc />
    public partial class FixZlecenieRelacja : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Zlecenia_Warsztaty_WarsztatId",
                table: "Zlecenia");

            migrationBuilder.DropForeignKey(
                name: "FK_Zlecenia_Warsztaty_WarsztatId1",
                table: "Zlecenia");

            migrationBuilder.DropIndex(
                name: "IX_Zlecenia_WarsztatId1",
                table: "Zlecenia");

            migrationBuilder.DropColumn(
                name: "WarsztatId1",
                table: "Zlecenia");

            migrationBuilder.AddForeignKey(
                name: "FK_Zlecenia_Warsztaty_WarsztatId",
                table: "Zlecenia",
                column: "WarsztatId",
                principalTable: "Warsztaty",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Zlecenia_Warsztaty_WarsztatId",
                table: "Zlecenia");

            migrationBuilder.AddColumn<int>(
                name: "WarsztatId1",
                table: "Zlecenia",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Zlecenia_WarsztatId1",
                table: "Zlecenia",
                column: "WarsztatId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Zlecenia_Warsztaty_WarsztatId",
                table: "Zlecenia",
                column: "WarsztatId",
                principalTable: "Warsztaty",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Zlecenia_Warsztaty_WarsztatId1",
                table: "Zlecenia",
                column: "WarsztatId1",
                principalTable: "Warsztaty",
                principalColumn: "Id");
        }
    }
}
