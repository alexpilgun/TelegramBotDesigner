using System;
using System.Collections.Generic;
using System.Text;
using BotDesignerLib;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackingBot
{
    public class AppDbContext: LibDbContext
    {
        public DbSet<DomainDataContext> DomainDataContexts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies().UseNpgsql("Host=localhost;Port=5432;Database=tglibdb;Username=postgres;Password=password");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<DomainDataContext>()
                .HasOne(b => b.GoogleSheetsConnector).WithOne(b => b.DomainDataContext)
                .HasForeignKey<GoogleSheetsConnector>(b => b.DomainDataContextId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<DomainDataContext>()
                .HasMany(b => b.Expenses).WithOne(b => b.DomainDataContext)
                .HasForeignKey(p => p.DomainDataContextId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<DomainDataContext>()
                .HasMany(b => b.ExpenseCategories).WithOne(b => b.DomainDataContext)
                .HasForeignKey(b => b.DomainDataContextId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DomainDataContext>()
                .HasOne(p => p.CurrentExpense).WithOne()
                .HasForeignKey<DomainDataContext>(p => p.CurrentExpenseId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder
                .Entity<DomainDataContext>()
                .HasOne(b => b.CurrentExpenseCategory).WithOne()
                .HasForeignKey<DomainDataContext>(p => p.CurrentExpenseCategoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
