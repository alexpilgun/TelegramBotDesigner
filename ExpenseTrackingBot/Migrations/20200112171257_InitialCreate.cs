using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ExpenseTrackingBot.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "State",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SchemaName = table.Column<string>(nullable: false),
                    CurrentMessageBlockName = table.Column<string>(nullable: false),
                    CurrentMessageId = table.Column<string>(nullable: false),
                    WaitForUserTransition = table.Column<bool>(nullable: false),
                    HasBeenAtLastMessage = table.Column<bool>(nullable: false),
                    ProcessedUserInput = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_State", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Chats",
                columns: table => new
                {
                    СhatId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StateId = table.Column<long>(nullable: false),
                    DataContextId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.СhatId);
                    table.ForeignKey(
                        name: "FK_Chats_State_StateId",
                        column: x => x.StateId,
                        principalTable: "State",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Expense",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ExpenseValue = table.Column<decimal>(nullable: false),
                    ExpenseCategoryId = table.Column<string>(nullable: true),
                    ExpenseDate = table.Column<DateTime>(nullable: false),
                    DomainDataContextId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expense", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseCategory",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    DomainDataContextId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataContext",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    UserId = table.Column<long>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    CurrentExpenseId = table.Column<string>(nullable: true),
                    CurrentExpenseCategoryId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataContext", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataContext_ExpenseCategory_CurrentExpenseCategoryId",
                        column: x => x.CurrentExpenseCategoryId,
                        principalTable: "ExpenseCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DataContext_Expense_CurrentExpenseId",
                        column: x => x.CurrentExpenseId,
                        principalTable: "Expense",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GoogleSheetsConnector",
                columns: table => new
                {
                    ChatId = table.Column<string>(nullable: false),
                    SpreadsheetId = table.Column<string>(nullable: true),
                    Sheetname = table.Column<string>(nullable: true),
                    SheetId = table.Column<int>(nullable: true),
                    DomainDataContextId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoogleSheetsConnector", x => x.ChatId);
                    table.ForeignKey(
                        name: "FK_GoogleSheetsConnector_DataContext_DomainDataContextId",
                        column: x => x.DomainDataContextId,
                        principalTable: "DataContext",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chats_DataContextId",
                table: "Chats",
                column: "DataContextId");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_StateId",
                table: "Chats",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_DataContext_CurrentExpenseCategoryId",
                table: "DataContext",
                column: "CurrentExpenseCategoryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DataContext_CurrentExpenseId",
                table: "DataContext",
                column: "CurrentExpenseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Expense_DomainDataContextId",
                table: "Expense",
                column: "DomainDataContextId");

            migrationBuilder.CreateIndex(
                name: "IX_Expense_ExpenseCategoryId",
                table: "Expense",
                column: "ExpenseCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseCategory_DomainDataContextId",
                table: "ExpenseCategory",
                column: "DomainDataContextId");

            migrationBuilder.CreateIndex(
                name: "IX_GoogleSheetsConnector_DomainDataContextId",
                table: "GoogleSheetsConnector",
                column: "DomainDataContextId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_DataContext_DataContextId",
                table: "Chats",
                column: "DataContextId",
                principalTable: "DataContext",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Expense_DataContext_DomainDataContextId",
                table: "Expense",
                column: "DomainDataContextId",
                principalTable: "DataContext",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Expense_ExpenseCategory_ExpenseCategoryId",
                table: "Expense",
                column: "ExpenseCategoryId",
                principalTable: "ExpenseCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseCategory_DataContext_DomainDataContextId",
                table: "ExpenseCategory",
                column: "DomainDataContextId",
                principalTable: "DataContext",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expense_DataContext_DomainDataContextId",
                table: "Expense");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseCategory_DataContext_DomainDataContextId",
                table: "ExpenseCategory");

            migrationBuilder.DropTable(
                name: "Chats");

            migrationBuilder.DropTable(
                name: "GoogleSheetsConnector");

            migrationBuilder.DropTable(
                name: "State");

            migrationBuilder.DropTable(
                name: "DataContext");

            migrationBuilder.DropTable(
                name: "Expense");

            migrationBuilder.DropTable(
                name: "ExpenseCategory");
        }
    }
}
