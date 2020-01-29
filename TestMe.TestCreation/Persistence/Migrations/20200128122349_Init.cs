using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TestMe.TestCreation.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "TestCreation");

            migrationBuilder.CreateTable(
                name: "Inbox",
                schema: "TestCreation",
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
                    table.PrimaryKey("PK_Inbox", x => x.EventId);
                });

            migrationBuilder.CreateTable(
                name: "Owners",
                schema: "TestCreation",
                columns: table => new
                {
                    OwnerId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QuestionsCatalogsCount = table.Column<int>(nullable: false),
                    MembershipLevel = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owners", x => x.OwnerId);
                });

            migrationBuilder.CreateTable(
                name: "Catalog",
                schema: "TestCreation",
                columns: table => new
                {
                    CatalogId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(maxLength: 2048, nullable: false),
                    OwnerId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    QuestionsCount = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catalog", x => x.CatalogId);
                    table.ForeignKey(
                        name: "FK_Catalog_Owners_OwnerId",
                        column: x => x.OwnerId,
                        principalSchema: "TestCreation",
                        principalTable: "Owners",
                        principalColumn: "OwnerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Catalog_Owners_OwnerId1",
                        column: x => x.OwnerId,
                        principalSchema: "TestCreation",
                        principalTable: "Owners",
                        principalColumn: "OwnerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                schema: "TestCreation",
                columns: table => new
                {
                    QuestionId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Content = table.Column<string>(maxLength: 2048, nullable: false),
                    CatalogId = table.Column<long>(nullable: false),
                    OwnerId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    xmin = table.Column<uint>(type: "xid", nullable: false, defaultValue: 0u)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.QuestionId);
                    table.ForeignKey(
                        name: "FK_Questions_Catalog_CatalogId",
                        column: x => x.CatalogId,
                        principalSchema: "TestCreation",
                        principalTable: "Catalog",
                        principalColumn: "CatalogId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionsCatalogItem",
                schema: "TestCreation",
                columns: table => new
                {
                    CatalogOfQuestionsItemId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CatalogOfQuestionsCatalogId = table.Column<long>(nullable: false),
                    TestId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionsCatalogItem", x => x.CatalogOfQuestionsItemId);
                    table.ForeignKey(
                        name: "FK_QuestionsCatalogItem_Catalog_CatalogOfQuestionsCatalogId",
                        column: x => x.CatalogOfQuestionsCatalogId,
                        principalSchema: "TestCreation",
                        principalTable: "Catalog",
                        principalColumn: "CatalogId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tests",
                schema: "TestCreation",
                columns: table => new
                {
                    TestId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(maxLength: 2048, nullable: false),
                    CatalogId = table.Column<long>(nullable: false),
                    OwnerId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tests", x => x.TestId);
                    table.ForeignKey(
                        name: "FK_Tests_Catalog_CatalogId",
                        column: x => x.CatalogId,
                        principalSchema: "TestCreation",
                        principalTable: "Catalog",
                        principalColumn: "CatalogId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Answer",
                schema: "TestCreation",
                columns: table => new
                {
                    AnswerId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsCorrect = table.Column<bool>(nullable: false),
                    OrdinalNumber = table.Column<short>(nullable: false),
                    Content = table.Column<string>(maxLength: 2048, nullable: false),
                    QuestionId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answer", x => x.AnswerId);
                    table.ForeignKey(
                        name: "FK_Answer_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalSchema: "TestCreation",
                        principalTable: "Questions",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionItem",
                schema: "TestCreation",
                columns: table => new
                {
                    QuestionItemId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QuestionId = table.Column<long>(nullable: false),
                    TestId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionItem", x => x.QuestionItemId);
                    table.ForeignKey(
                        name: "FK_QuestionItem_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalSchema: "TestCreation",
                        principalTable: "Questions",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionItem_Tests_TestId",
                        column: x => x.TestId,
                        principalSchema: "TestCreation",
                        principalTable: "Tests",
                        principalColumn: "TestId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answer_QuestionId",
                schema: "TestCreation",
                table: "Answer",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Catalog_CatalogId",
                schema: "TestCreation",
                table: "Catalog",
                column: "CatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_Catalog_OwnerId",
                schema: "TestCreation",
                table: "Catalog",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Catalog_OwnerId1",
                schema: "TestCreation",
                table: "Catalog",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionItem_QuestionId",
                schema: "TestCreation",
                table: "QuestionItem",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionItem_QuestionItemId",
                schema: "TestCreation",
                table: "QuestionItem",
                column: "QuestionItemId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionItem_TestId",
                schema: "TestCreation",
                table: "QuestionItem",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_CatalogId",
                schema: "TestCreation",
                table: "Questions",
                column: "CatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionsCatalogItem_CatalogOfQuestionsCatalogId",
                schema: "TestCreation",
                table: "QuestionsCatalogItem",
                column: "CatalogOfQuestionsCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_CatalogId",
                schema: "TestCreation",
                table: "Tests",
                column: "CatalogId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Answer",
                schema: "TestCreation");

            migrationBuilder.DropTable(
                name: "Inbox",
                schema: "TestCreation");

            migrationBuilder.DropTable(
                name: "QuestionItem",
                schema: "TestCreation");

            migrationBuilder.DropTable(
                name: "QuestionsCatalogItem",
                schema: "TestCreation");

            migrationBuilder.DropTable(
                name: "Questions",
                schema: "TestCreation");

            migrationBuilder.DropTable(
                name: "Tests",
                schema: "TestCreation");

            migrationBuilder.DropTable(
                name: "Catalog",
                schema: "TestCreation");

            migrationBuilder.DropTable(
                name: "Owners",
                schema: "TestCreation");
        }
    }
}
