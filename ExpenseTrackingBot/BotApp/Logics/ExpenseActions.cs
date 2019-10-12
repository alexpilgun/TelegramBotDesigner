using System;
using System.Collections.Generic;
using System.Text;
using BotDesignerLib;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace ExpenseTrackingBot
{
    public static class ExpenseActions
    {
        public static LibActionResult ProcessExpenseValue(string userInput, Chat chat, TelegramBotClient botClient)
        {
            bool status = false;
            if (Decimal.TryParse(userInput, out decimal val))
            {
                ((DomainDataContext)chat.DataContext).Expenses.CurrentObject.ExpenseValue = val;
                status = true;
            }
            
            return new LibActionResult() { Status = status };
        }

        public static LibActionResult SendExpenseCategoriesList(string userInput, Chat chat, TelegramBotClient botClient)
        {
            var status = false;
            var messageText = "Выбери категорию расхода:";
            var inlineKeyboard = new InlineKeyboardMarkup(new List<InlineKeyboardButton> ()
            { 
                InlineKeyboardButton.WithCallbackData(
                    "Категория 1",
                    "Category_1"
                    ),
                InlineKeyboardButton.WithCallbackData(
                    "Категория 2",
                    "Category_2"
                    )
                }
                );

            status = TelegramActions.sendMessage(chat.СhatId, messageText, inlineKeyboard, botClient);

            return new LibActionResult() { Status = status };
        }

        public static LibActionResult SetExpenseCategory(string userInput, Chat chat, TelegramBotClient botClient)
        {
            var expenseCategory = new ExpenseCategory();
            expenseCategory.Name = userInput;
            ((DomainDataContext)chat.DataContext).Expenses.CurrentObject.Category = expenseCategory;

            return new LibActionResult() { Status = true };
        }

        public static LibActionResult PrintExpense(string userInput, Chat chat, TelegramBotClient botClient)
        {
            string messageText = String.Format("{0} - {1}",
            ((DomainDataContext)chat.DataContext).Expenses.CurrentObject.Category.Name,
            ((DomainDataContext)chat.DataContext).Expenses.CurrentObject.ExpenseValue
            );
            var status = TelegramActions.sendMessage(chat.СhatId, messageText, null, botClient);

            return new LibActionResult() { Status = status };
        }

        public static LibActionResult SetNewCurrentExpense(string userInput, Chat chat, TelegramBotClient botClient)
        {
            //var newExpense = new Expense();
            ((DomainDataContext)chat.DataContext).Expenses.CurrentObject = new Expense();
            return new LibActionResult() { Status = true };
        }
    }
}
