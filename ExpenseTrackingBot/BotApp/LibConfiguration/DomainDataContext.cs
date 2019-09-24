using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTrackingBot.BotApp
{
    public class DomainDataContext: BotDesignerLib.DataContext
    {
        public Guid Id { get; }
        public string UserId { get; set; }
        public List<Expense> Expenses { get; set; }
        public List<ExpenseCategory> Categories { get; set; }
        public GoogleSheetsConnector GSheetsConnector { get; set; }

        public DomainDataContext (string userId)
        {
            Id = new Guid();
            UserId = userId;
            Expenses = new List<Expense>();
            Categories = new List<ExpenseCategory>();
            GSheetsConnector = new GoogleSheetsConnector();
        }

}

    public class Expense
    {
        public Guid Id { get; }
        public decimal ExpenseValue { get; set; }
        public ExpenseCategory Category { get; set; }
        public DateTime ExpenseDate { get; set; }

        public Expense()
        {
            Id = new Guid();
        }
    }

    public class ExpenseCategory
    {
        public Guid Id { get; }
        public string Name { get;set;}

        public ExpenseCategory()
        {
            Id = new Guid();
        }
    }

    public class GoogleSheetsConnector
    {
        public Guid Id { get; }
        public string GoogleSheetsSourceSpreadsheetId { get; set; }

        public GoogleSheetsConnector ()
        {
            Id = new Guid();
        }
    }
}
