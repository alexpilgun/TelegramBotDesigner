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
using System.Linq;
using System.Text.RegularExpressions;

namespace ExpenseTrackingBot
{
    public static class GoogleAuthActions
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

        public static LibActionResult RemoveGoogleAuthorization(string userInput, Chat chat, TelegramBotClient botClient)
        {
            var connector = ((DomainDataContext)chat.DataContext).GoogleSheetsConnector;

            connector.UserCredential = null;
            connector.SheetsService = null;
            var userCredPath = new StringBuilder();
            userCredPath.Append(GoogleSheetsConnectorConfig.DataStore.FolderPath);
            userCredPath.Append("\\");
            userCredPath.Append("Google.Apis.Auth.OAuth2.Responses.TokenResponse-");
            userCredPath.Append(chat.СhatId.ToString());

            File.Delete(userCredPath.ToString());

            return new LibActionResult { Status = true };
        }

        public static LibActionResult SetSpreadsheet(string userInput, Chat chat, TelegramBotClient botClient)
        {
            var connector = ((DomainDataContext)chat.DataContext).GoogleSheetsConnector;
            Spreadsheet spr = null;
            string spreadsheetIdCandidate = null;

            Regex fullSheetsUrlRegexp = new Regex(@"(?<=https:\/\/docs.google.com\/spreadsheets\/d\/)[A-Za-z0-9-_]{44}(?=\/)");
            Match spreadsheetIdUrlMatch = fullSheetsUrlRegexp.Match(userInput);

            Regex exactSheetIdRegexp = new Regex(@"[A-Za-z0-9-_]{44}");
            Match spreadsheetIdExactMatch = exactSheetIdRegexp.Match(userInput);


            if(spreadsheetIdUrlMatch != null)
            {
                spreadsheetIdCandidate = spreadsheetIdUrlMatch.Value;
            }
            else if(spreadsheetIdExactMatch != null)
            {
                spreadsheetIdCandidate = spreadsheetIdExactMatch.Value;
            }
            else
            {
                return new LibActionResult() 
                { 
                    Status = false,
                    ErrorMessage = "Пришли корректную ссылку на файл Google Sheets или идентификатор файла"
                };
            }
            
            var r = AuthorizeAndGetSheetsService(userInput, chat, botClient);

            if(r.Status == false)
            {
                return new LibActionResult()
                {
                    Status = false,
                    ErrorMessage = "Не удалось авторизовать твой аккаунт у Гугла:("
                };
            }
            
            try
            {
                spr = connector.SheetsService.Spreadsheets.Get(spreadsheetIdCandidate).Execute();
                connector.SpreadsheetId = spreadsheetIdCandidate;
            }
            catch (Google.GoogleApiException e)
            {
                return new LibActionResult() 
                { 
                    Status = false,
                    ErrorMessage = "Google не нашел такой файл, попробуй снова :("
                };
            }

            return new LibActionResult() { Status = true };
        }

        public static LibActionResult SendSheetsListForSelect(string userInput, Chat chat, TelegramBotClient botClient)
        {
            var connector = ((DomainDataContext)chat.DataContext).GoogleSheetsConnector;
            var status = false;
            var messageText = "Выбери лист, на котором будут учитываться расходы";

            var r = AuthorizeAndGetSheetsService(userInput, chat, botClient);

            if (r.Status == false)
            {
                return new LibActionResult()
                {
                    Status = false,
                    ErrorMessage = "Не удалось авторизовать твой аккаунт у Гугла:("
                };
            }

            var sheetsList = connector.SheetsService.Spreadsheets.Get(connector.SpreadsheetId)
                .Execute().Sheets;

            var buttonsList = new List<InlineKeyboardButton>();
            foreach (var sheet in sheetsList)
            {
                buttonsList.Add(
                        InlineKeyboardButton.WithCallbackData(
                            sheet.Properties.Title,
                            sheet.Properties.SheetId.ToString()
                            )
                    );
            }

            var inlineKeyboard = new InlineKeyboardMarkup(buttonsList);
            status = TelegramActions.sendMessage(chat.СhatId, messageText, inlineKeyboard, botClient);

            return new LibActionResult() { Status = status };
        }

        public static LibActionResult SetSheet(string userInput, Chat chat, TelegramBotClient botClient)
        {
            var connector = ((DomainDataContext)chat.DataContext).GoogleSheetsConnector;
            int sheetNum = -1;
            if( Int32.TryParse(userInput, out sheetNum))
            {
                connector.SheetId = sheetNum;
            }
            else
            {
                return new LibActionResult()
                {
                    Status = false,
                    ErrorMessage = "Идентификатор листа должен быть числом"
                };
            }

            var r = AuthorizeAndGetSheetsService(userInput, chat, botClient);

            if (r.Status == false)
            {
                return new LibActionResult()
                {
                    Status = false,
                    ErrorMessage = "Не удалось авторизовать твой аккаунт у Гугла:("
                };
            }

            try
            {
                connector.Sheetname = connector.SheetsService.Spreadsheets.Get(connector.SpreadsheetId)
                .Execute().Sheets.Where(x => x.Properties.SheetId == connector.SheetId).First().Properties.Title;
            }
            catch (Exception e)
            {
                string errMsg = null;
                if(e is Google.GoogleApiException || e is InvalidOperationException)
                {
                    errMsg = "Не удалось найти такой лист.";
                }
                else
                {
                    errMsg = "Не удалось установить лист по неизвестной причине.";
                }
                return new LibActionResult() { Status = false, ErrorMessage = errMsg };
            }
            
            return new LibActionResult() { Status = true };
        }
    }
}
