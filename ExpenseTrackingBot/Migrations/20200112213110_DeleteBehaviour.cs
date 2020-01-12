using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseTrackingBot.Migrations
{
    public partial class DeleteBehaviour : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DataContext_ExpenseCategory_CurrentExpenseCategoryId",
                table: "DataContext");

            migrationBuilder.DropForeignKey(
                name: "FK_DataContext_Expense_CurrentExpenseId",
                table: "DataContext");

            migrationBuilder.AddForeignKey(
                name: "FK_DataContext_ExpenseCategory_CurrentExpenseCategoryId",
                table: "DataContext",
                column: "CurrentExpenseCategoryId",
                principalTable: "ExpenseCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_DataContext_Expense_CurrentExpenseId",
                table: "DataContext",
                column: "CurrentExpenseId",
                principalTable: "Expense",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DataContext_ExpenseCategory_CurrentExpenseCategoryId",
                table: "DataContext");

            migrationBuilder.DropForeignKey(
                name: "FK_DataContext_Expense_CurrentExpenseId",
                table: "DataContext");

            migrationBuilder.AddForeignKey(
                name: "FK_DataContext_ExpenseCategory_CurrentExpenseCategoryId",
                table: "DataContext",
                column: "CurrentExpenseCategoryId",
                principalTable: "ExpenseCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DataContext_Expense_CurrentExpenseId",
                table: "DataContext",
                column: "CurrentExpenseId",
                principalTable: "Expense",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
