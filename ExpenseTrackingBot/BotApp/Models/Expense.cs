using System;
using System.Collections.Generic;
using System.Text;
using BotDesignerLib;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackingBot
{
    public class Expense
    {
        [Key]
        public string Id { get; set; }
        public decimal ExpenseValue { get; set; }
        public virtual ExpenseCategory Category { get; set; }
        public string ExpenseCategoryId { get; set; }
        public DateTime ExpenseDate { get; set; }
        public virtual DomainDataContext DomainDataContext { get; set; }
        public string DomainDataContextId { get; set; }
        public Expense() : base()
        {
            Id = Guid.NewGuid().ToString();
            ExpenseValue = 0M;
            ExpenseDate = DateTime.Now;
        }
    }

    [Serializable]
    public class ExpenseCategory
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public virtual DomainDataContext DomainDataContext { get; set; }
        public string DomainDataContextId { get; set; }
        public ExpenseCategory() { }
        public ExpenseCategory(string name) : base()
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
        }
    }
}
