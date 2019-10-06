using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TestMe.TestCreation.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "TestCreation");

            migrationBuilder.CreateTable(
                name: "Owner",
                schema: "TestCreation",
                columns: table => new
                {
                    OwnerId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    QuestionsCatalogsCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owner", x => x.OwnerId);
                });

            migrationBuilder.CreateTable(
                name: "Catalog",
                schema: "TestCreation",
                columns: table => new
                {
                    CatalogId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(maxLength: 2048, nullable: true),
                    OwnerId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    QuestionsCount = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catalog", x => x.CatalogId);
                    table.ForeignKey(
                        name: "FK_Catalog_Owner_OwnerId",
                        column: x => x.OwnerId,
                        principalSchema: "TestCreation",
                        principalTable: "Owner",
                        principalColumn: "OwnerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Catalog_Owner_OwnerId1",
                        column: x => x.OwnerId,
                        principalSchema: "TestCreation",
                        principalTable: "Owner",
                        principalColumn: "OwnerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Question",
                schema: "TestCreation",
                columns: table => new
                {
                    QuestionId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Content = table.Column<string>(maxLength: 2048, nullable: true),
                    CatalogId = table.Column<long>(nullable: false),
                    OwnerId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    xmin = table.Column<uint>(type: "xid", nullable: false, defaultValue: 0u)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.QuestionId);
                    table.ForeignKey(
                        name: "FK_Question_Catalog_CatalogId",
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
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CatalogOfQuestionsCatalogId = table.Column<long>(nullable: true),
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
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Test",
                schema: "TestCreation",
                columns: table => new
                {
                    TestId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Title = table.Column<string>(maxLength: 2048, nullable: true),
                    CatalogId = table.Column<long>(nullable: false),
                    OwnerId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test", x => x.TestId);
                    table.ForeignKey(
                        name: "FK_Test_Catalog_CatalogId",
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
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IsCorrect = table.Column<bool>(nullable: false),
                    OrdinalNumber = table.Column<short>(nullable: false),
                    Content = table.Column<string>(maxLength: 2048, nullable: true),
                    QuestionId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answer", x => x.AnswerId);
                    table.ForeignKey(
                        name: "FK_Answer_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalSchema: "TestCreation",
                        principalTable: "Question",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionItem",
                schema: "TestCreation",
                columns: table => new
                {
                    QuestionItemId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    QuestionId = table.Column<long>(nullable: true),
                    TestId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionItem", x => x.QuestionItemId);
                    table.ForeignKey(
                        name: "FK_QuestionItem_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalSchema: "TestCreation",
                        principalTable: "Question",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionItem_Test_TestId",
                        column: x => x.TestId,
                        principalSchema: "TestCreation",
                        principalTable: "Test",
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
                name: "IX_Question_CatalogId",
                schema: "TestCreation",
                table: "Question",
                column: "CatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_Question_QuestionId",
                schema: "TestCreation",
                table: "Question",
                column: "QuestionId");

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
                name: "IX_QuestionsCatalogItem_CatalogOfQuestionsCatalogId",
                schema: "TestCreation",
                table: "QuestionsCatalogItem",
                column: "CatalogOfQuestionsCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_Test_CatalogId",
                schema: "TestCreation",
                table: "Test",
                column: "CatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_Test_TestId",
                schema: "TestCreation",
                table: "Test",
                column: "TestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Answer",
                schema: "TestCreation");

            migrationBuilder.DropTable(
                name: "QuestionItem",
                schema: "TestCreation");

            migrationBuilder.DropTable(
                name: "QuestionsCatalogItem",
                schema: "TestCreation");

            migrationBuilder.DropTable(
                name: "Question",
                schema: "TestCreation");

            migrationBuilder.DropTable(
                name: "Test",
                schema: "TestCreation");

            migrationBuilder.DropTable(
                name: "Catalog",
                schema: "TestCreation");

            migrationBuilder.DropTable(
                name: "Owner",
                schema: "TestCreation");
        }
    }
}
