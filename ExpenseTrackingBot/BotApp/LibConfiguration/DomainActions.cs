using System;
using System.Collections.Generic;
using System.Text;
using BotDesignerLib;

namespace ExpenseTrackingBot
{
    public class DomainActions
    {
        // ToDo: Method to get customKeyboard in case action is list in block
        public static LibActionResult SaveUserName(string userInput, Chat chat)
        {
            DomainDataContext chatDomainDataContext = (DomainDataContext)chat.DataContext;
            chatDomainDataContext.UserName = userInput;
            return new LibActionResult() { Status = true };
        }

        public static LibActionResult SendUserName(string userInput, Chat chat)
        {
            DomainDataContext chatDomainDataContext = (DomainDataContext)chat.DataContext;
            string userName = chatDomainDataContext.UserName;
            TelegramActions.sendMessage(chat.СhatId, userName, null, Program.config.BotClient);

            return new LibActionResult() { Status = true };
        }
    }
    public static class ExpenseActions
    {
        public static LibActionResult ProcessExpenseValue(string userInput, Chat chat)
        {
            bool status = false;
            if (Decimal.TryParse(userInput, out decimal val))
            {
                ((DomainDataContext)chat.DataContext).Expenses.CurrentObject.ExpenseValue = val;
                status = true;
            }
            
            return new LibActionResult() { Status = status };
        }

        public static LibActionResult SendExpenseCategoriesList(Chat chat)
        {
            var status = false;
            var messageText = "Выбери категорию расхода:";
            
            //bool status = TelegramActions.sendMessage(chat.СhatId, messageText, null, botClient);

            return new LibActionResult() { Status = status };
        }
    }
}
