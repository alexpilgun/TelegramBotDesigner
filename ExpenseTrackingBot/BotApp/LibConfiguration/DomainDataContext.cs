using System;
using System.Collections.Generic;
using System.Text;
using BotDesignerLib;
using LiteDB;

namespace ExpenseTrackingBot
{
    [Serializable]
    public class DomainDataContext: IDataContext
    {
        public Guid Id { get; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public GoogleSheetsConnector GoogleSheetsConnector { get; set; }
        public CollectionObject<Expense> Expenses { get; set; }
        public CollectionObject<ExpenseCategory> ExpenseCategories { get; set; }

        public DomainDataContext (long userId)
        {
            Id = new Guid();
            UserId = userId;
            Expenses = new CollectionObject<Expense>();
            ExpenseCategories = new CollectionObject<ExpenseCategory>();
            GoogleSheetsConnector = new GoogleSheetsConnector(userId);
        }
    }

    [Serializable]
    public class Expense: GuidEntity
    {
        public decimal ExpenseValue { get; set; }
        public ExpenseCategory Category { get; set; }
        public DateTime ExpenseDate { get; set; }

        public Expense() : base()
        {
            ExpenseValue = 0M;
            ExpenseDate = DateTime.Now;
        }
    }
    [Serializable]
    public class ExpenseCategory : GuidEntity
    {
        public string Name { get;set;}

        public ExpenseCategory(string name) : base()
        {
            Name = name;
        }
    }
}

