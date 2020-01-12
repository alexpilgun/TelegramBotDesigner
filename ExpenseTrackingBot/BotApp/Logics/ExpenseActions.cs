using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using BotDesignerLib;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackingBot
{
    public static class ExpenseActions
    {
        public static LibActionResult ProcessExpenseValue(string userInput, Chat chat, TelegramBotClient botClient)
        {
            bool status = false;
            if (Decimal.TryParse(userInput, out decimal val))
            {
                ((DomainDataContext)chat.DataContext).CurrentExpense.ExpenseValue = val;
                status = true;
            }
            
            return new LibActionResult() { Status = status };
        }

        public static LibActionResult SetExpenseCategory(string userInput, Chat chat, TelegramBotClient botClient)
        {
            var expenseCategories = ((DomainDataContext)chat.DataContext).ExpenseCategories;
            var selectedCategory = expenseCategories.Where(x => x.Name == userInput).FirstOrDefault();
            
            if(selectedCategory == null)
            {
                return new LibActionResult { Status = false, ErrorMessage = "Категория не найдена" };
            }

            ((DomainDataContext)chat.DataContext).CurrentExpense.Category = selectedCategory;

            return new LibActionResult() { Status = true };
        }

        public static LibActionResult PrintExpense(string userInput, Chat chat, TelegramBotClient botClient)
        {
            string messageText = String.Format("{0} - {1}",
            ((DomainDataContext)chat.DataContext).CurrentExpense.Category.Name,
            ((DomainDataContext)chat.DataContext).CurrentExpense.ExpenseValue
            );
            var status = TelegramActions.sendMessage(chat.СhatId, messageText, null, botClient);

            return new LibActionResult() { Status = status };
        }

        public static LibActionResult SetNewCurrentExpense(string userInput, Chat chat, TelegramBotClient botClient)
        {
            //var newExpense = new Expense();
            ((DomainDataContext)chat.DataContext).CurrentExpense = new Expense();
            return new LibActionResult() { Status = true };
        }

        public static LibActionResult DeleteCurrentExpense(string userInput, Chat chat, TelegramBotClient botClient)
        {
            using (var ctx = new AppDbContext())
            {
                var currentExpense = ((DomainDataContext)chat.DataContext).CurrentExpense;
                ctx.Remove(currentExpense);
                ctx.SaveChanges();
            }

            //((DomainDataContext)chat.DataContext).Expenses.Remove();
            //((DomainDataContext)chat.DataContext).CurrentExpense = null;

            return new LibActionResult() { Status = true };
        }
    }
}
