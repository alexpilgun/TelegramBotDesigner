using System;
using System.Collections.Generic;
using System.Text;
using BotDesignerLib;

namespace ExpenseTrackingBot
{
    public class DomainDataContext: IDataContext
    {
        public Guid Id { get; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public CollectionObject<Expense> Expenses { get; set; }
        public CollectionObject<ExpenseCategory> Categories { get; set; }
        public GoogleSheetsConnector GSheetsConnector { get; set; }

        public DomainDataContext (string userId)
        {
            Id = new Guid();
            UserId = userId;
            Expenses = new CollectionObject<Expense>();
            Categories = new CollectionObject<ExpenseCategory>();
            GSheetsConnector = new GoogleSheetsConnector();
        }

        public DomainDataContext() { }

}

    public class Expense: GuidEntity
    {
        public decimal ExpenseValue { get; set; }
        public ExpenseCategory Category { get; set; }
        public DateTime ExpenseDate { get; set; }

        public Expense()
        {

        }
    }

    public class ExpenseCategory : GuidEntity
    {
        public string Name { get;set;}

        public ExpenseCategory()
        {
            
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
