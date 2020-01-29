using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TestMe.UserManagement.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "UserManagement");

            migrationBuilder.CreateTable(
                name: "Outbox",
                schema: "UserManagement",
                columns: table => new
                {
                    EventId = table.Column<Guid>(nullable: false),
                    RoutingKey = table.Column<string>(nullable: false),
                    CorrelationId = table.Column<string>(nullable: false),
                    Payload = table.Column<string>(nullable: false),
                    PostDateTime = table.Column<DateTimeOffset>(nullable: true),
                    ReceivedDateTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Outbox", x => x.EventId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "UserManagement",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    EmailAddress_Value = table.Column<string>(maxLength: 512, nullable: true),
                    Password_Hash = table.Column<string>(nullable: true),
                    Password_Salt = table.Column<string>(nullable: true),
                    Role = table.Column<int>(nullable: false),
                    MembershipLevel = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Outbox_PostDateTime_CorrelationId",
                schema: "UserManagement",
                table: "Outbox",
                columns: new[] { "PostDateTime", "CorrelationId" },
                filter: "\"PostDateTime\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmailAddress_Value",
                schema: "UserManagement",
                table: "Users",
                column: "EmailAddress_Value",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Outbox",
                schema: "UserManagement");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "UserManagement");
        }
    }
}
