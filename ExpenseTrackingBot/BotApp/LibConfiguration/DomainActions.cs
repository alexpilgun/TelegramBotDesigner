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
            DomainDataContext chatDomainDataContext = (DomainDataContext)chat.State.DataContext;
            chatDomainDataContext.UserName = userInput;
            return new LibActionResult() { Status = true };
        }

        public static LibActionResult SendUserName(string userInput, Chat chat)
        {
            DomainDataContext chatDomainDataContext = (DomainDataContext)chat.State.DataContext;
            string userName = chatDomainDataContext.UserName;
            TelegramActions.sendMessage(chat.chatId, userName, null, Program.config.BotClient);

            return new LibActionResult() { Status = true };
        }
    }
}
