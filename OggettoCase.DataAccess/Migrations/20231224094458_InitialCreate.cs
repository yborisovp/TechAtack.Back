using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OggettoCase.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "oggetto_case");

            migrationBuilder.CreateTable(
                name: "categories",
                schema: "oggetto_case",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "calendars",
                schema: "oggetto_case",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    external_event_id = table.Column<string>(type: "text", nullable: false),
                    external_calendar_id = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    started_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ended_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    owner_id = table.Column<long>(type: "bigint", nullable: false),
                    link_to_meeting = table.Column<string>(type: "text", nullable: false),
                    additional_links = table.Column<List<string>>(type: "text[]", nullable: true),
                    event_details = table.Column<List<string>>(type: "text[]", nullable: true),
                    isDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_calendars", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "oggetto_case",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    external_id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    surname = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    role = table.Column<int>(type: "integer", nullable: false),
                    email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    authentication_type = table.Column<int>(type: "integer", nullable: false),
                    access_token = table.Column<string>(type: "text", nullable: false),
                    is_approved = table.Column<bool>(type: "boolean", nullable: false),
                    categoryId = table.Column<int>(type: "integer", nullable: true),
                    photo_url = table.Column<string>(type: "text", nullable: false),
                    refresh_token = table.Column<string>(type: "text", nullable: false),
                    refresh_token_expiration_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    calendarId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_users", x => x.id);
                    table.ForeignKey(
                        name: "fK_users_calendars_calendarId",
                        column: x => x.calendarId,
                        principalSchema: "oggetto_case",
                        principalTable: "calendars",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fK_users_categories_categoryId",
                        column: x => x.categoryId,
                        principalSchema: "oggetto_case",
                        principalTable: "categories",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "comments",
                schema: "oggetto_case",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    text = table.Column<string>(type: "character varying(355)", maxLength: 355, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    calendar_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_comments", x => x.id);
                    table.ForeignKey(
                        name: "calendar_event_id",
                        column: x => x.calendar_id,
                        principalSchema: "oggetto_case",
                        principalTable: "calendars",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fK_comments_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "oggetto_case",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "iX_calendars_owner_id",
                schema: "oggetto_case",
                table: "calendars",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "iX_comments_calendar_id",
                schema: "oggetto_case",
                table: "comments",
                column: "calendar_id");

            migrationBuilder.CreateIndex(
                name: "iX_comments_user_id",
                schema: "oggetto_case",
                table: "comments",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "iX_users_calendarId",
                schema: "oggetto_case",
                table: "users",
                column: "calendarId");

            migrationBuilder.CreateIndex(
                name: "iX_users_categoryId",
                schema: "oggetto_case",
                table: "users",
                column: "categoryId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "owner_id",
                schema: "oggetto_case",
                table: "calendars");

            migrationBuilder.DropTable(
                name: "comments",
                schema: "oggetto_case");

            migrationBuilder.DropTable(
                name: "users",
                schema: "oggetto_case");

            migrationBuilder.DropTable(
                name: "calendars",
                schema: "oggetto_case");

            migrationBuilder.DropTable(
                name: "categories",
                schema: "oggetto_case");
        }
    }
}
