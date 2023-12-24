using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OggettoCase.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddOnDeleteCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "owner_id",
                schema: "oggetto_case",
                table: "calendars");

            migrationBuilder.AddForeignKey(
                name: "owner_id",
                schema: "oggetto_case",
                table: "calendars",
                column: "owner_id",
                principalSchema: "oggetto_case",
                principalTable: "users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "owner_id",
                schema: "oggetto_case",
                table: "calendars");

            migrationBuilder.AddForeignKey(
                name: "owner_id",
                schema: "oggetto_case",
                table: "calendars",
                column: "owner_id",
                principalSchema: "oggetto_case",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
