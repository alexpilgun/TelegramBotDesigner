using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using BotDesignerLib;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace ExpenseTrackingBot
{
    public static class ExpenseCategoryActions
    {
        public static LibActionResult CreateExpenseCategory(string userInput, Chat chat, TelegramBotClient botClient)
        {
            var categories = ((DomainDataContext)chat.DataContext).ExpenseCategories;
            var sameNameCategories = categories.Where(x => x.Name == userInput);

            if(sameNameCategories.Count() > 0)
            {
                return new LibActionResult() { Status = false, ErrorMessage = "Категория с таким именем уже существует." };
            }

            var newExpenseCategory = new ExpenseCategory(userInput);
            ((DomainDataContext)chat.DataContext).ExpenseCategories.Add(newExpenseCategory);

            return new LibActionResult() { Status = true };
        }

        public static LibActionResult EditExpenseCategory(string userInput, Chat chat, TelegramBotClient botClient)
        {
            var selectedCategory = ((DomainDataContext)chat.DataContext).CurrentExpenseCategory;
            
            if(selectedCategory == null)
            {
                return new LibActionResult { Status = false, ErrorMessage = "Не выбрана категория" };
            }

            selectedCategory.Name = userInput;

            return new LibActionResult() { Status = true };
        }

        public static LibActionResult DeleteExpenseCategory(string userInput, Chat chat, TelegramBotClient botClient)
        {
            var selectedCategory = ((DomainDataContext)chat.DataContext).CurrentExpenseCategory;

            if (selectedCategory == null)
            {
                return new LibActionResult { Status = false, ErrorMessage = "Не выбрана категория" };
            }

            ((DomainDataContext)chat.DataContext).ExpenseCategories.Remove(selectedCategory);
            ((DomainDataContext)chat.DataContext).CurrentExpenseCategory = null;

            return new LibActionResult() { Status = true };
        }

        public static LibActionResult SendExpenseCategoriesList(string userInput, Chat chat, TelegramBotClient botClient)
        {
            var status = false;
            string messageText = "Выбери категорию расходов:";
            var categories = ((DomainDataContext)chat.DataContext).ExpenseCategories;

            var buttonsList = new List<InlineKeyboardButton>();

            foreach (var category in categories)
            {
                buttonsList.Add(InlineKeyboardButton.WithCallbackData(category.Name));
            }

            var inlineKeyboard = new InlineKeyboardMarkup(buttonsList);
            status = TelegramActions.sendMessage(chat.СhatId, messageText, inlineKeyboard, botClient);

            return new LibActionResult() { Status = status };
        }

        public static LibActionResult SelectCategory(string userInput, Chat chat, TelegramBotClient botClient)
        {
            var categories = ((DomainDataContext)chat.DataContext).ExpenseCategories;
            var categoryCandidates = categories.Where(x => x.Name == userInput);

            if (categoryCandidates.Count() == 0)
            {
                return new LibActionResult() { Status = false, ErrorMessage = "Категория не найдена" };
            }
            else if(categoryCandidates.Count() > 1)
            {
                return new LibActionResult() { Status = false, ErrorMessage = "Найдено более двух категорий" };
            }
            else
            {
                ((DomainDataContext)chat.DataContext).CurrentExpenseCategory = categoryCandidates.First();
                return new LibActionResult() { Status = true };
            }
        }

        public static LibActionResult BulkCreateCategories(string userInput, Chat chat, TelegramBotClient botClient)
        {
            var parsedCategories = userInput.Split(";", StringSplitOptions.RemoveEmptyEntries);

            if(parsedCategories == null || parsedCategories.Length == 0)
            {
                return new LibActionResult() { Status = false, ErrorMessage = "" };
            }

            var existingCategories = ((DomainDataContext)chat.DataContext).ExpenseCategories;
            foreach (var categoryName in parsedCategories)
            {
                if (existingCategories.Where(x=> x.Name == categoryName).Count() > 0)
                {
                    continue;
                }

                var newExpenseCategory = new ExpenseCategory(categoryName);
                ((DomainDataContext)chat.DataContext).ExpenseCategories.Add(newExpenseCategory);
            }

            return new LibActionResult() { Status = true };
        }
    }
}
