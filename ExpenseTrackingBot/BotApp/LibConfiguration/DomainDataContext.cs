using System;
using System.Collections.Generic;
using System.Text;
using BotDesignerLib;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackingBot
{
    [Serializable]
    public class DomainDataContext: DataContext
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public virtual GoogleSheetsConnector GoogleSheetsConnector { get; set; }
        public virtual List<Expense> Expenses { get; set; }
        public virtual Expense CurrentExpense { get; set; }
        public string CurrentExpenseId { get; set; }
        public virtual List<ExpenseCategory> ExpenseCategories { get; set; }
        public virtual ExpenseCategory CurrentExpenseCategory { get; set; }
        public string CurrentExpenseCategoryId { get; set; }

        public DomainDataContext (long userId): base()
        {
            UserId = userId;
            Expenses = new List<Expense>();
            ExpenseCategories = new List<ExpenseCategory>();
            GoogleSheetsConnector = new GoogleSheetsConnector(userId);
        }
    }
}

