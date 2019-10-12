using System;
using System.Collections.Generic;
using System.Text;
using BotDesignerLib;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;


namespace ExpenseTrackingBot
{
    public class BaseContextActions
    {
        // ToDo: Method to get customKeyboard in case action is list in block
        public static LibActionResult SaveUserName(string userInput, Chat chat, TelegramBotClient botClient)
        {
            DomainDataContext chatDomainDataContext = (DomainDataContext)chat.DataContext;
            chatDomainDataContext.UserName = userInput;
            return new LibActionResult() { Status = true };
        }

        public static LibActionResult SendUserName(string userInput, Chat chat, TelegramBotClient botClient)
        {
            DomainDataContext chatDomainDataContext = (DomainDataContext)chat.DataContext;
            string userName = chatDomainDataContext.UserName;
            TelegramActions.sendMessage(chat.СhatId, userName, null, Program.config.BotClient);

            return new LibActionResult() { Status = true };
        }
    }
}
