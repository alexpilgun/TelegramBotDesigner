using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

using BotDesignerLib;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace ExpenseTrackingBot
{
    public static class GoogleSheetsAuthActions
    {
        public static LibActionResult AuthorizeAndGetSheetsService(string userInput, Chat chat, TelegramBotClient botClient)
        {
            var connector = ((DomainDataContext)chat.DataContext).GoogleSheetsConnector;

            if(connector.UserCredential != null & connector.SheetsService != null)
            {
                return new LibActionResult() { Status = true };
            }

            connector.AuthorizeUser();
            if(connector.UserCredential == null)
            {
                return new LibActionResult() { Status = false };
            }

            connector.CreateSheetsService();
            if(connector.SheetsService == null)
            {
                return new LibActionResult() { Status = false };
            }

            return new LibActionResult() { Status = true };
        }

        public static LibActionResult SetSpreadsheetId(string userInput, Chat chat, TelegramBotClient botClient)
        {
            var connector = ((DomainDataContext)chat.DataContext).GoogleSheetsConnector;
            connector.SpreadsheetId = userInput;
            return new LibActionResult() { Status = true };
        }
    }
}
